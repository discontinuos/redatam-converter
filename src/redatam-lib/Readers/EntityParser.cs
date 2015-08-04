using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace RedatamLib
{
	public class EntityParser
	{
		RedatamDatabase db;
		string rootPath;

		private static string[] validTypes = { "DBL", "LNG", "INT", "PCK", "CHR" };

		public EntityParser(RedatamDatabase db)
		{
			this.db = db;
		}

		public void Parse(string path)
		{
			rootPath = Path.GetDirectoryName(path);
			DataBlock dataBlock = new DataBlock(path);

			// separa en bloques los datos de cada entidad...
			Dictionary<string, DataBlock> dataParts = SplitDataBlocks(dataBlock, db.entityNames);

			ParseEntities(db.entityNames, dataParts);
		}

		public void ParseEntities(List<string> entitiesNames, Dictionary<string, DataBlock> dataParts)
		{
			string parent = "";
			foreach (string entity in entitiesNames)
			{
				db.Entities.Add(ParseEntity(dataParts[entity], entity, parent));
				parent = entity;
			}
		}

		public Entity ParseEntity(DataBlock dataBlock, string entity, string parent)
		{
			var e = new Entity();
			e.rootPath = rootPath;
			// construye el encabezado esperable
			var block = dataBlock.makeStringBlock(entity);
			var blockParent = dataBlock.makeStringBlock(parent);
			byte[] full;
			if (parent != "")
				full = dataBlock.addArrays(block, block, blockParent);
			else
				full = dataBlock.addArrays(block, blockParent);

			if (dataBlock.moveTo(full) == false)
				throw new Exception("Sequence not found.");
			// lee la entidad
			e.Name = dataBlock.eatShortString();
			e.RelationChild = dataBlock.eatShortString();
			if (e.RelationChild != "")
				e.RelationParent = dataBlock.eatShortString();
			e.Description = dataBlock.eatShortString();
			e.IndexFilename = dataBlock.eatShortString();
			e.s1 = dataBlock.eat16int();
			if (e.IndexFilename != "")
			{
				e.CodesVariable = dataBlock.eatShortString();
				e.LabelVariable = dataBlock.eatShortString();
				e.Level = dataBlock.eat32int();
				e.b1 = dataBlock.eatByte();
				//e.VariableCount = no es confiable...que guarde rangos... eat32int();
				// en base al count de variables, que busque los "dataset"
				while (true)
				{
					Variable v = ParseVariable(dataBlock, e);
					if (v != null)
						e.Variables.Add(v);
					else
						break;
				}
			}
			e.VariableCount = e.Variables.Count;
			return e;
		}

		private Variable ParseVariable(DataBlock dataBlock, Entity e)
		{
			// lee las variables
			if (JumptToDataSet(dataBlock) == false)
				return null;
			Variable v = new Variable(e);
			v.Name = dataBlock.eatShortString();
			v.Declaration = dataBlock.eatShortString();
			v.Filter = dataBlock.eatShortString();
			v.Range = dataBlock.eatShortString();
			v.Type = dataBlock.eatShortString();
			v.ValuesLabelsRaw = dataBlock.eatShortString();
			v.Label = dataBlock.eatShortString();
			v.Group = dataBlock.eatShortString();

			v.ParseDeclaration();
			v.ParseValueLabels();
			v.ParseMissingAndPrecision();
			return v;
		}

		private bool JumptToDataSet(DataBlock dataBlock)
		{
			if (dataBlock.moveTo("DATASET") == false)
				return false;
			// valida el tipo de dato
			if (checkDataType(dataBlock) == false)
					return false;
				//
			dataBlock.move(-2);
			// retrocede para leer el nombre
			if (dataBlock.moveBackString(32) < 1)
			{
				dataBlock.move(6);
				// este no es válido... busca si hay más...
				return JumptToDataSet(dataBlock);
			}
			else
				return true;
		}

		private bool checkDataType(DataBlock dataBlock)
		{
				dataBlock.move(8); // "DATASET "			
				if (dataBlock.n + 3 > dataBlock.data.Length)
						return false;
				string type = dataBlock.eatChars(3); // "DBL", "LNG", etc
				if (new List<string>(validTypes).Contains(type) == false)
				{
						// este no es válido... busca si hay más...
						return JumptToDataSet(dataBlock);
				}
				// retrocede hasta el inicio de DATASET
				dataBlock.move(-11);
				return true;
		}

		private Dictionary<string, DataBlock> SplitDataBlocks(DataBlock dataBlock, List<string> entitiesNames)
		{
			Dictionary<string, DataBlock> dataParts = new Dictionary<string, DataBlock>();

			int prevStart = -1;
			int iStart = 0;
			string parent = "";
			for (int i = 0; i < entitiesNames.Count; i++)
			{
				string entity = entitiesNames[i];
				iStart = ParseBeginning(dataBlock, entity, parent);
				parent = entity;
				if (prevStart != -1)
				{
					dataParts[entitiesNames[i - 1]] = dataBlock.getPart(prevStart, iStart);
				}
				prevStart = iStart;
			}
			// guarda el último
			iStart = dataBlock.data.Length;
			dataParts[entitiesNames[entitiesNames.Count - 1]] = dataBlock.getPart(prevStart, iStart);
			return dataParts;
		}

		private int ParseBeginning(DataBlock dataBlock, string entity, string parent)
		{
			var e = new Entity();
			e.rootPath = rootPath;
			// construye el encabezado esperable
			var block = dataBlock.makeStringBlock(entity);
			var blockParent = dataBlock.makeStringBlock(parent);
			byte[] full;
			if (parent != "")
				full = dataBlock.addArrays(block, block, blockParent);
			else
				full = dataBlock.addArrays(block, blockParent);

			if (dataBlock.moveTo(full) == false)
				throw new Exception("Sequence not found.");
			return dataBlock.n;
		}

	}
}

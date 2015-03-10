using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Spss;
using System.Runtime.InteropServices;

namespace RedatamConverter
{
	class RedatamDatabase
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
		out ulong lpFreeBytesAvailable,
		out ulong lpTotalNumberOfBytes,
		out ulong lpTotalNumberOfFreeBytes);

		string rootPath;
		public List<string> entityNames = new List<string>(); 
		public readonly List<Entity> Entities = new List<Entity>();
		public FuzzyEntityParser FuzzyEntityParser; 

		public RedatamDatabase()
		{
			FuzzyEntityParser = new FuzzyEntityParser(this);
		}
		
		public void Parse(string path)
		{
			rootPath = Path.GetDirectoryName(path);
			DataBlock dataBlock = new DataBlock(path);
			
			// separa en bloques los datos de cada entidad...
			Dictionary<string, DataBlock> dataParts = SplitDataBlocks(dataBlock, entityNames);

			ParseEntities(entityNames, dataParts);
		}

		private void ParseEntities(List<string> entitiesNames, Dictionary<string, DataBlock> dataParts)
		{
			string parent = "";
			foreach (string entity in entitiesNames)
			{
				Entities.Add(ParseEntity(dataParts[entity], entity, parent));
				parent = entity;
			}
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


		private Entity ParseEntity(DataBlock dataBlock, string entity, string parent)
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
			e.CodesVariable = dataBlock.eatShortString();
			e.LabelVariable = dataBlock.eatShortString();
			e.Level = dataBlock.eat32int();
			e.b1 = dataBlock.eatByte();
			//e.VariableCount = no es confiable...que guarde rangos... eat32int();
			// en base al count de variables, que busque los "dataset"
			while(true)
			{
				Variable v = ParseVariable(dataBlock, e);
				if (v != null)
					e.Variables.Add(v);
				else
					break;
			}
			e.VariableCount = e.Variables.Count;
			return e;
		}

		private Variable ParseVariable(DataBlock dataBlock, Entity e)
		{
			// lee las variables
			Variable v = new Variable(e);
			if (dataBlock.moveTo("DATASET") == false)
				return null;
			dataBlock.move(-2);
			// retrocede para leer el nombre
			if (dataBlock.moveBackString() == false)
				throw new Exception("backString not found");
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
			return v;
		}
	
		internal long GetTotalRowsSize()
		{
			long ret = 0;
			foreach (var entity in Entities)
			{
				entity.OpenPointer();
				ret += entity.GetPointerRows();
				entity.ClosePointer();
			}
			return ret;
		}
	}
}

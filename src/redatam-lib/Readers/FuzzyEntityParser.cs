using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedatamLib
{
	public class FuzzyEntityParser
	{
		RedatamDatabase db;
		public FuzzyEntityParser(RedatamDatabase db)
		{
			this.db = db;
		}

		public void ParseEntities(string path)
		{
			List<List<Entity>> candidates = new List<List<Entity>>();

			List<Entity> entitiesNames;
			DataBlock dataBlock = new DataBlock(path);
			
			// hace la hipótesis de que es una entidad...
			for (int i = 0; i < dataBlock.data.Length; i++)
			{
				dataBlock.n = i;
				entitiesNames = new List<Entity>();
				TryEntities(dataBlock, "", entitiesNames);
				if (entitiesNames.Count > 0)
					candidates.Add(entitiesNames);
			}
			db.entityNames = GetBest(candidates);
		}

		private void TryEntities(DataBlock dataBlock, string parent, List<Entity> entitiesNames)
		{
			// lee el length
			string entityNameString;
			if (dataBlock.PlausibleString(out entityNameString) == false || entityNameString  == "")
				return;
			//
			if (checkEntityStart(dataBlock) == false)
				return;
			Entity entity = new Entity() { Name = entityNameString };
			entitiesNames.Add(entity);
			// ahora se va a buscar a sus hijos
			var block = dataBlock.makeStringBlock(entityNameString);
			List<Entity> variants = new List<Entity>();
			List<int> allBlockOccurrences = dataBlock.GetAllMatches(block);
			
			foreach (int startN in allBlockOccurrences)
			{
				dataBlock.n = startN;
				ProcessOcurrence(dataBlock, variants, entity.Children);
			}
			if (variants.Count > 0)
			{
				entity.Children.Clear();
				entity.Children.AddRange(variants);
			}
		}

		private bool checkEntityStart(DataBlock dataBlock)
		{
			string relationChild;
			string name = dataBlock.eatShortString();
			if (dataBlock.eatPlausibleString(out relationChild) == false)
				return false;
			if (relationChild != "")
			{
		//		if (eatPlausibleString(out relationParent) == false)
			//		return false;
			}
			string description;
			string indexfilename;
			if (dataBlock.eatPlausibleString(out description, false) == false)
				return false;
			if (dataBlock.eatPlausibleString(out indexfilename, false) == false)
				return false;
			return description.EndsWith(".ptr") || 
				indexfilename.EndsWith(".ptr");
		}


		private void ProcessOcurrence(DataBlock dataBlock, List<Entity> leaves, List<Entity> entitiesNames)
		{
			if (dataBlock.moveBackString() == -1)
				return;
			
			int keepN = dataBlock.n;
			string child = dataBlock.eatShortString();
			dataBlock.n = keepN;
			if (entitiesNames.Count > 0 && entitiesNames[entitiesNames.Count - 1].Name == child)
				return;
			if (dataBlock.IsText(child) == false || child == "") return;
			// avanzó ok...
			List<Entity> names = new List<Entity>();
			TryEntities(dataBlock, child, names);
			if (names.Count > 0)
				leaves.AddRange(names);
		}

		private List<Entity> GetBest(List<List<Entity>> leaves)
		{
			if (leaves.Count == 0) return new List<Entity>();
			int max = 0;
			int nMax = 0;
			for (int n = 0; n < leaves.Count; n++)
			{
				int current = CalculateTreeSize(leaves[n]);
				if (current > max)
				{
					max = current;
					nMax = n;
				}
			}
			return leaves[nMax];
		}

		private int CalculateTreeSize(List<Entity> list)
		{
			int ret = 0;
			foreach (Entity entity in list)
			{
				ret++;
				ret += CalculateTreeSize(entity.Children);
			}
			return ret;
		}
		
	}
}

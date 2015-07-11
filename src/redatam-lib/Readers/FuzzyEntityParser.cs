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
			List<List<string>> candidates = new List<List<string>>();

			List<string> entitiesNames;
			DataBlock dataBlock = new DataBlock(path);
			
			// hace la hipótesis de que es una entidad...
			for (int i = 0; i < dataBlock.data.Length; i++)
			{
				dataBlock.n = i;
				entitiesNames = new List<string>();
				TryEntities(dataBlock, "", entitiesNames);
				if (entitiesNames.Count > 1)
					candidates.Add(entitiesNames);
			}
			db.entityNames = GetBest(candidates);
		}

		private void TryEntities(DataBlock dataBlock, string parent, List<string> entitiesNames)
		{
			// lee el length
			string entity;
			if (dataBlock.PlausibleString(out entity) == false || entity == "")
				return;
			//
			if (checkEntityStart(dataBlock) == false)
				return;
			
			entitiesNames.Add(entity);
			// ahora se va a buscar a sus hijos
			var block = dataBlock.makeStringBlock(entity);
			List<List<string>> variants = new List<List<string>>();
			List<int> allBlockOccurrences = dataBlock.GetAllMatches(block);
			
			foreach (int startN in allBlockOccurrences)
			{
				dataBlock.n = startN;
				ProcessOcurrence(dataBlock, variants, entitiesNames);
			}
			if (variants.Count > 0)
			{
				entitiesNames.Clear();
				entitiesNames.AddRange(GetBest(variants));
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

		
		private void ProcessOcurrence(DataBlock dataBlock, List<List<string>> leaves, List<string> entitiesNames)
		{
			if (dataBlock.moveBackString() == -1)
				return;
			
			int keepN = dataBlock.n;
			string child = dataBlock.eatShortString();
			dataBlock.n = keepN;
			if (entitiesNames.Count > 0 && entitiesNames[entitiesNames.Count - 1] == child)
				return;
			if (dataBlock.IsText(child) == false || child == "") return;
			// avanzó ok...
			List<string> names = new List<string>();
			names.AddRange(entitiesNames);
			TryEntities(dataBlock, child, names);
			if (names.Count > entitiesNames.Count)
				leaves.Add(names);
		}

		private List<string> GetBest(List<List<string>> leaves)
		{
			if (leaves.Count == 0) return new List<string>();
			int max = 0;
			int nMax = 0;
			for(int n = 0; n< leaves.Count; n++)
				if (leaves[n].Count > max)
				{
					max = leaves[n].Count;
					nMax = n;
				}
			return leaves[nMax];
		}
		
	}
}

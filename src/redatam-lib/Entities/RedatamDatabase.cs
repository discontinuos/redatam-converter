using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace RedatamLib
{
	public class RedatamDatabase
	{
		public List<string> entityNames = new List<string>(); 
		public readonly List<Entity> Entities = new List<Entity>();
		public FuzzyEntityParser FuzzyEntityParser;
		public string DictionaryFile; 

		public RedatamDatabase()
		{
			FuzzyEntityParser = new FuzzyEntityParser(this);
		}
		

		public long GetTotalRowsSize()
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

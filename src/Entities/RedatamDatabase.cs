using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Spss;
using System.Runtime.InteropServices;

namespace RedatamConverter
{
	class RedatamDatabase
	{
		public List<string> entityNames = new List<string>(); 
		public readonly List<Entity> Entities = new List<Entity>();
		public FuzzyEntityParser FuzzyEntityParser; 

		public RedatamDatabase()
		{
			FuzzyEntityParser = new FuzzyEntityParser(this);
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

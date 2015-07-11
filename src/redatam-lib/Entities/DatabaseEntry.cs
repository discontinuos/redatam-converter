using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedatamLib
{
	public class DatabaseEntry
	{
		public string DictFolder;
		public string Caption;
		public string Inl;
		public RedatamDatabase Database;
		public List<KeyValuePair<string, string>> FilesQueue;
	}
}

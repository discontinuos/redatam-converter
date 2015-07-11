using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace RedatamLib
{
	public class IniSection 
	{
		public string Name;
		public readonly Dictionary<string, string> Items = new Dictionary<string,string>();

		public IniSection(string key) 
		{ Name = key;  } 
	
		public string SafeGet(string key, string defaultValue = "")
		{
 			if (this.Items.ContainsKey(key) == false) 
				return defaultValue;
			else
				return this.Items[key];
		}
	}
}
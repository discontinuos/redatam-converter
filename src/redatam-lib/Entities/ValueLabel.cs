using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedatamLib
{
	public class ValueLabel
	{
		public ValueLabel(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		public string Key;
		public string Value;
	}
}

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.IO;
	
namespace RedatamLib
{
	public class ReadIni
	{
		public static List<IniSection> Read(string path)
		{
			List<IniSection> ret = new List<IniSection>();
			string[] lines = File.ReadAllLines(path, Encoding.Default);

			IniSection currentSection = null;
			foreach (string line in lines)
			{
				if (line.StartsWith("[") && line.EndsWith("]"))
				{
					if (currentSection != null)
						ret.Add(currentSection);
					currentSection = new IniSection(cleanBrackets(line));
				}
				else if (line.Contains("="))
				{
					int n = line.IndexOf("=");
					string key = line.Substring(0, n);
					if (currentSection != null)
						currentSection.Items[key] = line.Substring(n+1);
				}
			}
			if (currentSection != null)
				ret.Add(currentSection);
			
			return ret;
		}

		private static string cleanBrackets(string line)
		{
			return line.Substring(1, line.Length - 2);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedatamLib
{
	public class VariableParser
	{
		Variable variable;

		public VariableParser(Variable variable)
		{
			this.variable = variable;
		}

		public void ParseValueLabels()
		{
			string[] items = this.variable.ValuesLabelsRaw.Split(new char[] { '\t' },StringSplitOptions.RemoveEmptyEntries);
			this.variable.ValueLabels.Clear();
			if (this.variable.ValuesLabelsRaw == "") return;
			foreach (string item in items)
			{
				int i = item.IndexOf(" ");
				if (i == -1) break;
				string part1 = item.Substring(0, i);
				string part2 = item.Substring(i+1);
				int x;
				if (int.TryParse(part1, out x) == false)
				{
					this.variable.ValueLabels.Add(new ValueLabel(
						"0",
						item));
				}
				else
					this.variable.ValueLabels.Add(new ValueLabel(
						part1,
						part2 ));
			}
		}

		private void ParseDecimals(string extras)
		{
			if (extras.StartsWith("DECIMALS "))
			{
				string label = eatStringFromString(ref extras);
				string value = eatStringFromString(ref extras);
				this.variable.Decimals = int.Parse(value);
			}
		}

		private string GetMissingLabel(string extras, string tag)
		{
			if (extras.StartsWith(tag))
			{
				string label = eatStringFromString(ref extras);
				string value = eatStringFromString(ref extras);
				this.variable.ValueLabels.Add(new ValueLabel(
					value,
					label));
			}
			return extras;
		}

		private string eatStringFromString(ref string extras)
		{
			if (extras.StartsWith("'"))
			{
				int nEnd = extras.IndexOf("'", 1);
				if (nEnd != -1)
				{
					string retQ = extras.Substring(1, nEnd - 1);
					extras = extras.Substring(nEnd + 1).TrimStart();
					return retQ;
				}
			}

			int n = extras.IndexOf(" ");
			if (n == -1) n = extras.Length;
			string ret = extras.Substring(0, n);
			if (n < extras.Length)
				extras = extras.Substring(n + 1);
			else
				extras = "";
			return ret;
		}
		public override string ToString()
		{
			return this.variable.Name;
		}
		
		public void ParseDeclaration()
		{
			string info = this.variable.Declaration;
			string dataset = eatStringFromString(ref info);
			string type = eatStringFromString(ref info);
			string fileRaw = eatStringFromString(ref info);
			string sizeLabel = eatStringFromString(ref info);
			string size = eatStringFromString(ref info);

			if (this.variable.Type == "STRING" && type != "CHR")
				throw new Exception("Inconsistent type declaration");
			if (this.variable.Type == "REAL" && type != "DBL")
				throw new Exception("Inconsistent type declaration");
			switch (type)
			{
				// si se agregan tipos nuevos, incorporarlos en 
				// entityParser => validTypes
				case "DBL":
					this.variable.Size = 64;
					break;
				case "LNG":
					this.variable.Size = 32;
					break;
				case "INT":
					this.variable.Size = 16;
					this.variable.Type = "INT16";
					break;
				case "BIN":
					this.variable.Size = int.Parse(size);
					this.variable.BinaryDataSet = true;
					break;
				case "PCK":
				case "CHR":
					this.variable.Size = int.Parse(size);
					break;
				default:
					throw new Exception("Data type '" + type + "' is not supported. Contact idiscontinuos for support.");
			}
			this.variable.Filename = fileRaw;
		}
		
		public void ParseMissingAndPrecision()
		{
			// se fija si hay missing y na
			string extras = this.variable.Group.Trim();
			extras = GetMissingLabel(extras, "MISSING");
			extras = GetMissingLabel(extras, "NOTAPPLICABLE");
			ParseDecimals(extras);
		}
	}
}

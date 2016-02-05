using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedatamLib
{
	public class Variable
	{
		ICursorReader reader;
		Entity entity;

		public Variable(Entity entity)
		{
			this.entity = entity;
		}
		public Variable(string name, string type, string label)
		{
			Name = name;
			Type = type;
			Label = label;
		}
		public string Name;
		public string Label;
		public string Type;
		public string Range;
		public int Decimals;
		public string Declaration;
		public string Group;
		public string ValuesLabelsRaw;
		public List<KeyValuePair<string, string>> ValueLabels = new List<KeyValuePair<string, string>>();
		public string Filter;
		public int Size;
		public string Filename;
		private bool BinaryDataSet = false;
		public bool Selected = true;

		public void ParseValueLabels()
		{
			string[] items = ValuesLabelsRaw.Split(new char[] { '\t' },StringSplitOptions.RemoveEmptyEntries);
			ValueLabels.Clear();
			if (ValuesLabelsRaw == "") return;
			foreach (string item in items)
			{
				int i = item.IndexOf(" ");
				if (i == -1) break;
				string part1 = item.Substring(0, i);
				string part2 = item.Substring(i+1);
				int x;
				if (int.TryParse(part1, out x) == false)
				{
					ValueLabels.Add(new KeyValuePair<string,string>(
						"0",
						item));
				}
				else
					ValueLabels.Add(new KeyValuePair<string,string>(
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
				this.Decimals = int.Parse(value);
			}
		}

		private string GetMissingLabel(string extras, string tag)
		{
			if (extras.StartsWith(tag))
			{
				string label = eatStringFromString(ref extras);
				string value = eatStringFromString(ref extras);
				ValueLabels.Add(new KeyValuePair<string, string>(
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
			return this.Name;
		}

		public object GetData()
		{
			switch (Type)
			{
				case "STRING":
					return reader.ReadString();
				case "INTEGER":
					return reader.ReadNumber();
				case "INT16":
					return reader.ReadInt16();
				case "REAL":
					return reader.ReadDouble();
				default:
					throw new Exception("Unsupported data type: " + Type);
			}
		}

		public void ParseDeclaration()
		{
			string info = this.Declaration;
			string dataset = eatStringFromString(ref info);
			string type = eatStringFromString(ref info);
			string fileRaw = eatStringFromString(ref info);
			string sizeLabel = eatStringFromString(ref info);
			string size = eatStringFromString(ref info);

			if (this.Type == "STRING" && type != "CHR")
				throw new Exception("Inconsistent type declaration");
			if (this.Type == "REAL" && type != "DBL")
				throw new Exception("Inconsistent type declaration");
			switch (type)
			{
				// si se agregan tipos nuevos, incorporarlos en 
				// entityParser => validTypes
				case "DBL":
					this.Size = 64;
					break;
				case "LNG":
					this.Size = 32;
					break;
				case "INT":
					this.Size = 16;
					this.Type = "INT16";
					break;
				case "BIN":
					this.Size = int.Parse(size);
					this.BinaryDataSet = true;
					break;
				case "PCK":
				case "CHR":
					this.Size = int.Parse(size);
					break;
				default:
					throw new Exception("Data type '" + type + "' is not supported. Contact idiscontinuos for support.");
			}
			this.Filename = fileRaw;
		}

		public void OpenData()
		{
			if (DataFileExists())
			{
				//ubica el nombre de archivo y el tamaño de variable
				string file = ResolveDataFilename();
				reader = new CursorReader(file, this.Type == "STRING", BinaryDataSet, Size);
				reader.Open();
			}
			else
				reader = new NullCursorReader();
		}
		public bool DataFileExists()
		{
			//ubica el nombre de archivo y el tamaño de variable
			string file = ResolveDataFilename();
			return File.Exists(file);
		}

		private string ResolveDataFilename()
		{
				return RedatamDatabase.OptimisticCombine(this.entity.rootPath, Filename.Replace("'", ""));
		}

		public void CloseData()
		{
			reader.Close();
		}

		public void ParseMissingAndPrecision()
		{
			// se fija si hay missing y na
			string extras = this.Group.Trim();
			extras = GetMissingLabel(extras, "MISSING");
			extras = GetMissingLabel(extras, "NOTAPPLICABLE");
			ParseDecimals(extras);
		}

		public bool FileSizeFails( out long expectedSize,  out long actual)
		{
			expectedSize = GetExpectedFileSize();
			actual = reader.Length;	
			return expectedSize > actual;
		}

		private long GetExpectedFileSize()
		{
			switch (Type)
			{
				case "STRING":
					return CalculateCharSize();
				case "INTEGER":
				case "INT16":
				case "REAL":
					return CalculateBitsSize();
				default:
					throw new Exception("Unsupported data type: " + Type);
			}
		}

		private long CalculateCharSize()
		{
			long entityRows = this.entity.RowsCount;
			long bytes = this.Size * entityRows;
			return bytes;
		}

		private long CalculateBitsSize()
		{
			long entityRows = this.entity.RowsCount;
			long bits = (this.Size * entityRows);
			long bytes = (bits / 8);
			if (bits % 8 > 0)
				bytes++;
			return bytes;
		}
	}
}

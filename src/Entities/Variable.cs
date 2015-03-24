using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedatamConverter
{
	class Variable
	{
		CursorReader reader;
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

		internal void ParseValueLabels()
		{
			string[] items = ValuesLabelsRaw.Split(new char[] { '\t' },StringSplitOptions.RemoveEmptyEntries);
			ValueLabels.Clear();
			if (ValuesLabelsRaw == "") return;
			foreach (string item in items)
			{
				int i = item.IndexOf(" ");
				ValueLabels.Add(new KeyValuePair<string,string>(
					item.Substring(0, i),
					item.Substring(i+1)  ));
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

		internal object GetData()
		{
			switch (Type)
			{
				case "STRING":
					return reader.ReadString();
				case "INTEGER":
					return reader.ReadNumber();
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
			if (type == "DBL")
				this.Size = 64;
			else
				this.Size = int.Parse(size);
			this.Filename = fileRaw;
		}
		internal void OpenData()
		{
			//ubica el nombre de archivo y el tamaño de variable
			string file = Path.Combine(this.entity.rootPath, Filename.Replace("'", ""));
			
			reader = new CursorReader(file, this.Type == "STRING", Size);
			reader.Open();
		}

		internal void CloseData()
		{
			reader.Close();
		}

		internal void ParseMissingAndPrecision()
		{
			// se fija si hay missing y na
			string extras = this.Group.Trim();
			extras = GetMissingLabel(extras, "MISSING");
			extras = GetMissingLabel(extras, "NOTAPPLICABLE");
			ParseDecimals(extras);
		}
	}
}

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
		public List<ValueLabel> ValueLabels = new List<ValueLabel>();
		public string Filter;
		public int Size;
		public string Filename;
		public bool BinaryDataSet = false;
		public bool Selected = true;


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

		public bool FileSizeFails( out long expectedSize,  out long actual)
		{
			expectedSize = GetExpectedFileSize();
			actual = reader.Length;	
			return expectedSize > actual;
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

	}
}

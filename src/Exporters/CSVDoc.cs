using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Spss;

namespace RedatamConverter
{
	class CSVDoc
	{
		const string delimiter = ";";
		const string quote = "\"";
		public TextWriter Stream;
		public readonly List<string> Columns = new List<string>();
		public readonly List<string> Labels = new List<string>();
		public readonly string Filename;

		CSVDoc variableLabels;

		public CSVDoc(string filename)
		{
			Stream = new System.IO.StreamWriter(filename);
			Filename = filename;
		}

		public void Close()
		{
			Stream.Close();
			Stream.Dispose();
		}

		public void CommitDictionary()
		{
			WriteLine(Columns);
			// genera el archivo de VariableLabels
			if (this.Labels.Count > 0)
				WriteVariableLabels();
		}

		private void WriteVariableLabels()
		{
			if (this.Columns.Count != this.Labels.Count)
				throw new Exception("Variable count differs from Labels count.");
			string folder = Path.Combine(Path.GetDirectoryName(this.Filename), "Labels");
			if (Directory.Exists(folder) == false)
				Directory.CreateDirectory(folder);
			string path = 
				Path.Combine(folder,
				Path.GetFileNameWithoutExtension(this.Filename));
			string file = path + "-VARIABLES.csv";
			CSVDoc labels = new CSVDoc(file);
			labels.Columns.Add("variable");
			labels.Columns.Add("label");
			labels.CommitDictionary();
			for(int n = 0; n < this.Columns.Count; n++)
				labels.WriteLine(new List<string>() { this.Columns[n], this.Labels[n] });
			labels.Close();
		}

		public void WriteLine(IEnumerable<object> values)
		{
			StringBuilder sb = new StringBuilder();
			bool isFirst = true;
			foreach (object o in values)
			{
				if (isFirst == false)
					sb.Append(delimiter);
				else
					isFirst = false;
				string output;
				if (o == null)
					output = "";
				else if (o is string)
					output = o as string;
				else
					output = o.ToString();
				if (output.Contains(quote))
					output = quote + output.Replace(quote, quote + quote) + quote;
				else if (output.Contains(delimiter))
					output = quote + output + quote;
				sb.Append(output);
			}
			Stream.WriteLine(sb.ToString());
		}

		public void WriteLine(Dictionary<string, object> data)
		{
			// ordena los valores del diccionario según la lista de columnas
			List<object> values = new List<object>();
			foreach (string col in Columns)
			{
				if (data.ContainsKey(col))
					values.Add(data[col]);
				else
					values.Add(null);
			}
			WriteLine(values);
		}
	}
}

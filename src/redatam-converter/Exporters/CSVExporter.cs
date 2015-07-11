using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RedatamLib;

namespace RedatamConverter
{
	class CSVExporter : RelationalExporter<CSVDoc>
	{
		public CSVExporter(RedatamDatabase dbr)
			: base(dbr)
		{ }

		protected override CSVDoc CreateTable(string folder, Entity e)
		{
			string filename = CreateFilename(folder, e.Name, "csv");
			CSVDoc doc = new CSVDoc(filename);

			return doc;
		}
		protected override void BeginDataWrite(CSVDoc doc)
		{
			doc.CommitDictionary();
		}
		protected override void CloseTable(CSVDoc doc)
		{
			doc.Close();
		}

		protected override void CreateVariable(CSVDoc doc, Variable v)
		{
			doc.Columns.Add(v.Name);
			doc.Labels.Add(v.Label);
			// genera las etiquetas
			if (v.ValueLabels.Count > 0)
				WriteVariableValueLabels(doc, v);
		}

		private static void WriteVariableValueLabels(CSVDoc doc, Variable v)
		{
			string path = Path.Combine(Path.GetDirectoryName(doc.Filename), "Labels",
				Path.GetFileNameWithoutExtension(doc.Filename)); 
			string file = path + "-" + v.Name + "-LABELS.CSV";
			CSVDoc labels = new CSVDoc(file);
			labels.Columns.Add(v.Name);
			labels.Columns.Add("label");
			labels.CommitDictionary();
			foreach (var item in v.ValueLabels)
				labels.WriteLine(new List<string>() { item.Key, item.Value });
			labels.Close();
		}

		protected override void WriteVariablesData(CSVDoc doc, Dictionary<string, object> data)
		{
			doc.WriteLine(data);
		}

	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Spss;

namespace RedatamConverter
{
	class SpssExporter : RelationalExporter<SpssDataDocument>
	{
		public SpssExporter(RedatamDatabase dbr)
			: base(dbr)
		{ }

		protected override SpssDataDocument CreateTable(string folder, Entity e)
		{
			string filename = CreateFilename(folder, e.Name, "sav");
			SpssDataDocument doc = SpssDataDocument.Create(filename);
			return doc;
		}
		protected override void BeginDataWrite(SpssDataDocument doc)
		{
			doc.CommitDictionary();
		}
		protected override void CloseTable(SpssDataDocument doc)
		{
			doc.Close();
		}

		protected override void CreateVariable(SpssDataDocument doc, Variable v)
		{
			SpssVariable var;
			switch (v.Type)
			{
				case "STRING":
					var = CreateStringVariable(v);
					break;
				case "INTEGER":
					var = CreateNumericVariable(v);
					break;
				case "REAL":
					var = CreateNumericVariable(v);
					break;
				default:
					throw new Exception("Unsupported type: " + v.Type);
			}
			var.Name = v.Name;
			var.Label = v.Label;

			doc.Variables.Add(var);
		}

		private SpssVariable CreateNumericVariable(Variable v)
		{
			SpssVariable var;
			SpssNumericVariable var1 = new SpssNumericVariable();
			var1.PrintDecimal = v.Decimals;
			foreach (var item in v.ValueLabels)
			{
				long key = long.Parse(item.Key);
				if (var1.ValueLabels.ContainsKey(key) == false)
					var1.ValueLabels.Add(key, item.Value);
			}
			if (v.ValueLabels.Count > 0)
				var1.MeasurementLevel = MeasurementLevelCode.SPSS_MLVL_NOM;
			var = var1;
			return var;
		}

		private SpssVariable CreateStringVariable(Variable v)
		{
			SpssStringVariable var1 = new SpssStringVariable();
			foreach (var item in v.ValueLabels)
				if (var1.ValueLabels.ContainsKey(item.Key) == false)
					var1.ValueLabels.Add(item.Key, item.Value);
			var1.Length = v.Size;
			return var1;
		}

		protected override void WriteVariablesData(SpssDataDocument doc, Dictionary<string, object> data)
		{
			SpssCase case1 = doc.Cases.New();

			foreach (var pair in data)
				case1[pair.Key] = pair.Value;

			case1.Commit();
		}

	}
}

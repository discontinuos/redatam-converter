using System;
using System.Collections.Generic;
using System.Linq;
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
		
		protected override void CopyVariablesData(SpssDataDocument doc, string id, string parentId, Entity e, int n, int currentParent)
		{
			SpssCase case1 = doc.Cases.New();
			// pone su id
			case1[id] = n;
			if (parentId != "")
				case1[parentId] = currentParent;

			// pone el valor de cada variable
			foreach (Variable v in e.Variables)
			{
				case1[v.Name] = v.GetData();
			}
			case1.Commit();
		}

		protected override SpssDataDocument CreateTable(string folder, Entity e)
		{
			string filename = CreateFilename(folder, e.Name);
			SpssDataDocument doc = SpssDataDocument.Create(filename);
			return doc;
		}

		protected override void CreateIdVariables(Entity parent, Entity e, SpssDataDocument doc)
		{
			var var = CreateVariable(new Variable(MakeId(e), "INTEGER", "Identifier for " + e.Name));
			doc.Variables.Add(var);
			if (parent != null)
			{
				var = CreateVariable(new Variable(MakeId(parent), "INTEGER", "Parent entity Id"));
				doc.Variables.Add(var);
			}
		}
		protected override void CloseTable(SpssDataDocument table)
		{
			table.Close();
		}
		protected override string MakeId(Entity e)
		{
			if (e == null)
				return "";
			else
				return e.Name + "_REF_ID";
		}

		private SpssVariable CreateVariable(Variable v)
		{
			SpssVariable var;
			if (v.Type == "STRING")
			{
				var = CreateStringVariable(v);
			}
			else if (v.Type == "INTEGER")
			{
				var = CreateNumericVariable(v);
			}
			else
				throw new Exception("Unsupported type: " + v.Type);

			var.Name = v.Name;
			var.Label = v.Label;

			return var;
		}

		private SpssVariable CreateNumericVariable(Variable v)
		{
			SpssVariable var;
			SpssNumericVariable var1 = new SpssNumericVariable();
			foreach (var item in v.ValueLabels)
			{
				long key = long.Parse(item.Key);
				if (var1.ValueLabels.ContainsKey(key) == false)
					var1.ValueLabels.Add(key, item.Value);
			}
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

	  protected override void CreateMetaData(List<Variable> list, SpssDataDocument doc)
		{
			foreach (Variable v in list)
			{
				SpssVariable var = CreateVariable(v);
				doc.Variables.Add(var);
			}
			doc.CommitDictionary();
		}

	}
}

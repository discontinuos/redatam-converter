using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Spss;

namespace RedatamReader
{
	class SpssExporter
	{
		RedatamDatabase db;
		public int GlobalCurrentRow = 0;
		
		public EventHandler CallBack;

		public string CurrentEntity;
		public long CurrentEntityTotal;
		public long EntityCurrentRow;
		public bool Cancelled;

		public SpssExporter(RedatamDatabase dbr)
		{
			db = dbr;
		}
		
		public void SaveAs(string folder)
		{
			// crea archivos para cada entidad
			Entity parent = null;
			foreach (Entity e in db.Entities)
			{
				CreateFilename(folder, e.Name);
			}
			foreach (Entity e in db.Entities)
			{
				CurrentEntity = e.Name;
				string filename = CreateFilename(folder, e.Name);
				SpssDataDocument doc = SpssDataDocument.Create(filename);

				CreateIdVariables(parent, e, doc);
				CreateMetaData(e.Variables, doc);
				try
				{
					CreateData(e, parent, doc);
				}
				catch
				{
					doc.Close();
					throw;
				}
				parent = e;
			}
		}

		private void CreateData(Entity e, Entity parentEntity, SpssDataDocument doc)
		{
			// calcula columnas de ids
			string id = MakeId(e);
			string parentId = MakeId(parentEntity);
			// abre los archivos para todas las variables
			OpenVariablesData(e);
			//
			this.CurrentEntityTotal = e.GetPointerRows();

			int currentParent = -1;
			int parentLimit = int.MinValue;
			int n = 1;
			EntityCurrentRow = 0;
			// pone valores
			while (HasData(e))
			{
				SpssCase case1 = doc.Cases.New();
				// pone su id
				case1[id] = n;
				//ResolveParentId(parentEntity, parentId, ref currentParent, ref parentLimit, n, case1);
				ResolveParentId(e, parentId, ref currentParent, ref parentLimit, n, case1);
				CopyVariablesData(e, case1);
				case1.Commit();
				GlobalCurrentRow++;
				EntityCurrentRow++;

				if (GlobalCurrentRow % 43 == 0)
				{
					CallBack(this, null);
					if (Cancelled)
					{
						CloseVariablesData(e, parentEntity);
						throw new Exception("Cancelled.");
					}
				}
				n++;
			}

			CloseVariablesData(e, parentEntity);
		}

		private bool HasData(Entity e)
		{
			// pone el valor de cada variable
			foreach (Variable v in e.Variables)
			{
				if (v.HasData() == false)
					return false;
				else
					return true;
			}
			return false;
		}

		private static void CopyVariablesData(Entity e, SpssCase case1)
		{
			// pone el valor de cada variable
			foreach (Variable v in e.Variables)
			{
				case1[v.Name] = v.GetData();
			}
		}

		private static void ResolveParentId(Entity parentEntity, string parentId, ref int currentParent, ref int parentLimit, int n, SpssCase case1)
		{
			if (parentId != "")
			{
				while (n > parentLimit)
				{
					parentLimit = parentEntity.GetPointerData();
					currentParent++;
				}
				case1[parentId] = currentParent;
			}
		}

		private static void OpenVariablesData(Entity e)
		{
			e.OpenPointer();
			foreach (Variable v in e.Variables)
				v.OpenData();
		}
		private static void CloseVariablesData(Entity e, Entity parentEntity)
		{
			e.ClosePointer();
			foreach (Variable v in e.Variables)
				v.CloseData();
			if (parentEntity != null)
			{
				// inicializa el punter del padre
				parentEntity.ClosePointer();
			}
		}

		private void CreateIdVariables(Entity parent, Entity e, SpssDataDocument doc)
		{
			var var = CreateVariable(new Variable(MakeId(e), "INTEGER", "Identifier for " + e.Name));
			doc.Variables.Add(var);
			if (parent != null)
			{
				var = CreateVariable(new Variable(MakeId(parent), "INTEGER", "Parent entity Id"));
				doc.Variables.Add(var);
			}
		}

		private static string MakeId(Entity e)
		{
			if (e == null)
				return "";
			else
				return e.Name + "_REF_ID";
		}

		private string CreateFilename(string folder, string file)
		{
			string ret = Path.Combine(folder, file) + ".sav";
			if (File.Exists(ret)) File.Delete(ret);
			return ret;
		}
		private void CreateMetaData(List<Variable> list, SpssDataDocument doc)
		{
			foreach (Variable v in list)
			{
				SpssVariable var = CreateVariable(v);
				doc.Variables.Add(var);
			}
			doc.CommitDictionary();
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
	}
}

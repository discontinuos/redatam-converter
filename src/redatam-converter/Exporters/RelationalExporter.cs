using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RedatamLib;


namespace RedatamConverter
{
	abstract class RelationalExporter<T>: IExporter
	{
		RedatamDatabase db;
		
		public EventHandler CallBack { get; set; }


		public string CurrentEntity { get; set; }
		public long CurrentEntityTotal { get; set; }
		public long EntityCurrentRow { get; set; }
		public long GlobalCurrentRow { get; set; }
		public long GlobalDataItemsCurrent { get; set; }
		public bool Cancelled { get; set; }

		public string Folder;
		protected abstract void WriteVariablesData(T doc, Dictionary<string, object> data);
		protected abstract void BeginDataWrite(T doc);
		protected abstract void CreateVariable(T doc, Variable v);
		
		protected abstract void CloseTable(T table);
		protected abstract T CreateTable(string folder, Entity e);

		public RelationalExporter(RedatamDatabase dbr)
		{
			db = dbr;
		}
		

		public void SaveAs(string folder)
		{
			Folder = folder;
			// crea archivos para cada entidad
			ExportLevel(null, db.Entities);
		}

		private void ExportLevel(Entity parent, List<Entity> entities)
		{
			foreach (Entity e in entities)
			{
				CurrentEntity = e.Name;
				T doc = CreateTable(this.Folder, e);

				CreateIdVariables(parent, e, doc);
				CreateDataVariables(e.SelectedVariables, doc);
				BeginDataWrite(doc);
				try
				{
					CreateData(e, parent, doc);
				}
				finally
				{
					CloseTable(doc);
				}
				ExportLevel(e, e.Children);
			}
		}



		private void CreateData(Entity e, Entity parentEntity, T doc)
		{
			// calcula columnas de ids
			string id = MakeId(e);
			string parentId = MakeId(parentEntity);
			// abre los archivos para todas las variables
			OpenVariablesData(e);
			//
			this.CurrentEntityTotal = e.CalculateRowCount(parentEntity);

			int currentParent = -1;
			int parentLimit = int.MinValue;
			int n = 1;
			EntityCurrentRow = 0;
			// pone valores
			int exportVariablesCount = e.SelectedVariables.Count + 2;
			while (n <= this.CurrentEntityTotal)
			{
				AdvanceParentId(e, parentId, ref currentParent, ref parentLimit, n);

				var data = PrepareVariablesData(id, parentId, e, n, currentParent);

				WriteVariablesData(doc, data);

				GlobalCurrentRow++;
				EntityCurrentRow++;

				GlobalDataItemsCurrent += exportVariablesCount;

				if (GlobalCurrentRow % 143 == 0 || n == this.CurrentEntityTotal)
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

		protected Dictionary<string, object> PrepareVariablesData(string id, string parentId, Entity e, int n, int currentParent)
		{
			Dictionary<string, object> ret = new Dictionary<string, object>();
			// pone su id
			ret[id] = n;
			if (parentId != "")
				ret[parentId] = currentParent;

			// pone el valor de cada variable
			foreach (Variable v in e.SelectedVariables)
			{
				ret[v.Name] = v.GetData();
			}
			return ret;
		}
		protected void CreateIdVariables(Entity parent, Entity e, T doc)
		{
			CreateVariable(doc, new Variable(MakeId(e), "INTEGER", "Identifier for " + e.Name));
			if (parent != null)
				CreateVariable(doc, new Variable(MakeId(parent), "INTEGER", "Parent entity Id"));
		}
		protected void CreateDataVariables(IList<Variable> list, T doc)
		{
			foreach (Variable v in list)
				CreateVariable(doc, v);
		}

		protected string MakeId(Entity e)
		{
			if (e == null)
				return "";
			else
				return e.Name + "_REF_ID";
		}

		private static void AdvanceParentId(Entity parentEntity, string parentId, ref int currentParent, ref int parentLimit, int n)
		{
			if (parentId != "")
			{
				while (n > parentLimit)
				{
					parentLimit = parentEntity.GetPointerData();
					currentParent++;
				}
			}
		}

		private static void OpenVariablesData(Entity e)
		{
			e.OpenPointer();
			foreach (Variable v in e.SelectedVariables)
				v.OpenData();
		}
		private static void CloseVariablesData(Entity e, Entity parentEntity)
		{
			e.ClosePointer();
			foreach (Variable v in e.SelectedVariables)
				v.CloseData();
		}


		protected string CreateFilename(string folder, string file, string extension)
		{
			string ret = Path.Combine(folder, file) + "." + extension;
			if (File.Exists(ret)) File.Delete(ret);
			return ret;
		}

		public string[] GetInvalidSizes()
		{
			List<string> ret = new List<string>();
			CheckEntityFileSizes(ret, db.Entities, null);
			return ret.ToArray();
		}

		private void CheckEntityFileSizes(List<string> ret, List<Entity> entities, Entity parentEntity)
		{
			foreach (Entity e in entities)
			{
				if (e.DataFileExists())
				{
					OpenVariablesData(e);
					e.CalculateRowCount(parentEntity);
					foreach (Variable v in e.SelectedVariables)
					{
						long expected, actual;
						if (v.FileSizeFails(out expected, out actual))
						{
							string errorText = v.Filename.Replace("'", "") + ": entity '" + e.Name
																	+ "', variable '" + v.Name + "'. Expected: " + expected + " bytes (Actual: " + actual + " bytes).";
							ret.Add(errorText);
						}
					}
					CheckEntityFileSizes(ret, e.Children, e);
					CloseVariablesData(e, parentEntity);
				}
			}
		}

		public string[] GetMissingFiles()
		{
			List<string> ret = new List<string>();
			CheckEntityFiles(ret, db.Entities);
			return ret.ToArray();
		}

		private void CheckEntityFiles(List<string> ret, List<Entity> entities)
		{
			foreach (Entity e in entities)
			{
				foreach (Variable v in e.SelectedVariables)
				{
					if (v.DataFileExists() == false)
					{
						string errorText = v.Filename.Replace("'", "") + ": entity '" + e.Name
																+ "', variable '" + v.Name + "'.";
						ret.Add(errorText);
					}
				}
				CheckEntityFiles(ret, e.Children);
			}
		}
	}
}

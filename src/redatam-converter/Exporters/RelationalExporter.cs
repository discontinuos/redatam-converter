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
		public long GlobalCurrentRow { get; set; }
		public long EntityCurrentRow { get; set; }
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
			Entity parent = null;
			foreach (Entity e in db.Entities)
			{
				CurrentEntity = e.Name;
				T doc = CreateTable(folder, e);

				CreateIdVariables(parent, e, doc);
				CreateDataVariables(e.Variables, doc);
				BeginDataWrite(doc);
				try
				{
					CreateData(e, parent, doc);
				}
				finally
				{
					CloseTable(doc);
				}
				parent = e;
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
			this.CurrentEntityTotal = e.GetPointerRows();

			int currentParent = -1;
			int parentLimit = int.MinValue;
			int n = 1;
			EntityCurrentRow = 0;
			// pone valores
			while (n <= this.CurrentEntityTotal)
			{
				AdvanceParentId(e, parentId, ref currentParent, ref parentLimit, n);

				var data = PrepareVariablesData(id, parentId, e, n, currentParent);

				WriteVariablesData(doc, data);

				GlobalCurrentRow++;
				EntityCurrentRow++;

				if (GlobalCurrentRow % 43 == 0 || n == this.CurrentEntityTotal)
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
			foreach (Variable v in e.Variables)
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
		protected void CreateDataVariables(List<Variable> list, T doc)
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


		protected string CreateFilename(string folder, string file, string extension)
		{
			string ret = Path.Combine(folder, file) + "." + extension;
			if (File.Exists(ret)) File.Delete(ret);
			return ret;
		}

		public string[] GetMissingFiles()
		{
			List<string> ret = new List<string>();
			foreach (Entity e in db.Entities)
			{
				foreach (Variable v in e.Variables)
				{
					if (v.DataFileExists() == false)
					{
						string errorText = v.Filename.Replace("'", "") + ": entity '" + e.Name
																+ "', variable '" + v.Name + "'.";
						ret.Add(errorText);
					}
				}
			}
			return ret.ToArray();
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Spss;
using System.Runtime.InteropServices;


namespace RedatamConverter
{
	abstract class RelationalExporter<T>
	{
		
		RedatamDatabase db;
		public int GlobalCurrentRow = 0;
		
		public EventHandler CallBack;

		public string CurrentEntity;
		public long CurrentEntityTotal;
		public long EntityCurrentRow;
		public bool Cancelled;
		public string Folder;
		protected abstract void CopyVariablesData(T doc, string id, string parentId, Entity e, int n, int currentParent);
		protected abstract void CreateMetaData(List<Variable> list, T doc);
		protected abstract void CreateIdVariables(Entity parent, Entity e, T doc);
		protected abstract void CloseTable(T table);
		protected abstract T CreateTable(string folder, Entity e);
		protected abstract string MakeId(Entity e);

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
				CreateFilename(folder, e.Name);
			}
			foreach (Entity e in db.Entities)
			{
				CurrentEntity = e.Name;
				T doc = CreateTable(folder, e);

				CreateIdVariables(parent, e, doc);
				CreateMetaData(e.Variables, doc);
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

				CopyVariablesData(doc, id, parentId, e, n, currentParent);

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
					if (CheckSpace() == false)
						throw new Exception("Disk full (less than 1MB free). Not enough space to complete the export.");
				}
				n++;
			}

			CloseVariablesData(e, parentEntity);
		}

		private bool CheckSpace()
		{
			ulong FreeBytesAvailable;
			ulong TotalNumberOfBytes;
			ulong TotalNumberOfFreeBytes;
			int n = Folder.IndexOf("\\");
			if (n == -1)
				n = Folder.IndexOf("/");
			if (n == -1) return true;
			string drive = Folder.Substring(0, n);
			bool success = RedatamDatabase.GetDiskFreeSpaceEx(drive, out FreeBytesAvailable, out TotalNumberOfBytes,
									 out TotalNumberOfFreeBytes);
			return (FreeBytesAvailable > 1024 * 1024);
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


		protected string CreateFilename(string folder, string file)
		{
			string ret = Path.Combine(folder, file) + ".sav";
			if (File.Exists(ret)) File.Delete(ret);
			return ret;
		}
	
	}
}

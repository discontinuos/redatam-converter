using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedatamLib
{
	public class Entity
	{
		ICursorReader reader;

		public string rootPath;
		public string Name;
		public List<Entity> Children = new List<Entity>();
		public string Description;
		public string IndexFilename;
		public int i1;
		public string Alias;
		public int s1;
		public byte b1;
		public string RelationChild;
		public string RelationParent;
		public string CodesVariable;
		public string LabelVariable;
		public int Level;
		public int VariableCount;
		public string c1;
		public readonly List<Variable> Variables = new List<Variable>();


		public override string ToString()
		{
			return this.Name;
		}

		public void OpenPointer()
		{
			//ubica el nombre de archivo y el tamaño de variable
			if (this.IndexFilename != "")
			{
				string file = Path.Combine(this.rootPath, this.IndexFilename);
				reader = new CursorReader(file, false, 16);
			}
			else
			{
				reader = new NullCursorReader();
			}
			reader.Open();
		}

		public bool HasData()
		{
			// se fija si el puntero tiene más datos
			return reader.IsLastPos() == false;
		}

		public int GetPointerData()
		{
			return (int) reader.ReadInt32();
		}
		
		public void ClosePointer()
		{
			reader.Close();
		}

		public long GetPointerRows()
		{
			if (reader.Length == 0)
				return 0;
			else
				return reader.ReadLastInt32();
		}	
	}
}

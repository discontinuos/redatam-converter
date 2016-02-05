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
		public long RowsCount;
		public int VariableCount;
		public string c1;
		public readonly List<Variable> Variables = new List<Variable>();


		public override string ToString()
		{
			return this.Name;
		}

		public IList<Variable> SelectedVariables
		{
			get
			{
				List<Variable> selected = new List<Variable>();
				foreach (Variable v in Variables)
					if (v.Selected)
						selected.Add(v);
				return selected;
			}
		}

		public void OpenPointer()
		{
			//ubica el nombre de archivo y el tamaño de variable
			if (this.IndexFilename != "")
			{
				string file = RedatamDatabase.OptimisticCombine(this.rootPath, this.IndexFilename);
				reader = new CursorReader(file, 16);
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
			return (int)reader.ReadInt32();
		}

		public void ClosePointer()
		{
			reader.Close();
		}

		public long CalculateRowCount(Entity parentEntity)
		{
			long ret = 0;
			if (reader.Length > 0)
			{
				long pos = 1;
				if (parentEntity != null)
				{
					if (parentEntity.RowsCount > 0)
						pos = parentEntity.RowsCount;
				}
				ret = reader.ReadInt32At(pos);
			}
			this.RowsCount = ret;
			return ret;
		}

		internal static List<Tuple<string, string>> Linealize(Entity parent, List<Entity> entitiesNames)
		{
			var ret = new List<Tuple<string, string>>();
			foreach (Entity e in entitiesNames)
			{
				if (parent == null)
					ret.Add(new Tuple<string, string>("", e.Name));
				else
					ret.Add(new Tuple<string, string>(parent.Name, e.Name));
				var children = Linealize(e, e.Children);
				if (children.Count > 0)
					ret.AddRange(children);
			}
			return ret;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace RedatamLib
{
	public class RedatamDatabase
	{
		public List<Entity> entityNames = new List<Entity>();
		public readonly List<Entity> Entities = new List<Entity>();
		public FuzzyEntityParser FuzzyEntityParser;
		public XmlEntityParser XmlEntityParser;
		public string DictionaryFile; 

		public RedatamDatabase()
		{
			FuzzyEntityParser = new FuzzyEntityParser(this);
			XmlEntityParser = new XmlEntityParser(this);
		}


		public long GetTotalDataItems()
		{
			long ret = 0;
			ret += GetEntitiesTotalDataItems(Entities, null);
			return ret;
		}

		private long GetEntitiesTotalDataItems(List<Entity> entities, Entity parent)
		{
			long ret = 0;
			foreach (var entity in entities)
			{
				ret += (entity.SelectedVariables.Count + 2) * entity.RowsCount;

				ret += GetEntitiesTotalDataItems(entity.Children, entity);
			}
			return ret;
		}

		public long GetTotalRowsSize()
		{
			long ret = 0;
			ret += GetEntitiesRowsSize(Entities, null);
			return ret;
		}

		private long GetEntitiesRowsSize(List<Entity> entities, Entity parent)
		{
			long ret = 0;
			foreach (var entity in entities)
			{
				entity.OpenPointer();
				long entityCount = entity.CalculateRowCount(parent);
				ret += entityCount;
				entity.ClosePointer();

				ret += GetEntitiesRowsSize(entity.Children, entity);
			}
			return ret;
		}

		public static string OptimisticCombine(string path, string file)
		{
				string filefull = Path.Combine(path, file);
				if (File.Exists(filefull)) return filefull;
				string fOnly = Path.GetFileName(file);
				filefull = Path.Combine(path, fOnly);
				return filefull;
		}

		public void OpenDictionary(string file)
		{
			try
			{
				var ext = Path.GetExtension(file).ToLower();
				if (ext == ".dic")
				{
					this.FuzzyEntityParser.ParseEntities(file);
					// Parse de entidades y variables
					try
					{
						EntityParser parser = new EntityParser(this);
						parser.Parse(file);
					}
					catch (Exception e)
					{
						throw new Exception("An error ocurred while parsing the dictionary variables and labels (" + e.Message + ").");
					}
				}
				else if (ext == ".dicx")
					this.XmlEntityParser.Parse(file);
				else
					throw new Exception("Dictionary files must by .dic or .dicx files.");
			}
			catch (Exception e)
			{
				throw new Exception("An error ocurred while discovering the dictionary entities (" + e.Message + ").");
			}
	
		}

		public List<Entity> GetEntitiesList()
		{
			List<Entity> ret = new List<Entity>();
			doGetEntitiesList(ret, this.Entities);
			return ret;
		}

		private void doGetEntitiesList(List<Entity> ret, List<Entity> list)
		{
			foreach (Entity e in list)
			{
				ret.Add(e);
				doGetEntitiesList(ret, e.Children);
			}
		}

	}
}

﻿using System;
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
		public string DictionaryFile; 

		public RedatamDatabase()
		{
			FuzzyEntityParser = new FuzzyEntityParser(this);
		}
		

		public long GetTotalRowsSize()
		{
			long ret = 0;
			ret += GetEntitiesRowsSize(Entities);
			return ret;
		}

		private long GetEntitiesRowsSize(List<Entity> entities)
		{
			long ret = 0;
			foreach (var entity in entities)
			{
				entity.OpenPointer();
				ret += entity.GetPointerRows();
				entity.ClosePointer();

				ret += GetEntitiesRowsSize(entity.Children);
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
				this.FuzzyEntityParser.ParseEntities(file);
			}
			catch (Exception e)
			{
				throw new Exception("An error ocurred while discovering the dictionary entities (" + e.Message + ").");
			}
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
	}
}

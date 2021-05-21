using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace RedatamLib
{
	public class XmlEntityParser
	{
		RedatamDatabase db;
		string rootPath;

		private static string[] validTypes = { "DBL", "LNG", "INT", "BIN", "PCK", "CHR" };

		public XmlEntityParser(RedatamDatabase db)
		{
			this.db = db;
		}

		public void Parse(string path)
		{
			rootPath = Path.GetDirectoryName(path);
			XmlDocument xmldoc = new XmlDocument();
      FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
      xmldoc.Load(fs);
			db.Entities.AddRange(ParseEntities(xmldoc));
		}

		public List<Entity> ParseEntities(XmlDocument doc)
		{
			List<Entity> ret = new List<Entity>();
			var root = getChildByName(getChildByName(doc, "redDictionary_XML"), "root");
			ret.Add(ReadEntity(root));
			return ret;
		}

		public static XmlNode getChildByName(XmlNode node, string name)
		{
			foreach (XmlNode n in node.ChildNodes)
				if (n.Name == name)
					return n;
			throw new Exception("Node not found: " + name);
		}
		
		
		public static bool hasChildByName(XmlNode node, string name)
		{
			foreach (XmlNode n in node.ChildNodes)
				if (n.Name == name)
					return true;
			return false;
		}

		public static List<XmlNode> getChildrenByName(XmlNode node, string name)
		{
			List<XmlNode> ret = new List<XmlNode>();
			foreach (XmlNode n in node.ChildNodes)
			{
				if (n.Name == name)
					ret.Add(n);
			}
			return ret;
		}

		public Entity ReadEntity(XmlNode node)
		{
			var e = new Entity();
			e.rootPath = rootPath;
			// lee la entidad
			e.Name = getChildByName(node, "name").InnerText;
			if (hasChildByName(node, "label"))
				e.Description = getChildByName(node, "label").InnerText;
			e.IndexFilename = getChildByName(node, "filename").InnerText;
			if (hasChildByName(node, "refCode"))
				e.CodesVariable = getChildByName(node, "refCode").InnerText;
			if (hasChildByName(node, "refLabel"))
				e.LabelVariable =getChildByName(node, "refLabel").InnerText;
			// e.Level
			ReadVariables(node, e);
			e.VariableCount = e.Variables.Count;

			foreach (XmlNode childXml in getChildrenByName(node, "entity"))
			{
				Entity child = ReadEntity(childXml);
				e.Children.Add(child);
			}
			return e;
		}

		private void ReadVariables(XmlNode node, Entity e)
		{
			foreach (XmlNode variable in getChildrenByName(node, "variable"))
			{
				// lee las variables
				Variable v = new Variable(e);

				v.Name = getChildByName(variable, "name").InnerText;
				if (hasChildByName(variable, "filter"))
					v.Filter = getChildByName(variable, "filter").InnerText;
				v.Range = readRange(variable);
				if (hasChildByName(variable, "varType"))
					v.Type = getChildByName(variable, "varType").InnerText;
				else
					v.Type = "INTEGER";
				v.Label = getChildByName(variable, "label").InnerText;
				if (hasChildByName(variable, "group"))
					v.Group =  getChildByName(variable, "group").InnerText;

				var parser = new XmlVariableParser(v);
				parser.ParseDeclaration(variable);
				parser.ParseValueLabels(variable);
			//	parser.ParseMissingAndPrecision(variable);

				e.Variables.Add(v);
			}
		}

		private string readRange(XmlNode variable)
		{
			if (hasChildByName(variable, "range"))
			{
				var range = getChildByName(variable, "range");
				var min = getChildByName(range, "rangeMin");
				var max = getChildByName(range, "rangeMax");
				return min.InnerText + "-" + max.InnerText;
			}
			else
				return "";
		}
	}
}

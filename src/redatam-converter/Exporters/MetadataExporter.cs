using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RedatamLib;
using System.Xml;
using System.Xml.Linq;

namespace RedatamConverter
{
	class MetadataExporter
	{
		RedatamDatabase db;
		XmlDocument doc;

		public MetadataExporter(RedatamDatabase dbr)
		{
			db = dbr;
		}


		public void Save(string filename)
		{
			// exporta la metadata
			doc = new XmlDocument();
			List<XObject> content = GetEntitiesNodes(db.Entities);
			XElement element = new XElement("database", content);
			File.WriteAllText(filename, element.ToString());
		}

		private List<XObject> GetEntitiesNodes(List<Entity> list)
		{
			List<XObject> ret = new List<XObject>();
			foreach (Entity entity in list)
			{
				XAttribute name = new XAttribute("name", entity.Name);
				XAttribute index = new XAttribute("pointer", entity.IndexFilename);
				List<XObject> variables = GetVariablesNodes(entity);
				List<XObject> subEntities = GetEntitiesNodes(entity.Children);

				List<object> content = new List<object>() { name, index };
				if (variables.Count > 0)
					content.Add(variables);
				if (subEntities.Count > 0)
					content.Add(subEntities);

				XElement element = new XElement("entity", content);
				ret.Add(element);
			}
			return ret;
		}

		private List<XObject> GetVariablesNodes(Entity entity)
		{
			List<XObject> ret = new List<XObject>();
			foreach (Variable variable in entity.Variables)
			{
				List<object> content = new List<object>();
				content.Add(new XAttribute("name", variable.Name));
				content.Add(new XAttribute("label", variable.Label));
				content.Add(new XAttribute("type", variable.Type));
				content.Add(new XAttribute("size", variable.Size));
				content.Add(new XAttribute("decimals", variable.Decimals));
				content.Add(new XAttribute("range", variable.Range));
				content.Add(new XAttribute("filter", variable.Filter));
				content.Add(new XAttribute("data", variable.Filename));
				if (variable.ValueLabels.Count > 0)
					AddValueLabels(content, variable);
				XElement element = new XElement("variable", content);
				ret.Add(element);
			}
			return ret;
		}

		private void AddValueLabels(List<object> content, Variable variable)
		{
			List<XElement> list = new List<XElement>();
			foreach (var item in variable.ValueLabels)
			{
				list.Add(new XElement("valueLabel", new XAttribute("name", item.Key), new XAttribute("value", item.Value)));
			}
			XElement ele = new XElement("valueLabels", list);
			content.Add(ele);
		}

	}
}

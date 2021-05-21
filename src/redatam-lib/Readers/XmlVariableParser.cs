using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace RedatamLib
{
	public class XmlVariableParser
	{
		Variable variable;

		public XmlVariableParser(Variable variable)
		{
			this.variable = variable;
		}

		public void ParseValueLabels(XmlNode variable)
		{
			this.variable.ValueLabels.Clear();
			if (XmlEntityParser.hasChildByName(variable, "valueLabels"))
			{
				var values = XmlEntityParser.getChildByName(variable, "valueLabels");
				foreach (XmlNode ele in values.ChildNodes)
				{
					var value = XmlEntityParser.getChildByName(ele, "value");
					var label = XmlEntityParser.getChildByName(ele, "label");
					this.variable.ValueLabels.Add(new ValueLabel(value.InnerText, label.InnerText));
				}
			}
		}

		private void ParseDecimals(string extras)
		{
			if (extras.StartsWith("DECIMALS "))
			{
				string label = eatStringFromString(ref extras);
				string value = eatStringFromString(ref extras);
				this.variable.Decimals = int.Parse(value);
			}
		}

		private string GetMissingLabel(string extras, string tag)
		{
			if (extras.StartsWith(tag))
			{
				string label = eatStringFromString(ref extras);
				string value = eatStringFromString(ref extras);
				this.variable.ValueLabels.Add(new ValueLabel(
					value,
					label));
			}
			return extras;
		}

		private string eatStringFromString(ref string extras)
		{
			if (extras.StartsWith("'"))
			{
				int nEnd = extras.IndexOf("'", 1);
				if (nEnd != -1)
				{
					string retQ = extras.Substring(1, nEnd - 1);
					extras = extras.Substring(nEnd + 1).TrimStart();
					return retQ;
				}
			}

			int n = extras.IndexOf(" ");
			if (n == -1) n = extras.Length;
			string ret = extras.Substring(0, n);
			if (n < extras.Length)
				extras = extras.Substring(n + 1);
			else
				extras = "";
			return ret;
		}
		public override string ToString()
		{
			return this.variable.Name;
		}
		
		public void ParseDeclaration(XmlNode node)
		{
			var choice = XmlEntityParser.getChildByName(node, "varDicChoice");
			var typeNode = XmlEntityParser.getChildByName(choice, "datasetType");
			var type = typeNode.InnerText;
			var sizeNode = XmlEntityParser.getChildByName(choice, "datasetSize");
			string size = sizeNode.InnerText;

			if (this.variable.Type == "STRING" && type != "CHR")
				throw new Exception("Inconsistent type declaration");
			if (this.variable.Type == "REAL" && type != "DBL")
				throw new Exception("Inconsistent type declaration");
			switch (type)
			{
				// si se agregan tipos nuevos, incorporarlos en 
				// entityParser => validTypes
				case "DBL":
					this.variable.Size = 64;
					break;
				case "LNG":
					this.variable.Size = 32;
					break;
				case "INT":
					this.variable.Size = 16;
					this.variable.Type = "INT16";
					break;
				case "BIN":
					this.variable.Size = int.Parse(size);
					this.variable.BinaryDataSet = true;
					break;
				case "PCK":
				case "CHR":
					this.variable.Size = int.Parse(size);
					break;
				default:
					throw new Exception("Data type '" + type + "' is not supported. Contact idiscontinuos for support.");
			}
			var fileNode = XmlEntityParser.getChildByName(node, "filename");
			
			this.variable.Filename = fileNode.InnerText;
		}
		/*
		public void ParseMissingAndPrecision()
		{
			// se fija si hay missing y na
			string extras = this.variable.Group.Trim();
			extras = GetMissingLabel(extras, "MISSING");
			extras = GetMissingLabel(extras, "NOTAPPLICABLE");
			ParseDecimals(extras);
		}*/
	}
}

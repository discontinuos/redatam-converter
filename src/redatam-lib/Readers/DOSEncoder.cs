using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace RedatamLib
{
	public class MsDOSEncoder
	{
		// Detecta archivos generados en MsDOS
		// (al menos 6 caracteres acentuados en el rango 160 130 162 163 164 de al menos 3 tipos)
		// (int) { char }
		// 
		List<int> oldChars = new List<int> { 160, 130, 162, 163, 164 };
		RedatamDatabase db;
		Dictionary<char, int> matches;
		int sumMatches;
		bool testing;

		public MsDOSEncoder(RedatamDatabase db)
		{
			this.db = db;
		}

		public void ReencodeLabels()
		{
			testing = false;
			ProcessEntities(db.Entities);
		}	

		public bool RequiresProcessing()
		{
			testing = true;
			matches = new Dictionary<char, int>();
			sumMatches = 0;
			return ProcessEntities(db.Entities);
		}

		private bool ProcessEntities(List<Entity> entities)
		{
			foreach (var entity in entities)
			{
				foreach (var variable in entity.Variables)
				{
					Process(ref variable.Label);
					foreach (var value in variable.ValueLabels)
					{
						Process(ref value.Value);
					}
				}
				if (ProcessEntities(entity.Children))
				{
					if (testing)
						return true;
				}
			}
			return (matches.Count >= 3 && sumMatches >= 6);
		}

		private void Process(ref string value)
		{
			if (testing)
			{
				CountMsDOSChars(value);
			}
			else
			{
				value = ReplaceMsDOSChars(value);
			}
		}

		private static string ReplaceMsDOSChars(string value)
		{
			byte[] text = System.Text.Encoding.Default.GetBytes(value);
			value = System.Text.Encoding.GetEncoding(850).GetString(text);
			return value;
		}

		private void CountMsDOSChars(string value)
		{
			for (var n = 0; n < value.Length; n++)
			{
				char c = value[n];
				if (oldChars.Contains((int)c))
				{
					int existingCount = 0;
					matches.TryGetValue(c, out existingCount);
					matches[c] = existingCount + 1;
					sumMatches++;
				}
			}
		}
	}
}

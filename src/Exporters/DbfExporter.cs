using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Spss;
using db;
using System.Data;

namespace RedatamConverter
{
	// EN CONSTRUCCION
	class DbfExporter
	{
		private static List<string> DataSetToLines(DataSet ds)
		{
			List<string> lineas = new List<string>();
			for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
				lineas.Add(ds.Tables[0].Rows[n]["linea"] as string);
			return lineas;
		}

		private static void LinesToDbf(List<string> lineas)
		{
			string dbfPath = Path.GetTempPath() + "\\" + new Random().Next().ToString() + ".dbf";

			if (File.Exists(dbfPath))
				File.Delete(dbfPath);

			xdbf dbf = null;
			dbf = new xdbf();

			string columnsString = /*@"varname c(3) , varname n(5,0), " +
                                        "varname n(9,2) 
                                        "varname d(8)";*/
											lineas[0];
			dbf.creaTable(dbfPath, columnsString);
			// identifica las columnas....
			string[] columns = columnsString.Substring(0, columnsString.LastIndexOf("(")).Split('(');
			// limpia...
			for (int n = 0; n < columns.Length; n++)
				columns[n] = columns[n].Substring(0, columns[n].LastIndexOf(' '));

			for (int i = 1; i < lineas.Count; i++)
			{
				string[] value = lineas[i].Split(',');
				dbf.AppendBlank();
				for (int j = 0; j < columns.Length; j++)
				{
					dbf.Replace(columns[j], value[j]);
				}
			}

			dbf.Close();
		}
	}
}
	
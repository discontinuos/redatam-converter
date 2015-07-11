using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Spss;

namespace RedatamConverter
{
	public interface IExporter
	{
		EventHandler CallBack { get; set; }
		void SaveAs(string folder);

		string CurrentEntity { get; set; }
		long CurrentEntityTotal { get; set; }
		long GlobalCurrentRow { get; set; }
		long EntityCurrentRow { get; set; }
		bool Cancelled { get; set; }
		string[] GetMissingFiles();
			

	}
}

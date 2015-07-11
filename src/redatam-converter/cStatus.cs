using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RedatamConverter
{
	public partial class cStatus : UserControl
	{
		public event EventHandler CancelClick;

		public cStatus()
		{
			InitializeComponent();
		}

		internal void InitializeProgress(string file, string folder)
		{
			lblDict.Text = file;
			lblFolder.Text = folder;

			lblEntity.Text = "";
			lblRows.Text = "";
			lblTotals.Text = "";
			lblEllapsed.Text = "";
			lblRemaining.Text = "";
			progress.Value = 0;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (CancelClick != null)
				CancelClick(this, null);
		}
	}
}

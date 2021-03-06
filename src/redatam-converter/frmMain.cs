﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using RedatamLib;

namespace RedatamConverter
{

	public partial class frmMain : Form
	{
		RedatamDatabase db;
		Type exporterType;
		long totalRows;
		long globalDataItemsCount;
		DateTime startTime;
		string folder;
		Exception exception;
		List<string> entities = new List<string>();
		List<string> skipColumns = new List<string>() { "ValuesLabelsRaw", "ValueLabels", "Selected" };
		bool cancelled = false;
		cStatus status = new cStatus();
		
		string testsFolder {
			get 
			{
				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				if (path.EndsWith("Debug")) path = Path.GetDirectoryName(path);
				if (path.EndsWith("bin")) path = Path.GetDirectoryName(path);
				return Path.Combine(path, "tests");
			}
		}
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable,		out ulong lpTotalNumberOfBytes,		out ulong lpTotalNumberOfFreeBytes);


		public frmMain()
		{
			InitializeComponent();
			status = new cStatus();
			this.Controls.Add(status);
			status.Visible = false;
			status.CancelClick += new EventHandler(status_CancelClick);
			var v = Assembly.GetExecutingAssembly().GetName().Version;
			Text += " " + v.Major + "." + v.Minor;

			#if DEBUG
				btnTest.Visible = true;
				btnRegenTest.Visible = true;
			#else
				btnTest.Visible = false;
				btnRegenTest.Visible = false;
			#endif

			ProcessCommandLineArguments();
		}

		private void ProcessCommandLineArguments()
		{
			if (Environment.GetCommandLineArgs().Length < 2) return;
			string cmd = Environment.GetCommandLineArgs()[1].Trim();
			if (cmd.Length > 0)
			{
				if (cmd.StartsWith("\"")) cmd = cmd.Substring(1);
				if (cmd.EndsWith("\"")) cmd = cmd.Substring(0, cmd.Length - 1);
				if (File.Exists(cmd) == false)
					MessageBox.Show(this, "Could not find file: " + cmd, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				else
				{
					lblFile.Text = cmd;
					ProcessDictionary(lblFile.Text);
				}
			}
		}


		private void btnOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.Filter = "REDATAM dictionaries (.dic, .dicx)|*.dic;*.dicx|All Files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;

			openFileDialog1.Multiselect = true;

			DialogResult userClickedOK = openFileDialog1.ShowDialog(this);

			if (userClickedOK == System.Windows.Forms.DialogResult.OK)
			{
				folderTo.Text = "...";
				lblFile.Text = openFileDialog1.FileName;
				ProcessDictionary(lblFile.Text);
			}
		}

		private void ProcessDictionary(string file)
		{
			// Discovery de entidades
			db = new RedatamDatabase();
			lwEntities.Items.Clear();
			lwVariables.Items.Clear();
			lwLabels.Items.Clear();

			try
			{
				db.OpenDictionary(file);
				ValidateIntegrity();
			}
			catch (Exception e)
			{
				FillEntityNamesListView();
				MessageBox.Show(this, e.Message);
				return;
			}
			finally
			{
				FillEntitiesListView();
			}
			EnableSaveButtons(true);
		}

		private void ValidateIntegrity()
		{
			exporterType = typeof(CSVExporter);			
			checkFiles(false);
			checkFileSizes(false);
			exporterType = null;
			
		}

		private void EnableSaveButtons(bool value)
		{
			btnSaveSPSS.Enabled = value;
			btnSaveCSV.Enabled = value;
			btnExportMetadata.Enabled = value;
		}
		void status_CancelClick(object sender, EventArgs e)
		{
			cancelled = true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}


		private void FillEntityNamesListView()
		{
			lwEntities.Items.Clear();
			lwVariables.Items.Clear();
			lwLabels.Items.Clear();
			if (db == null) return;
			AddEntities(0, db.entityNames, true);
		}

		private void AddEntities(int level, List<Entity> entities, bool nullTag = false)
		{
			foreach (Entity entity in entities)
			{
				ListViewItem item = new ListViewItem();
				if (nullTag)
					item.Tag = null;
				else
					item.Tag = entity;
				item.Text = new String(' ', level * 2) + entity.Name;
				lwEntities.Items.Add(item);
				AddEntities(level + 1, entity.Children);
			}
		}

		private void FillEntitiesListView()
		{
			lwEntities.Items.Clear();
			lwVariables.Items.Clear();
			lwLabels.Items.Clear();
			if (db == null) return;
			AddEntities(0, db.Entities);
			SelectFirst(lwEntities);
		}

		private void lwEntities_SelectedIndexChanged(object sender, EventArgs e)
		{
			lwVariables.Items.Clear();
			lwLabels.Items.Clear();
			if (db == null) return;
			if (lwEntities.SelectedItems.Count == 0) return ;
			Entity entity = lwEntities.SelectedItems[0].Tag as Entity;
			if (entity == null)
				return;

			FillVariablesListView(entity);
			SelectFirst(lwVariables);
		}

		private void FillVariablesListView(Entity entity)
		{
			if (lwVariables.Columns.Count < 3)
			{
				CreateColumns();
			}
			foreach (Variable v in entity.Variables)
			{
				ListViewItem item = new ListViewItem();
				item.Tag = v;
				foreach (FieldInfo fi in typeof(Variable).GetFields(BindingFlags.Public | BindingFlags.Instance))
				{
					if (this.skipColumns.Contains(fi.Name) == false)
					{
						object val = fi.GetValue(v);
						string value;
						if (val != null)
							value = val.ToString();
						else
							value = "";
						if (item.Text == "")
							item.Text = value;
						else
							item.SubItems.Add(value);
					}
				}
				item.Checked = v.Selected;
				lwVariables.Items.Add(item);
			}
		}

		private void CreateColumns()
		{
			lwVariables.Columns.Clear();
			var cols = typeof(Variable).GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo fi in cols)
			{
				if (this.skipColumns.Contains(fi.Name) == false)
				{
					ColumnHeader ch = new ColumnHeader();
					ch.Tag = fi;
					ch.Text = fi.Name;
					ch.Width = 90;
					lwVariables.Columns.Add(ch);
				}
			}
		}

		private void lwVariables_SelectedIndexChanged(object sender, EventArgs e)
		{
			lwLabels.Items.Clear();

			if (db == null) return;
			if (lwVariables.SelectedItems.Count == 0) return;
			Variable variable = lwVariables.SelectedItems[0].Tag as Variable;
			foreach (var item in variable.ValueLabels)
			{
				ListViewItem i = new ListViewItem();
				i.Text = item.Key;
				i.SubItems.Add(item.Value);
				lwLabels.Items.Add(i);
			}
			SelectFirst(lwLabels);
		}

		private void SelectFirst(ListView lwLabels)
		{
			if (lwLabels.Items.Count > 0)
				lwLabels.SelectedIndices.Add(0);
		}

		public void updateCallback(object sender, EventArgs e)
		{
			if (this.InvokeRequired)
				this.Invoke(new EventHandler(updateProgress), sender, e);
		}
		public void updateProgress(object sender, EventArgs e)
		{
			// actualiza con estimado de tiempo y de filas...
			IExporter exporter = (IExporter)sender;
			status.lblEntity.Text = exporter.CurrentEntity;
			status.lblRows.Text = MakeStatusLine(exporter.CurrentEntityTotal, exporter.EntityCurrentRow); ;
			status.lblTotals.Text = MakeStatusLine(this.totalRows, exporter.GlobalCurrentRow);
			double currentProgress = (exporter.GlobalCurrentRow / (double) this.totalRows);

			//TimeSpan remaining = TimeSpan.FromSeconds(((DateTime.Now - this.startTime).TotalSeconds / exporter.GlobalCurrentRow * this.totalRows));
			TimeSpan ellapsed = (DateTime.Now - this.startTime);
			TimeSpan estimatedTotalTime = TimeSpan.FromSeconds(ellapsed.TotalSeconds / exporter.GlobalDataItemsCurrent * globalDataItemsCount);
			TimeSpan remaining = estimatedTotalTime - ellapsed;

			status.lblEllapsed.Text = FormatTimeSpan(ellapsed, true) + ".";
			status.lblRemaining.Text = FormatTimeSpan(remaining) + ".";
			status.progress.Value = (int)(100 * currentProgress);

			if (cancelled)
				exporter.Cancelled = true;
			else
			{
				while (CheckSpace(folderTo.Text) == false)
				{
					if (MessageBox.Show(this, "Disk full (less than 1MB free). Not enough space to complete the export.", "Error", MessageBoxButtons.RetryCancel)
						== System.Windows.Forms.DialogResult.Cancel)
					{
						exporter.Cancelled = true;
						break;
					}
				}
			}
		}

		private bool CheckSpace(string folder)
		{
			ulong FreeBytesAvailable;
			ulong TotalNumberOfBytes;
			ulong TotalNumberOfFreeBytes;
			int n = folder.IndexOf("\\");
			if (n == -1)
				n = folder.IndexOf("/");
			if (n == -1) return true;
			string drive = folder.Substring(0, n);
			bool success = GetDiskFreeSpaceEx(drive, out FreeBytesAvailable, out TotalNumberOfBytes,
									 out TotalNumberOfFreeBytes);
			return (FreeBytesAvailable > 1024 * 1024);
		}


		private string FormatTimeSpan(TimeSpan remaining, bool seconds = false)
		{
			TimeSpan show;
			if (remaining.TotalSeconds < 60)
				seconds = true;
			if (seconds)
				show = TimeSpan.FromSeconds((int)remaining.TotalSeconds);
			else
			{

				show = TimeSpan.FromMinutes(Math.Round(remaining.TotalMinutes, 0));
			}
			return ToReadableString(show);
		}

		public string ToReadableString(TimeSpan span)
		{
			string formatted = string.Format("{0}{1}{2}{3}",
					span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? String.Empty : "s") : string.Empty,
					span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? String.Empty : "s") : string.Empty,
					span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? String.Empty : "s") : string.Empty,
					span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? String.Empty : "s") : string.Empty);

			if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

			if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

			return formatted;
		}
		private string MakeStatusLine(long total, long current)
		{
			return string.Format("{0:N0}", current) + " / " + string.Format("{0:N0}", total) + " rows.";
		}
		public void go()
		{
			try
			{
				IExporter export = Activator.CreateInstance(exporterType, db) as IExporter;
				export.CallBack = new EventHandler(updateCallback);
				export.SaveAs(folder);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			finally
			{
				this.Invoke(new EventHandler(EndProgress));
			}
		}
		public void EndProgress(object o, EventArgs e)
		{
			if (exception != null)
				MessageBox.Show(this, exception.Message, "Ey!");
			else
			{
				MessageBox.Show(this, "The database was sucessfully exported.", "Done!");
				System.Diagnostics.Process.Start(folder);
			}
			status.Visible = false;
			EnableForm(true);
		}

		private void EnableForm(bool value)
		{
			foreach(Control c in this.Controls)
				if (c != status)
					c.Enabled = value;
		}
		private void InitializeProgress(string folder)
		{
			exception = null;
			totalRows = db.GetTotalRowsSize();
			globalDataItemsCount = db.GetTotalDataItems();
			status.InitializeProgress(lblFile.Text, folder);
			cancelled = false;
			startTime = DateTime.Now;
		}
		private void btnSaveData_Click(object sender, EventArgs e)
		{
			exporterType = typeof(SpssExporter);
			ShowSaveDialog("SPSS");
		}

		private void ShowSaveDialog(string format)
		{
			FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
			folderBrowserDialog1.Description = "Select the folder where " + format + " data files will be saved.";
			if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
			{
				folderTo.Text = folderBrowserDialog1.SelectedPath;
				if (checkFiles() == false)
					return;
				if (checkFileSizes() == false)
					return;
				SaveData(folderTo.Text);
			}
		}

		private bool checkFileSizes(bool askForAction = true)
		{
			IExporter export = Activator.CreateInstance(exporterType, db) as IExporter;
			export.CallBack = new EventHandler(updateCallback);
			string[] files = export.GetInvalidSizes();
			if (files.Length == 0)
				return true;
			else
			{
				var messageBoxButtons = MessageBoxButtons.OK;
				string question = "";
				if (askForAction)
				{
					question = "\n\nDo you want to export the data anyway? Missing data will be assigned zero (0) values.";
					messageBoxButtons = MessageBoxButtons.YesNoCancel;
				}
				return MessageBox.Show(this,
								"The following data files have invalid sizes. This means that values in the corresponding variables cannot be trusted (using Redatam xProcess or Redatam Converter): \n\n-" +
								string.Join("\n- ", files) + question,
								"Exporting", messageBoxButtons, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes;
			}
		}

		private bool checkFiles(bool askForAction = true)
		{
			IExporter export = Activator.CreateInstance(exporterType, db) as IExporter;
			export.CallBack = new EventHandler(updateCallback);
			string[] files = export.GetMissingFiles();
			if (files.Length == 0)
				return true;
			else
			{
				var messageBoxButtons = MessageBoxButtons.OK;
				string question = "";
				if (askForAction)
				{
					question = "\n\nDo you want to export the data anyway? Missing data will be assigned zero (0) values.";
					messageBoxButtons = MessageBoxButtons.YesNoCancel;
				}
				return MessageBox.Show(this,
								"The following data files are missing: \n\n-" +
								string.Join("\n- ", files) + question,
								"Exporting", messageBoxButtons, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes;
			}
		}

		private void SaveData(string targetFolder)
		{
			ShowProgress();
			EnableForm(false);

			folder = targetFolder;
			// calcula filas totales...
			InitializeProgress(folder);

			Thread t = new Thread(go);
			t.Start();
		}

		private void ShowProgress()
		{
			status.BringToFront();
			status.Visible = true;
		}


		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}


		private void frmMain_Resize(object sender, EventArgs e)
		{
			status.Left = ClientRectangle.Width / 2 - status.Width / 2;
			status.Top = ClientRectangle.Height / 2 - status.Height / 2;
		}

		private void btnCopyEntities_Click(object sender, EventArgs e)
		{
			CopyToClipboard(lwEntities);
		}

		private void btnCopyVariables_Click(object sender, EventArgs e)
		{
			CopyToClipboard(lwVariables);
		}

		private void btnCopyLabels_Click(object sender, EventArgs e)
		{
			CopyToClipboard(lwLabels);
		}

		private void CopyToClipboard(ListView lstMyListView)
		{
			Clipboard.Clear();
			StringBuilder buffer = new StringBuilder();

			// Setup the columns

			for (int i = 0; i < lstMyListView.Columns.Count; i++)
			{
				buffer.Append(lstMyListView.Columns[i].Text);
				if (i < lstMyListView.Columns.Count - 1)
					buffer.Append("\t");
			}
			buffer.Append("\n");

			// Build the data row by row

			for (int i = 0; i < lstMyListView.Items.Count; i++)
			{

				for (int j = 0; j < lstMyListView.Columns.Count; j++)
				{
					buffer.Append(lstMyListView.Items[i].SubItems[j].Text);
					if (j < lstMyListView.Columns.Count -1)
						buffer.Append("\t");
				}
				buffer.Append("\n");
			}

			Clipboard.SetText(buffer.ToString());
		}

		private void btnSaveCSV_Click(object sender, EventArgs e)
		{
			exporterType = typeof(CSVExporter);
			ShowSaveDialog("CSV");
		}

		private void btnExportMetadata_Click(object sender, EventArgs e)
		{
			SaveFileDialog fileBrowserDialog1 = new SaveFileDialog();
			fileBrowserDialog1.DefaultExt = "xml";
			fileBrowserDialog1.OverwritePrompt = true;
			fileBrowserDialog1.Filter = "XML file (*.xml)|*.xml|All files (*.*)|*.*";
			fileBrowserDialog1.Title = "Select the filename for definitions";
			if (fileBrowserDialog1.ShowDialog(this) == DialogResult.OK)
			{
				MetadataExporter exporter = new MetadataExporter(this.db);
				exporter.Save(fileBrowserDialog1.FileName);
			}
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			try
			{
				string path = Path.Combine(testsFolder, "dictionaries");
				string outpath = Path.Combine(testsFolder, "output");
				string sucessPath = Path.Combine(testsFolder, "success");

				Cursor.Current = Cursors.WaitCursor;
				ExportDicOutput(path, outpath);
				Cursor.Current = Cursors.Default;
				var success = true;
				// compara
				foreach (string file in Directory.GetFiles(outpath, "*.xml"))
				{
					string targetFile = Path.Combine(sucessPath, Path.GetFileName(file));
					if (File.ReadAllText(file) != File.ReadAllText(targetFile))
					{
						MessageBox.Show(this, "Test failed. File: " + Path.GetFileName(file), "Done");
						success = false;
					}
				}
				if (success)
				{
					MessageBox.Show(this, "All tests run successfully", "Done");
				}
			}
			catch (Exception ex)
			{
				Cursor.Current = Cursors.Default;

				MessageBox.Show(this, "An error ocurred (" + ex.Message + ").");
			}
		}
		
		private void btnRegenTest_Click(object sender, EventArgs e)
		{
			try
			{
				string path = Path.Combine(testsFolder, "dictionaries");
				string outpath = Path.Combine(testsFolder, "success");
				Cursor.Current = Cursors.WaitCursor;
				ExportDicOutput(path, outpath);
				Cursor.Current = Cursors.Default;
				MessageBox.Show(this, "Data generated successfully", "Done");
			}
			catch (Exception ex)
			{
				Cursor.Current = Cursors.Default;

				MessageBox.Show(this, "An error ocurred (" + ex.Message + ").");
			}

		}

		private static void ExportDicOutput(string path, string outpath)
		{
			if (Directory.Exists(path) == false)
			{
				throw new Exception("Input directory not found: " + path);
			}
			if (Directory.Exists(outpath))
			{
				ClearXMLs(outpath);
			}
			foreach (string file in Directory.GetFiles(path, "*.dic"))
			{
				RedatamDatabase db = new RedatamDatabase();
				db.OpenDictionary(file);

				MetadataExporter exp = new MetadataExporter(db);
				exp.Save(outpath + "\\" + Path.GetFileNameWithoutExtension(file) + ".xml");
			}
		}

		private static void ClearXMLs(string outpath)
		{
			foreach (var file in Directory.GetFiles(outpath, "*.xml"))
				File.Delete(file);
		}

		private void btnCheckAll_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lwVariables.Items)
			{
				CheckVariableItem(item, true);
			}
		}

		private void btnCheckNone_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lwVariables.Items)
			{
				CheckVariableItem(item, false); 
			}
		}

		private void CheckVariableItem(ListViewItem item, bool state)
		{
			item.Checked = state;
			((Variable)item.Tag).Selected = state;
		}

		private void lwVariables_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			((Variable)e.Item.Tag).Selected = e.Item.Checked;
			
		}


	}
}

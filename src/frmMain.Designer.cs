namespace RedatamConverter
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lwEntities = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lwVariables = new System.Windows.Forms.ListView();
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lwLabels = new System.Windows.Forms.ListView();
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblFile = new System.Windows.Forms.Label();
			this.btnSaveData = new System.Windows.Forms.Button();
			this.btnOpen = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.folderTo = new System.Windows.Forms.Label();
			this.btnCopyEntities = new System.Windows.Forms.Button();
			this.btnCopyVariables = new System.Windows.Forms.Button();
			this.btnCopyLabels = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lwEntities
			// 
			this.lwEntities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)));
			this.lwEntities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lwEntities.FullRowSelect = true;
			this.lwEntities.HideSelection = false;
			this.lwEntities.Location = new System.Drawing.Point(12, 36);
			this.lwEntities.Name = "lwEntities";
			this.lwEntities.Size = new System.Drawing.Size(175, 297);
			this.lwEntities.TabIndex = 2;
			this.lwEntities.UseCompatibleStateImageBehavior = false;
			this.lwEntities.View = System.Windows.Forms.View.Details;
			this.lwEntities.SelectedIndexChanged += new System.EventHandler(this.lwEntities_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Entity";
			this.columnHeader1.Width = 115;
			// 
			// lwVariables
			// 
			this.lwVariables.AllowColumnReorder = true;
			this.lwVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lwVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
			this.lwVariables.FullRowSelect = true;
			this.lwVariables.HideSelection = false;
			this.lwVariables.Location = new System.Drawing.Point(202, 36);
			this.lwVariables.Name = "lwVariables";
			this.lwVariables.Size = new System.Drawing.Size(421, 128);
			this.lwVariables.TabIndex = 2;
			this.lwVariables.UseCompatibleStateImageBehavior = false;
			this.lwVariables.View = System.Windows.Forms.View.Details;
			this.lwVariables.SelectedIndexChanged += new System.EventHandler(this.lwVariables_SelectedIndexChanged);
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Name";
			this.columnHeader2.Width = 115;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Value";
			this.columnHeader3.Width = 121;
			// 
			// lwLabels
			// 
			this.lwLabels.AllowColumnReorder = true;
			this.lwLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lwLabels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
			this.lwLabels.FullRowSelect = true;
			this.lwLabels.Location = new System.Drawing.Point(202, 203);
			this.lwLabels.Name = "lwLabels";
			this.lwLabels.Size = new System.Drawing.Size(421, 130);
			this.lwLabels.TabIndex = 2;
			this.lwLabels.UseCompatibleStateImageBehavior = false;
			this.lwLabels.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Value";
			this.columnHeader4.Width = 115;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Description";
			this.columnHeader5.Width = 121;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.folderTo);
			this.groupBox1.Controls.Add(this.lblFile);
			this.groupBox1.Controls.Add(this.btnSaveData);
			this.groupBox1.Controls.Add(this.btnOpen);
			this.groupBox1.Location = new System.Drawing.Point(12, 361);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(611, 82);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Actions";
			// 
			// lblFile
			// 
			this.lblFile.AutoSize = true;
			this.lblFile.Location = new System.Drawing.Point(128, 24);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(16, 13);
			this.lblFile.TabIndex = 5;
			this.lblFile.Text = "...";
			// 
			// btnSaveData
			// 
			this.btnSaveData.Enabled = false;
			this.btnSaveData.Location = new System.Drawing.Point(22, 48);
			this.btnSaveData.Name = "btnSaveData";
			this.btnSaveData.Size = new System.Drawing.Size(100, 23);
			this.btnSaveData.TabIndex = 4;
			this.btnSaveData.Text = "Save as SPSS data...";
			this.btnSaveData.UseVisualStyleBackColor = true;
			this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.Location = new System.Drawing.Point(22, 19);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(100, 23);
			this.btnOpen.TabIndex = 4;
			this.btnOpen.Text = "Open .dic ...";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(199, 187);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Labels:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(199, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Variables:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Entities:";
			// 
			// folderTo
			// 
			this.folderTo.AutoSize = true;
			this.folderTo.Location = new System.Drawing.Point(128, 53);
			this.folderTo.Name = "folderTo";
			this.folderTo.Size = new System.Drawing.Size(16, 13);
			this.folderTo.TabIndex = 5;
			this.folderTo.Text = "...";
			// 
			// btnCopyEntities
			// 
			this.btnCopyEntities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCopyEntities.Location = new System.Drawing.Point(121, 334);
			this.btnCopyEntities.Name = "btnCopyEntities";
			this.btnCopyEntities.Size = new System.Drawing.Size(66, 21);
			this.btnCopyEntities.TabIndex = 5;
			this.btnCopyEntities.Text = "Copy";
			this.btnCopyEntities.UseVisualStyleBackColor = true;
			this.btnCopyEntities.Click += new System.EventHandler(this.btnCopyEntities_Click);
			// 
			// btnCopyVariables
			// 
			this.btnCopyVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCopyVariables.Location = new System.Drawing.Point(557, 165);
			this.btnCopyVariables.Name = "btnCopyVariables";
			this.btnCopyVariables.Size = new System.Drawing.Size(66, 21);
			this.btnCopyVariables.TabIndex = 6;
			this.btnCopyVariables.Text = "Copy";
			this.btnCopyVariables.UseVisualStyleBackColor = true;
			this.btnCopyVariables.Click += new System.EventHandler(this.btnCopyVariables_Click);
			// 
			// btnCopyLabels
			// 
			this.btnCopyLabels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCopyLabels.Location = new System.Drawing.Point(557, 334);
			this.btnCopyLabels.Name = "btnCopyLabels";
			this.btnCopyLabels.Size = new System.Drawing.Size(66, 21);
			this.btnCopyLabels.TabIndex = 6;
			this.btnCopyLabels.Text = "Copy";
			this.btnCopyLabels.UseVisualStyleBackColor = true;
			this.btnCopyLabels.Click += new System.EventHandler(this.btnCopyLabels_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(635, 446);
			this.Controls.Add(this.btnCopyLabels);
			this.Controls.Add(this.btnCopyVariables);
			this.Controls.Add(this.btnCopyEntities);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lwLabels);
			this.Controls.Add(this.lwVariables);
			this.Controls.Add(this.lwEntities);
			this.Name = "frmMain";
			this.Text = "REDATAM Converter";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Resize += new System.EventHandler(this.frmMain_Resize);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lwEntities;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView lwVariables;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ListView lwLabels;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.Button btnSaveData;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label folderTo;
		private System.Windows.Forms.Button btnCopyEntities;
		private System.Windows.Forms.Button btnCopyVariables;
		private System.Windows.Forms.Button btnCopyLabels;
	}
}


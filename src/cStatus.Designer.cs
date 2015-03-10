namespace RedatamConverter
{
	partial class cStatus
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panProgress = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label19 = new System.Windows.Forms.Label();
			this.lblRemaining = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.lblEllapsed = new System.Windows.Forms.Label();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblRows = new System.Windows.Forms.Label();
			this.lblEntity = new System.Windows.Forms.Label();
			this.lblTotals = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblFolder = new System.Windows.Forms.Label();
			this.lblDict = new System.Windows.Forms.Label();
			this.panProgress.SuspendLayout();
			this.SuspendLayout();
			// 
			// panProgress
			// 
			this.panProgress.Controls.Add(this.btnCancel);
			this.panProgress.Controls.Add(this.label19);
			this.panProgress.Controls.Add(this.lblRemaining);
			this.panProgress.Controls.Add(this.label17);
			this.panProgress.Controls.Add(this.lblEllapsed);
			this.panProgress.Controls.Add(this.progress);
			this.panProgress.Controls.Add(this.label7);
			this.panProgress.Controls.Add(this.label6);
			this.panProgress.Controls.Add(this.label3);
			this.panProgress.Controls.Add(this.lblRows);
			this.panProgress.Controls.Add(this.lblEntity);
			this.panProgress.Controls.Add(this.lblTotals);
			this.panProgress.Controls.Add(this.label2);
			this.panProgress.Controls.Add(this.label1);
			this.panProgress.Controls.Add(this.lblFolder);
			this.panProgress.Controls.Add(this.lblDict);
			this.panProgress.Location = new System.Drawing.Point(4, 3);
			this.panProgress.Name = "panProgress";
			this.panProgress.Size = new System.Drawing.Size(516, 241);
			this.panProgress.TabIndex = 5;
			this.panProgress.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(423, 202);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(14, 212);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(60, 13);
			this.label19.TabIndex = 2;
			this.label19.Text = "Remaining:";
			// 
			// lblRemaining
			// 
			this.lblRemaining.AutoSize = true;
			this.lblRemaining.Location = new System.Drawing.Point(93, 212);
			this.lblRemaining.Name = "lblRemaining";
			this.lblRemaining.Size = new System.Drawing.Size(35, 13);
			this.lblRemaining.TabIndex = 3;
			this.lblRemaining.Text = "label1";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(15, 186);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(50, 13);
			this.label17.TabIndex = 4;
			this.label17.Text = "Ellapsed:";
			// 
			// lblEllapsed
			// 
			this.lblEllapsed.AutoSize = true;
			this.lblEllapsed.Location = new System.Drawing.Point(93, 186);
			this.lblEllapsed.Name = "lblEllapsed";
			this.lblEllapsed.Size = new System.Drawing.Size(35, 13);
			this.lblEllapsed.TabIndex = 5;
			this.lblEllapsed.Text = "label1";
			// 
			// progress
			// 
			this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.progress.Location = new System.Drawing.Point(17, 105);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(481, 19);
			this.progress.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(14, 164);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(37, 13);
			this.label7.TabIndex = 0;
			this.label7.Text = "Rows:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(14, 138);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(36, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "Entity:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 78);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Total rows:";
			// 
			// lblRows
			// 
			this.lblRows.AutoSize = true;
			this.lblRows.Location = new System.Drawing.Point(93, 164);
			this.lblRows.Name = "lblRows";
			this.lblRows.Size = new System.Drawing.Size(35, 13);
			this.lblRows.TabIndex = 0;
			this.lblRows.Text = "label1";
			// 
			// lblEntity
			// 
			this.lblEntity.AutoSize = true;
			this.lblEntity.Location = new System.Drawing.Point(93, 138);
			this.lblEntity.Name = "lblEntity";
			this.lblEntity.Size = new System.Drawing.Size(35, 13);
			this.lblEntity.TabIndex = 0;
			this.lblEntity.Text = "label1";
			// 
			// lblTotals
			// 
			this.lblTotals.AutoSize = true;
			this.lblTotals.Location = new System.Drawing.Point(93, 78);
			this.lblTotals.Name = "lblTotals";
			this.lblTotals.Size = new System.Drawing.Size(35, 13);
			this.lblTotals.TabIndex = 0;
			this.lblTotals.Text = "label1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Folder:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Exporting:";
			// 
			// lblFolder
			// 
			this.lblFolder.AutoSize = true;
			this.lblFolder.Location = new System.Drawing.Point(93, 52);
			this.lblFolder.Name = "lblFolder";
			this.lblFolder.Size = new System.Drawing.Size(35, 13);
			this.lblFolder.TabIndex = 0;
			this.lblFolder.Text = "label1";
			// 
			// lblDict
			// 
			this.lblDict.AutoSize = true;
			this.lblDict.Location = new System.Drawing.Point(93, 26);
			this.lblDict.Name = "lblDict";
			this.lblDict.Size = new System.Drawing.Size(35, 13);
			this.lblDict.TabIndex = 0;
			this.lblDict.Text = "label1";
			// 
			// cStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panProgress);
			this.Name = "cStatus";
			this.Size = new System.Drawing.Size(521, 251);
			this.panProgress.ResumeLayout(false);
			this.panProgress.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox panProgress;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label19;
		public System.Windows.Forms.Label lblRemaining;
		private System.Windows.Forms.Label label17;
		public System.Windows.Forms.Label lblEllapsed;
		public System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.Label lblRows;
		public System.Windows.Forms.Label lblEntity;
		public System.Windows.Forms.Label lblTotals;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblFolder;
		private System.Windows.Forms.Label lblDict;
	}
}

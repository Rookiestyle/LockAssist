namespace LockAssist
{
	partial class OptionsForm
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabQuickUnlock = new System.Windows.Forms.TabPage();
			this.cbPINDBSpecific = new System.Windows.Forms.CheckBox();
			this.tbModeExplain = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.cbActive = new System.Windows.Forms.CheckBox();
			this.lQUPINLength = new System.Windows.Forms.Label();
			this.lQUMode = new System.Windows.Forms.Label();
			this.rbPINEnd = new System.Windows.Forms.RadioButton();
			this.rbPINFront = new System.Windows.Forms.RadioButton();
			this.tbPINLength = new System.Windows.Forms.TextBox();
			this.cbPINMode = new System.Windows.Forms.ComboBox();
			this.tabSoftlock = new System.Windows.Forms.TabPage();
			this.tbSoftlockExplain = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cbSLIdle = new System.Windows.Forms.CheckBox();
			this.cbSLOnMinimize = new System.Windows.Forms.CheckBox();
			this.tSLIdleSeconds = new System.Windows.Forms.TextBox();
			this.tabAdditional = new System.Windows.Forms.TabPage();
			this.tbLockWorkspace = new System.Windows.Forms.TextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.cbLockWorkspace = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabQuickUnlock.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabSoftlock.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabAdditional.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabQuickUnlock);
			this.tabControl1.Controls.Add(this.tabSoftlock);
			this.tabControl1.Controls.Add(this.tabAdditional);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(480, 450);
			this.tabControl1.TabIndex = 6;
			// 
			// tabQuickUnlock
			// 
			this.tabQuickUnlock.BackColor = System.Drawing.Color.Transparent;
			this.tabQuickUnlock.Controls.Add(this.cbPINDBSpecific);
			this.tabQuickUnlock.Controls.Add(this.tbModeExplain);
			this.tabQuickUnlock.Controls.Add(this.panel2);
			this.tabQuickUnlock.Location = new System.Drawing.Point(4, 29);
			this.tabQuickUnlock.Name = "tabQuickUnlock";
			this.tabQuickUnlock.Padding = new System.Windows.Forms.Padding(3);
			this.tabQuickUnlock.Size = new System.Drawing.Size(472, 417);
			this.tabQuickUnlock.TabIndex = 0;
			this.tabQuickUnlock.Text = "Quick Unlock settings";
			this.tabQuickUnlock.UseVisualStyleBackColor = true;
			// 
			// cbPINDBSpecific
			// 
			this.cbPINDBSpecific.AutoSize = true;
			this.cbPINDBSpecific.Location = new System.Drawing.Point(11, 316);
			this.cbPINDBSpecific.Name = "cbPINDBSpecific";
			this.cbPINDBSpecific.Size = new System.Drawing.Size(205, 24);
			this.cbPINDBSpecific.TabIndex = 5;
			this.cbPINDBSpecific.Text = "Settings are DB specific";
			this.cbPINDBSpecific.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.cbPINDBSpecific.UseVisualStyleBackColor = true;
			// 
			// tbModeExplain
			// 
			this.tbModeExplain.Dock = System.Windows.Forms.DockStyle.Top;
			this.tbModeExplain.ForeColor = System.Drawing.SystemColors.WindowText;
			this.tbModeExplain.Location = new System.Drawing.Point(3, 183);
			this.tbModeExplain.Multiline = true;
			this.tbModeExplain.Name = "tbModeExplain";
			this.tbModeExplain.ReadOnly = true;
			this.tbModeExplain.Size = new System.Drawing.Size(466, 116);
			this.tbModeExplain.TabIndex = 34;
			this.tbModeExplain.TabStop = false;
			this.tbModeExplain.Text = "Requirements for mode \'Database password\'\r\n - Database masterkey contains a passw" +
    "ord\r\n - Option \'Remember master password\' is active\r\n\r\nQuick Unlock Entry will b" +
    "e used as fallback";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.cbActive);
			this.panel2.Controls.Add(this.lQUPINLength);
			this.panel2.Controls.Add(this.lQUMode);
			this.panel2.Controls.Add(this.rbPINEnd);
			this.panel2.Controls.Add(this.rbPINFront);
			this.panel2.Controls.Add(this.tbPINLength);
			this.panel2.Controls.Add(this.cbPINMode);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(466, 180);
			this.panel2.TabIndex = 35;
			// 
			// cbActive
			// 
			this.cbActive.AutoSize = true;
			this.cbActive.Location = new System.Drawing.Point(8, 11);
			this.cbActive.Name = "cbActive";
			this.cbActive.Size = new System.Drawing.Size(182, 24);
			this.cbActive.TabIndex = 42;
			this.cbActive.Text = "Enable Quick Unlock";
			this.cbActive.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.cbActive.UseVisualStyleBackColor = true;
			// 
			// lQUPINLength
			// 
			this.lQUPINLength.AutoSize = true;
			this.lQUPINLength.Location = new System.Drawing.Point(4, 86);
			this.lQUPINLength.Name = "lQUPINLength";
			this.lQUPINLength.Size = new System.Drawing.Size(87, 20);
			this.lQUPINLength.TabIndex = 41;
			this.lQUPINLength.Text = "PIN length:";
			// 
			// lQUMode
			// 
			this.lQUMode.AutoSize = true;
			this.lQUMode.Location = new System.Drawing.Point(4, 44);
			this.lQUMode.Name = "lQUMode";
			this.lQUMode.Size = new System.Drawing.Size(53, 20);
			this.lQUMode.TabIndex = 40;
			this.lQUMode.Text = "Mode:";
			// 
			// rbPINEnd
			// 
			this.rbPINEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbPINEnd.AutoSize = true;
			this.rbPINEnd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rbPINEnd.Location = new System.Drawing.Point(268, 145);
			this.rbPINEnd.Name = "rbPINEnd";
			this.rbPINEnd.Size = new System.Drawing.Size(194, 24);
			this.rbPINEnd.TabIndex = 39;
			this.rbPINEnd.TabStop = true;
			this.rbPINEnd.Text = "Use {0} last characters";
			this.rbPINEnd.UseVisualStyleBackColor = true;
			// 
			// rbPINFront
			// 
			this.rbPINFront.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.rbPINFront.AutoSize = true;
			this.rbPINFront.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rbPINFront.Location = new System.Drawing.Point(268, 115);
			this.rbPINFront.Name = "rbPINFront";
			this.rbPINFront.Size = new System.Drawing.Size(195, 24);
			this.rbPINFront.TabIndex = 38;
			this.rbPINFront.TabStop = true;
			this.rbPINFront.Text = "Use {0} first characters";
			this.rbPINFront.UseVisualStyleBackColor = true;
			// 
			// tbPINLength
			// 
			this.tbPINLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPINLength.Location = new System.Drawing.Point(362, 83);
			this.tbPINLength.MaxLength = 3;
			this.tbPINLength.Name = "tbPINLength";
			this.tbPINLength.Size = new System.Drawing.Size(100, 26);
			this.tbPINLength.TabIndex = 37;
			this.tbPINLength.Tag = "32";
			this.tbPINLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbPINLength.TextChanged += new System.EventHandler(this.tbPINLength_TextChanged);
			// 
			// cbPINMode
			// 
			this.cbPINMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPINMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPINMode.FormattingEnabled = true;
			this.cbPINMode.Items.AddRange(new object[] {
            "Quick Unlock entry only",
            "Database password"});
			this.cbPINMode.Location = new System.Drawing.Point(109, 41);
			this.cbPINMode.Name = "cbPINMode";
			this.cbPINMode.Size = new System.Drawing.Size(353, 28);
			this.cbPINMode.TabIndex = 36;
			this.cbPINMode.SelectedIndexChanged += new System.EventHandler(this.cbPINMode_SelectedIndexChanged);
			// 
			// tabSoftlock
			// 
			this.tabSoftlock.Controls.Add(this.tbSoftlockExplain);
			this.tabSoftlock.Controls.Add(this.panel1);
			this.tabSoftlock.Location = new System.Drawing.Point(4, 29);
			this.tabSoftlock.Name = "tabSoftlock";
			this.tabSoftlock.Padding = new System.Windows.Forms.Padding(3);
			this.tabSoftlock.Size = new System.Drawing.Size(472, 417);
			this.tabSoftlock.TabIndex = 1;
			this.tabSoftlock.Text = "Privacy Mode configuration";
			this.tabSoftlock.UseVisualStyleBackColor = true;
			// 
			// tbSoftlockExplain
			// 
			this.tbSoftlockExplain.Dock = System.Windows.Forms.DockStyle.Top;
			this.tbSoftlockExplain.Location = new System.Drawing.Point(3, 73);
			this.tbSoftlockExplain.Multiline = true;
			this.tbSoftlockExplain.Name = "tbSoftlockExplain";
			this.tbSoftlockExplain.ReadOnly = true;
			this.tbSoftlockExplain.Size = new System.Drawing.Size(466, 210);
			this.tbSoftlockExplain.TabIndex = 35;
			this.tbSoftlockExplain.TabStop = false;
			this.tbSoftlockExplain.Text = "Soft Lock explanation";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cbSLIdle);
			this.panel1.Controls.Add(this.cbSLOnMinimize);
			this.panel1.Controls.Add(this.tSLIdleSeconds);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(466, 70);
			this.panel1.TabIndex = 36;
			// 
			// cbSLIdle
			// 
			this.cbSLIdle.AutoSize = true;
			this.cbSLIdle.Location = new System.Drawing.Point(8, 11);
			this.cbSLIdle.Name = "cbSLIdle";
			this.cbSLIdle.Size = new System.Drawing.Size(179, 24);
			this.cbSLIdle.TabIndex = 17;
			this.cbSLIdle.Text = "Seconds of inactivity";
			this.cbSLIdle.UseVisualStyleBackColor = true;
			// 
			// cbSLOnMinimize
			// 
			this.cbSLOnMinimize.AutoSize = true;
			this.cbSLOnMinimize.Location = new System.Drawing.Point(8, 41);
			this.cbSLOnMinimize.Name = "cbSLOnMinimize";
			this.cbSLOnMinimize.Size = new System.Drawing.Size(215, 24);
			this.cbSLOnMinimize.TabIndex = 16;
			this.cbSLOnMinimize.Text = "Privacy mode on Minimize";
			this.cbSLOnMinimize.UseVisualStyleBackColor = true;
			// 
			// tSLIdleSeconds
			// 
			this.tSLIdleSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tSLIdleSeconds.Location = new System.Drawing.Point(363, 7);
			this.tSLIdleSeconds.MaxLength = 5;
			this.tSLIdleSeconds.Name = "tSLIdleSeconds";
			this.tSLIdleSeconds.Size = new System.Drawing.Size(100, 26);
			this.tSLIdleSeconds.TabIndex = 14;
			this.tSLIdleSeconds.Tag = "9999";
			this.tSLIdleSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tabAdditional
			// 
			this.tabAdditional.Controls.Add(this.tbLockWorkspace);
			this.tabAdditional.Controls.Add(this.panel3);
			this.tabAdditional.Location = new System.Drawing.Point(4, 29);
			this.tabAdditional.Name = "tabAdditional";
			this.tabAdditional.Padding = new System.Windows.Forms.Padding(3);
			this.tabAdditional.Size = new System.Drawing.Size(472, 417);
			this.tabAdditional.TabIndex = 2;
			this.tabAdditional.Text = "Additional";
			this.tabAdditional.UseVisualStyleBackColor = true;
			// 
			// tbLockWorkspace
			// 
			this.tbLockWorkspace.Dock = System.Windows.Forms.DockStyle.Top;
			this.tbLockWorkspace.ForeColor = System.Drawing.SystemColors.WindowText;
			this.tbLockWorkspace.Location = new System.Drawing.Point(3, 53);
			this.tbLockWorkspace.Multiline = true;
			this.tbLockWorkspace.Name = "tbLockWorkspace";
			this.tbLockWorkspace.ReadOnly = true;
			this.tbLockWorkspace.Size = new System.Drawing.Size(466, 237);
			this.tbLockWorkspace.TabIndex = 36;
			this.tbLockWorkspace.TabStop = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.cbLockWorkspace);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(466, 50);
			this.panel3.TabIndex = 37;
			// 
			// cbLockWorkspace
			// 
			this.cbLockWorkspace.AutoSize = true;
			this.cbLockWorkspace.Location = new System.Drawing.Point(8, 11);
			this.cbLockWorkspace.Name = "cbLockWorkspace";
			this.cbLockWorkspace.Size = new System.Drawing.Size(203, 24);
			this.cbLockWorkspace.TabIndex = 42;
			this.cbLockWorkspace.Text = "Global Lock Workspace";
			this.cbLockWorkspace.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.cbLockWorkspace.UseVisualStyleBackColor = true;
			// 
			// OptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.tabControl1);
			this.Name = "OptionsForm";
			this.Size = new System.Drawing.Size(480, 450);
			this.Load += new System.EventHandler(this.UnlockOptions_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabQuickUnlock.ResumeLayout(false);
			this.tabQuickUnlock.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tabSoftlock.ResumeLayout(false);
			this.tabSoftlock.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabAdditional.ResumeLayout(false);
			this.tabAdditional.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabQuickUnlock;
		private System.Windows.Forms.CheckBox cbPINDBSpecific;
		private System.Windows.Forms.TabPage tabSoftlock;
		private System.Windows.Forms.TextBox tbSoftlockExplain;
		private System.Windows.Forms.TextBox tbModeExplain;
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.TextBox tSLIdleSeconds;
		public System.Windows.Forms.CheckBox cbSLIdle;
		public System.Windows.Forms.CheckBox cbSLOnMinimize;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.CheckBox cbActive;
		private System.Windows.Forms.Label lQUPINLength;
		private System.Windows.Forms.Label lQUMode;
		private System.Windows.Forms.RadioButton rbPINEnd;
		private System.Windows.Forms.RadioButton rbPINFront;
		private System.Windows.Forms.TextBox tbPINLength;
		internal System.Windows.Forms.ComboBox cbPINMode;
		private System.Windows.Forms.TabPage tabAdditional;
		private System.Windows.Forms.TextBox tbLockWorkspace;
		private System.Windows.Forms.Panel panel3;
		internal System.Windows.Forms.CheckBox cbLockWorkspace;
	}
}
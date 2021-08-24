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
            this.tcLockAssistOptions = new System.Windows.Forms.TabControl();
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
            this.tcLockAssistOptions.SuspendLayout();
            this.tabQuickUnlock.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcLockAssistOptions
            // 
            this.tcLockAssistOptions.Controls.Add(this.tabQuickUnlock);
            this.tcLockAssistOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcLockAssistOptions.Location = new System.Drawing.Point(0, 0);
            this.tcLockAssistOptions.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tcLockAssistOptions.Name = "tcLockAssistOptions";
            this.tcLockAssistOptions.SelectedIndex = 0;
            this.tcLockAssistOptions.Size = new System.Drawing.Size(853, 697);
            this.tcLockAssistOptions.TabIndex = 6;
            // 
            // tabQuickUnlock
            // 
            this.tabQuickUnlock.BackColor = System.Drawing.Color.Transparent;
            this.tabQuickUnlock.Controls.Add(this.cbPINDBSpecific);
            this.tabQuickUnlock.Controls.Add(this.tbModeExplain);
            this.tabQuickUnlock.Controls.Add(this.panel2);
            this.tabQuickUnlock.Location = new System.Drawing.Point(10, 48);
            this.tabQuickUnlock.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabQuickUnlock.Name = "tabQuickUnlock";
            this.tabQuickUnlock.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabQuickUnlock.Size = new System.Drawing.Size(833, 639);
            this.tabQuickUnlock.TabIndex = 0;
            this.tabQuickUnlock.Text = "Quick Unlock settings";
            this.tabQuickUnlock.UseVisualStyleBackColor = true;
            // 
            // cbPINDBSpecific
            // 
            this.cbPINDBSpecific.AutoSize = true;
            this.cbPINDBSpecific.Location = new System.Drawing.Point(20, 490);
            this.cbPINDBSpecific.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbPINDBSpecific.Name = "cbPINDBSpecific";
            this.cbPINDBSpecific.Size = new System.Drawing.Size(354, 36);
            this.cbPINDBSpecific.TabIndex = 5;
            this.cbPINDBSpecific.Text = "Settings are DB specific";
            this.cbPINDBSpecific.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbPINDBSpecific.UseVisualStyleBackColor = true;
            // 
            // tbModeExplain
            // 
            this.tbModeExplain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbModeExplain.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbModeExplain.Location = new System.Drawing.Point(5, 284);
            this.tbModeExplain.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tbModeExplain.Multiline = true;
            this.tbModeExplain.Name = "tbModeExplain";
            this.tbModeExplain.ReadOnly = true;
            this.tbModeExplain.Size = new System.Drawing.Size(823, 178);
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
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(823, 279);
            this.panel2.TabIndex = 35;
            // 
            // cbActive
            // 
            this.cbActive.AutoSize = true;
            this.cbActive.Location = new System.Drawing.Point(14, 17);
            this.cbActive.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbActive.Name = "cbActive";
            this.cbActive.Size = new System.Drawing.Size(317, 36);
            this.cbActive.TabIndex = 42;
            this.cbActive.Text = "Enable Quick Unlock";
            this.cbActive.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbActive.UseVisualStyleBackColor = true;
            // 
            // lQUPINLength
            // 
            this.lQUPINLength.AutoSize = true;
            this.lQUPINLength.Location = new System.Drawing.Point(7, 133);
            this.lQUPINLength.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lQUPINLength.Name = "lQUPINLength";
            this.lQUPINLength.Size = new System.Drawing.Size(155, 32);
            this.lQUPINLength.TabIndex = 41;
            this.lQUPINLength.Text = "PIN length:";
            // 
            // lQUMode
            // 
            this.lQUMode.AutoSize = true;
            this.lQUMode.Location = new System.Drawing.Point(7, 68);
            this.lQUMode.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lQUMode.Name = "lQUMode";
            this.lQUMode.Size = new System.Drawing.Size(94, 32);
            this.lQUMode.TabIndex = 40;
            this.lQUMode.Text = "Mode:";
            // 
            // rbPINEnd
            // 
            this.rbPINEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbPINEnd.AutoSize = true;
            this.rbPINEnd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbPINEnd.Location = new System.Drawing.Point(483, 225);
            this.rbPINEnd.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.rbPINEnd.Name = "rbPINEnd";
            this.rbPINEnd.Size = new System.Drawing.Size(334, 36);
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
            this.rbPINFront.Location = new System.Drawing.Point(484, 178);
            this.rbPINFront.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.rbPINFront.Name = "rbPINFront";
            this.rbPINFront.Size = new System.Drawing.Size(335, 36);
            this.rbPINFront.TabIndex = 38;
            this.rbPINFront.TabStop = true;
            this.rbPINFront.Text = "Use {0} first characters";
            this.rbPINFront.UseVisualStyleBackColor = true;
            // 
            // tbPINLength
            // 
            this.tbPINLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPINLength.Location = new System.Drawing.Point(639, 129);
            this.tbPINLength.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tbPINLength.MaxLength = 3;
            this.tbPINLength.Name = "tbPINLength";
            this.tbPINLength.Size = new System.Drawing.Size(175, 38);
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
            this.cbPINMode.Location = new System.Drawing.Point(189, 64);
            this.cbPINMode.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cbPINMode.Name = "cbPINMode";
            this.cbPINMode.Size = new System.Drawing.Size(624, 39);
            this.cbPINMode.TabIndex = 36;
            this.cbPINMode.SelectedIndexChanged += new System.EventHandler(this.cbPINMode_SelectedIndexChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tcLockAssistOptions);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "OptionsForm";
            this.Size = new System.Drawing.Size(853, 697);
            this.Load += new System.EventHandler(this.UnlockOptions_Load);
            this.tcLockAssistOptions.ResumeLayout(false);
            this.tabQuickUnlock.ResumeLayout(false);
            this.tabQuickUnlock.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tcLockAssistOptions;
		private System.Windows.Forms.TabPage tabQuickUnlock;
		private System.Windows.Forms.CheckBox cbPINDBSpecific;
		private System.Windows.Forms.TextBox tbModeExplain;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.CheckBox cbActive;
		private System.Windows.Forms.Label lQUPINLength;
		private System.Windows.Forms.Label lQUMode;
		private System.Windows.Forms.RadioButton rbPINEnd;
		private System.Windows.Forms.RadioButton rbPINFront;
		private System.Windows.Forms.TextBox tbPINLength;
		internal System.Windows.Forms.ComboBox cbPINMode;
	}
}
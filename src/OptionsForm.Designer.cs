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
            this.components = new System.ComponentModel.Container();
            this.tcLockAssistOptions = new System.Windows.Forms.TabControl();
            this.tabQuickUnlock = new System.Windows.Forms.TabPage();
            this.tbModeExplain = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lQUAttempts = new System.Windows.Forms.Label();
            this.nQUAttempts = new System.Windows.Forms.NumericUpDown();
            this.cbPINDBSpecific = new System.Windows.Forms.CheckBox();
            this.cbQUValidity = new System.Windows.Forms.ComboBox();
            this.nQUValidity = new System.Windows.Forms.NumericUpDown();
            this.cbQUValidityActive = new System.Windows.Forms.CheckBox();
            this.cbActive = new System.Windows.Forms.CheckBox();
            this.lQUPINLength = new System.Windows.Forms.Label();
            this.lQUMode = new System.Windows.Forms.Label();
            this.rbPINEnd = new System.Windows.Forms.RadioButton();
            this.rbPINFront = new System.Windows.Forms.RadioButton();
            this.tbPINLength = new System.Windows.Forms.TextBox();
            this.cbPINMode = new System.Windows.Forms.ComboBox();
            this.tabSoftLock = new System.Windows.Forms.TabPage();
            this.tbSoftLockDesc = new System.Windows.Forms.TextBox();
            this.pSL = new System.Windows.Forms.Panel();
            this.cbSLValidityInterval = new System.Windows.Forms.ComboBox();
            this.nSLValiditySeconds = new System.Windows.Forms.NumericUpDown();
            this.cbSLValidityActive = new System.Windows.Forms.CheckBox();
            this.cbSLInterval = new System.Windows.Forms.ComboBox();
            this.cbSLOnMinimize = new System.Windows.Forms.CheckBox();
            this.nSLSeconds = new System.Windows.Forms.NumericUpDown();
            this.cbSLActive = new System.Windows.Forms.CheckBox();
            this.tabLockWorkspace = new System.Windows.Forms.TabPage();
            this.tbLockWorkspace = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbLockWorkspace = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tcLockAssistOptions.SuspendLayout();
            this.tabQuickUnlock.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nQUAttempts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nQUValidity)).BeginInit();
            this.tabSoftLock.SuspendLayout();
            this.pSL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nSLValiditySeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSLSeconds)).BeginInit();
            this.tabLockWorkspace.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcLockAssistOptions
            // 
            this.tcLockAssistOptions.Controls.Add(this.tabQuickUnlock);
            this.tcLockAssistOptions.Controls.Add(this.tabSoftLock);
            this.tcLockAssistOptions.Controls.Add(this.tabLockWorkspace);
            this.tcLockAssistOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcLockAssistOptions.Location = new System.Drawing.Point(0, 0);
            this.tcLockAssistOptions.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tcLockAssistOptions.Name = "tcLockAssistOptions";
            this.tcLockAssistOptions.SelectedIndex = 0;
            this.tcLockAssistOptions.Size = new System.Drawing.Size(426, 360);
            this.tcLockAssistOptions.TabIndex = 100;
            // 
            // tabQuickUnlock
            // 
            this.tabQuickUnlock.BackColor = System.Drawing.Color.Transparent;
            this.tabQuickUnlock.Controls.Add(this.tbModeExplain);
            this.tabQuickUnlock.Controls.Add(this.panel2);
            this.tabQuickUnlock.Location = new System.Drawing.Point(4, 25);
            this.tabQuickUnlock.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabQuickUnlock.Name = "tabQuickUnlock";
            this.tabQuickUnlock.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabQuickUnlock.Size = new System.Drawing.Size(418, 331);
            this.tabQuickUnlock.TabIndex = 0;
            this.tabQuickUnlock.Text = "Quick Unlock settings";
            this.tabQuickUnlock.UseVisualStyleBackColor = true;
            // 
            // tbModeExplain
            // 
            this.tbModeExplain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbModeExplain.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbModeExplain.Location = new System.Drawing.Point(2, 237);
            this.tbModeExplain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbModeExplain.Multiline = true;
            this.tbModeExplain.Name = "tbModeExplain";
            this.tbModeExplain.ReadOnly = true;
            this.tbModeExplain.Size = new System.Drawing.Size(414, 89);
            this.tbModeExplain.TabIndex = 110;
            this.tbModeExplain.TabStop = false;
            this.tbModeExplain.Text = "Requirements for mode \'Database password\'\r\n - Database masterkey contains a passw" +
    "ord\r\n - Option \'Remember master password\' is active\r\n\r\nQuick Unlock Entry will b" +
    "e used as fallback";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lQUAttempts);
            this.panel2.Controls.Add(this.nQUAttempts);
            this.panel2.Controls.Add(this.cbPINDBSpecific);
            this.panel2.Controls.Add(this.cbQUValidity);
            this.panel2.Controls.Add(this.nQUValidity);
            this.panel2.Controls.Add(this.cbQUValidityActive);
            this.panel2.Controls.Add(this.cbActive);
            this.panel2.Controls.Add(this.lQUPINLength);
            this.panel2.Controls.Add(this.lQUMode);
            this.panel2.Controls.Add(this.rbPINEnd);
            this.panel2.Controls.Add(this.rbPINFront);
            this.panel2.Controls.Add(this.tbPINLength);
            this.panel2.Controls.Add(this.cbPINMode);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(414, 234);
            this.panel2.TabIndex = 35;
            // 
            // lQUAttempts
            // 
            this.lQUAttempts.AutoSize = true;
            this.lQUAttempts.Location = new System.Drawing.Point(4, 175);
            this.lQUAttempts.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lQUAttempts.Name = "lQUAttempts";
            this.lQUAttempts.Size = new System.Drawing.Size(17, 16);
            this.lQUAttempts.TabIndex = 111;
            this.lQUAttempts.Text = "?:";
            this.lQUAttempts.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nQUAttempts
            // 
            this.nQUAttempts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nQUAttempts.Location = new System.Drawing.Point(339, 173);
            this.nQUAttempts.Margin = new System.Windows.Forms.Padding(2);
            this.nQUAttempts.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nQUAttempts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nQUAttempts.Name = "nQUAttempts";
            this.nQUAttempts.Size = new System.Drawing.Size(71, 22);
            this.nQUAttempts.TabIndex = 110;
            this.nQUAttempts.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbPINDBSpecific
            // 
            this.cbPINDBSpecific.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPINDBSpecific.AutoSize = true;
            this.cbPINDBSpecific.Location = new System.Drawing.Point(239, 9);
            this.cbPINDBSpecific.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbPINDBSpecific.Name = "cbPINDBSpecific";
            this.cbPINDBSpecific.Size = new System.Drawing.Size(171, 20);
            this.cbPINDBSpecific.TabIndex = 109;
            this.cbPINDBSpecific.Text = "Settings are DB specific";
            this.cbPINDBSpecific.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbPINDBSpecific.UseVisualStyleBackColor = true;
            this.cbPINDBSpecific.CheckedChanged += new System.EventHandler(this.cbPINDBSpecific_CheckedChanged);
            // 
            // cbQUValidity
            // 
            this.cbQUValidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbQUValidity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQUValidity.FormattingEnabled = true;
            this.cbQUValidity.Location = new System.Drawing.Point(180, 143);
            this.cbQUValidity.Margin = new System.Windows.Forms.Padding(2);
            this.cbQUValidity.Name = "cbQUValidity";
            this.cbQUValidity.Size = new System.Drawing.Size(230, 24);
            this.cbQUValidity.TabIndex = 108;
            // 
            // nQUValidity
            // 
            this.nQUValidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nQUValidity.DecimalPlaces = 2;
            this.nQUValidity.Location = new System.Drawing.Point(96, 142);
            this.nQUValidity.Margin = new System.Windows.Forms.Padding(2);
            this.nQUValidity.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nQUValidity.Name = "nQUValidity";
            this.nQUValidity.Size = new System.Drawing.Size(71, 22);
            this.nQUValidity.TabIndex = 107;
            // 
            // cbQUValidityActive
            // 
            this.cbQUValidityActive.AutoSize = true;
            this.cbQUValidityActive.Location = new System.Drawing.Point(7, 143);
            this.cbQUValidityActive.Margin = new System.Windows.Forms.Padding(2);
            this.cbQUValidityActive.Name = "cbQUValidityActive";
            this.cbQUValidityActive.Size = new System.Drawing.Size(76, 20);
            this.cbQUValidityActive.TabIndex = 106;
            this.cbQUValidityActive.Text = "Validity:";
            // 
            // cbActive
            // 
            this.cbActive.AutoSize = true;
            this.cbActive.Location = new System.Drawing.Point(7, 9);
            this.cbActive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbActive.Name = "cbActive";
            this.cbActive.Size = new System.Drawing.Size(154, 20);
            this.cbActive.TabIndex = 101;
            this.cbActive.Text = "Enable Quick Unlock";
            this.cbActive.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbActive.UseVisualStyleBackColor = true;
            // 
            // lQUPINLength
            // 
            this.lQUPINLength.AutoSize = true;
            this.lQUPINLength.Location = new System.Drawing.Point(4, 69);
            this.lQUPINLength.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lQUPINLength.Name = "lQUPINLength";
            this.lQUPINLength.Size = new System.Drawing.Size(71, 16);
            this.lQUPINLength.TabIndex = 41;
            this.lQUPINLength.Text = "PIN length:";
            // 
            // lQUMode
            // 
            this.lQUMode.AutoSize = true;
            this.lQUMode.Location = new System.Drawing.Point(4, 35);
            this.lQUMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lQUMode.Name = "lQUMode";
            this.lQUMode.Size = new System.Drawing.Size(45, 16);
            this.lQUMode.TabIndex = 40;
            this.lQUMode.Text = "Mode:";
            // 
            // rbPINEnd
            // 
            this.rbPINEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbPINEnd.AutoSize = true;
            this.rbPINEnd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbPINEnd.Location = new System.Drawing.Point(248, 116);
            this.rbPINEnd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbPINEnd.Name = "rbPINEnd";
            this.rbPINEnd.Size = new System.Drawing.Size(163, 20);
            this.rbPINEnd.TabIndex = 105;
            this.rbPINEnd.TabStop = true;
            this.rbPINEnd.Text = "Use {0} last characters";
            this.rbPINEnd.UseVisualStyleBackColor = true;
            // 
            // rbPINFront
            // 
            this.rbPINFront.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbPINFront.AutoSize = true;
            this.rbPINFront.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbPINFront.Location = new System.Drawing.Point(249, 92);
            this.rbPINFront.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbPINFront.Name = "rbPINFront";
            this.rbPINFront.Size = new System.Drawing.Size(162, 20);
            this.rbPINFront.TabIndex = 104;
            this.rbPINFront.TabStop = true;
            this.rbPINFront.Text = "Use {0} first characters";
            this.rbPINFront.UseVisualStyleBackColor = true;
            // 
            // tbPINLength
            // 
            this.tbPINLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPINLength.Location = new System.Drawing.Point(322, 67);
            this.tbPINLength.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbPINLength.MaxLength = 3;
            this.tbPINLength.Name = "tbPINLength";
            this.tbPINLength.Size = new System.Drawing.Size(90, 22);
            this.tbPINLength.TabIndex = 103;
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
            this.cbPINMode.Location = new System.Drawing.Point(96, 33);
            this.cbPINMode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbPINMode.Name = "cbPINMode";
            this.cbPINMode.Size = new System.Drawing.Size(314, 24);
            this.cbPINMode.TabIndex = 102;
            this.cbPINMode.SelectedIndexChanged += new System.EventHandler(this.cbPINMode_SelectedIndexChanged);
            // 
            // tabSoftLock
            // 
            this.tabSoftLock.Controls.Add(this.tbSoftLockDesc);
            this.tabSoftLock.Controls.Add(this.pSL);
            this.tabSoftLock.Location = new System.Drawing.Point(4, 25);
            this.tabSoftLock.Margin = new System.Windows.Forms.Padding(2);
            this.tabSoftLock.Name = "tabSoftLock";
            this.tabSoftLock.Padding = new System.Windows.Forms.Padding(2);
            this.tabSoftLock.Size = new System.Drawing.Size(418, 331);
            this.tabSoftLock.TabIndex = 1;
            this.tabSoftLock.Text = "Softlock";
            this.tabSoftLock.UseVisualStyleBackColor = true;
            // 
            // tbSoftLockDesc
            // 
            this.tbSoftLockDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbSoftLockDesc.Location = new System.Drawing.Point(2, 103);
            this.tbSoftLockDesc.Margin = new System.Windows.Forms.Padding(2);
            this.tbSoftLockDesc.Multiline = true;
            this.tbSoftLockDesc.Name = "tbSoftLockDesc";
            this.tbSoftLockDesc.ReadOnly = true;
            this.tbSoftLockDesc.Size = new System.Drawing.Size(414, 149);
            this.tbSoftLockDesc.TabIndex = 205;
            // 
            // pSL
            // 
            this.pSL.Controls.Add(this.cbSLValidityInterval);
            this.pSL.Controls.Add(this.nSLValiditySeconds);
            this.pSL.Controls.Add(this.cbSLValidityActive);
            this.pSL.Controls.Add(this.cbSLInterval);
            this.pSL.Controls.Add(this.cbSLOnMinimize);
            this.pSL.Controls.Add(this.nSLSeconds);
            this.pSL.Controls.Add(this.cbSLActive);
            this.pSL.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSL.Location = new System.Drawing.Point(2, 2);
            this.pSL.Margin = new System.Windows.Forms.Padding(2);
            this.pSL.Name = "pSL";
            this.pSL.Size = new System.Drawing.Size(414, 101);
            this.pSL.TabIndex = 1;
            // 
            // cbSLValidityInterval
            // 
            this.cbSLValidityInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSLValidityInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSLValidityInterval.FormattingEnabled = true;
            this.cbSLValidityInterval.Location = new System.Drawing.Point(178, 66);
            this.cbSLValidityInterval.Margin = new System.Windows.Forms.Padding(2);
            this.cbSLValidityInterval.Name = "cbSLValidityInterval";
            this.cbSLValidityInterval.Size = new System.Drawing.Size(230, 24);
            this.cbSLValidityInterval.TabIndex = 207;
            // 
            // nSLValiditySeconds
            // 
            this.nSLValiditySeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nSLValiditySeconds.DecimalPlaces = 2;
            this.nSLValiditySeconds.Location = new System.Drawing.Point(112, 66);
            this.nSLValiditySeconds.Margin = new System.Windows.Forms.Padding(2);
            this.nSLValiditySeconds.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nSLValiditySeconds.Name = "nSLValiditySeconds";
            this.nSLValiditySeconds.Size = new System.Drawing.Size(71, 22);
            this.nSLValiditySeconds.TabIndex = 206;
            // 
            // cbSLValidityActive
            // 
            this.cbSLValidityActive.AutoSize = true;
            this.cbSLValidityActive.Location = new System.Drawing.Point(7, 67);
            this.cbSLValidityActive.Margin = new System.Windows.Forms.Padding(2);
            this.cbSLValidityActive.Name = "cbSLValidityActive";
            this.cbSLValidityActive.Size = new System.Drawing.Size(142, 20);
            this.cbSLValidityActive.TabIndex = 205;
            this.cbSLValidityActive.Text = "cbSLValidityActivw";
            this.cbSLValidityActive.UseVisualStyleBackColor = true;
            // 
            // cbSLInterval
            // 
            this.cbSLInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSLInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSLInterval.FormattingEnabled = true;
            this.cbSLInterval.Location = new System.Drawing.Point(178, 7);
            this.cbSLInterval.Margin = new System.Windows.Forms.Padding(2);
            this.cbSLInterval.Name = "cbSLInterval";
            this.cbSLInterval.Size = new System.Drawing.Size(230, 24);
            this.cbSLInterval.TabIndex = 203;
            // 
            // cbSLOnMinimize
            // 
            this.cbSLOnMinimize.AutoSize = true;
            this.cbSLOnMinimize.Location = new System.Drawing.Point(7, 37);
            this.cbSLOnMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.cbSLOnMinimize.Name = "cbSLOnMinimize";
            this.cbSLOnMinimize.Size = new System.Drawing.Size(129, 20);
            this.cbSLOnMinimize.TabIndex = 204;
            this.cbSLOnMinimize.Text = "cbSLOnMinimize";
            this.cbSLOnMinimize.UseVisualStyleBackColor = true;
            // 
            // nSLSeconds
            // 
            this.nSLSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nSLSeconds.DecimalPlaces = 2;
            this.nSLSeconds.Location = new System.Drawing.Point(112, 7);
            this.nSLSeconds.Margin = new System.Windows.Forms.Padding(2);
            this.nSLSeconds.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nSLSeconds.Name = "nSLSeconds";
            this.nSLSeconds.Size = new System.Drawing.Size(71, 22);
            this.nSLSeconds.TabIndex = 202;
            // 
            // cbSLActive
            // 
            this.cbSLActive.AutoSize = true;
            this.cbSLActive.Location = new System.Drawing.Point(7, 9);
            this.cbSLActive.Margin = new System.Windows.Forms.Padding(2);
            this.cbSLActive.Name = "cbSLActive";
            this.cbSLActive.Size = new System.Drawing.Size(97, 20);
            this.cbSLActive.TabIndex = 201;
            this.cbSLActive.Text = "cbSLActive";
            this.cbSLActive.UseVisualStyleBackColor = true;
            // 
            // tabLockWorkspace
            // 
            this.tabLockWorkspace.BackColor = System.Drawing.Color.Transparent;
            this.tabLockWorkspace.Controls.Add(this.tbLockWorkspace);
            this.tabLockWorkspace.Controls.Add(this.panel1);
            this.tabLockWorkspace.Location = new System.Drawing.Point(4, 25);
            this.tabLockWorkspace.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabLockWorkspace.Name = "tabLockWorkspace";
            this.tabLockWorkspace.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabLockWorkspace.Size = new System.Drawing.Size(418, 331);
            this.tabLockWorkspace.TabIndex = 0;
            this.tabLockWorkspace.Text = "Lock Workspace";
            this.tabLockWorkspace.UseVisualStyleBackColor = true;
            // 
            // tbLockWorkspace
            // 
            this.tbLockWorkspace.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbLockWorkspace.Location = new System.Drawing.Point(2, 48);
            this.tbLockWorkspace.Margin = new System.Windows.Forms.Padding(2);
            this.tbLockWorkspace.Multiline = true;
            this.tbLockWorkspace.Name = "tbLockWorkspace";
            this.tbLockWorkspace.ReadOnly = true;
            this.tbLockWorkspace.Size = new System.Drawing.Size(414, 136);
            this.tbLockWorkspace.TabIndex = 302;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbLockWorkspace);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 45);
            this.panel1.TabIndex = 36;
            // 
            // cbLockWorkspace
            // 
            this.cbLockWorkspace.AutoSize = true;
            this.cbLockWorkspace.Location = new System.Drawing.Point(7, 9);
            this.cbLockWorkspace.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbLockWorkspace.Name = "cbLockWorkspace";
            this.cbLockWorkspace.Size = new System.Drawing.Size(131, 20);
            this.cbLockWorkspace.TabIndex = 301;
            this.cbLockWorkspace.Text = "Lock Workspace";
            this.cbLockWorkspace.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cbLockWorkspace.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tcLockAssistOptions);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "OptionsForm";
            this.Size = new System.Drawing.Size(426, 360);
            this.Load += new System.EventHandler(this.UnlockOptions_Load);
            this.tcLockAssistOptions.ResumeLayout(false);
            this.tabQuickUnlock.ResumeLayout(false);
            this.tabQuickUnlock.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nQUAttempts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nQUValidity)).EndInit();
            this.tabSoftLock.ResumeLayout(false);
            this.tabSoftLock.PerformLayout();
            this.pSL.ResumeLayout(false);
            this.pSL.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nSLValiditySeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSLSeconds)).EndInit();
            this.tabLockWorkspace.ResumeLayout(false);
            this.tabLockWorkspace.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tcLockAssistOptions;
		private System.Windows.Forms.TabPage tabQuickUnlock;
        private System.Windows.Forms.TabPage tabLockWorkspace; 
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
        private System.Windows.Forms.ComboBox cbQUValidity;
        private System.Windows.Forms.NumericUpDown nQUValidity;
        private System.Windows.Forms.CheckBox cbQUValidityActive;
        private System.Windows.Forms.TextBox tbLockWorkspace;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbLockWorkspace;
        private System.Windows.Forms.TabPage tabSoftLock;
        private System.Windows.Forms.Panel pSL;
        private System.Windows.Forms.CheckBox cbSLOnMinimize;
        private System.Windows.Forms.NumericUpDown nSLSeconds;
        private System.Windows.Forms.CheckBox cbSLActive;
        private System.Windows.Forms.TextBox tbSoftLockDesc;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ComboBox cbSLInterval;
        private System.Windows.Forms.ComboBox cbSLValidityInterval;
        private System.Windows.Forms.NumericUpDown nSLValiditySeconds;
        private System.Windows.Forms.CheckBox cbSLValidityActive;
    private System.Windows.Forms.NumericUpDown nQUAttempts;
    private System.Windows.Forms.Label lQUAttempts;
  }
}

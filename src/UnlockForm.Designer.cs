using KeePass.UI;
namespace LockAssist
{
	partial class UnlockForm
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
            this.lLabel = new System.Windows.Forms.Label();
            this.bUnlock = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbTogglePin = new System.Windows.Forms.CheckBox();
            this.stbPIN = new KeePass.UI.SecureTextBoxEx();
            this.cbContinueUnlock = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lLabel
            // 
            this.lLabel.AutoSize = true;
            this.lLabel.Location = new System.Drawing.Point(11, 35);
            this.lLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lLabel.Name = "lLabel";
            this.lLabel.Size = new System.Drawing.Size(114, 16);
            this.lLabel.TabIndex = 1;
            this.lLabel.Text = "Quick Unlock PIN:";
            // 
            // bUnlock
            // 
            this.bUnlock.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bUnlock.Location = new System.Drawing.Point(174, 93);
            this.bUnlock.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bUnlock.Name = "bUnlock";
            this.bUnlock.Size = new System.Drawing.Size(89, 28);
            this.bUnlock.TabIndex = 2;
            this.bUnlock.Text = "Unlock";
            this.bUnlock.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(268, 93);
            this.bCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(89, 28);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // cbTogglePin
            // 
            this.cbTogglePin.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbTogglePin.AutoSize = true;
            this.cbTogglePin.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbTogglePin.Checked = true;
            this.cbTogglePin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTogglePin.Location = new System.Drawing.Point(324, 30);
            this.cbTogglePin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbTogglePin.Name = "cbTogglePin";
            this.cbTogglePin.Size = new System.Drawing.Size(32, 26);
            this.cbTogglePin.TabIndex = 1;
            this.cbTogglePin.Text = "***";
            this.cbTogglePin.UseVisualStyleBackColor = true;
            this.cbTogglePin.CheckedChanged += new System.EventHandler(this.togglePIN_CheckedChanged);
            // 
            // stbPIN
            // 
            this.stbPIN.Location = new System.Drawing.Point(175, 32);
            this.stbPIN.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.stbPIN.Name = "stbPIN";
            this.stbPIN.Size = new System.Drawing.Size(138, 22);
            this.stbPIN.TabIndex = 0;
            // 
            // cbContinueUnlock
            // 
            this.cbContinueUnlock.AutoSize = true;
            this.cbContinueUnlock.Location = new System.Drawing.Point(14, 66);
            this.cbContinueUnlock.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbContinueUnlock.Name = "cbContinueUnlock";
            this.cbContinueUnlock.Size = new System.Drawing.Size(138, 20);
            this.cbContinueUnlock.TabIndex = 4;
            this.cbContinueUnlock.Text = "cbContinueUnlock";
            this.cbContinueUnlock.UseVisualStyleBackColor = true;
            this.cbContinueUnlock.CheckedChanged += new System.EventHandler(this.cbContinueUnlock_CheckedChanged);
            // 
            // UnlockForm
            // 
            this.AcceptButton = this.bUnlock;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(367, 150);
            this.Controls.Add(this.cbContinueUnlock);
            this.Controls.Add(this.cbTogglePin);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bUnlock);
            this.Controls.Add(this.lLabel);
            this.Controls.Add(this.stbPIN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnlockForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quick Unlock";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private SecureTextBoxEx stbPIN;
		private System.Windows.Forms.Label lLabel;
		private System.Windows.Forms.Button bUnlock;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.CheckBox cbTogglePin;
        private System.Windows.Forms.CheckBox cbContinueUnlock;
    }
}
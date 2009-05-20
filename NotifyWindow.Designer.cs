namespace Koffeinfrei.ExecUrl
{
    partial class NotifyWindow
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
            this.timerFadeShow = new System.Windows.Forms.Timer(this.components);
            this.timerFadeHide = new System.Windows.Forms.Timer(this.components);
            this.textParameter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // timerFadeShow
            // 
            this.timerFadeShow.Enabled = true;
            this.timerFadeShow.Interval = global::Koffeinfrei.ExecUrl.Settings.Default.TimerIntervalNotificationFade;
            this.timerFadeShow.Tick += new System.EventHandler(this.timerFadeShow_Tick);
            // 
            // timerFadeHide
            // 
            this.timerFadeHide.Interval = global::Koffeinfrei.ExecUrl.Settings.Default.TimerIntervalNotificationFade;
            this.timerFadeHide.Tick += new System.EventHandler(this.timerFadeHide_Tick);
            // 
            // textParameter
            // 
            this.textParameter.Location = new System.Drawing.Point(1, 1);
            this.textParameter.Name = "textParameter";
            this.textParameter.Size = new System.Drawing.Size(209, 20);
            this.textParameter.TabIndex = 17;
            this.textParameter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textParameter_KeyPress);
            // 
            // NotifyWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(211, 22);
            this.Controls.Add(this.textParameter);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "NotifyWindow";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.NotifyWindow_Load);
            this.MouseEnter += new System.EventHandler(this.NotifyWindow_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.NotifyWindow_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerFadeShow;
        private System.Windows.Forms.Timer timerFadeHide;
        private System.Windows.Forms.TextBox textParameter;
    }
}
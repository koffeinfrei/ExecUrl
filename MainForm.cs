// =========================================================================================
// 
//  Copyright 2009 by Alexis Reigel
//  All rights reserved
// 
//  http://www.koffeinfrei.org
// 
//  Author             : mail@koffeinfrei.org
// 
//  URL                : $HeadURL: svn://enigma/PD/CMS2008/Implementation/trunk/Suche/GridSoft.PD.CMS2008.Suche.Results/SearchService.cs $
//  Revision           : $LastChangedRevision: 2600 $
//  Last modified      : $LastChangedDate: 2008-11-26 22:32:47 +0100 (Mi, 26 Nov 2008) $
//  Last author        : $LastChangedBy: areigel $
//  Id                 : $Id: SearchService.cs 2600 2008-11-26 21:32:47Z areigel $
// =========================================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using MouseKeyboardLibrary;

namespace Koffeinfrei.ExecUrl
{
    /// <summary>
    /// The main form, i.e. the main application container. Not a visible window.
    /// </summary>
    public partial class MainForm : DockableForm
    {
        private const int IconWidth = 16;
        private const int IconHeight = 16;

        private UrlParams urlParams;
        private int lastSelectedUrlIndex;
        private KeyboardHook keyboardHook = new KeyboardHook();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            urlParams = UrlParams.Load();

            InitUrls();

            // global hot keys
            keyboardHook.KeyDown += keyboardHook_KeyDown;
            keyboardHook.Start();

            SetWindowPosition();
            Visible = false;
            Hide();
        }

        /// <summary>
        /// Handles the KeyDown event of the global hotkey.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            // display context menu
            if (e.Control && e.Shift && e.KeyCode == Keys.OemMinus)
            {
                trayMenu.Show(Left + Width, Top + Height);
                trayMenu.Focus();
                trayMenu.Items[0].Select();
            }

            // directly call menu item
            if (e.Control && 
                e.KeyCode >= Keys.NumPad0 && e.KeyCode < Keys.NumPad0 + urlParams.Count)
            {
                UrlParam urlParam = urlParams[e.KeyValue - (int) Keys.NumPad0];
                ShowParamDialog(urlParam);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handle the visibility on start.
        /// </summary>
        /// <remarks>
        /// Initial opacity is 0%, such that on start the window is not visible.
        /// When initially shown, we hide the window and set its opacity back to 100%.
        /// This way, we don't have any flickering on startup.
        /// </remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            Hide();
            Opacity = 1;
        }

        /// <summary>
        /// Handles the MouseClick event of the tray control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void tray_MouseClick(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button == MouseButtons.Left)
            {
                UrlParam urlParam = urlParams[lastSelectedUrlIndex];
                ShowParamDialog(urlParam);
            }
        }

        /// <summary>
        /// Handles the Click event of the menuItemQuit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of the menuItemExecUrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void menuItemExecUrl_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            for (int i = 0; i < urlParams.Count; ++i)
            {
                UrlParam urlParam = urlParams[i];
                if (item.Name == urlParam.Name)
                {
                    lastSelectedUrlIndex = i;
                    ShowParamDialog(urlParam);
                    break;
                }
            }
        }

        /// <summary>
        /// Handles the FormClosed event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosedEventArgs"/> instance containing the event data.</param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
            tray.Dispose();
        }

        /// <summary>
        /// Fills the tray context menu with the available entries from the config.
        /// </summary>
        private void InitUrls()
        {
            for (int i = 0; i < urlParams.Count; ++i)
            {
                UrlParam url = urlParams[i];

                ToolStripMenuItem item = new ToolStripMenuItem
                                             {
                                                 Name = url.Name,
                                                 Text = url.Name
                                             };
                item.Size = new Size(152, 22);
                item.Click += menuItemExecUrl_Click;

                // generate menu item picture
                Bitmap bitmap = new Bitmap(IconWidth, IconHeight);
                SolidBrush brush = new SolidBrush(DefaultForeColor);
                Graphics graphics = Graphics.FromImage(bitmap);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                graphics.FillRectangle(new SolidBrush(DefaultBackColor), 0, 0, IconWidth, IconHeight);
                graphics.TextContrast = 0; // high contrast (0-12)
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphics.DrawString(i.ToString(), DefaultFont, brush, (float) (IconWidth/2.0), (float) (IconHeight/2.0),
                                    stringFormat);

                item.Image = bitmap;

                trayMenu.Items.Insert(trayMenu.Items.Count - 1, item);
            }
        }

        /// <summary>
        /// Shows the param dialog which allows to enter the parameter for the selected
        /// URL in the <paramref name="urlParam"/> object.
        /// </summary>
        /// <param name="urlParam">The URL param.</param>
        private void ShowParamDialog(UrlParam urlParam)
        {
            NotifyWindow notifyWindow = new NotifyWindow();
            DialogResult result = notifyWindow.ShowDialog();

            if (result == DialogResult.OK)
            {
                string parameterValue = notifyWindow.ParameterValue;
                if (urlParam.Browser == UrlParam.BrowserType.SystemDefault)
                {
                    Process.Start(string.Format(urlParam.Url, parameterValue));
                }
                else if (urlParam.Browser == UrlParam.BrowserType.IE)
                {
                    ProcessStartInfo process = new ProcessStartInfo("iexplore",
                                                                    string.Format(urlParam.Url, parameterValue));
                    Process.Start(process);
                }
                else if (urlParam.Browser == UrlParam.BrowserType.Firefox)
                {
                    ProcessStartInfo process = new ProcessStartInfo("firefox",
                                                                    string.Format(urlParam.Url, parameterValue));
                    Process.Start(process);
                }
            }
        }
    }
}
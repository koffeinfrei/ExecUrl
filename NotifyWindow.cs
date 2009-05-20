// =========================================================================================
// 
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Library General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//
//  Copyright 2009 by Alexis Reigel
//  http://www.koffeinfrei.org | mail@koffeinfrei.org
// 
// =========================================================================================
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Koffeinfrei.ExecUrl
{
    /// <summary>
    /// This class is the notify window (guification) that usually appears in the lower right
    /// corner of your desktop.
    /// </summary>
    public partial class NotifyWindow : GradientForm
    {
        /// <summary>
        /// Stores the full pixel from top of the window. Used for fading the window.
        /// </summary>
        private int fullTop;

        /// <summary>
        /// Stores the full height of the window. Used for fading the window.
        /// </summary>
        private int fullHeight;

        /// <summary>
        /// Provides the number of pixel the fading should advance per ticks.
        /// </summary>
        /// <remarks>See the settings value <c>NotificationFadePixels</c></remarks>
        private readonly int numPixelsFade;

        /// <summary>
        /// Whether the mouse is on the notification window.
        /// </summary>
        private bool mouseEntered;

        /// <summary>
        /// The <see cref="FadeType"/> used for this window.
        /// </summary>
        private readonly FadeType fadeType;

        /// <summary>
        /// Provides a selection of the fade type that is used for fading the window.
        /// <list type="table">
        /// <item>
        /// <term>Slide</term>
        /// <description>The window slides in and out.</description>
        /// </item>
        /// <item>
        /// <term>Opacity</term>
        /// <description>The window fades by opacity.</description>
        /// </item>
        /// </list>
        /// </summary>
        private enum FadeType
        {
            /// <summary>
            /// The window slides in and out.
            /// </summary>
            Slide = 1,
            /// <summary>
            /// The window fades by opacity.
            /// </summary>
            Opacity
        }

        /// <summary>
        /// Gets or sets the parameter value that will be used to create the URL.
        /// </summary>
        /// <value>The parameter value.</value>
        public string ParameterValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyWindow"/> class.
        /// Displays an empty notification.
        /// </summary>
        public NotifyWindow()
        {
            InitializeComponent();

            numPixelsFade = Settings.Default.NotificationFadePixels;
            fadeType = (FadeType) Settings.Default.NotificationFadeType;
        }

        /// <summary>
        /// Handles the Load event of the <see cref="NotifyWindow"/>. Sets the position and
        /// opacity to its initial values and sets the labels.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NotifyWindow_Load(object sender, EventArgs e)
        {
            SetWindowPosition();

            if (fadeType == FadeType.Slide)
            {
                fullHeight = Height;
                fullTop = Top;
                Height = 0;
                Top += fullHeight;
            }
            else if (fadeType == FadeType.Opacity)
            {
                Opacity = 0;
            }

            textParameter.Focus();
        }

        /// <summary>
        /// Handles the MouseEnter event of the <see cref="NotifyWindow"/>. This stops the timer
        /// that handles the disappearance of the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NotifyWindow_MouseEnter(object sender, EventArgs e)
        {
            Debug.WriteLine("NotifyWindow_MouseEnter");
            mouseEntered = true;
            if (timerFadeHide.Enabled)
            {
                timerFadeHide.Stop();
                timerFadeShow.Start();
            }
        }

        /// <summary>
        /// Handles the MouseLeave event of the <see cref="NotifyWindow"/>. This starts the timer
        /// that handles the disappearance of the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NotifyWindow_MouseLeave(object sender, EventArgs e)
        {
            // check if still inside window (event fires if childcontrol is entered too)
            if (MousePosition.Y < Top || MousePosition.Y > Top + Height ||
                MousePosition.X < Left || MousePosition.X > Left + Width)
            {
                Debug.WriteLine("NotifyWindow_MouseLeave");
                mouseEntered = false;
            }
        }


        /// <summary>
        /// Handles the Tick event of the timerFadeShow control. This timer handles the
        /// appearance of the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timerFadeShow_Tick(object sender, EventArgs e)
        {
            if (fadeType == FadeType.Slide)
            {
                // TODO check for taskbar position (top, bottom, ...)
                // see SetWindowPosition
                if (Height < fullHeight)
                {
                    if (Height + numPixelsFade > fullHeight)
                    {
                        Top = fullTop;
                        Height = fullHeight;
                    }
                    else
                    {
                        Height += numPixelsFade;
                        Top -= numPixelsFade;
                    }
                }
                else
                {
                    timerFadeShow.Stop();
                }
            }
            else if (fadeType == FadeType.Opacity)
            {
                if (Opacity < 1)
                {
                    Opacity += 0.1;
                }
                else
                {
                    timerFadeShow.Stop();
                }
            }
        }

        /// <summary>
        /// Handles the Tick event of the timerFadeHide control. This timer handles the
        /// disappearance of the window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timerFadeHide_Tick(object sender, EventArgs e)
        {
            if (fadeType == FadeType.Slide)
            {
                if (Height > 0)
                {
                    Height -= numPixelsFade;
                    Top += numPixelsFade;
                }
                else
                {
                    timerFadeHide.Stop();
                    Dispose();
                    Close();
                }
            }
            else if (fadeType == FadeType.Opacity)
            {
                if (Opacity > 0)
                {
                    Opacity -= 0.1;
                }
                else
                {
                    timerFadeHide.Stop();
                    Dispose();
                    Close();
                }
            }
        }

        private void textParameter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                ParameterValue = textParameter.Text;
                DialogResult = DialogResult.OK;
                Dispose();
                Close();
            }
            else if (e.KeyChar == (char) Keys.Escape)
            {
                Dispose();
                Close();
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
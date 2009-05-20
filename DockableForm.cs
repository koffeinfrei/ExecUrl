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
using System.Drawing;
using System.Windows.Forms;

namespace Koffeinfrei.ExecUrl
{
    /// <summary>
    /// This is the base class for all windows that stick to the windows task bar.
    /// </summary>
    public class DockableForm : Form
    {
        private delegate void SetTextCallback(Control control, string text);

        private delegate string GetTextCallback(Control control);

        private delegate void SetForegroundColorCallback(Label label, Color color);

        private delegate void SetVisibilityCallback(Control control, bool visible);

        private delegate void SetEnabledCallback(Control control, bool enabled);

        private delegate void SetBackgroundPictureCallback(Control control, Bitmap picture);

        private ApplicationState applicationState;

        /// <summary>
        /// Provides a selection of the application state.
        /// <list type="table">
        /// <item>
        /// <term>Offline</term>
        /// <description>The application is in offline mode.</description>
        /// </item>
        /// <item>
        /// <term>Online</term>
        /// <description>The application is online, meaning has a connection to the server.</description>
        /// </item>
        /// <item>
        /// <term>NotConfigured</term>
        /// <description>The application is not properly configured, meaning that either
        /// the user or the group is not set.</description>
        /// </item>
        /// </list>
        /// </summary>
        internal enum ApplicationState
        {
            /// <summary>
            /// The application is in offline mode.
            /// </summary>
            Offline,
            /// <summary>
            /// The application is online, meaning has a connection to the server.
            /// </summary>
            Online,
            /// <summary>
            /// The application is not properly configured, meaning that either
            /// the user or the group is not set.
            /// </summary>
            NotConfigured
        }

        /// <summary>
        /// Checks the state of the application.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if the the current <see cref="ApplicationState"/> matches
        /// the given <paramref name="state"/>.</returns>
        internal bool CheckApplicationState(ApplicationState state)
        {
            return applicationState == state;
        }

        /// <summary>
        /// Sets the state of the application.
        /// </summary>
        /// <param name="state">The state.</param>
        internal virtual void SetApplicationState(ApplicationState state)
        {
            SetApplicationState(state, null);
        }

        /// <summary>
        /// Sets the state of the application with the given additional message.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="message">The message.</param>
        internal virtual void SetApplicationState(ApplicationState state, string message)
        {
            applicationState = state;
        }

        /// <summary>
        /// Sets the window position to stick to the taskbar.
        /// </summary>
        protected void SetWindowPosition()
        {
            // get position of form near system tray
            int screenHeight = SystemInformation.PrimaryMonitorSize.Height;
            int screenWidth = SystemInformation.PrimaryMonitorSize.Width;
            int desktopHeight = SystemInformation.WorkingArea.Height;
            int desktopWidth = SystemInformation.WorkingArea.Width;
            int desktopTop = SystemInformation.WorkingArea.Top;
            int desktopBottom = SystemInformation.WorkingArea.Bottom;
            int desktopLeft = SystemInformation.WorkingArea.Left;
            int desktopRight = SystemInformation.WorkingArea.Right;

            // taskbar -> bottom
            if (desktopLeft == 0 && desktopTop == 0 && desktopHeight <= screenHeight)
            {
                Top = desktopBottom - Height;
                Left = desktopRight - Width;
            }
                // taskbar -> right
            else if (desktopLeft == 0 && desktopTop == 0 && desktopWidth <= screenWidth)
            {
                Top = desktopBottom - Height;
                Left = desktopRight - Width;
            }
                // taskbar -> top
            else if (desktopLeft == 0 && desktopTop > 0 && desktopHeight <= screenHeight)
            {
                Top = desktopTop;
                Left = desktopRight - Width;
            }
                // taskbar -> left
            else if (desktopLeft > 0 && desktopTop == 0 && desktopWidth <= screenWidth)
            {
                Top = desktopBottom - Height;
                Left = desktopLeft;
            }
        }

        /// <summary>
        /// Sets the text for asynchronous calls in a thread safe manner.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        protected void SetTextSafe(Control control, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (control.InvokeRequired)
            {
                SetTextCallback d =
                    new SetTextCallback(SetTextSafe);
                Invoke(d, new object[] {control, text});
            }
            else
            {
                control.Text = text;
            }
        }

        /// <summary>
        /// Gets the text for asynchronous calls in a thread safe manner.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        protected string GetTextSafe(Control control)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (control.InvokeRequired)
            {
                GetTextCallback d =
                    new GetTextCallback(GetTextSafe);
                string text = (string) Invoke(d, new object[] {control});
                return text;
            }
            else
            {
                return control.Text;
            }
        }

        /// <summary>
        /// Sets the foreground color for asynchronous calls in a thread safe manner.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="color">The color.</param>
        protected void SetForegroundColorSafe(Label label, Color color)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (label.InvokeRequired)
            {
                SetForegroundColorCallback d =
                    new SetForegroundColorCallback(SetForegroundColorSafe);
                Invoke(d, new object[] {label, color});
            }
            else
            {
                label.ForeColor = color;
            }
        }

        /// <summary>
        /// Sets the visibility for asynchronous calls in a thread safe manner.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="visible">The visibility is set to <paramref name="visible"/>.</param>
        protected void SetVisibilitySafe(Control control, bool visible)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (control.InvokeRequired)
            {
                SetVisibilityCallback d =
                    new SetVisibilityCallback(SetVisibilitySafe);
                Invoke(d, new object[] {control, visible});
            }
            else
            {
                control.Visible = visible;
            }
        }

        /// <summary>
        /// Sets the enabled for asynchronous calls in a thread safe manner.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="enabled">The <paramref name="control"/> is set to 
        /// <paramref name="enabled"/>.</param>
        protected void SetEnabledSafe(Control control, bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (control.InvokeRequired)
            {
                SetEnabledCallback d =
                    new SetEnabledCallback(SetEnabledSafe);
                Invoke(d, new object[] {control, enabled});
            }
            else
            {
                control.Enabled = enabled;
            }
        }

        /// <summary>
        /// Sets the background picture for asynchronous calls in a thread safe manner.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="picture">The picture.</param>
        protected void SetBackgroundPictureSafe(Control control, Bitmap picture)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (control.InvokeRequired)
            {
                SetBackgroundPictureCallback d =
                    new SetBackgroundPictureCallback(SetBackgroundPictureSafe);
                Invoke(d, new object[] {control, picture});
            }
            else
            {
                control.BackgroundImage = picture;
            }
        }
    }
}
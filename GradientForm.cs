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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Koffeinfrei.ExecUrl
{
    /// <summary>
    /// This is the base class for all classes that have a gradient background.
    /// </summary>
    public partial class GradientForm : DockableForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GradientForm"/> class.
        /// </summary>
        public GradientForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Paints a gradient as background for the main window
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void GradientForm_Paint(object sender, PaintEventArgs e)
        {
            Rectangle baseRectangle =
                new Rectangle(0, 0, Width - 1, Height - 1);

            Brush gradientBrush =
                new LinearGradientBrush(
                    baseRectangle,
                    Color.WhiteSmoke,
                    Color.FromArgb(180, 10, 95, 255),
                    LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(gradientBrush, baseRectangle);
        }
    }
}
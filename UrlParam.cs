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
using System.Collections.Generic;
using System.IO;

namespace Koffeinfrei.ExecUrl
{
    /// <summary>
    /// This class holds an entry for an url that can be executed.
    /// </summary>
    public class UrlParam
    {
        /// <summary>
        /// Defines the browser type to be used to execute the url.
        /// </summary>
        public enum BrowserType
        {
            /// <summary>
            /// 
            /// </summary>
            SystemDefault,
            /// <summary>
            /// 
            /// </summary>
            Firefox,
            /// <summary>
            /// 
            /// </summary>
            IE
        }

        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the URL of the entry.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the browser type of the entry. Default is <see cref="BrowserType.SystemDefault"/>.
        /// </summary>
        /// <value>The browser.</value>
        public BrowserType Browser { get; set; }
    }

    /// <summary>
    /// This class is the collection of all <see cref="UrlParam"/>s defined in the configuration file.
    /// </summary>
    public class UrlParams : List<UrlParam>
    {
        private static UrlParams inst = new UrlParams();

        /// <summary>
        /// Don't allow public instantiation.
        /// </summary>
        private UrlParams()
        {
        }

        /// <summary>
        /// Loads the configuration into the collection.
        /// </summary>
        /// <returns></returns>
        public static UrlParams Load()
        {
            StreamReader reader = new StreamReader(Settings.Default.UrlParamsFile);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Trim().Length > 0 && !line.StartsWith("#"))
                {
                    string[] parts = line.Split('\t');
                    UrlParam param = new UrlParam { Name = parts[0], Url = parts[1] };

                    if (parts.Length > 2)
                    {
                        string browser = parts[2];
                        if (browser.Equals("firefox", StringComparison.InvariantCultureIgnoreCase))
                        {
                            param.Browser = UrlParam.BrowserType.Firefox;
                        }
                        else if (browser.Equals("ie", StringComparison.InvariantCultureIgnoreCase))
                        {
                            param.Browser = UrlParam.BrowserType.IE;
                        }
                    }
                    inst.Add(param);
                }
            }
            return inst;
        }
    }
}
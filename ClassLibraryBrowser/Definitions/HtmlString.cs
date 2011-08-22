/*
 * **************************************************************************
 *
 * Copyright (c) The IronSmalltalk Project. 
 *
 * This source code is subject to terms and conditions of the 
 * license agreement found in the solution directory. 
 * See: $(SolutionDir)\License.htm ... in the root of this distribution.
 * By using this source code in any fashion, you are agreeing 
 * to be bound by the terms of the license agreement.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions
{
    public struct HtmlString
    {
        public string Html; // { get; set; }

        public string Text
        {
            get { return this.StripHtml(); } // Not perfect, but OK for now 
        }

        public HtmlString(XmlNode elem)
        {
            if (elem != null)
                this.Html = elem.InnerText;
            else
                this.Html = null;
        }

        public HtmlString(string html)
        {
            this.Html = html;
        }

        public void SaveXml(XmlWriter xml, string elementName)
        {
            this.SaveXml(xml, elementName, false);
        }

        public void SaveXml(XmlWriter xml, string elementName, bool alwaysEmit)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");
            if (String.IsNullOrWhiteSpace(elementName))
                throw new ArgumentNullException("elementName");

            if (alwaysEmit || !String.IsNullOrWhiteSpace(this.Html))
                xml.WriteElementString(elementName, this.Html);
        }

        private string StripHtml()
        {
            if (this.Html == null)
                return null;
            
            bool intag = false;
            StringBuilder txt = new StringBuilder();
            foreach (char ch in this.Html.Replace("</p>", "\r\n").Replace("</P>", "\r\n"))
            {
                if (intag)
                {
                    if (ch == '>')
                        intag = false;
                }
                else
                {
                    if (ch == '<')
                        intag = true;
                    else
                        txt.Append(ch);
                }
            }
            return System.Web.HttpUtility.HtmlDecode(txt.ToString());
        }

    }
}

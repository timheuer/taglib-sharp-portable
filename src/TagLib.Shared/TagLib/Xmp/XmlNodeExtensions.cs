//
// XmlNodeExtensions.cs:
//
// Author:
//   Ruben Vermeersch (ruben@savanne.be)
//
// Copyright (C) 2009 Ruben Vermeersch
//
// This library is free software; you can redistribute it and/or modify
// it  under the terms of the GNU Lesser General Public License version
// 2.1 as published by the Free Software Foundation.
//
// This library is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
// USA
//

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace TagLib.Xmp
{
	internal static class XmlNodeExtensions
	{
        //public static bool In (this XmlNode node, string ns)
        //{
        //    return node.NamespaceURI == ns;
        //}

        public static bool In(this XAttribute node, string ns)
        {
            return node.Name.NamespaceName == ns;
        }

        public static bool In(this XElement node, string ns)
        {
            return node.Name.NamespaceName == ns;
        }

        //public static bool Is (this XmlNode node, string ns, string name)
        //{
        //    return node.In (ns) && node.LocalName == name;
        //}

        public static bool Is(this XElement node, string ns, string name)
        {
            return node.Name == XName.Get(name, ns);
        }

        public static bool Is(this XAttribute node, string ns, string name)
        {
            return node.Name == XName.Get(name, ns);
        }

		// 7.2.2 coreSyntaxTerms
		//		rdf:RDF | rdf:ID | rdf:about | rdf:parseType | rdf:resource | rdf:nodeID | rdf:datatype
		public static bool IsCoreSyntax (this XElement node)
		{
			return node.In (XmpTag.RDF_NS) && (
				node.Name.LocalName == XmpTag.RDF_URI ||
				node.Name.LocalName == XmpTag.ID_URI ||
				node.Name.LocalName == XmpTag.ABOUT_URI ||
				node.Name.LocalName == XmpTag.PARSE_TYPE_URI ||
				node.Name.LocalName == XmpTag.RESOURCE_URI ||
				node.Name.LocalName == XmpTag.NODE_ID_URI ||
				node.Name.LocalName == XmpTag.DATA_TYPE_URI
					);
		}

		// 7.2.4 oldTerms
		//		rdf:aboutEach | rdf:aboutEachPrefix | rdf:bagID
		public static bool IsOld (this XElement node)
		{
			return node.In (XmpTag.RDF_NS) && (
				node.Name.LocalName == XmpTag.ABOUT_EACH_URI ||
				node.Name.LocalName == XmpTag.ABOUT_EACH_PREFIX_URI ||
				node.Name.LocalName == XmpTag.BAG_ID_URI
				);
		}

		// 7.2.5 nodeElementURIs
		//		anyURI - ( coreSyntaxTerms | rdf:li | oldTerms )
		public static bool IsNodeElement (this XElement node)
		{
			return !node.IsCoreSyntax () &&
				!node.Is (XmpTag.RDF_NS, XmpTag.LI_URI) &&
				!node.IsOld ();
		}

		// 7.2.6 propertyElementURIs
		//		anyURI - ( coreSyntaxTerms | rdf:Description | oldTerms )
		public static bool IsPropertyElement (this XElement node)
		{
			return !node.IsCoreSyntax () &&
				!node.Is (XmpTag.RDF_NS, XmpTag.DESCRIPTION_URI) &&
				!node.IsOld ();
		}

		// 7.2.7 propertyAttributeURIs
		//		anyURI - ( coreSyntaxTerms | rdf:Description | rdf:li | oldTerms )
		public static bool IsPropertyAttribute (this XElement node)
		{
			return node is XAttribute &&
				!node.IsCoreSyntax () &&
				!node.Is (XmpTag.RDF_NS, XmpTag.DESCRIPTION_URI) &&
				!node.Is (XmpTag.RDF_NS, XmpTag.LI_URI) &&
				!node.IsOld ();
		}
	}
}

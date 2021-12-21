using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace TinCore.Common
{
    public static class Utils
    {
        public static string ProcessXML(string xml, string attribute)
        {
            if (string.IsNullOrEmpty(xml))
                return xml;
            try
            {
                var cxml = XDocument.Parse(xml);
                if (cxml is XDocument)
                    if (cxml.Root.HasAttributes && cxml.Root.Attribute(attribute) != null)
                        return cxml.Root.Attribute(attribute).Value;
            }
            catch (Exception ex)
            { }

            return string.Empty;
        }

        public static List<string> GetListFromXML(string xmlStr, string attribute)
        {
            var xml = ProcessXML(xmlStr, attribute);
            if (!string.IsNullOrEmpty(xml))
            {
                return JsonConvert.DeserializeObject<List<string>>(xml);
            }
            return new List<string>();
        }

        public static Dictionary<string,string> GetXMLAttributes(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return new Dictionary<string, string>();
            try
            {
                var cxml = XDocument.Parse(xml);
                if (cxml is XDocument)
                    if (cxml.Root.HasAttributes)
                        return cxml.Root.Attributes().ToDictionary(sp => sp.Name.LocalName, sp => sp.Value);
            }
            catch (Exception ex)
            { }

            return new Dictionary<string, string>();
        }
    }
}

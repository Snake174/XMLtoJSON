using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XMLtoJSON
{
    class Program
    {
        class Prop
        {
            public string Name;
            public string Type;
            public string Value;
        }

        class ProductOccurence
        {
            public string Id;
            public string Name;
            public List<Prop> Props;
        }

        static void Main(string[] args)
        {
            string xmlFile = "test.xml";
            string jsonFile = "test.json";
            List<ProductOccurence> products = new List<ProductOccurence>();

            if (!File.Exists(xmlFile))
            {
                Console.WriteLine("test.xml file not exists");
                Console.ReadLine();
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            XmlElement xRoot = xmlDoc.DocumentElement;
            XmlNodeList xmlNodeList = xRoot.SelectNodes("/Root/ModelFile/ProductOccurence");

            foreach (XmlNode xNode in xmlNodeList)
            {
                string Id = xNode.Attributes.GetNamedItem("Id").InnerText;
                string Name = xNode.Attributes.GetNamedItem("Name").InnerText;

                ProductOccurence po = new ProductOccurence
                {
                    Id = Id,
                    Name = Name,
                    Props = new List<Prop>(),
                };

                XmlNode xmlAttrNodeList = xNode.SelectSingleNode("Attributes");

                if (xmlAttrNodeList != null)
                {
                    foreach (XmlNode Attributes in xmlAttrNodeList.ChildNodes)
                    {
                        string _name = Attributes.Attributes.GetNamedItem("Name").InnerText;
                        string _type = Attributes.Attributes.GetNamedItem("Type").InnerText;
                        string _value = Attributes.Attributes.GetNamedItem("Value").InnerText;

                        po.Props.Add(new Prop {
                            Name = _name,
                            Type = _type,
                            Value = _value
                        });
                    }
                }

                products.Add(po);
            }

            string result = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

            using (var tw = new StreamWriter(jsonFile))
            {
                tw.WriteLine(result);
                tw.Close();
            }

            Console.WriteLine("OK");
            Console.ReadLine();
        }
    }
}

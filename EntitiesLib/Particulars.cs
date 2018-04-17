using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace EntitiesLib
{
    public class Particulars
    {
        public int ParticularsID { get; set; }
        public string ParticularsName { get; set; }
        public List<ParticularsSubType> ParticularsSubTypes { get; set; }
        
        public List<Particulars> GetAllParticulars()
        {
            return FetchDataFromXML();

        }

        private List<Particulars> FetchDataFromXML()
        {
            List<Particulars> lstPart= new List<Particulars>();
            XDocument doc = new XDocument();
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf('\\')) + "\\" + "Configuration.xml";
            doc = XDocument.Load(path);

            var lv1s = from lv1 in doc.Descendants("Particular")
                       select new {
                                ID = lv1.Attribute("id").Value,
                                Header = lv1.Attribute("name").Value,
                                Children = lv1.Descendants("SubType")
                            };
            Particulars p;
            foreach (var lv1 in lv1s)
            {
                p = new Particulars();
                p.ParticularsID = 1;
                p.ParticularsName = lv1.Header;
                List<ParticularsSubType> sp = new List<ParticularsSubType>();
                foreach (var lv2 in lv1.Children)
                {
                    ParticularsSubType spp= new ParticularsSubType();
                    spp.SubTypeID = Convert.ToInt16(lv2.Attribute("id").Value);
                    spp.SubTypeName =lv2.Attribute("name").Value;
                    sp.Add(spp);
                }
                p.ParticularsSubTypes = sp;
                lstPart.Add(p);
            }
            return lstPart;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Wake_up_Robot_Client.Models
{
    /// <summary>
    /// XML model class, used to serialize and deserialize alarms from and to an XML file
    /// </summary>
    /// Thanks to http://www.switchonthecode.com/tutorials/csharp-tutorial-xml-serialization
    static class mXML
    {
        static private String xmlLoc = @"..\..\alarms.xml";

        /// <summary>
        /// Gets the content of xml file (Deserialization)
        /// </summary>
        static public List<Alarm> GetAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Alarm>));
            TextReader textReader = new StreamReader(xmlLoc);
            alarms = (List<Alarm>)deserializer.Deserialize(textReader);
            textReader.Close();
            return alarms;
        }

        /// <summary>
        /// Stores list of alarms in the xml file (Serialization)
        /// </summary>
        static public void StoreAlarms(List<Alarm> alarms)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Alarm>));
            TextWriter textWriter = new StreamWriter(xmlLoc);
            serializer.Serialize(textWriter, alarms);
            textWriter.Close();
        }
    }
}

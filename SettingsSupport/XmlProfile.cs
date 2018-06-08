using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

namespace SettingsSupport
{
    ///////////////////////////////////////
    public interface IProfile
    {
        string Name { get; set; }

        void SetValue(string section, string entry, object value);
        void SetValue(string section, string entry, object value, bool addType);

        string GetValue(string section, string entry, string defaultValue);
        int GetValue(string section, string entry, int defaultValue);
        long GetValue(string section, string entry, long defaultValue);
        float GetValue(string section, string entry, float defaultValue);
        double GetValue(string section, string entry, double defaultValue);
        bool GetValue(string section, string entry, bool defaultValue);
        object GetValue(string section, string entry);
        object GetValue(string section, string entry, Type type);
        object GetValue(string section, string entry, Type type, object defaultValue);

        void Flush();
    }

    ///////////////////////////////////////
    public class XmlBuffer : IDisposable
    {
        private XmlProfile m_profile;
        private XmlDocument m_doc;
        private FileStream m_file;
        internal bool m_needsFlushing;

        internal XmlBuffer(XmlProfile profile, bool lockFile)
        {
            m_profile = profile;
            if (lockFile && File.Exists(m_profile.Name))
                m_file = new FileStream(m_profile.Name, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        }

        internal void Load(XmlTextWriter writer)
        {
            writer.Flush();
            writer.BaseStream.Position = 0;
            m_doc.Load(writer.BaseStream);
            m_needsFlushing = true;
        }

        internal XmlDocument XmlDocument
        {
            get
            {
                if (m_doc == null)
                {
                    m_doc = new XmlDocument();
                    if (m_file != null)
                    {
                        m_file.Position = 0;
                        m_doc.Load(m_file);
                    }
                    else if (File.Exists(m_profile.Name))
                        m_doc.Load(m_profile.Name);
                }
                return m_doc;
            }
        }

        internal bool IsEmpty
        {
            get { return XmlDocument.InnerXml == String.Empty; }
        }

        public bool NeedsFlushing
        {
            get { return m_needsFlushing; }
        }

        public bool Locked
        {
            get { return m_file != null; }
        }

        public void Flush()
        {
            if (m_profile == null)
                throw new InvalidOperationException("Cannot flush an XmlBuffer object that has been closed.");

            if (m_doc == null)
                return;

            if (m_file == null)
                m_doc.Save(m_profile.Name);
            else
            {
                m_file.SetLength(0);
                m_doc.Save(m_file);
            }

            m_needsFlushing = false;
        }

        public void Reset()
        {
            if (m_profile == null)
                throw new InvalidOperationException("Cannot reset an XmlBuffer object that has been closed.");

            m_doc = null;
            m_needsFlushing = false;
        }

        public void Close()
        {
            if (m_profile == null)
                return;

            if (m_needsFlushing)
                Flush();

            m_doc = null;

            if (m_file != null)
            {
                m_file.Close();
                m_file = null;
            }

            if (m_profile != null)
            {
                m_profile.m_buffer = null;
                m_profile = null;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

    ///////////////////////////////////////
    public class XmlProfile : IProfile
    {
        public XmlProfile()
        {
            string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            Name = fileName.Substring(0, fileName.LastIndexOf('.')) + ".profile.xml";
        }

        public XmlProfile(string fileName)
        {
            if (fileName == null || fileName == "")
                throw new InvalidOperationException("Name is null or empty.");
            Name = fileName;
        }

        public string RootName
        {
            get { return m_rootName; }
            set { m_rootName = value; }
        }

        public Encoding Encoding
        {
            get { return m_encoding; }
            set { m_encoding = value; }
        }

        public bool Buffering
        {
            get { return m_buffer != null; }
        }

        public XmlBuffer Buffer()
        {
            return Buffer(true);
        }

        public XmlBuffer Buffer(bool lockFile)
        {
            if (m_buffer == null)
                m_buffer = new XmlBuffer(this, lockFile);
            return m_buffer;
        }

        // IMyProfile methods
        public virtual string Name
        {
            get; set;
        }
        public virtual void SetValue(string section, string entry, object value)
        {
            DoSetValue(section, entry, value, true);
        }
        public virtual void SetValue(string section, string entry, object value, bool addType)
        {
            DoSetValue(section, entry, value, addType);
        }
        public virtual string GetValue(string section, string entry, string defaultValue)
        {
            return (string)DoGetValue(section, entry, typeof(string)) ?? defaultValue;
        }
        public virtual int GetValue(string section, string entry, int defaultValue)
        {
            return (int?)DoGetValue(section, entry, typeof(int)) ?? defaultValue;
        }
        public virtual long GetValue(string section, string entry, long defaultValue)
        {
            return (long?)DoGetValue(section, entry, typeof(long)) ?? defaultValue;
        }
        public virtual float GetValue(string section, string entry, float defaultValue)
        {
            return (float?)DoGetValue(section, entry, typeof(float)) ?? defaultValue;
        }
        public virtual double GetValue(string section, string entry, double defaultValue)
        {
            return (double?)DoGetValue(section, entry, typeof(double)) ?? defaultValue;
        }
        public virtual bool GetValue(string section, string entry, bool defaultValue)
        {
            return (bool?)DoGetValue(section, entry, typeof(bool)) ?? defaultValue;
        }
        public virtual object GetValue(string section, string entry)
        {
            return DoGetValue(section, entry, null);
        }
        public virtual object GetValue(string section, string entry, Type type)
        {
            return DoGetValue(section, entry, type);
        }
        public virtual object GetValue(string section, string entry, Type type, object defaultValue)
        {
            return DoGetValue(section, entry, type) ?? defaultValue;
        }
        public virtual void Flush()
        {
            if (m_buffer != null && m_buffer.m_needsFlushing)
                m_buffer.Flush();
        }


        ///////////////////////////////////////
        // Implementation
        ///////////////////////////////////////
        private string m_rootName = "profile";
        private Encoding m_encoding = Encoding.UTF8;
        internal XmlBuffer m_buffer;

        protected void DoSetValue(string section, string entry, object value, bool addType)
        {
            if (value == null)
            {
                RemoveEntry(section, entry);    // Remove the entry
                return;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
            string valueString = (converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string))) ?
                                  converter.ConvertToString(value) : null;

            if ((m_buffer == null || m_buffer.IsEmpty) && !File.Exists(Name))
            {
                // The file does not exist
                using (XmlTextWriter writer = (m_buffer == null) ?
                                new XmlTextWriter(Name, Encoding) :
                                new XmlTextWriter(new MemoryStream(), Encoding))    // If there's a buffer, write to it without creating the file
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();
                    writer.WriteStartElement(RootName);
                    writer.WriteStartElement("section");
                    writer.WriteAttributeString("name", null, section);
                    writer.WriteStartElement("entry");
                    writer.WriteAttributeString("name", null, entry);
                    if (addType)
                        writer.WriteAttributeString("type", null, TypeName(value));
                    writer.WriteAttributeString("serializeAs", null, valueString != null ? "String" : "Xml");
                    if (valueString != null)
                        writer.WriteString(valueString);
                    else
                        new XmlSerializer(value.GetType()).Serialize(writer, value);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    if (m_buffer != null)
                        m_buffer.Load(writer);
                }
                return;
            }

            // The file exists
            XmlDocument doc = GetXmlDocument();
            XmlElement root = doc.DocumentElement;

            // Get the section element and add it if it's not there
            XmlNode sectionNode = root.SelectSingleNode(GetSectionsPath(section));
            if (sectionNode == null)
            {
                XmlElement element = doc.CreateElement("section");
                AddAttribute(element, "name", section);
                sectionNode = root.AppendChild(element);
            }

            // Get the entry element and add it if it's not there; otherwise, clean it
            XmlNode entryNode = sectionNode.SelectSingleNode(GetEntryPath(entry));
            if (entryNode == null)
                entryNode = sectionNode.AppendChild(doc.CreateElement("entry"));
            else
                entryNode.RemoveAll();

            AddAttribute(entryNode, "name", entry);
            if (addType)
                AddAttribute(entryNode, "type", TypeName(value));
            AddAttribute(entryNode, "serializeAs", valueString != null ? "String" : "Xml");

            if (valueString != null)
                entryNode.InnerText = valueString;
            else
            {
                using (XmlWriter writer = entryNode.CreateNavigator().AppendChild())
                {
                    writer.WriteWhitespace(""); // hack to avoid exception
                    new XmlSerializer(value.GetType()).Serialize(writer, value);
                }
            }

            Save(doc);
        }

        protected object DoGetValue(string section, string entry, Type type)
        {
            XmlDocument doc = GetXmlDocument();
            XmlElement root = doc != null ? doc.DocumentElement : null;
            XmlNode entryNode = root != null ? root.SelectSingleNode(GetSectionsPath(section) + "/" + GetEntryPath(entry)) : null;
            if (entryNode == null)
                return null;

            XmlAttribute attribute = entryNode.Attributes["type"];
            if (attribute != null)
            {
                Type t = Type.GetType(attribute.Value);
                if(t != null)
                    type = t;   // override type
            }
            if (type == null)
                throw new InvalidOperationException("Type not specified or invalid.");

            attribute = entryNode.Attributes["serializeAs"];
            if (attribute == null)
                throw new InvalidOperationException("Serialization not specified.");
            switch (attribute.Value)
            {
                case "Xml":
                    using (StringReader sb = new StringReader(entryNode.InnerXml))
                        return new XmlSerializer(type).Deserialize(sb);

                case "String":
                    return entryNode.InnerText != null ? TypeDescriptor.GetConverter(type).ConvertFromString(entryNode.InnerText) : null;

                default:
                    throw new InvalidOperationException("Unknown serialization.");
            }
        }

        protected void RemoveEntry(string section, string entry)
        {
            // Get the entry's node, if it exists
            XmlDocument doc = GetXmlDocument();
            XmlNode entryNode = doc != null ? doc.DocumentElement.SelectSingleNode(GetSectionsPath(section) + "/" + GetEntryPath(entry)) : null;
            if (entryNode == null)
                return;

            entryNode.ParentNode.RemoveChild(entryNode);
            Save(doc);
        }

        private string GetSectionsPath(string section) { return "section[@name=\"" + section + "\"]"; }
        private string GetEntryPath(string entry) { return "entry[@name=\"" + entry + "\"]"; }

        protected XmlDocument GetXmlDocument()
        {
            if (m_buffer != null)
                return m_buffer.XmlDocument;

            if (!File.Exists(Name))
                return null;

            XmlDocument doc = new XmlDocument();
            doc.Load(Name);
            return doc;
        }

        protected void AddAttribute(XmlNode node, string name, string value)
        {
            XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            node.Attributes.Append(attribute);
        }
        
        protected void Save(XmlDocument doc)
        {
            if (m_buffer != null)
                m_buffer.m_needsFlushing = true;
            else
                doc.Save(Name);
        }

        protected string TypeName(object value)
        {
            return value.GetType().AssemblyQualifiedName;
            //Type t = value.GetType();
            //string s = t.AssemblyQualifiedName;
            //return s.Substring(0, s.IndexOf(',')) + "," + t.Assembly.GetName().Name;
        }
    }
}

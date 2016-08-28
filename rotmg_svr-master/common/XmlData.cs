using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using System.Reflection;
using log4net;

namespace common
{
    public class XmlData : IDisposable
    {
        static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        static ILog log = LogManager.GetLogger(typeof(XmlData));

        public XmlData(string path = "data")
        {
            ObjectTypeToElement = new ReadOnlyDictionary<ushort, XElement>(
                type2elem_obj = new Dictionary<ushort, XElement>());

            ObjectTypeToId = new ReadOnlyDictionary<ushort, string>(
                type2id_obj = new Dictionary<ushort, string>());
            IdToObjectType = new ReadOnlyDictionary<string, ushort>(
                id2type_obj = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase));

            TileTypeToElement = new ReadOnlyDictionary<ushort, XElement>(
                type2elem_tile = new Dictionary<ushort, XElement>());

            TileTypeToId = new ReadOnlyDictionary<ushort, string>(
                type2id_tile = new Dictionary<ushort, string>());
            IdToTileType = new ReadOnlyDictionary<string, ushort>(
                id2type_tile = new Dictionary<string, ushort>(StringComparer.InvariantCultureIgnoreCase));

            Tiles = new ReadOnlyDictionary<ushort, TileDesc>(
                tiles = new Dictionary<ushort, TileDesc>());
            Items = new ReadOnlyDictionary<ushort, Item>(
                items = new Dictionary<ushort, Item>());
            ObjectDescs = new ReadOnlyDictionary<ushort, ObjectDesc>(
                objDescs = new Dictionary<ushort, ObjectDesc>());
            Portals = new ReadOnlyDictionary<ushort, PortalDesc>(
                portals = new Dictionary<ushort, PortalDesc>());

            this.addition = new XElement("ExtData");

            assign = new AutoAssign(this);

            string basePath = Path.Combine(AssemblyDirectory, path);
            log.InfoFormat("Loading game data from '{0}'...", basePath);
            var xmls = Directory.EnumerateFiles(basePath, "*.xml", SearchOption.AllDirectories).ToArray();
            for (int i = 0; i < xmls.Length; i++)
            {
                log.InfoFormat("Loading '{0}'({1}/{2})...", xmls[i], i + 1, xmls.Length);
                using (Stream stream = File.OpenRead(xmls[i]))
                    ProcessXml(XElement.Load(stream));
            }
            log.Info("Finish loading game data.");
            log.InfoFormat("{0} Items", items.Count);
            log.InfoFormat("{0} Tiles", tiles.Count);
            log.InfoFormat("{0} Objects", objDescs.Count);
            log.InfoFormat("{0} Additions", addition.Elements().Count());
        }

        class AutoAssign : SimpleSettings
        {
            XmlData dat;
            ushort nextSignedId;
            ushort nextFullId;
            internal AutoAssign(XmlData dat)
                : base("autoId")
            {
                this.dat = dat;
                nextSignedId = GetValue<ushort>("nextSigned", "24576"); //0x6000
                nextFullId = GetValue<ushort>("nextFull", "32768");     //0x8000
            }

            public ushort Assign(string id, XElement elem)
            {
                ushort type = GetValue<ushort>(id, "0");
                if (type == 0)
                {
                    var cls = elem.Element("Class");
                    bool isFull = cls.Value == "Dye" ||
                        (cls.Value == "Equipment" && !elem.Elements("Projectile").Any());
                    if (isFull)
                    {
                        type = nextFullId++;
                        SetValue("nextFull", nextFullId.ToString());
                    }
                    else
                    {
                        type = nextSignedId++;
                        SetValue("nextSigned", nextSignedId.ToString());
                    }
                    SetValue(id, type.ToString());
                    log.InfoFormat("Auto assigned '{0}' to 0x{1:x4}", id, type);
                }
                return type;
            }
        }

        AutoAssign assign;
        public void AddObjects(XElement root)
        {
            foreach (var elem in root.XPathSelectElements("//Object"))
            {
                if (elem.Element("Class") == null) continue;
                string cls = elem.Element("Class").Value;
                string id = elem.Attribute("id").Value;

                ushort type;
                var typeAttr = elem.Attribute("type");
                if (typeAttr == null)
                    type = assign.Assign(id, elem);
                else
                    type = (ushort)Utils.FromString(typeAttr.Value);

                if (type2id_obj.ContainsKey(type))
                    log.WarnFormat("'{0}' and '{1}' has the same ID of 0x{2:x4}!", id, type2id_obj[type], type);
                if (id2type_obj.ContainsKey(id))
                    log.WarnFormat("0x{0:x4} and 0x{1:x4} has the same name of {2}!", type, id2type_obj[id], id);

                type2id_obj[type] = id;
                id2type_obj[id] = type;
                type2elem_obj[type] = elem;

                switch (cls)
                {
                    case "Equipment":
                        items[type] = new Item(type, elem);
                        break;
                    case "Dye":
                        items[type] = new Item(type, elem);
                        break;
                    case "Portal":
                        try
                        {
                            portals[type] = new PortalDesc(type, elem);
                        }
                        catch
                        {
                            Console.WriteLine("Error for portal: " + type + " id: " + id);
                            /*3392,1792,1795,1796,1805,1806,1810,1825 -- no location, assume nexus?* 
        *  Tomb Portal of Cowardice,  Dungeon Portal,  Portal of Cowardice,  Realm Portal,  Glowing Portal of Cowardice,  Glowing Realm Portal,  Nexus Portal,  Locked Wine Cellar Portal*/
                        }
                        break;
                    default:
                        objDescs[type] = new ObjectDesc(type, elem);
                        break;
                }

                var extAttr = elem.Attribute("ext");
                bool ext;
                if (extAttr != null && bool.TryParse(extAttr.Value, out ext) && ext)
                {
                    if (elem.Attribute("type") == null)
                        elem.Add(new XAttribute("type", type));
                    this.addition.Add(elem);
                    updateCount++;
                }
            }
        }
        public void AddGrounds(XElement root)
        {
            foreach (var elem in root.XPathSelectElements("//Ground"))
            {
                string id = elem.Attribute("id").Value;

                ushort type;
                var typeAttr = elem.Attribute("type");
                type = (ushort)Utils.FromString(typeAttr.Value);

                if (type2id_tile.ContainsKey(type))
                    log.WarnFormat("'{0}' and '{1}' has the same ID of 0x{2:x4}!", id, type2id_tile[type], type);
                if (id2type_tile.ContainsKey(id))
                    log.WarnFormat("0x{0:x4} and 0x{1:x4} has the same name of {2}!", type, id2type_tile[id], id);

                type2id_tile[type] = id;
                id2type_tile[id] = type;
                type2elem_tile[type] = elem;

                tiles[type] = new TileDesc(type, elem);

                var extAttr = elem.Attribute("ext");
                bool ext;
                if (extAttr != null && bool.TryParse(extAttr.Value, out ext) && ext)
                {
                    this.addition.Add(elem);
                    updateCount++;
                }
            }
        }
        void ProcessXml(XElement root)
        {
            AddObjects(root);
            AddGrounds(root);
        }


        Dictionary<ushort, XElement> type2elem_obj;
        Dictionary<ushort, string> type2id_obj;
        Dictionary<string, ushort> id2type_obj;

        Dictionary<ushort, XElement> type2elem_tile;
        Dictionary<ushort, string> type2id_tile;
        Dictionary<string, ushort> id2type_tile;

        Dictionary<ushort, TileDesc> tiles;
        Dictionary<ushort, Item> items;
        Dictionary<ushort, ObjectDesc> objDescs;
        Dictionary<ushort, PortalDesc> portals;


        public IDictionary<ushort, XElement> ObjectTypeToElement { get; private set; }

        public IDictionary<ushort, string> ObjectTypeToId { get; private set; }
        public IDictionary<string, ushort> IdToObjectType { get; private set; }

        public IDictionary<ushort, XElement> TileTypeToElement { get; private set; }

        public IDictionary<ushort, string> TileTypeToId { get; private set; }
        public IDictionary<string, ushort> IdToTileType { get; private set; }

        public IDictionary<ushort, TileDesc> Tiles { get; private set; }
        public IDictionary<ushort, Item> Items { get; private set; }
        public IDictionary<ushort, ObjectDesc> ObjectDescs { get; private set; }
        public IDictionary<ushort, PortalDesc> Portals { get; private set; }

        int updateCount = 0;
        int prevUpdateCount = -1;
        XElement addition;
        string[] addXml;

        void UpdateXml()
        {
            if (prevUpdateCount != updateCount)
            {
                addXml = new string[] { addition.ToString() };
                prevUpdateCount = updateCount;
            }
        }

        public string[] AdditionXml
        {
            get
            {
                UpdateXml();
                return addXml;
            }
        }

        public void Dispose()
        {
            assign.Dispose();
        }
    }
}
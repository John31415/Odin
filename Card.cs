using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    public class Card : IClass
    {
        public Dictionary<string, object> properties { get; set; }
        public long Owner { get; set; }
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public long Power { get; set; }
        public string Range { get; set; }
        public string Description { get; set; }

        public Card(int owner, int id, string type, string name, string faction, int power, string range, string description)
        {
            Owner = owner;
            Id = id;
            Type = type;
            Name = name;
            Faction = faction;
            Power = power;
            Range = range;
            Description = description;
            properties = new Dictionary<string, object>();
            properties["Owner"] = Owner;
            properties["Type"] = Type;
            properties["Name"] = Name;
            properties["Faction"] = Faction;
            properties["Power"] = Power;
            properties["Range"] = Range;
        }

        public void Set(string prop, object value)
        {
            long val1 = 0;
            string val2 = "";
            if(value is long) val1 = (long)value;
            if(value is string) val2 = (string)value;
            if (prop == "Owner")
            {
                Owner = val1;
                properties[prop] = val1;
            }
            if (prop == "Type")
            {
                Type = val2;
                properties[prop] = val2;
            }
            if (prop == "Name")
            {
                Name = val2;
                properties[prop] = val2;
            }
            if (prop == "Faction")
            {
                Faction = val2;
                properties[prop] = val2;
            }
            if (prop == "Power")
            {
                Power = val1;
                properties[prop] = val1;
            }
            if (prop == "Range")
            {
                Range = val2;
                properties[prop] = val2;
            }
        }
    }
}

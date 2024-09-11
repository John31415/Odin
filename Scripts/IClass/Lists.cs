using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    public class Lists : IClass
    {
        public Dictionary<string, object> properties { get; set; }
        public List<Card> Cards { get; set; }

        public Lists()
        {
            Cards = new List<Card>();
            properties = new Dictionary<string, object>();
            //properties["Find"] =
            properties["Push"] = new Push();
            properties["SendBottom"] = new SendBottom();
            properties["Pop"] = new Pop();
            properties["Remove"] = new Remove();
            properties["Shuffle"] = new Shuffle();
        }

        public Lists(List<Card> cards)
        {
            Cards = cards;
            properties = new Dictionary<string, object>();
            //properties["Find"] =
            properties["Push"] = new Push();
            properties["SendBottom"] = new SendBottom();
            properties["Pop"] = new Pop();
            properties["Remove"] = new Remove();
            properties["Shuffle"] = new Shuffle();
        }
    }
}

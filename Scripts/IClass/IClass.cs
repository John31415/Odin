using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    public interface IClass
    {
        Dictionary<string, object> properties { get; set; }
    }
}

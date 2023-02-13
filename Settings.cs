using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMX_StartupSetter
{
    public class Settings
    {
        public string ConfigFile { set; get; } = "data.json";
        public string SourceName { set; get; } = "Basic Dimmer";
        public string Destination { set; get; } = "127.0.0.1";
        public int Universe { set; get; } = 1;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RozumConnectionLib
{
    public class Gripper
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("tcp")]
        public Position Tcp { get; set; }
        [JsonProperty("shape")]
        public List<CapsuleObstacle> Shape { get; set; }

    }
}

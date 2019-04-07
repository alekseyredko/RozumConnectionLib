using Newtonsoft.Json;
using System.Collections.Generic;

namespace RozumConnectionLib
{
    public class Gripper
    {
        [JsonProperty("name")]
        public string Name { get; set; }        
        [JsonProperty("point")]
        public Point Point { get; set; }
        [JsonProperty("rotation")]
        public Rotation Rotation { get; set; }
        [JsonProperty("shape")]
        public List<CapsuleObstacle> Shape { get; set; }

    }
}

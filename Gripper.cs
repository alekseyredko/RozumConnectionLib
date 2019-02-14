using Newtonsoft.Json;
using System.Collections.Generic;

namespace RozumConnectionLib
{
    public class Gripper
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("tcp")]
        public Position Tcp { get; set; }
        [JsonProperty("shape")]
        public List<CapsuleObstacle> Shape { get; set; }

    }
}

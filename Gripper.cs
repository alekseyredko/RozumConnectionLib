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
        [JsonProperty("radius")]
        public double Radius { get; set; }
        [JsonProperty("point")]
        public Point Point { get; set; }
        [JsonProperty("rotation")]
        public Rotation Rotation { get; set; }
        [JsonProperty("shape")]
        public Obstacle Shape { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}; Point: {Point}; Rotation: {Rotation}; Radius: {Radius}; Shape: {Shape}";
        }
    }
}

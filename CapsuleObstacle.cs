using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RozumConnectionLib
{
    public class Obstacle
    {
        [JsonProperty("obstacle_type")]
        public string ObstacleType { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class CapsuleObstacle: Obstacle
    {
        [JsonProperty("radius")]
        public float Radius { get; set; }
        [JsonProperty("start_point")]
        public Point StartPoint { get; set; }
        [JsonProperty("end_point")]
        public Point EndPoint { get; set; }
    }

    public class PlaneObstacle: Obstacle
    {
        [JsonProperty("points")]
        public List<Point> Points { get; set; }
    }

    public class BoxObstacle: Obstacle
    {
        [JsonProperty("sides")]
        public List<Point> Sides { get; set; }
        [JsonProperty("center_position")]
        public Position CenterPosition { get; set; }
    }
}

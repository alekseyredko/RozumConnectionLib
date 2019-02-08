using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RozumConnectionLib
{
    public class Position
    {
        [JsonIgnore]
        public string Name { get; set; }
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty("point")]
        public Point Point { get; set; }
        [JsonProperty("rotation")]
        public Rotation Rotation { get; set; }       

        public double this[int index]
        {
            get
            {
                if (index < 0 || index > 5) throw new IndexOutOfRangeException();
                if (index >= 0 && index <= 2) return Point[index];
                return Rotation[index];
            }
            set
            {
                if (index < 0 || index > 5) throw new IndexOutOfRangeException();
                if (index >= 0 && index <= 2) Point[index] = value;
                else Rotation[index] = value;
            }
        }

        public Position(IEnumerable<double> position)
        {
            var enumerable = position as double[] ?? position.ToArray();
            Point = new Point(enumerable.Take(3));
            Rotation = new Rotation(enumerable.Skip(3));
        }

        public Position()
        {
            Point = new Point();
            Rotation = new Rotation();
        }
       
        public override string ToString()
        {
            return $"Point: {Point}; Rotation: {Rotation};";
        }

        public double[] ToArray()
        {
            return Point.ToArray().Concat(Rotation.ToArray()).ToArray();
        }
    }
}

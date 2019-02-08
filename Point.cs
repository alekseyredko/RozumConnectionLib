using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SQLite;

namespace RozumConnectionLib
{
    public class Point
    {
        [JsonProperty("x")]
        public double X { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }
        [JsonProperty("z")]
        public double Z { get; set; }
       
        public double this[int index]
        {
            get
            {
                if (index < 0 && index > 2) return double.NaN;
                return ToArray()[index];
            }
            set
            {                
                switch (index)
                {
                    case 0:
                        X = value;
                        return;
                    case 1:
                        Y = value;
                        return;
                    case 2:
                        Z = value;
                        return;
                    default:
                        return;
                }
            }
        }

        public Point(){}

        public Point(IEnumerable<double> point)
        {
            var enumerable = point as double[] ?? point.ToArray();
            X = enumerable.ElementAt(0);
            Y = enumerable.ElementAt(1);
            Z = enumerable.ElementAt(2);
        }

        public double[] ToArray() => new[] {X, Y, Z};

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }
}
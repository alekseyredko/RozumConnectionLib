using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    [Serializable]
    public class Point: ISerializable
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public IEnumerable<double> Coordinate
        {
            get => new[] {X, Y, Z};
            set
            {
                if (value.Count() != 3) return;
                X = value.ElementAt(0);
                Y = value.ElementAt(1);
                Z = value.ElementAt(2);
            }
        }

        public Point(){}

        public Point(SerializationInfo info, StreamingContext context)
        {
            Coordinate = new List<double>
            {
                info.GetDouble("x"), info.GetDouble("y"), info.GetDouble("z")
            };
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", X);
            info.AddValue("y", Y);
            info.AddValue("z", Z);
        }

        public double[] ToArray() => new[] {X, Y, Z};

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }
}
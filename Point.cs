using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SQLite;

namespace RozumConnectionLib
{
    [Serializable]
    public class Point: ISerializable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        [Ignore]
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RozumConnectionLib
{
    [Serializable]
    public class Point: ISerializable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        [Ignore]
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

        public Point(SerializationInfo info, StreamingContext context)
        {
            X = info.GetDouble("x");
            Y = info.GetDouble("y");
            Z = info.GetDouble("z");           
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
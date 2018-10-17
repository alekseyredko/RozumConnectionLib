using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{    
    public class Point: ICloneable
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

        public double[] ToArray() => new[] {X, Y, Z};

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
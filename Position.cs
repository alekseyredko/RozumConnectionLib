using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{    
    public class Position: ICloneable
    {
        public Point Point { get; set; }
        public Rotation Rotation { get; set; }

        public IEnumerable<double> Array
        {
            get => Point.Coordinate.Concat(Rotation.Angles);
            set
            {
                if(value.Count()!=6) return;
                Point = new Point {Coordinate = value.Take(3)};
                Rotation = new Rotation {Angles = value.Skip(3)};
            }
        }

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

        public Position()
        {
            Point = new Point();
            Rotation = new Rotation();
        }

        public override string ToString()
        {
            return $"Point: {Point}; Rotation: {Rotation};";
        }

        public object Clone()
        {
            var position = (Position)this.MemberwiseClone();
            position.Point = (Point)this.Point.Clone();
            position.Rotation = (Rotation) this.Rotation.Clone();
            return position;
        }

        public double[] ToArray()
        {
            return Point.Coordinate.Concat(Rotation.Angles).ToArray();
        }
    }
}

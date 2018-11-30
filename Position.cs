using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    [Serializable]
    public class Position: ISerializable
    {
        [Key]
        public int Id { get; set; }
        public Point Point { get; set; }
        public Rotation Rotation { get; set; }
        [NotMapped]
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

        public Position(SerializationInfo info, StreamingContext context)
        {
            var point = (Point)info.GetValue("point", typeof(Point));
            var rotation = (Rotation)info.GetValue("rotation", typeof(Rotation));
            Point = point;
            Rotation = rotation;
        }

        public override string ToString()
        {
            return $"Point: {Point}; Rotation: {Rotation};";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("point", Point);
            info.AddValue("rotation", Rotation);
        }

        public double[] ToArray()
        {
            return Point.Coordinate.Concat(Rotation.Angles).ToArray();
        }
    }
}

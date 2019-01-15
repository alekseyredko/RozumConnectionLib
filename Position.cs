﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RozumConnectionLib
{
    [Serializable]
    public class Position: ISerializable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }    
        [ForeignKey(typeof(Point))]
        public int PointKey { get; set; }
        [ForeignKey(typeof(Rotation))]
        public int RotationKey { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Point Point { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Rotation Rotation { get; set; }       
        public string Name { get; set; }
        [Ignore]
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
            return Point.ToArray().Concat(Rotation.ToArray()).ToArray();
        }
    }
}

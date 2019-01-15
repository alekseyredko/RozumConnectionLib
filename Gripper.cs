using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    [Serializable]
    public class Gripper: ISerializable
    {      
        public Gripper()
        {
            Name = "";
            Radius = 0;
            Point = new Point();
            Rotation = new Rotation();
        }

        public Gripper(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Radius = info.GetDouble("radius");
            Point = (Point)info.GetValue("point", typeof(Point));
            Rotation = (Rotation) info.GetValue("rotation", typeof(Rotation));           
        }

        public string Name { get; set; }

        public double Radius { get; set; }
        
        public int Id { get; set; }        
        
        public Point Point { get; set; }
        
        public Rotation Rotation { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("point", Point);
            info.AddValue("rotation", Rotation);
            info.AddValue("radius", Radius);
        }

        public override string ToString()
        {
            return $"Name: {Name}; Point: {Point}; Rotation: {Rotation}; Radius: {Radius};";
        }
    }
}

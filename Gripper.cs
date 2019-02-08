using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
            Shape = (Shape)info.GetValue("shape", typeof(Shape));
        }

        public string Name { get; set; }

        public double Radius { get; set; }

        public Point Point { get; set; }

        public Rotation Rotation { get; set; }

        public Shape Shape { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("point", Point);
            info.AddValue("rotation", Rotation);
            info.AddValue("radius", Radius);
            info.AddValue("shape", Radius);
        }

        public override string ToString()
        {
            return $"Name: {Name}; Point: {Point}; Rotation: {Rotation}; Radius: {Radius};";
        }
    }
}

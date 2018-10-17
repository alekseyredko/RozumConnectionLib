using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RozumConnectionLib
{    
    public class Gripper: ICloneable
    {      
        public Gripper()
        {
            Name = "";
            Radius = 0;
            Point = new Point();
            Rotation = new Rotation();
        }
      

        public string Name { get; set; }

        public double Radius { get; set; }

        public Point Point { get; set; }

        public Rotation Rotation { get; set; }
      
        public override string ToString()
        {
            return $"Name: {Name}; Point: {Point}; Rotation: {Rotation}; Radius: {Radius};";
        }

        public object Clone()
        {
            var gripper = (Gripper)this.MemberwiseClone();
            gripper.Point = (Point) this.Point.Clone();
            gripper.Rotation = (Rotation) this.Rotation.Clone();
            return gripper;
        }
    }
}

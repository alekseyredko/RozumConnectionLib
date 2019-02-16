using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RozumConnectionLib
{
    public class Pose
    {
        [JsonProperty("angles")]
        public List<double> Angles;
             
        public Pose()
        {
            Angles = new List<double>();
        }

        public Pose(IEnumerable<double> angles)
        {
            Angles = angles.ToList();
        }

        public double this[int index]
        {
            get
            {
                if(index<0 && index>5) throw new IndexOutOfRangeException();
                return Angles[index];
            }
            set
            {
                if (index < 0 && index > 5) throw new IndexOutOfRangeException();
                Angles[index] = value;
            }
        }

        public double[] ToArray()
        {
            return Angles.ToArray();
        }

        public override string ToString()
        {
            return string.Join(", ", Angles);
        }
    }
}

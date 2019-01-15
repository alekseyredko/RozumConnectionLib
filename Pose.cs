using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    [Serializable]
    public class Pose: ISerializable
    {        
        public List<double> Angles;

        [IgnoreDataMember]
        public int Id { get; set; }
        [IgnoreDataMember]
        public string Name { get; set; }

        public Pose()
        {
            Angles = new List<double>();
        }

        public Pose(IEnumerable<double> angles)
        {
            Angles = angles.ToList();
        }

        public Pose(SerializationInfo info, StreamingContext context)
        {
            Angles = info.GetValue("angles", typeof(List<double>)) as List<double>;
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("angles", Angles);
        }
    }
}

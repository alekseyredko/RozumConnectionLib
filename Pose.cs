using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    [Serializable]
    public class Pose: ISerializable
    {        
        private double[] _angles;
             
        public Pose()
        {
            _angles = new double[6];
        }

        public Pose(IEnumerable<double> angles)
        {
            _angles = angles.ToArray();
        }

        public Pose(SerializationInfo info, StreamingContext context)
        {
            _angles = info.GetValue("angles", typeof(double[])) as double[];
        }

        public double this[int index]
        {
            get
            {
                if(index<0 && index>5) throw new IndexOutOfRangeException();
                return _angles[index];
            }
            set
            {
                if (index < 0 && index > 5) throw new IndexOutOfRangeException();
                _angles[index] = value;
            }
        }

        public double[] ToArray()
        {
            return _angles;
        }

        public override string ToString()
        {
            return string.Join(", ", _angles);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("angles", _angles);
        }
    }
}

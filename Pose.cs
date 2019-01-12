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
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        
        private double[] _angles;

        [Ignore]
        public IEnumerable<double> Angles
        {
            get => _angles;
            set => _angles = value.ToArray();
        }

        public Pose()
        {
            _angles = new double[6];
        }

        public Pose(IEnumerable<double> angles)
        {
            Angles = angles;
        }

        public Pose(SerializationInfo info, StreamingContext context)
        {
            Angles = info.GetValue("angles", typeof(IEnumerable<double>)) as IEnumerable<double>;
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

        public override string ToString()
        {
            return string.Join(", ", Angles);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("angles", _angles);
        }
    }
}

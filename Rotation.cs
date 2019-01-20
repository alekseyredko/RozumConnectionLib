using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    [Serializable]
    public class Rotation:ISerializable
    {
        [IgnoreDataMember]
        public int Id { get; set; }
        public double Roll { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }
       
        public double this[int index]
        {
            get
            {
                if (index < 0 && index > 2) throw new IndexOutOfRangeException();
                return ToArray()[index];
            }
            set
            {
                switch (index)
                {
                    case 0:
                        Roll = value;
                        return;
                    case 1:
                        Pitch = value;
                        return;
                    case 2:
                        Yaw = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Rotation(){}

        public Rotation(IEnumerable<double> rotation)
        {
            var enumerable = rotation as double[] ?? rotation.ToArray();
            Roll = enumerable.ElementAt(0);
            Pitch = enumerable.ElementAt(1);
            Yaw = enumerable.ElementAt(2);
        }

        public Rotation(SerializationInfo info, StreamingContext context)
        {
            Roll = info.GetDouble("roll");
            Pitch = info.GetDouble("pitch");
            Yaw = info.GetDouble("yaw");            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("roll", Roll);
            info.AddValue("pitch", Pitch);
            info.AddValue("yaw", Yaw);
        }

        public double[] ToArray() => new[] {Roll, Pitch, Yaw};

        public override string ToString()
        {
            return $"Roll: {Roll}, Pitch: {Pitch}, Yaw: {Yaw}";
        }
    }
}

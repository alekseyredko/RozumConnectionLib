using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RozumConnectionLib
{
    [Serializable]
    public class Rotation:ISerializable
    {
        public double Roll { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }

        public IEnumerable<double> Angles
        {
            get => new[] {Roll, Pitch, Yaw};
            set
            {
                if(value.Count()!=3) return;
                Roll = value.ElementAt(0);
                Pitch = value.ElementAt(1);
                Yaw = value.ElementAt(2);
            }
        }

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

        public Rotation(SerializationInfo info, StreamingContext context)
        {
            Angles = new List<double>
            {
                info.GetDouble("roll"), info.GetDouble("pitch"), info.GetDouble("yaw")
            };
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

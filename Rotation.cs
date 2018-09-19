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

        public override string ToString()
        {
            return $"Roll: {Roll}, Pitch: {Pitch}, Yaw: {Yaw}";
        }
    }
}

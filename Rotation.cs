using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace RozumConnectionLib
{
    public class Rotation
    {
        [JsonProperty("roll")]
        public double Roll { get; set; }
        [JsonProperty("pitch")]
        public double Pitch { get; set; }
        [JsonProperty("yaw")]
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

        public double[] ToArray() => new[] {Roll, Pitch, Yaw};

        public override string ToString()
        {
            return $"Roll: {Roll}, Pitch: {Pitch}, Yaw: {Yaw}";
        }
    }
}

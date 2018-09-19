using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    public class MotorStatus: ISerializable
    {
        public double[] Temperature{get;set;}
        public double[] Amperage{get;set;}
        public MotorStatus()
        {
            Temperature = new double[6];
            Amperage = new double[6];
        }

        public MotorStatus(SerializationInfo info, StreamingContext context)
        {
            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //throw new System.NotImplementedException();
        }
    }
}

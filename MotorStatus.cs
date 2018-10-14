using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    public class MotorStatus
    {
        public IEnumerable<JointStatus> Joints { get; set; }

        public MotorStatus()
        {
            Joints = new[]
            {
                new JointStatus(), 
                new JointStatus(), 
                new JointStatus(), 
                new JointStatus(), 
                new JointStatus(),
                new JointStatus()
            };
        }       
    }

    public class JointStatus
    {
        public JointStatus()
        {
        }

        public double Angle { get; set; }
        public double RotorVelocity { get; set; }
        public double RmsCurrent { get; set; }
        public double Voltage { get; set; }
        public double PhaseCurrent { get; set; }
        public double StatorTemperature { get; set; }
        public double ServoTemperature { get; set; }
        public double VelocityError { get; set; }
        public double VelocitySetPoint { get; set; }
        public double VelocityFeedback { get; set; }
        public double VelocityOutput { get; set; }
        public double PositionError { get; set; }
        public double PositionSetPoint { get; set; }
        public double PositionFeedback { get; set; }
        public double PositionOutput { get; set; }
    }   
}
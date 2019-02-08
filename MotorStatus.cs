using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RozumConnectionLib
{
    public class MotorStatus
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty("joints")]
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
        [JsonIgnore]
        public int Id { get; set; }
        public JointStatus()
        {
        }
        [JsonProperty("angle")]
        public float Angle { get; set; }
        [JsonProperty("rotor_velocity")]
        public float RotorVelocity { get; set; }
        [JsonProperty("rms_current")]
        public float RmsCurrent { get; set; }
        [JsonProperty("voltage")]
        public float Voltage { get; set; }
        [JsonProperty("phase_current")]
        public float PhaseCurrent { get; set; }
        [JsonProperty("stator_temperature")]
        public float StatorTemperature { get; set; }
        [JsonProperty("servo_temperature")]
        public float ServoTemperature { get; set; }
        [JsonProperty("velocity_error")]
        public float VelocityError { get; set; }
        [JsonProperty("velocity_setpoint")]
        public float VelocitySetPoint { get; set; }
        [JsonProperty("velocity_feedback")]
        public float VelocityFeedback { get; set; }
        [JsonProperty("velocity_output")]
        public float VelocityOutput { get; set; }
        [JsonProperty("position_error")]
        public float PositionError { get; set; }
        [JsonProperty("position_setpoint")]
        public float PositionSetPoint { get; set; }
        [JsonProperty("position_feedback")]
        public float PositionFeedback { get; set; }
        [JsonProperty("position_output")]
        public float PositionOutput { get; set; }
    }   
}
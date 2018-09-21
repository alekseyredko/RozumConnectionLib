using System.Runtime.Serialization;

namespace RozumConnectionLib
{
    //TODO: дополнить класс согласно добавленным полям
    //[{"angle":90.00067,"rotorVelocity":3.2169332E-14,"rmsCurrent":0.004157792,"voltage":47.779926,"phaseCurrent":-0.25154263,"statorTemperature":28.068298,"servoTemperature":35.608078,"velocityError":156.95378,"velocitySetpoint":3.3687648E-15,"velocityFeedback":157.08081,"velocityOutput":1.0697115E-7,"positionError":-6.713867E-4,"positionSetpoint":90.0,"positionFeedback":90.00067,"positionOutput":0.0},{"angle":-0.0013427734,"rotorVelocity":-5.728508E-39,"rmsCurrent":0.022188274,"voltage":47.764835,"phaseCurrent":-0.95467126,"statorTemperature":28.87384,"servoTemperature":34.96337,"velocityError":-0.043006897,"velocitySetpoint":-3.3811733E-37,"velocityFeedback":-0.0011717909,"velocityOutput":-3.2667933E-24,"positionError":0.001341582,"positionSetpoint":-2.7840837E-11,"positionFeedback":-0.0013427734,"positionOutput":0.0},{"angle":-0.006866455,"rotorVelocity":1.8278054,"rmsCurrent":0.013658118,"voltage":47.583733,"phaseCurrent":0.7769646,"statorTemperature":30.795898,"servoTemperature":36.25274,"velocityError":0.11828613,"velocitySetpoint":-1.0521758E-31,"velocityFeedback":-0.013156016,"velocityOutput":-5.8834594E-5,"positionError":0.0068676467,"positionSetpoint":1.8984148E-11,"positionFeedback":-0.006866455,"positionOutput":0.0},{"angle":-89.9644,"rotorVelocity":-1.0356051E-19,"rmsCurrent":0.006196696,"voltage":47.779926,"phaseCurrent":-0.43654698,"statorTemperature":31.785034,"servoTemperature":41.732597,"velocityError":-157.27646,"velocitySetpoint":-6.11111E-18,"velocityFeedback":-157.01752,"velocityOutput":-0.0,"positionError":-0.035598755,"positionSetpoint":-90.0,"positionFeedback":-89.9644,"positionOutput":0.0},{"angle":-0.001373291,"rotorVelocity":0.0,"rmsCurrent":0.0012400642,"voltage":47.598824,"phaseCurrent":-0.14242665,"statorTemperature":34.239258,"servoTemperature":39.798523,"velocityError":0.12060547,"velocitySetpoint":0.0,"velocityFeedback":-0.0023947184,"velocityOutput":-19.140734,"positionError":0.0013732888,"positionSetpoint":-8.6764155E-12,"positionFeedback":-0.001373291,"positionOutput":0.0},{"angle":3.33786E-6,"rotorVelocity":-0.0,"rmsCurrent":8.5708394E-4,"voltage":47.870476,"phaseCurrent":-0.10038215,"statorTemperature":35.102264,"servoTemperature":41.732597,"velocityError":-0.0032396317,"velocitySetpoint":-0.0,"velocityFeedback":5.825672E-6,"velocityOutput":-6.027853E-5,"positionError":-3.3378603E-6,"positionSetpoint":-1.8514179E-13,"positionFeedback":3.33786E-6,"positionOutput":0.0}]
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

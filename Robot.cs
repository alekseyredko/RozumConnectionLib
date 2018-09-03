using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace RozumConnectionLib
{
    public abstract class Robot
    {
        public double[] JointAngles { get; set; }
        public RobotPosition Position { get; set; }
        public abstract Task<string> GetPositionAsync();
        public abstract Task<string> GetJointAnglesAsync();
        public abstract Task<string> SetPositionAsync(double[] position, int speed);
        public abstract Task<string> SetJointAnglesAsync(double[] angles, int speed);
    }

    public class RobotPosition: INotifyPropertyChanged
    {
        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Roll { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }

        public double[] Point
        {
            get
            {
                return new double[] { X, Y, Z };
            }
            set
            {
                X = value[0];
                Y = value[1];
                Z = value[2];
            }
        }

        public double[] Rotation
        {
            get
            {
                return new double[] { Roll, Pitch, Yaw };
            }
            set
            {
                Roll = value[0];
                Pitch = value[1];
                Yaw = value[2];
            }
        }

        public double[] Array
        {
            get
            {
                return ToArray();
            }
            set
            {
                X = value[0];
                Y = value[1];
                Z = value[2];
                Roll = value[3];
                Pitch = value[4];
                Yaw = value[5];
            }
        }

        public double[] ToArray()
        {
            return new double[] { X, Y, Z, Roll, Pitch, Yaw };
        }
    }
  
    public class RobotMotorStatus
    {
        public double[] Temperature{get;set;}
        public double[] Amperage{get;set;}
        public RobotMotorStatus()
        {
            Temperature = new double[6];
            Amperage = new double[6];
        }
    }

    public enum RobotStatusMotion
    {
        IDLE, RUNNING, ZERO_GRAVITY, NOT_RESPOND
    }

    public enum RobotMode
    {
        Freeze, Relax
    }
}

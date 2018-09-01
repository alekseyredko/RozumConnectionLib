using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace RozumConnectionLib
{
    public abstract class Robot: INotifyPropertyChanged
    {          
        protected void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private double[] _jointAngles;
        public double[] JointAngles
        {
            get
            {
                return _jointAngles;
            }
            set
            {
                _jointAngles = value;
                RaisePropertyChanged("JointAngles");
            }
        } 

        private RobotPosition _position;
        public RobotPosition Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                RaisePropertyChanged("Position");
            }
        }
        public abstract Task<string> GetPosition();
        public abstract Task<string> GetJointAngles();
        public abstract Task<string> SetPosition(double[] position, int speed);
        public abstract Task<string> SetJointAngles(double[] angles, int speed);
    }

    public class RobotPosition: INotifyPropertyChanged
    {
        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private double _x;
        private double _y;
        private double _z;
        private double _roll;
        private double _pitch;
        private double _yaw;

        public double X 
        { 
            get
            {
                return _x;
            }
            set
            {
                RaisePropertyChanged("X"); 
                _x = value; 
            } 
        }
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                RaisePropertyChanged("Y"); 
                _y = value; 
            } 
        }
        public double Z
        {
            get
            {
                return _z;
            }
            set
            {
                RaisePropertyChanged("Z"); 
                _z = value; 
            } 
        }
        public double Roll
        {
            get
            {
                return _roll;
            }
            set
            {
                RaisePropertyChanged("Roll"); 
                _roll = value; 
            } 
        }
        public double Pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                RaisePropertyChanged("Pitch"); 
                _pitch = value; 
            } 
        }
        public double Yaw
        {
            get
            {
                return _yaw;
            }
            set
            {
                RaisePropertyChanged("Yaw"); 
                _yaw = value; 
            } 
        }

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

    //TODO: валидация данных
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
        IDLE, RUNNING, ZERO_GRAVITY
    }

    public enum RobotMode
    {
        Freeze, Relax
    }
}

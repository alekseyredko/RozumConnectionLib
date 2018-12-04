using System.Collections.Generic;
using System.Threading.Tasks;

namespace RozumConnectionLib
{
    public abstract class Robot
    {
        public string ID { get; set; }
        public Gripper Tool { get; set; }
        public bool[] InputPorts { get; set; }
        public bool[] OutputPorts { get; set; }
        public Pose JointAngles { get; set; }
        public Position Position { get; set; }
        public Position BasePosition { get; set; }
        public abstract Task<string> GetPositionAsync();
        public abstract Task<string> GetPoseAsync();        
    }

    public enum RobotStatusMotion
    {
        IDLE,
        ZERO_GRAVITY,
        RUNNING,
        MOTION_FAILED,
        EMERGENCY,
        ERROR        
    }

    public enum MotionType
    {
        JOINT, LINEAR
    }

    public enum RobotMoveMode
    {
        SPEED,
        TIME
    }

    public enum RobotMode
    {
        Freeze,
        Relax
    }
}
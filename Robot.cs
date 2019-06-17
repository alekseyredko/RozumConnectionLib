using System.Collections.Generic;
using System.Threading.Tasks;

namespace RozumConnectionLib
{
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
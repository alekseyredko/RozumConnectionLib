using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RozumConnectionLib
{
    public class RealRobot : Robot
    {
        private RozumConnection _connection;

        private string _url;

        public RealRobot(string ip)
        {
            Status = RobotStatusMotion.ERROR;
            InitConnection(ip);

            InitValues();
        }

        public RealRobot()
        {
            Status = RobotStatusMotion.ERROR;
            IsConnected = false;

            InitValues();
        }

        public double TcpMoveStep { get; set; }
        public double JointMoveStep { get; set; }
        public int Speed { get; set; }

        public string URL
        {
            get => _url;
            set
            {
                _url = value;
                InitConnection(value);
            }
        }

        public MotorStatus MotorStatus { get; protected set; }

        public RobotMode Mode { get; private set; }

        public RobotStatusMotion Status { get; protected set; }

        public bool IsConnected { get; protected set; }

        public bool IsGripperOpened { get; protected set; }

        private void InitValues()
        {
            ID = "";
            Tool = new Gripper();
            InputPorts = new bool[6];
            OutputPorts = new bool[6];
            JointAngles = new Pose();
            Position = new Position();
            BasePosition = new Position();
            MotorStatus = new MotorStatus();
        }

        private void InitConnection(string ip)
        {
            if (IPAddress.TryParse(ip, out var iP))
            {
                _connection = new RozumConnection($"http://{ip}:8081/");
                //GetStatusMotionAsync().Wait(2000);               
                //IsConnected = Status != RobotStatusMotion.ERROR;
                IsConnected = true;
            }
            else
            {
                _connection = new RozumConnection(URL);
                IsConnected = false;
            }
        }

        public async Task<string> SetDigitalOutput(int port, bool value)
        {
            var response = await _connection.SetDigitalOutput(port, value);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    OutputPorts[port - 1] = value;
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public void WaitForInputSignal(int port, bool state, int askPeriod = 50)
        {
            var response = GetDigitalInputAsync(port).Result;
            var result = state ? "HIGH" : "LOW";
            if (response == "Robot does not respond") return;            
            while (response!=result)
            {
                response = GetDigitalInputAsync(port).Result;
                Thread.Sleep(askPeriod);
            }
        }

        public void WaitForOutputSignal(int port, bool state, int askPeriod = 50)
        {
            var response = GetDigitalOutputAsync(port).Result;
            var result = state ? "HIGH" : "LOW";
            if (response == "Robot does not respond") return; 
            while (response!=result)
            {
                response = GetDigitalInputAsync(port).Result;
                Thread.Sleep(askPeriod);
            }
        }

        public async Task<string> SetModeAsync(RobotMode mode)
        {
            HttpResponseMessage response;
            if (mode == RobotMode.Freeze)
            {
                response = await _connection.SetFreezeMode();
                Mode = RobotMode.Freeze;
            }
            else
            {
                response = await _connection.SetRelaxMode();
                Mode = RobotMode.Relax;
            }

            return response.StatusCode == HttpStatusCode.OK ? "OK" : "Robot does not respond";
        }

        public async Task<string> GetStatusMotionAsync()
        {
            var response = await _connection.GetStatusMotionStr();            

            switch (response)
            {
                case "\"IDLE\"":
                    Status = RobotStatusMotion.IDLE;
                    return "IDLE";
                case "\"RUNNING\"":
                    Status = RobotStatusMotion.RUNNING;
                    return "RUNNING";
                case "\"ZERO_GRAVITY\"":
                    Status = RobotStatusMotion.ZERO_GRAVITY;
                    return "ZERO_GRAVITY";
                case "\"MOTION_FAILED\"":
                    Status = RobotStatusMotion.MOTION_FAILED;
                    return "MOTION_FAILED";
                case "\"EMERGENCY\"":
                    Status = RobotStatusMotion.EMERGENCY;
                    return "EMERGENCY";
                default:
                    Status = RobotStatusMotion.ERROR;
                    return "Robot does not respond";
            }
        }
        
        public string GetStatusMotion()
        {
            var response = _connection.GetStatusMotionStr().Result;            

            switch (response)
            {
                case "\"IDLE\"":
                    Status = RobotStatusMotion.IDLE;
                    return "IDLE";
                case "\"RUNNING\"":
                    Status = RobotStatusMotion.RUNNING;
                    return "RUNNING";
                case "\"ZERO_GRAVITY\"":
                    Status = RobotStatusMotion.ZERO_GRAVITY;
                    return "ZERO_GRAVITY";
                case "\"MOTION_FAILED\"":
                    Status = RobotStatusMotion.MOTION_FAILED;
                    return "MOTION_FAILED";
                case "\"EMERGENCY\"":
                    Status = RobotStatusMotion.EMERGENCY;
                    return "EMERGENCY";
                default:
                    Status = RobotStatusMotion.ERROR;
                    return "Robot does not respond";
            }
        }

        public async Task<string> GetMotorStatusAsync()
        {
            var response = await _connection.GetMotorStatus();            

            if (response.StatusCode != HttpStatusCode.OK) return "Robot does not respond";

            var content = await response.Content.ReadAsStringAsync();
            MotorStatus.Joints = JsonConvert.DeserializeObject<IEnumerable<JointStatus>>(content);

            return "OK";
        }

        //TODO: jointAngles в объект    
        public override async Task<string> GetPoseAsync()
        {
            var response = await _connection.GetPose();

            if (response.StatusCode != HttpStatusCode.OK) return "Robot does not respond";
            var content = await response.Content.ReadAsStringAsync();
            JointAngles = JsonConvert.DeserializeObject<Pose>(content);           
            IsConnected = true;
            return "OK";

        }

        public override async Task<string> GetPositionAsync()
        {
            var response = await _connection.GetPosition();

            if (response.StatusCode != HttpStatusCode.OK) return "Robot does not respond";
            var content = await response.Content.ReadAsStringAsync();
            Position = JsonConvert.DeserializeObject<Position>(content);
            return "OK";
        }

        public async Task<string> GetIdAsync()
        {
            var response = await _connection.GetId();

            if (response.StatusCode != HttpStatusCode.OK) return "Robot does not respond";
            ID = await response.Content.ReadAsStringAsync();
            return "OK";
        }

        public async Task<string> GetDigitalInputAsync(int port)
        {
            var response = await _connection.GetDigitalInput(port);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    InputPorts[port - 1] = content == "\"HIGH\"";
                    return content == "\"LOW\"" ? "LOW" : "HIGH";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> GetDigitalOutputAsync(int port)
        {
            var response = await _connection.GetDigitalOutput(port);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    OutputPorts[port - 1] = content == "\"HIGH\"";
                    return content == "\"LOW\"" ? "LOW" : "HIGH";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetBasePositionAsync(double[] position)
        {
            var response = await _connection.SetBasePosition(position);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    BasePosition.Array = position;
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetBasePositionAsync()
        {
            var response = await _connection.SetBasePosition(BasePosition);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetBasePositionAsync(Position basePosition)
        {
            var response = await _connection.SetBasePosition(basePosition);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetToolAsync()
        {
            var response = await _connection.SetTool(Tool);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetToolAsync(Gripper tool)
        {
            var response = await _connection.SetTool(tool);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Tool = tool;
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> RecoverAsync()
        {
            var response = await _connection.Recover();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return "OK";
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> PackAsync()
        {
            var response = await _connection.Pack();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return "OK";
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> GetToolAsync()
        {
            var response = await _connection.GetTool();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    Tool = JsonConvert.DeserializeObject<Gripper>(content);
                    return "OK";
            }

            return "Robot does not respond";
        }

        public async Task<string> GetBasePositionAsync()
        {
            var response = await _connection.GetBasePosition();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                BasePosition = JsonConvert.DeserializeObject<Position>(content);
                return "OK";
            }

            return "Robot does not respond";
        }

        public async Task<string> SetPoseAsync(Pose pose, int value, MotionType type = MotionType.JOINT)
        {
            var response = await _connection.PutPose(pose, value, type);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    JointAngles = pose;
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public override async Task<string> SetPoseAsync(IEnumerable<double> angles, int value, MotionType type = MotionType.JOINT)
        {
            var response = await _connection.PutPose(angles, value, type);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    JointAngles.Angles = angles;
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetPoseAsync(int value, MotionType type = MotionType.JOINT)
        {
            var response = await _connection.PutPose(JointAngles.Angles, value, type);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        //при недостижимой позиции код ошибки равен 500
        public override async Task<string> SetPositionAsync(IEnumerable<double> position, int value,
            MotionType type = MotionType.JOINT)
        {
            var response = await _connection.PutPosition(position, value, type);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Position.Array = position;
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetPositionAsync(Position position, int value,
            MotionType type = MotionType.JOINT)
        {
            var response = await _connection.PutPosition(position, value, type);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Position = position;
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                case HttpStatusCode.InternalServerError:
                    return "Robot does not respond";
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> SetPositionAsync(int value, MotionType type = MotionType.JOINT)
        {
            var response = await _connection.PutPosition(Position, value, type);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> RunPositionsAsync(double[][] positions, int value, MotionType type = MotionType.JOINT)
        {
            var response = await _connection.RunPositions(positions, value, type);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Position.Array = positions.Last();
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> RunPositionsAsync(IEnumerable<Position> positions, int value,
            MotionType type = MotionType.JOINT)
        {
            var response = await _connection.RunPositions(positions, value, type);            

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Position = positions.Last();
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> RunPosesAsync(double[][] angles, int value)
        {
            var response = await _connection.RunPoses(angles, value);            

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    JointAngles.Angles = angles.Last();
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }
       
        public async Task<string> RunPosesAsync(IEnumerable<Pose> poses, int value)
        {
            var response = await _connection.RunPoses(poses, value);            

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    JointAngles = poses.Last();
                    return "OK";
                case HttpStatusCode.PreconditionFailed:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return "Robot does not respond";
            }
        }

        public async Task<string> OpenGripperAsync(int timeout = 500)
        {
            var response = await _connection.OpenGripper(timeout);            

            if (response.StatusCode != HttpStatusCode.OK) return "Robot does not respond";
            IsGripperOpened = true;
            return "Tool opened";
        }
        
        public async Task<string> CloseGripperAsync(int timeout = 500)
        {
            var response = await _connection.CloseGripper(timeout);
            
            if (response.StatusCode != HttpStatusCode.OK) return "Robot does not respond";
            IsGripperOpened = false;
            return "Tool closed";
        }

        public void WaitMotion(int askingPeriod = 50)
        {
            while (GetStatusMotionAsync().Result != "IDLE") Thread.Sleep(askingPeriod);
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RozumConnectionLib
{
    public class RealRobot : Robot
    {
        private RozumConnection connection;
        
        public double TcpMoveStep{get;set;}
        public double JointMoveStep{get;set;}
        public int Speed{get;set;}

        public string URL
        {
            get
            {
                return connection.URL;
            }
            set
            {
                InitConnection(value);
            }
        }

        public RobotMotorStatus MotorStatus{get;protected set;}

        public RobotMode Mode{get;private set;}

        public RobotStatusMotion Status{get;protected set;}

        public bool IsConnected{get;protected set;}

        public bool IsGripperOpened{get;protected set;}

        public RealRobot(string url)
        {
            Status = RobotStatusMotion.NOT_RESPOND;
            InitConnection(url);
            
            JointAngles = new double[6];           
            Position = new RobotPosition();
            MotorStatus = new RobotMotorStatus();            
        }        
        
        public RealRobot()
        {
            Status = RobotStatusMotion.NOT_RESPOND;
            IsConnected = false;
            URL="http://0.0.0.0:0/";
            
            JointAngles = new double[6];
            Position = new RobotPosition();
            MotorStatus = new RobotMotorStatus();                        
        }
        
        private void InitConnection(string ip)
        {
            if (IPAddress.TryParse(ip, out IPAddress iP))
            {
                connection = new RozumConnection($"http://{ip}:8081/");
                GetStatusMotionAsync().Wait();
                if (Status != RobotStatusMotion.NOT_RESPOND)
                {
                    IsConnected = true;
                } 
                else IsConnected = false;
            }
            else IsConnected = false;
        }

        public async Task<string> SetModeAsync(RobotMode mode)
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                if (mode == RobotMode.Freeze)
                {
                    response = await connection.SetFreezeMode();
                    Mode = RobotMode.Freeze;
                }
                else
                {
                    response = await connection.SetRelaxMode();
                    Mode = RobotMode.Relax;
                }
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);            

            if (response.StatusCode == HttpStatusCode.OK)
            {                
                return "OK";
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> GetStatusMotionAsync()
        {                       
            string response;
            if (IsConnected)
            {
                response = await connection.GetStatusMotionStr();
            }
            else response = "";

            if (response == "\"IDLE\"")
            {                
                Status = RobotStatusMotion.IDLE;
                return "IDLE";
            }
            else if (response == "\"RUNNING\"")
            {               
                Status = RobotStatusMotion.RUNNING;
                return "RUNNING";
            }
            else if (response == "\"ZERO_GRAVITY\"")
            {               
                Status = RobotStatusMotion.ZERO_GRAVITY;
                return "ZERO_GRAVITY";
            }
            else
            {     
                Status = RobotStatusMotion.NOT_RESPOND;
                return "Robot does not respond";
            }    
        }

        public async Task<string> GetMotorStatusAsync()
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.GetMotorStatus();
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();                
                var dicts = JsonConvert.DeserializeObject
                   <List<Dictionary<string, double>>>(await response.Content.ReadAsStringAsync());

                MotorStatus.Amperage = new double[6];
                MotorStatus.Temperature = new double[6];
                for (int i = 0; i < dicts.Count; i++)
                {
                    MotorStatus.Amperage[i] = dicts[i]["current"];
                    MotorStatus.Temperature[i] = dicts[i]["temperature"];
                }               
                return "OK";
            }
            else
            {                         
                return "Robot does not respond";
            }           
        }
        
        public override async Task<string> GetJointAnglesAsync()
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.GetPose();
            } 
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                JointAngles = JsonConvert.DeserializeObject<Dictionary<string, double[]>>(content)["angles"];
                JointAngles = JointAngles.Select(x=>Math.Round(x, 3)).ToArray();
                IsConnected = true;                
                return "OK";
            }
            else
            {                                                                        
                return "Robot does not respond";
            }           
        }

        public override async Task<string> GetPositionAsync()
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.GetPosition();
            }            
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = await connection.GetPositionArray();                
                Position.Point = array.Take(3).Select(x=>Math.Round(x,3)).ToArray();
                Position.Rotation = array.Skip(3).Select(x=>Math.Round(x,3)).ToArray();                
                return "OK";
            }
            else
            {                             
                return "Robot does not respond";
            }   
        }

        public override async Task<string> SetJointAnglesAsync(double[] angles, int value, RobotMoveMode mode = RobotMoveMode.SPEED)
        {     
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.PutPose(angles, value, mode);
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (response.StatusCode == HttpStatusCode.OK)
            {                
                JointAngles = angles;
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> SetJointAnglesAsync(int value, RobotMoveMode mode = RobotMoveMode.SPEED)
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.PutPose(JointAngles, value ,mode);
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
           
            if (response.StatusCode == HttpStatusCode.OK)
            {                
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {               
                return "Robot does not respond";
            }
        }

        public override async Task<string> SetPositionAsync(double[] position, int value, RobotMoveMode mode = RobotMoveMode.SPEED)
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.PutPosition(position, value, mode);
            }
            response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
           
            if(response.StatusCode == HttpStatusCode.OK)
            {      
                Position.Array = position;
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> SetPositionAsync(int value, RobotMoveMode mode = RobotMoveMode.SPEED)
        {
            HttpResponseMessage response;
            if (IsConnected)
            {                
                response = await connection.PutPosition(Position.ToArray(), value, mode);
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if(response.StatusCode == HttpStatusCode.OK)
            {                
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> SetPositionsAsync(double[][] positions, int value, RobotMoveMode mode = RobotMoveMode.SPEED)
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.RunPositions(positions, value, mode);
            }
            response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Position.Array = positions.Last();
                return "OK";
            }
            else if (response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> SetPosesAsync(double[][] angles, int value, RobotMoveMode mode = RobotMoveMode.SPEED)
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.RunPoses(angles, value, mode);
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JointAngles = angles.Last();
                return "OK";
            }
            else if (response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> OpenGripperAsync()
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.OpenGripper();
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                IsGripperOpened = true;
                return "Gripper opened";
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> CloseGripperAsync()
        {
            HttpResponseMessage response;
            if (IsConnected)
            {
                response = await connection.CloseGripper();
            }
            else response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                IsGripperOpened = false;
                return "Gripper opened";
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public void WaitMotion(int askingPeriod = 50)
        {
            while (GetStatusMotionAsync().Result!="IDLE")
            {
                Thread.Sleep(askingPeriod);
            }
        }
    }
}

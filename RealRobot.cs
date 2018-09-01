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
            InitConnection(url);
            
            JointAngles = new double[6];           
            Position = new RobotPosition();
            MotorStatus = new RobotMotorStatus();            
        }        
        
        public RealRobot()
        {
            IsConnected = false;
            
            JointAngles = new double[6];
            Position = new RobotPosition();
            MotorStatus = new RobotMotorStatus();                        
        }
        
        private void InitConnection(string url)
        {
            if (IPAddress.TryParse(url.Split(':')[0], out IPAddress iP))
            {
                connection = new RozumConnection($"http://{url}/");
                IsConnected = true;
            }
            else IsConnected = false;
        }

        public async Task<string> SetMode(RobotMode mode)
        {
            HttpResponseMessage response;
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
            if (response.StatusCode == HttpStatusCode.OK)
            {
                RaisePropertyChanged("Mode");
                return "OK";
            }
            else
            {
                return "Robot does not respond";
            }
        }

        public async Task<string> GetStatusMotion()
        {
            //var response = await connection.GetStatusMotionStr();            
            var response = await new HttpClient().GetAsync(URL + "status/motion").Result.Content.ReadAsStringAsync();

            if (response == "\"IDLE\"")
            {
                RaisePropertyChanged("StatusMotion");
                Status = RobotStatusMotion.IDLE;
                return "IDLE";
            }
            else if (response == "\"RUNNING\"")
            {
                RaisePropertyChanged("StatusMotion");
                Status = RobotStatusMotion.RUNNING;
                return "RUNNING";
            }
            else if (response == "\"ZERO_GRAVITY\"")
            {
                RaisePropertyChanged("StatusMotion");
                Status = RobotStatusMotion.ZERO_GRAVITY;
                return "ZERO_GRAVITY";
            }
            else
            {                              
                return "Robot does not respond";
            }    
        }

        public async Task<string> GetMotorStatus()
        {
            var response = await connection.GetMotorStatus();            
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
                RaisePropertyChanged("MotorStatus");
                return "OK";
            }
            else
            {
                IsConnected = false;                
                return "Robot does not respond";
            }           
        }

        //TODO: оповещать ViewModel
        public override async Task<string> GetJointAngles()
        {
            var response = await connection.GetPose();            
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
                IsConnected = false;                
                return "Robot does not respond";
            }           
        }

        public override async Task<string> GetPosition()
        {
            var response = await connection.GetPosition();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = await connection.GetPositionArray();                
                Position.Point = array.Take(3).Select(x=>Math.Round(x,3)).ToArray();
                Position.Rotation = array.Skip(3).Select(x=>Math.Round(x,3)).ToArray();                
                return "OK";
            }
            else
            {
                IsConnected = false;                
                return "Robot does not respond";
            }   
        }

        public override async Task<string> SetJointAngles(double[] angles, int speed)
        {        
            var response = await connection.PutPose(angles, speed);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                IsConnected = true;
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                IsConnected = false;
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> SetJointAngles(int speed)
        {
            var response = await connection.PutPose(JointAngles, speed);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                IsConnected = true;
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                IsConnected = false;
                return await response.Content.ReadAsStringAsync();
            }
        }

        public override async Task<string> SetPosition(double[] position, int speed)
        {
            var response = await connection.PutPosition(position, speed);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                IsConnected = true;
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                IsConnected = false;
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> SetPosition(int speed)
        {
            var response = await connection.PutPosition(Position.ToArray(), speed);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                IsConnected = true;
                return "OK";
            }
            else if(response.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                IsConnected = false;
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> OpenGripper()
        {
            var response = await connection.OpenGripper();
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

        public async Task<string> CloseGripper()
        {
            var response = await connection.CloseGripper();
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
            while (GetStatusMotion().Result!="IDLE")
            {
                Thread.Sleep(askingPeriod);
            }
        }
    }
}

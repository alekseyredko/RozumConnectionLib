using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RozumConnectionLib
{    
    class RozumConnection
    {               
        public string URL { get; set; }
        private readonly HttpClient client = new HttpClient();

        public RozumConnection()
        {            
        }

        public RozumConnection(string url)
        {            
            URL = url;           
        }                

        public async Task<HttpResponseMessage> Recover()
        {
            try
            {
                return await client.PutAsync(URL + "recover", null);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Pack()
        {
            try
            {
                return await client.PutAsync(URL + "pack", null);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> SetDigitalOutput(int port, bool value)
        {
            try
            {
                if(value) return await client.PutAsync(URL + $"signal/output/{port}/high", null);
                return await client.PutAsync(URL + $"signal/output/{port}/low", null);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }       

        public async Task<HttpResponseMessage> SetTool(Gripper content)
        {
            try
            {
                return await client.PostAsync(URL + $"tool", 
                    new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));                                
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> GetTool()
        {
            try
            {
                return await client.GetAsync(URL + $"tool");                                
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        
        public async Task<HttpResponseMessage> GetDigitalInput(int port)
        {
            try
            {
                return await client.GetAsync(URL + $"signal/input/{port}");                                
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> GetDigitalOutput(int port)
        {
            try
            {
                return await client.GetAsync(URL + $"signal/output/{port}");                                
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> GetMotorStatus()
        {
            try
            {
                return await client.GetAsync(URL + "status/motors");                          
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<string> GetStatusMotionStr()
        {
            try
            {
                return await client.GetStringAsync(URL + "status/motion");                
            }
            catch (HttpRequestException)
            {
                return "Robot does not respond";
            }
        }       

        public async Task<HttpResponseMessage> GetPosition()
        {
            try
            {
                return await client.GetAsync(URL + "position");                
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> GetBasePosition()
        {
            try
            {
                return await client.GetAsync(URL + "base");                
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
        
        public async Task<HttpResponseMessage> GetPose()
        {
            try
            {
                return await client.GetAsync(URL + "pose");
            }
            catch (HttpRequestException)
            {                
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }           
        }           

        public async Task<HttpResponseMessage> GetId()
        {
            try
            {
                return await client.GetAsync(URL + "robot/id");
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> PutPosition(IEnumerable<double> position, int value, MotionType type, float maxVelocity)
        {
            try
            {
                var pos = new Position{Array = position};
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(pos),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"position?speed={value}&mode={(type == MotionType.JOINT? "JOINT": "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch (IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }           
        }

        public async Task<HttpResponseMessage> PutPosition(Position position, int value, MotionType type, float maxVelocity)
        {
            try
            {                
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(position),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"position?speed={value}&mode={(type == MotionType.JOINT? "JOINT": "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch (IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }           
        }

        public async Task<HttpResponseMessage> SetBasePosition(double[] position)
        {
            try
            {              
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(new Position{Array = position}),
                    Encoding.UTF8, "application/json");
                return await client.PostAsync(URL + $"base", httpContent);
            }
            catch (IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }           
        }

        public async Task<HttpResponseMessage> SetBasePosition(Position position)
        {
            try
            {                
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(position),
                    Encoding.UTF8, "application/json");
                return await client.PostAsync(URL + $"base", httpContent);
            }
            catch (IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }           
        }
       
        public async Task<HttpResponseMessage> PutPose(Pose pose, int value, MotionType type, float maxVelocity)
        {
            try
            {               
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(pose),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"pose?speed={value}&mode={(type == MotionType.JOINT ? "JOINT" : "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> PutPose(IEnumerable<double> coordinates, int value, MotionType type, float maxVelocity)
        {
            try
            {
                var dict = new Dictionary<string, IEnumerable<double>> {["angles"] = coordinates};
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(dict),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"pose?speed={value}&mode={(type == MotionType.JOINT ? "JOINT" : "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> SetFreezeMode()
        {
            try
            {
                return await client.PutAsync(URL + "freeze", null);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> SetRelaxMode()
        {
            try
            {
                return await client.PutAsync(URL + "relax", null);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> OpenGripper(int timeout)
        {
            try
            {
                var obj = JsonConvert.SerializeObject(new Dictionary<string, int> {{"timeout", timeout}});
                var content = new StringContent(
                    obj, 
                    Encoding.UTF8,
                    "application/json");
                return await client.PutAsync(URL + "gripper/open", content);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> CloseGripper(int timeout)
        {
            try
            {
                var obj = JsonConvert.SerializeObject(new Dictionary<string, int> { { "timeout", timeout } });
                var content = new StringContent(
                    obj,
                    Encoding.UTF8,
                    "application/json");
                return await client.PutAsync(URL + "gripper/close", content);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> RunPoses(double[][] angles, int value, MotionType type, float maxVelocity)
        {
            try
            {                
                var dict = new List<Dictionary<string, double[]>>();
                
                for (int i = 0; i < angles.GetLength(0); i++)
                {
                    dict.Add(new Dictionary<string, double[]>());
                    dict[i]["angles"] = angles[i];
                }

                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(dict),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"poses/run?speed={value}&mode={(type == MotionType.JOINT ? "JOINT" : "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch(IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);    
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> RunPoses(IEnumerable<Pose> poses, int value, MotionType type, float maxVelocity)
        {
            try
            {                               
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(poses),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"poses/run?speed={value}&mode={(type == MotionType.JOINT ? "JOINT" : "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch(IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);    
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> RunPositions(double[][] positions, int value, MotionType type, float maxVelocity)
        {
            try
            {
                var list = new List<Position>();

                for (int i = 0; i < positions.GetLength(0); i++)
                {
                    list.Add(new Position{Array = positions[i]});                   
                }

                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(list),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"positions/run?speed={value}&mode={(type == MotionType.JOINT? "JOINT": "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch(IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> RunPositions(IEnumerable<Position> positions, int value, MotionType type, float maxVelocity)
        {
            try
            {                
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(positions),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"positions/run?speed={value}&mode={(type == MotionType.JOINT? "JOINT": "LINEAR")}&tcp_max_velocity={maxVelocity}", httpContent);
            }
            catch(IndexOutOfRangeException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}

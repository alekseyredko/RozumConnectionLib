using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RozumConnectionLib
{    
    public class RozumConnection
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

        public void AwaitMotion(int askingPeriod = 20)
        {
            while (GetStatusMotionStr().Result!= "\"IDLE\"")
            {
                Thread.Sleep(askingPeriod);
            }            
        }
      
        public async Task<string> GetMotorStatusStr()
        {
            try
            {
                var response = await client.GetAsync(URL + "status/motors");          
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                return "Can't connect to robot";
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

        public async Task<List<Dictionary<string, double[]>>> GetMotorStatusDict()
        {
            try
            {
                var response = await client.GetAsync(URL + "status/motors");          
                var dict = new List<Dictionary<string, double[]>>();
                var obj = JsonConvert.DeserializeObject
                    <List<Dictionary<string, double[]>>>(await response.Content.ReadAsStringAsync());
                return obj;                
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<string> GetStatusMotionStr()
        {
            try
            {
                var response = await client.GetAsync(URL + "status/motion");
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                return "Can't connect to robot";
            }
        }       

        public async Task<string> GetPositionStr()
        {
            try
            {
                var response = await client.GetAsync(URL + "position");            
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                return "Can't connect to robot";
            }
        }

        public async Task<double[]> GetPositionArray()
        {
            try
            {
                var response = await client.GetStringAsync(URL + "position");
                var dict = JsonConvert.DeserializeObject
                    <Dictionary<string, Dictionary<string, double>>>(response);
                List<double> positon = new List<double>();
                foreach (var value in dict)
                {
                    foreach (var item in value.Value)
                    {
                        positon.Add(item.Value);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    positon[i] *=1000;
                }

                for (int i = 3; i < 6; i++)
                {
                    positon[i] = positon[i]*180/Math.PI;
                }

                return positon.ToArray();
            }
            catch (HttpRequestException)
            {
                return new double[0];
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

        public async Task<double[]> GetPoseArray()
        {
            try
            {
                var response = await client.GetStringAsync(URL + "pose");
                var dict = JsonConvert.DeserializeObject<Dictionary<string, double[]>>(response);
                return dict["angles"];
            }
            catch (HttpRequestException)
            {                
                return new double[0];
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

        //get in degrees
        public async Task<string> GetPoseStr()
        {
            try
            {
                return await client.GetStringAsync(URL + "pose");            
            }
            catch (HttpRequestException)
            {
                return "Can't connect to robot";
            }
        }

        //get in degrees
        public async Task<Dictionary<string, double[]>> GetPoseDict()
        {
            try
            {
                var response = await client.GetStringAsync(URL + "pose");
                var obj = JsonConvert.DeserializeObject
                    <Dictionary<string, double[]>>(response);
                return obj;
            }
            catch (HttpRequestException)
            {
                return new Dictionary<string, double[]>();
            }
        }

        public async Task<Dictionary<string, Dictionary<string, double>>> GetPositionDict()
        {
            try
            {
                var response = await client.GetStringAsync(URL + "position");
                var obj = JsonConvert.DeserializeObject
                    <Dictionary<string, Dictionary<string, double>>>(response);
                return obj;
            }
            catch (HttpRequestException)
            {
                return null;               
            }
        }        

        public async Task<HttpResponseMessage> PutPosition(Dictionary<string, Dictionary<string, double>> dict, int speed)
        {
            try
            {
                var httpContent = new StringContent(
                JsonConvert.SerializeObject(dict),
                Encoding.UTF8, "application/json");            
                return await client.PutAsync(URL + $"position?speed={speed}", httpContent);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }            
        }

        public async Task<HttpResponseMessage> PutPosition(double[] position, int speed)
        {
            try
            {
                var dict = new Dictionary<string, Dictionary<string, double>>();
                dict["point"] = new Dictionary<string, double>();
                dict["rotation"] = new Dictionary<string, double>();
                dict["point"]["x"] = position[0]/1000;
                dict["point"]["y"] = position[1]/1000;
                dict["point"]["z"] =  position[2]/1000;
                dict["rotation"]["roll"] =  position[3]*Math.PI/180;
                dict["rotation"]["pitch"] =  position[4]*Math.PI/180;
                dict["rotation"]["yaw"] =  position[5]*Math.PI/180;
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(dict),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"position?speed={speed}", httpContent); 
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

        //set in degrees
        public async Task<HttpResponseMessage> PutPose(Dictionary<string, double[]> dict, int speed)
        {
            try
            {
                var httpContent = new StringContent(
                JsonConvert.SerializeObject(dict),
                Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"pose?speed={speed}", httpContent);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }            
        }

        public async Task<HttpResponseMessage> PutPose(double[] coordinates, int speed)
        {
            try
            {
                var dict = new Dictionary<string, double[]>();
                dict["angles"] = coordinates;
                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(dict),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"pose?speed={speed}", httpContent);
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

        public async Task<HttpResponseMessage> OpenGripper()
        {
            try
            {
                return await client.PutAsync(URL + "gripper/open", null);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> CloseGripper()
        {
            try
            {
                return await client.PutAsync(URL + "gripper/close", null);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> RunPoses(double[][] angles, int speed)
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
                return await client.PutAsync(URL + $"poses/run?speed={speed}", httpContent);
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

        public async Task<HttpResponseMessage> RunPositions(double[][] angles, int speed)
        {
            try
            {
                var dict = new List<Dictionary<string, Dictionary<string, double>>>();

                for (int i = 0; i < angles.GetLength(0); i++)
                {
                    dict.Add(new Dictionary<string, Dictionary<string, double>>());
                    dict[i]["point"] = new Dictionary<string, double>();
                    dict[i]["rotation"] = new Dictionary<string, double>();
                    dict[i]["point"]["x"] = angles[i][0]/1000;
                    dict[i]["point"]["y"] = angles[i][1]/1000;
                    dict[i]["point"]["z"] = angles[i][2]/1000;
                    dict[i]["rotation"]["roll"] = angles[i][3]*Math.PI/180;
                    dict[i]["rotation"]["pitch"] = angles[i][4]*Math.PI/180;
                    dict[i]["rotation"]["yaw"] = angles[i][5]*Math.PI/180;
                }

                var httpContent = new StringContent(
                    JsonConvert.SerializeObject(dict),
                    Encoding.UTF8, "application/json");
                return await client.PutAsync(URL + $"positions/run?speed={speed}", httpContent);
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

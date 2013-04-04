using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CameraFinal
{
    public class HttpReq
    {

        public static string HttpGet(string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Credentials = new NetworkCredential("root", "root");
            string result = null;

            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            
            StreamReader reader = new StreamReader(resp.GetResponseStream());
            result = reader.ReadToEnd();

            resp.Close();
            reader.Close();
  
            return result;
        }

    }
}

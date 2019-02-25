using LifeSupport.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Utilities
{
     class JSONParser
    {
        public JSONParser()
        {

        }
        public static T readJSON<T>(T obj, String jsonFile)
        {
             using (StreamReader r = new StreamReader(jsonFile))
            {
                string JSONdata = r.ReadToEnd();
                obj = JsonConvert.DeserializeObject<T>(JSONdata);
            }
            return obj;
             
        }
    }
}

using LifeSupport.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        //This works with a singleton
        public static dynamic readJSONfile(String jsonfile)
        {

         //   List<String> Units = JsonConvert.DeserializeObject<IEnumerable<String>>(File.ReadAllText(jsonfile)).ToList();
            string JSONdata ;
            dynamic data;
            using (StreamReader r = new StreamReader(jsonfile))
            {
                JSONdata = r.ReadToEnd();
                data = JObject.Parse(JSONdata);
            }
            return data;
        }




    }
}

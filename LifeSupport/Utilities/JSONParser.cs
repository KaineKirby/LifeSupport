using LifeSupport.Config;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Utilities {
    class JSONParser {

        //GIVEN: A string to the JSON file path
        //RETURNED: the data from the json file
        public static dynamic ReadJsonFile(String jsonfile) {
            string JSONdata ;
            dynamic data;
            using (StreamReader r = new StreamReader(jsonfile)) {
                JSONdata = r.ReadToEnd();
                data = JObject.Parse(JSONdata);
            }
            return data;
        }




    }
}

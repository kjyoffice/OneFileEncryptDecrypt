using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class JsonWork
    {
        public static string ToJsonText(object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(
                data,
                new Newtonsoft.Json.JsonSerializerSettings() 
                { 
                    Formatting = Newtonsoft.Json.Formatting.Indented
                }
            );
        }
    }
}

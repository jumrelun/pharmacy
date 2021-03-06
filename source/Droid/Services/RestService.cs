﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pharmacy.Droid.Helpers;

namespace Pharmacy.Droid.Services
{
    public class RestService
    {
        public static async Task<Dictionary<string, string>> GetMobileSettings()
        {
            using (var client = new HttpClient())
            {
                var service = $"{Settings.FunctionURL}/api/MobileSettings/";

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                var token = Convert.ToBase64String(time.Concat(key).ToArray());
                token = SecurityHelper.Encrypt(token, Settings.CryptographyKey);

                byte[] byteData = Encoding.UTF8.GetBytes("{'Token':'" + token + "'}");
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var httpResponse = await client.PostAsync(service, content);

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(await httpResponse.Content.ReadAsStringAsync());
                        return result;
                    }
                }
            }
            return null;
        }
    }
}

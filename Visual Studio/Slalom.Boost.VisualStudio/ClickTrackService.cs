using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slalom.Boost.VisualStudio
{
    public class ClickTrackService : IClickTrackService
    {
        public Task Track(string userName, string name, string projectName, object additional)
        {
            using (var client = new HttpClient())
            {
                var task = client.PostAsync("http://slalom-boost-insight.azurewebsites.net/clicktracks/actions/add", new StringContent(JsonConvert.SerializeObject(new
                {
                    UserName = userName,
                    Name = name,
                    DateTime = DateTime.UtcNow,
                    Additional = additional != null ? JsonConvert.SerializeObject(additional) : null,
                    ProjectName = projectName
                }), Encoding.UTF8, "application/json"));

                //var result = task.Result;
                //if (!result.IsSuccessStatusCode)
                //{
                //    var content = result.Content.ReadAsStringAsync().Result;
                //    BoostOutputWindow.WriteLine("CT Failed");
                //    BoostOutputWindow.WriteLine(content);
                //}
                return task;
            }
        }
    }
}

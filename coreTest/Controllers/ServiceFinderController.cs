using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace coreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceFinderController : ControllerBase
    {
        private IConfiguration configuration;
        public ServiceFinderController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> Get()
        {
            string consulAddress = configuration["ConsulOption:ConsulAddress"].ToString();

            using (var consulClient = new ConsulClient(x => x.Address = new Uri(consulAddress)))
            {
                
                var services = consulClient.Catalog.Service("ServiceA").Result.Response;

                if (services != null && services.Any())
                {
                    var rand = new Random(Guid.NewGuid().GetHashCode()).Next(services.Count());

                    var service = services.ElementAt(rand);


                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/values/{rand}");
                        var result = await response.Content.ReadAsStringAsync();

                        return $"Node:{service.Node},ServiceID:{service.ServiceID},ServiceTags:{string.Join(",",service.ServiceTags)},Port:{service.ServicePort},result:{result}";
                    }
                }
            }


            return "未发现服务";
        }
    }
}
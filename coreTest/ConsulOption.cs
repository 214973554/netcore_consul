using System.Runtime.Serialization;

namespace coreTest
{
    public class ConsulOption
    {
        public string ServiceName { get; set; }
        public string ServiceIP { get; set; }
        public int ServicePort { get; set; }
        public string ServiceHealthCheck { get; set; }
        public string Address { get; set; }
    }
}
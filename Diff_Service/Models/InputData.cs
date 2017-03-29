using System.Runtime.Serialization;

namespace Diff_Service.Models
{
    [DataContract]
    public class InputData
    {
        [DataMember]
        public string Data { get; set; }
    }
}

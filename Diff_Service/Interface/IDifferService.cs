using Diff_Service.Models;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Diff_Service.Interface
{
    [ServiceContract]
    public interface IDifferService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/{id}", ResponseFormat = WebMessageFormat.Json)]
        OutputData CheckInputById(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/left", 
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, Method = "PUT")]
        HttpStatusCode AddLeftInput(string id, InputData data);

        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/right",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, Method = "PUT")]
        HttpStatusCode AddRightInput(string id, InputData data);
    }
}

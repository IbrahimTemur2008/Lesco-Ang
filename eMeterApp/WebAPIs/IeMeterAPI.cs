using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using static RestService.DataObjects.eMeterObjects;

namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IeMeterAPI" in both code and config file together.
    [ServiceContract]
    public interface IeMeterAPI
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SignIn", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage SignIn(UserCredentials credentials);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SignOut", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage SignOut(UserCredentials credentials);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ChangePassword", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage ChangePassword(UserCredentials request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/RegisterCustomer", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage RegisterCustomer(User user);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/AddMeter", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage AddMeter(MeterType meter);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "RecordReading", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResponseMessage RecordReading(RecordReadingRequest request);

        [OperationContract]
        [WebGet(UriTemplate = "/GetAllUsers", ResponseFormat = WebMessageFormat.Json)]
        List<User> GetAllUsers();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/CreateCredentials", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage CreateCredentials(Credentials credentials);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/RegisterMeterWithCustomer", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage RegisterMeterWithCustomer(MeterDetails meterDetails);
    }
}

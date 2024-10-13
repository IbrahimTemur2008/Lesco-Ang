using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static RestService.DataObjects.eMeterObjects;

namespace RestService
{
    
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
        [WebGet(UriTemplate = "/GetUser/{userId}", ResponseFormat = WebMessageFormat.Json)]
        User GetUser(string userId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/CreateCredentials", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage CreateCredentials(Credentials credentials);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/RegisterMeterWithCustomer", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMessage RegisterMeterWithCustomer(MeterDetails meterDetails);

        [OperationContract]
        [WebGet(UriTemplate = "/GetLogin/{userId}", ResponseFormat = WebMessageFormat.Json)]
        UserCredentials GetLogin(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "/GetMeter/{userId}", ResponseFormat = WebMessageFormat.Json)]
        MeterDetails GetMeter(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "/GetDashboardData/{userID}", ResponseFormat = WebMessageFormat.Json)]
        RecordReadingRequest GetDashboardData(string userID);
  }
}

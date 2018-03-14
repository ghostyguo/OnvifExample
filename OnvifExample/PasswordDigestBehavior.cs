using System.ServiceModel.Description;

namespace OnvifExample
{
    public class PasswordDigestBehavior : IEndpointBehavior
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public PasswordDigestBehavior(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        #region IEndpointBehavior Members
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new PasswordDigestMessageInspector(this.Username, this.Password));
        }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {

        }
        public void Validate(ServiceEndpoint endpoint)
        {

        }
        #endregion
    }
}

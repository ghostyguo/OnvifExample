using OnvifExample.OnvifDeviceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace OnvifExample
{
    public class DeviceViewModel
    {
        public DeviceClient Client { get; set; }
        public string IpAddress = "172.20.200.203";
        public string Username = "root";
        public string Password = "pass";

        public DeviceViewModel()
        {
            EndpointAddress DeviceServiceRemoteAddress = new EndpointAddress("http://" + IpAddress + "/onvif/device_service");
            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement
            {
                AuthenticationScheme = AuthenticationSchemes.Digest
            };
            var messageElement = new TextMessageEncodingBindingElement
            {
                MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
            };
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            Client = new DeviceClient(bind, DeviceServiceRemoteAddress);

            PasswordDigestBehavior passwordDigestBehavior = new PasswordDigestBehavior(Username, Password);

            Client.Endpoint.Behaviors.Add(passwordDigestBehavior);






            Client.GetDeviceInformation(out string Model,out string FirmwareVersion, out string SerialNumber, out string HardwareId);

            Console.WriteLine("Model = " + Model);
            Console.WriteLine("FirmwareVersion = " + FirmwareVersion);
            Console.WriteLine("SerialNumber = " + SerialNumber);
            Console.WriteLine("HardwareId = " + HardwareId);

        }
    }
}

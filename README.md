# OnvifExample
Onvif Example using C#

C# Example for onvif

This example has been written using VisualStudio 2017.
This example is a simple function that creates an Onvif Device that implements all functions described in the device.wdsl service.
For more information about onvif please refer to: 
https://www.onvif.org/profiles/specifications/

## Example walkthrough
### Step 1: 
Install Microsoft WSE https://msdn.microsoft.com/en-us/library/ms977317.aspx and add this dll as a reference to the project:
C:\ProgramFiles(x86)\Microsoft WSE\v3.0\Microsoft.Web.Services3.dll
### Step 2:
Add the wsdl link as a reference to the project:
https://www.onvif.org/ver10/device/wsdl/devicemgmt.wsdl
### Step 3:
Once the wsdl reference has been added to the project a Device object can be declared using:

      public DeviceClient Client { get; set; }
### Step 4:
Before calling any function defined in the service we need to connect to the service endpoint. In this case the endpoint is defined by the onvif specification as 

      /onvif/device_service
### Step 5:
Create and initialize the Client

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

### Step 6:
Authentication can be a little bit tricky because of the way Microsoft implements the WCF client proxy.
Basically it is solved using these two classes:

    PasswordDigestBehavior.cs
    PasswordDisgestMessageInspector.cs
    
There is a very good explanation on how they work here:
https://benpowell.org/supporting-the-ws-i-basic-profile-password-digest-in-a-wcf-client-proxy/
### Step 7:
Add authentication to the Soap envelope:

            PasswordDigestBehavior passwordDigestBehavior = new PasswordDigestBehavior(Username, Password);

            Client.Endpoint.Behaviors.Add(passwordDigestBehavior);

### Step 8:
Call any function defined in the wsdl. Example GetDeviceInformation

            Client.GetDeviceInformation(out string Model,out string FirmwareVersion, out string SerialNumber, out string HardwareId);

            Console.WriteLine("Model = " + Model);
            Console.WriteLine("FirmwareVersion = " + FirmwareVersion);
            Console.WriteLine("SerialNumber = " + SerialNumber);
            Console.WriteLine("HardwareId = " + HardwareId);

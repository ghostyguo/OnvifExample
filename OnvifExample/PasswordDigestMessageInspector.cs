using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;
using Microsoft.Web.Services3.Security.Tokens;

namespace OnvifExample
{
    internal class PasswordDigestMessageInspector : IClientMessageInspector
    {
        private string password;
        private string username;

        public PasswordDigestMessageInspector(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

            public void AfterReceiveReply(ref Message reply, object correlationState)
    {
    }
    public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
    {
        UsernameToken token = new UsernameToken(this.username, this.password, PasswordOption.SendHashed);
        // Serialize the token to XML
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement securityToken = token.GetXml(xmlDoc);
        // find nonce and add EncodingType attribute for BSP compliance
        XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsMgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
        XmlNodeList nonces = securityToken.SelectNodes("//wsse:Nonce", nsMgr);
        XmlAttribute encodingAttr = xmlDoc.CreateAttribute("EncodingType");
        encodingAttr.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
        if (nonces.Count > 0)
        {
            nonces[0].Attributes.Append(encodingAttr);
        }
        MessageHeader securityHeader = MessageHeader.CreateHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", securityToken, false);
        request.Headers.Add(securityHeader);
        // complete
        return Convert.DBNull;
    }

    }
}
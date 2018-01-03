using SimCorp.IMS.Messages;

namespace SimCorp.IMS.MobilePhoneLibrary.Provider {
    public class SMSProvider {

        // public delegate void SMSRecievedDelegate(string message);
        public delegate void SMSRecievedDelegate(MyMessage message);

        public event SMSRecievedDelegate SMSReceived;

        public void RaiseSMSReceivedEvent(MyMessage message) {
            SMSReceived?.Invoke(message);
        }

        public void ReceiveSMS(MyMessage message) {
            RaiseSMSReceivedEvent(message);
        }
        
    }
}

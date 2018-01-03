using SimCorp.IMS.Messages;
using System;
using System.Collections.Generic;

namespace SimCorp.IMS.SMSReceiverWFA {
    public class Format {

        //public delegate string FormatDelegate(string text);
        public delegate string FormatDelegate(MyMessage message);

        //public string OnSMSReceived(string message, FormatDelegate del) {
        public string OnSMSReceived(MyMessage message, FormatDelegate del) {
            return del(message);
        }

        public Dictionary<int, FormatDelegate> FormatType = new Dictionary<int, FormatDelegate>();

        public Format() {
            FormatType.Add(0, (message) => $"{message.Text}");
            FormatType.Add(1, (message) => $"[{message.ReceivingTime}] {message.Text}");
            FormatType.Add(2, (message) => $"{message.Text} [{message.ReceivingTime}]");
            FormatType.Add(3, (message) => $"{message.Text} Tin-din");
            FormatType.Add(4, (message) => $"{message.Text.ToLower()}");
            FormatType.Add(5, (message) => $"{message.Text.ToUpper()}");

        }
    }   
}

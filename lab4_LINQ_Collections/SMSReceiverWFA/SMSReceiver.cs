using SimCorp.IMS.Messages;
using SimCorp.IMS.MobilePhoneLibrary.General;
using SimCorp.IMS.MobilePhoneLibrary.MobilePhone;
using SimCorp.IMS.MobilePhoneLibrary.Provider;
using SimCorp.IMS.MobilePhoneLibrary.Storage;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static SimCorp.IMS.SMSReceiverWFA.Format;

namespace SimCorp.IMS.SMSReceiverWFA {

    public partial class SMSReceiverForm : Form {

        static Timer timer;
        IOutput output;
        SimCorpMobile MyMobile;
        Format Format = new Format();
        List<MyMessage> myReceivedMessages = new List<MyMessage>();

        public SMSReceiverForm() {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now.AddDays(1);

            MyMobile = new SimCorpMobile();
            output = new WFAOutputRichTextBox(richTextBox1);       
            //MyMobile.SMSProvider.SMSReceived += (message) => output.WriteLine(message.ToString()); 
            MyMobile.Storage.SMSAdded += (message) => storageLogTextBox.Text=($"{message.ToString()} was added to the mobile storage");
            MyMobile.Storage.SMSRemoved+=(message) =>storageLogTextBox.Text = ($"{message.ToString()} was removed from the mobile storage");
        }

        private void SMSReceiverForm_Load(object sender, System.EventArgs e) {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1500;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e) {

            //string text = "New SMS received";         //from lab3
            MyMessage message = new MyMessage();
            FormatDelegate currentFormat;
            currentFormat = Format.FormatType[comboBox1.SelectedIndex];

            //MyMobile.SMSProvider.ReceiveSMS(Format.OnSMSReceived(message.ToString(), currentFormat)); //lab4, task 0, variant 1

            if (message.Text != null) {
                myReceivedMessages.Add(message);
                if (!comboBox2.Items.Contains(message.User)) {
                    comboBox2.Items.Add(message.User);
                }
            }

            List<MyMessage> listToDisplay = new List<MyMessage>();
            listToDisplay = myReceivedMessages;
            MyFilter filter = new MyFilter();

            if (checkBoxAndLogic.Checked == true) {
                listToDisplay = filter.FilterAnd(myReceivedMessages,comboBox2.SelectedItem, textBox1.Text,dateTimePicker1.Value,dateTimePicker2.Value);                                   
            }
            if (checkBoxOrLogic.Checked == true) {
                listToDisplay = filter.FilterOr(myReceivedMessages, comboBox2.SelectedItem, textBox1.Text, dateTimePicker1.Value, dateTimePicker2.Value);
            }

            ShowMessages(listToDisplay, currentFormat);
            MyMobile.SMSProvider.ReceiveSMS(message);
           
        }

        private void ShowMessages(List<MyMessage> myReceivedMessages, FormatDelegate currentFormat) {
            MessageListView.Items.Clear();
            foreach (MyMessage message in myReceivedMessages) {
                MessageListView.Items.Add(new ListViewItem(new[] { message.User, Format.OnSMSReceived(message, currentFormat) }));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxOrLogic.Checked == true) { checkBoxAndLogic.Checked = false; }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxAndLogic.Checked == true) { checkBoxOrLogic.Checked = false; }
        }
    }
}
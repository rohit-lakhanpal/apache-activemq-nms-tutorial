using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.Util;

namespace Tutorial.Chat.Ui
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class ChatForm : Form
    {
        /// <summary>
        /// The producer
        /// </summary>
        private IMessageProducer _producer;
        /// <summary>
        /// The consumer
        /// </summary>
        private IMessageConsumer _consumer;
        /// <summary>
        /// The session
        /// </summary>
        private ISession _session;
        /// <summary>
        /// The queue
        /// </summary>
        private const string Queue = "queue://App.Message.Processing.Queue";
        /// <summary>
        /// The topic
        /// </summary>
        private const string Topic = "topic://App.Message.Chat.Topic";


        /// <summary>
        /// Gets or sets the form title.
        /// </summary>
        /// <value>
        /// The form title.
        /// </value>
        public string FormTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatForm"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ChatForm(string name): this()
        {
            this.FormTitle = name;
            this.Text = $"Chat Application: {name}";
            this.ActiveControl = inputTextBox;
        }

        /// <summary>
        /// </summary>
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatForm"/> class.
        /// </summary>
        public ChatForm()
        {
            InitializeComms();
            InitializeComponent();
            (new Thread(InitialiseListener) {IsBackground = true}).Start();
        }

        /// <summary>
        /// Initialises the listener.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void InitialiseListener()
        {
            while (true)
            {
                var message = (ITextMessage)_consumer.Receive();
                if (message == null) continue;
                if (string.IsNullOrWhiteSpace(message.Text)) continue;
                AppendOutput(message.Text);
            }
        }

        /// <summary>
        /// Appends the output.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AppendOutput(string value)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                outputTextBox.AppendText(value);
                outputTextBox.AppendText(Environment.NewLine);
            });
        }

        /// <summary>
        /// Initializes the comms.
        /// </summary>
        private void InitializeComms()
        {
            const string userName = "admin";
            const string password = "admin";
            const string uri = "activemq:tcp://localhost:61616";
            var connecturi = new Uri(uri);
            var factory = new NMSConnectionFactory(connecturi);
            var connection = factory.CreateConnection(userName, password);
            connection.Start();
            _session = connection.CreateSession();
            var queueDestination = SessionUtil.GetDestination(_session, Queue);
            var topicDestination = SessionUtil.GetDestination(_session, Topic);
            _consumer = _session.CreateConsumer(topicDestination);
            _producer = _session.CreateProducer(queueDestination);
        }

        /// <summary>
        /// Handles the Click event of the submitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void submitButton_Click(object sender, EventArgs e)
        {
            // On pressing submit, the application should send the contents of the input box over
            var inputText = this.inputTextBox.Text;
            if (!string.IsNullOrWhiteSpace(inputText))
            {
                var message = $"{this.FormTitle}: {inputText}";
                SendMessageToProcessingQueue(message);
            }
            // Set input textbox to empty & set focus to input text box
            this.inputTextBox.Text = string.Empty;
            this.ActiveControl = this.inputTextBox;
        }

        /// <summary>
        /// Sends the message to processing queue.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void SendMessageToProcessingQueue(string message)
        {
            // Create a text message
            var request = _producer.CreateTextMessage(message);
            _producer.Send(request);
        }
    }
}

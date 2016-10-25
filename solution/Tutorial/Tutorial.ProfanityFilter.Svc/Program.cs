using System;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.Util;

namespace Tutorial.ProfanityFilter.Svc
{
    /// <summary>
    /// This class is the starting point of the Tutorial.ProfanityFilter.Svc application.
    /// </summary>
    class Program
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
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            // Call constructor & then Start
            new Program().Start();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void Start()
        {
            while (true)
            {
                // Wait for a text message
                var request = _consumer.Receive() as ITextMessage;
                var requestText = request?.Text;
                if (string.IsNullOrWhiteSpace(requestText)) continue;

                // On consuming a text message send it for processing
                Task.Factory.StartNew(async () =>
                {
                    var messageToPublish = await ProfanityFilterService.Filter(requestText);
                    var response = _session.CreateTextMessage();
                    response.Text = messageToPublish;
                    _producer.Send(response);
                });
            }
        }


        /// <summary>
        /// Prevents a default instance of the <see cref="Program"/> class from being created.
        /// </summary>
        private Program()
        {
            // Initialise all the comms
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
            _consumer = _session.CreateConsumer(queueDestination);
            _producer = _session.CreateProducer(topicDestination);
        }
    }
}

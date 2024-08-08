using System.Text;
using MQTTnet.Server;

namespace TestMqttBroker
{
	public class MqttServerController
	{
		private ILogger<MqttServerController> _logger;

		public MqttServerController(ILogger<MqttServerController> logger)
		{
			_logger = logger;
			// Inject other services via constructor if needed
		}

		public Task ValidateConnection(ValidatingConnectionEventArgs e)
		{
			// TODO : Alex - Can check for permissions and validate token here
			//if (e.ClientId != "ValidClientId")
			//{
			//	e.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
			//}

			//if (e.UserName != "ValidUser")
			//{
			//	e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
			//}

			//if (e.Password != "SecretPassword")
			//{
			//	e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
			//}

			return Task.CompletedTask;
		}

		public Task OnClientConnected(ClientConnectedEventArgs e)
		{
			var data = new object[] { e.ClientId, e.Endpoint };
			_logger.LogInformation("Client '{Client}' connected on endpoint {Endpoint}", data);
			return Task.CompletedTask;
		}

		public Task OnClientDisconnected(ClientDisconnectedEventArgs e)
		{
			_logger.LogInformation("Client '{Client}' disconnected", e.ClientId);
			return Task.CompletedTask;
		}

		public async Task InterceptingSubscriptionAsync(InterceptingSubscriptionEventArgs e)
		{
			var topicFilter = e.TopicFilter;

			// List of restricted topics
			var restrictedTopics = new List<string> { "restricted/topic1", "restricted/topic2" };

			if (restrictedTopics.Contains(topicFilter.Topic))
			{
				Console.WriteLine($"Subscribing to topic '{topicFilter.Topic}' is not allowed.");
				e.ProcessSubscription = false;  // Prevent the subscription
			}
			else
			{
				Console.WriteLine($"Client is subscribing to topic '{topicFilter.Topic}'");
			}

			await Task.CompletedTask;
		}

		public Task InterceptingPublishAsync(InterceptingPublishEventArgs e)
		{
			var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
			Console.WriteLine($"Received message on topic '{e.ApplicationMessage.Topic}': {payload}");

			// Do something with the payload, save it to a database, etc.

			return Task.CompletedTask;
		}
	}
}

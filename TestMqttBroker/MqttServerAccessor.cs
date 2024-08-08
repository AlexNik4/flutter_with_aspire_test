using MQTTnet.Server;

namespace TestMqttBroker
{
	public interface IMqttServerAccessor
	{
		MqttServer? MqttServer { get; set; }
	}

	public class MqttServerAccessor : IMqttServerAccessor
	{
		public MqttServer? MqttServer { get; set; }
	}
}

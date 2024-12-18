using SimpleMqtt;
namespace RobotProject.Services;

public class MqttMessageProcessingService : IHostedService
{
    private readonly IUserRepository _userRepository;
    private readonly SimpleMqttClient _mqttClient;

    public MqttMessageProcessingService(IUserRepository userRepository, SimpleMqttClient mqttClient)
    {
      	_userRepository = userRepository;  
      	_mqttClient = mqttClient;
        
        _mqttClient.OnMessageReceived += (sender, args) => {
            Console.WriteLine($"Incoming MQTT message on {args.Topic}:{args.Message}");
        };
    }

    public async Task PublishMessage(string message, string topic) => await _mqttClient.PublishMessage(message, topic);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.SubscribeToTopic("#");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _mqttClient.Dispose();
    }
}
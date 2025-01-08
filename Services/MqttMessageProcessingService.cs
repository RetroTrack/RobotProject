using RobotProject.Services.Models;
using RobotProject.Services.Repository;
using SimpleMqtt;
namespace RobotProject.Services;

public class MqttMessageProcessingService : IHostedService
{
    private readonly IRobotRepository _robotRepository;
    public SimpleMqttClient MqttClient { get; set; }

    public MqttMessageProcessingService(IRobotRepository robotRepository, SimpleMqttClient mqttClient)
    {
        _robotRepository = robotRepository;
        MqttClient = mqttClient;
    }

    public void OnMessageReceived(object? sender, SimpleMqttMessage args)
    {
        Console.WriteLine($"Incoming MQTT message on {args.Topic}:{args.Message}");
        if (args.Message == null) return;

        switch (args.Topic)
        {
            case "robot/request-initialize":
                _ = PublishMessage(args.Message + (_robotRepository.IsRobotInDatabase(Convert.ToInt32(args.Message)) ? ":denied" : ":allowed"), "robot/feedback-initialize");
                break;
            case "robot/db-initialize":
                AddRobotToDatabase(args);
                break;
            case "robot/publish-measurement":
                _robotRepository.InsertMeasurement(args.Message);
                break;
            case "robot/publish-notification":
                _robotRepository.InsertNotification(args.Message);
                break;
        }
    }

    public async Task PublishMessage(string message, string topic) => await MqttClient.PublishMessage(message, topic);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        MqttClient.OnMessageReceived += OnMessageReceived;
        await MqttClient.SubscribeToTopic("#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        MqttClient.Dispose();
        return Task.CompletedTask;
    }

    public void AddRobotToDatabase(SimpleMqttMessage args)
    {
        if (args.Message == null) return;
        _robotRepository.InsertRobot(GetRobotFromString(args.Message));
    }

    public Robot GetRobotFromString(string robotString)
    {
        string[] mainSplit = robotString.Split(':');
        string[] commaSplit = mainSplit[1].Split(',');
        Robot robot = new Robot(Convert.ToInt32(mainSplit[0]), commaSplit[0]);

        int sensorCount = Convert.ToInt32(commaSplit[1]);
        for (int i = 0; i < sensorCount; i++)
        {
            robot.Sensors.Add(new Sensor(i, commaSplit[i + 2]));
        }
        return robot;
    }

    public string GetStringFromRobot(Robot robot)
    {
        string robotString = $"{robot.Id}:{robot.Name},{robot.Sensors.Count}";
        foreach (Sensor sensor in robot.Sensors)
        {
            robotString += $",{sensor.Name}";
        }
        return robotString;
    }
}
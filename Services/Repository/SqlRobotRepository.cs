using System.Data;
using Microsoft.Data.SqlClient;
using RobotProject.Services.Models;

namespace RobotProject.Services.Repository;
public class SqlRobotRepository : IRobotRepository
{
    private string _connectionString;

    public SqlRobotRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertRobot(Robot robot, string tables = "all")
    {
        if (IsRobotInDatabase(robot.Id)) return;
        using var connection = new SqlConnection(_connectionString);

        connection.Open();
        //Insert robot
        if (tables.Contains("all") || tables.Contains("robot"))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO [Robot] (RobotId, Name) VALUES (@RobotId, @Name)";
                command.Parameters.AddWithValue("@RobotId", robot.Id);
                command.Parameters.AddWithValue("@Name", robot.Name);
                command.ExecuteNonQuery();
            }


        //Insert robot sensors
        if (tables.Contains("all") || tables.Contains("sensor") || tables.Contains("measurement"))
            foreach (Sensor sensor in robot.Sensors)
            {

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO [Sensor] (SensorId, SensorName, RobotId) VALUES (@SensorId, @SensorName, @RobotId)";
                    command.Parameters.AddWithValue("@SensorId", sensor.Id);
                    command.Parameters.AddWithValue("@SensorName", sensor.Name);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }

                //Insert sensor measurements
                if (tables.Contains("all") || tables.Contains("measurement"))
                    foreach (Measurement measurement in sensor.measurements)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = $"INSERT INTO [Measurement] (MeasurementId, MeasurementValue, MeasurementTimestamp, SensorId) VALUES (@MeasurementId, @MeasurementValue, @MeasurementTimestamp, @SensorId)";
                            command.Parameters.AddWithValue("@MeasurementId", sensor.Id);
                            command.Parameters.AddWithValue("@MeasurementValue", measurement.Value);
                            command.Parameters.AddWithValue("@MeasurementTimestamp", measurement.Timestamp);
                            command.Parameters.AddWithValue("@SensorId", sensor.Id);
                            command.ExecuteNonQuery();
                        }
                    }
            }

        //Insert medicines linked to robot
        if (tables.Contains("all") || tables.Contains("medicine"))
            foreach (Medicine medicine in robot.Medicines)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO [Medicine] (MedicineId, MedicineName, MedcineDescription, RobotId) VALUES (@MedicineId, @MedicineName, @MedicineDescription, @RobotId)";
                    command.Parameters.AddWithValue("@MedicineId", medicine.Id);
                    command.Parameters.AddWithValue("@MedicineName", medicine.Name);
                    command.Parameters.AddWithValue("@MedicineDescription", medicine.Description);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }
            }

        //Insert reminders linked to robot
        if (tables.Contains("all") || tables.Contains("reminder"))
            foreach (Reminder reminder in robot.Reminders)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO [Reminder] (ReminderId, ReminderType, ReminderName, ReminderDescription, ReminderTimestamp, RobotId) VALUES (@ReminderId, @ReminderType, @ReminderName, @ReminderDescription, @ReminderTimestamp, @RobotId)";
                    command.Parameters.AddWithValue("@ReminderId", reminder.Id);
                    command.Parameters.AddWithValue("@ReminderType", reminder.Type);
                    command.Parameters.AddWithValue("@ReminderName", reminder.Name);
                    command.Parameters.AddWithValue("@ReminderDescription", reminder.Description);
                    command.Parameters.AddWithValue("@ReminderTimestamp", reminder.Timestamp);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }
            }

        //Insert notifications from robot
        if (tables.Contains("all") || tables.Contains("notification"))
            foreach (Notification notification in robot.Notifications)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO [Notification] (NotificationId, NotificationLevel, NotificationDescription, NotificationTimestamp, RobotId) VALUES (@NotificationId, @NotificationLevel, @NotificationDescription, @NotificationTimestamp, @RobotId)";
                    command.Parameters.AddWithValue("@NotificationId", notification.Id);
                    command.Parameters.AddWithValue("@NotificationLevel", (int)notification.Level);
                    command.Parameters.AddWithValue("@NotificationDescription", notification.Description);
                    command.Parameters.AddWithValue("@NotificationTimestamp", notification.Timestamp);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }
            }

        connection.Close();
    }

    public void UpdateRobot(Robot robot, string tables = "all")
    {
        if (!IsRobotInDatabase(robot.Id)) return;
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        // Update robot
        if (tables.Contains("all") || tables.Contains("robot"))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE [Robot] SET Name = @Name WHERE RobotId = @RobotId";
                command.Parameters.AddWithValue("@RobotId", robot.Id);
                command.Parameters.AddWithValue("@Name", robot.Name);
                command.ExecuteNonQuery();
            }

        // Update robot sensors
        if (tables.Contains("all") || tables.Contains("sensor") || tables.Contains("measurement"))
            foreach (Sensor sensor in robot.Sensors)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE [Sensor] SET SensorName = @SensorName WHERE SensorId = @SensorId AND RobotId = @RobotId";
                    command.Parameters.AddWithValue("@SensorId", sensor.Id);
                    command.Parameters.AddWithValue("@SensorName", sensor.Name);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }

                // Update sensor measurements
                if (tables.Contains("all") || tables.Contains("measurement"))
                    foreach (Measurement measurement in sensor.measurements)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = $"UPDATE [Measurement] SET MeasurementValue = @MeasurementValue, MeasurementTimestamp = @MeasurementTimestamp WHERE MeasurementId = @MeasurementId AND SensorId = @SensorId";
                            command.Parameters.AddWithValue("@MeasurementId", measurement.Id);
                            command.Parameters.AddWithValue("@MeasurementValue", measurement.Value);
                            command.Parameters.AddWithValue("@MeasurementTimestamp", measurement.Timestamp);
                            command.Parameters.AddWithValue("@SensorId", sensor.Id);
                            command.ExecuteNonQuery();
                        }
                    }
            }

        // Update medicines linked to robot
        if (tables.Contains("all") || tables.Contains("medicine"))
            foreach (Medicine medicine in robot.Medicines)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE [Medicine] SET MedicineName = @MedicineName, MedicineDescription = @MedicineDescription WHERE MedicineId = @MedicineId AND RobotId = @RobotId";
                    command.Parameters.AddWithValue("@MedicineId", medicine.Id);
                    command.Parameters.AddWithValue("@MedicineName", medicine.Name);
                    command.Parameters.AddWithValue("@MedicineDescription", medicine.Description);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }
            }

        // Update reminders linked to robot
        if (tables.Contains("all") || tables.Contains("reminder"))
            foreach (Reminder reminder in robot.Reminders)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE [Reminder] SET ReminderType = @ReminderType, ReminderName = @ReminderName, ReminderDescription = @ReminderDescription, ReminderTimestamp = @ReminderTimestamp WHERE ReminderId = @ReminderId AND RobotId = @RobotId";
                    command.Parameters.AddWithValue("@ReminderId", reminder.Id);
                    command.Parameters.AddWithValue("@ReminderType", reminder.Type);
                    command.Parameters.AddWithValue("@ReminderName", reminder.Name);
                    command.Parameters.AddWithValue("@ReminderDescription", reminder.Description);
                    command.Parameters.AddWithValue("@ReminderTimestamp", reminder.Timestamp);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }
            }

        // Update notifications from robot
        if (tables.Contains("all") || tables.Contains("notification"))
            foreach (Notification notification in robot.Notifications)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE [Notification] SET NotificationLevel = @NotificationLevel, NotificationDescription = @NotificationDescription, NotificationTimestamp = @NotificationTimestamp WHERE NotificationId = @NotificationId AND RobotId = @RobotId";
                    command.Parameters.AddWithValue("@NotificationId", notification.Id);
                    command.Parameters.AddWithValue("@NotificationLevel", (int)notification.Level);
                    command.Parameters.AddWithValue("@NotificationDescription", notification.Description);
                    command.Parameters.AddWithValue("@NotificationTimestamp", notification.Timestamp);
                    command.Parameters.AddWithValue("@RobotId", robot.Id);
                    command.ExecuteNonQuery();
                }
            }

        connection.Close();
    }


    public List<Robot> GetAllRobots()
    {
        using var connection = new SqlConnection(_connectionString);

        List<Robot> robots = [];
        connection.Open();

        //Get robots
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT * FROM [Robot]";
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                robots.Add(new Robot(reader.GetInt32(0), reader.GetString(1)));
            }
        }

        //Get sensors of robots
        foreach (Robot robot in robots)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select * FROM [Sensor] WHERE RobotId = @RobotId";
                command.Parameters.AddWithValue("@RobotId", robot.Id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    robot.Sensors.Add(new Sensor(reader.GetInt32(0), reader.GetString(1)));
                }
            }
        }

        //Get measurements of sensors
        foreach (Robot robot in robots)
        {
            foreach (Sensor sensor in robot.Sensors)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"Select * FROM [Measurement] WHERE SensorId = @SensorId";
                    command.Parameters.AddWithValue("@SensorId", sensor.Id);
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sensor.measurements.Add(new Measurement(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2)));
                    }
                }
            }
        }

        //Get medicines linked to robots
        foreach (Robot robot in robots)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select * FROM [Medicine] WHERE RobotId = @RobotId";
                command.Parameters.AddWithValue("@RobotId", robot.Id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    robot.Medicines.Add(new Medicine(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
            }
        }

        //Get reminders linked to robots
        foreach (Robot robot in robots)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select * FROM [Reminder] WHERE RobotId = @RobotId";
                command.Parameters.AddWithValue("@RobotId", robot.Id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    robot.Reminders.Add(new Reminder(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4)));
                }
            }
        }

        //Get notifications from robots
        foreach (Robot robot in robots)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select * FROM [Notification] WHERE RobotId = @RobotId";
                command.Parameters.AddWithValue("@RobotId", robot.Id);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    robot.Notifications.Add(
                        new Notification(reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetDateTime(3)));
                }
            }
        }

        connection.Close();
        return robots;
    }

    public bool IsRobotInDatabase(int id)
    {
        return GetAllRobots().FindAll(bot => bot.Id.Equals(id)).Count > 0;
    }

    public void InsertMeasurement(string msg)
    {
        int currentMax = 0;
        using var connection = new SqlConnection(_connectionString);

        connection.Open();
        try
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select MAX(MeasurementId) FROM [Measurement]";
                using var reader = command.ExecuteReader();
                while (reader.Read()) currentMax = reader.GetInt32(0);
            }
        }
        catch
        {
            currentMax = -1;
        }
        string[] mainSplit = msg.Split(';');
        string[] commaSplit = mainSplit[1].Split(',');
        try
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO [Measurement] (MeasurementId, MeasurementValue, MeasurementTimestamp, SensorId) VALUES (@MeasurementId, @MeasurementValue, @MeasurementTimestamp, @SensorId)";
                command.Parameters.AddWithValue("@MeasurementId", currentMax + 1);
                command.Parameters.AddWithValue("@MeasurementValue", Convert.ToInt32(commaSplit[0]));
                command.Parameters.AddWithValue("@MeasurementTimestamp", Convert.ToDateTime(commaSplit[1]));
                command.Parameters.AddWithValue("@SensorId", Convert.ToInt32(mainSplit[0]));
                command.ExecuteNonQuery();
            }
        }
        catch { }
        connection.Close();
    }

    public void InsertReminder(Reminder reminder, int robotId)
    {
        int currentMax = 0;
        using var connection = new SqlConnection(_connectionString);

        connection.Open();
        try
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select MAX(ReminderId) FROM [Reminder]";
                using var reader = command.ExecuteReader();
                while (reader.Read()) currentMax = reader.GetInt32(0);
            }
        }
        catch
        {
            currentMax = -1;
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO [Reminder] (ReminderId, ReminderType, ReminderName, ReminderDescription, ReminderTimestamp, RobotId) VALUES (@ReminderId, @ReminderType, @ReminderName, @ReminderDescription, @ReminderTimestamp, @RobotId)";
            command.Parameters.AddWithValue("@ReminderId", currentMax + 1);
            command.Parameters.AddWithValue("@ReminderType", reminder.Type);
            command.Parameters.AddWithValue("@ReminderName", reminder.Name);
            command.Parameters.AddWithValue("@ReminderDescription", reminder.Description);
            command.Parameters.AddWithValue("@ReminderTimestamp", reminder.Timestamp);
            command.Parameters.AddWithValue("@RobotId", robotId);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    public void InsertNotification(string message)
    {
        int currentMax = 0;
        using var connection = new SqlConnection(_connectionString);

        connection.Open();
        try
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"Select MAX(NotificationId) FROM [Notification]";
                using var reader = command.ExecuteReader();
                while (reader.Read()) currentMax = reader.GetInt32(0);
            }
        }
        catch
        {
            currentMax = -1;
        }
        string[] mainSplit = message.Split(';');
        var values = mainSplit[1].Split(["],["], StringSplitOptions.None)
                .Select(x => x.Trim('[', ']'))
                .ToList();


        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO [Notification] (NotificationId, NotificationLevel, NotificationDescription, NotificationTimestamp, RobotId) VALUES (@NotificationId, @NotificationLevel, @NotificationDescription, @NotificationTimestamp, @RobotId)";
            command.Parameters.AddWithValue("@NotificationId", currentMax + 1);
            command.Parameters.AddWithValue("@NotificationLevel", Convert.ToInt32(values[0]));
            command.Parameters.AddWithValue("@NotificationDescription", values[1]);
            command.Parameters.AddWithValue("@NotificationTimestamp", Convert.ToDateTime(values[2]));
            command.Parameters.AddWithValue("@RobotId", mainSplit[0]);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    public void InsertMedicine(Medicine medicine, int robotId)
    {
        int currentMax = 0;
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        if (medicine.Id != -1)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE [Medicine] SET MedicineName = @MedicineName, MedicineDescription = @MedicineDescription WHERE MedicineId = @MedicineId AND RobotId = @RobotId";
                command.Parameters.AddWithValue("@MedicineId", medicine.Id);
                command.Parameters.AddWithValue("@MedicineName", medicine.Name);
                command.Parameters.AddWithValue("@MedicineDescription", medicine.Description);
                command.Parameters.AddWithValue("@RobotId", robotId);
                command.ExecuteNonQuery();
            }
        }
        else
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"Select MAX(MedicineId) FROM [Medicine]";
                    using var reader = command.ExecuteReader();
                    while (reader.Read()) currentMax = reader.GetInt32(0);
                }
            }
            catch
            {
                currentMax = -1;
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO [Medicine] (MedicineId, MedicineName, MedicineDescription, RobotId) VALUES (@MedicineId, @MedicineName, @MedicineDescription, @RobotId)";
                command.Parameters.AddWithValue("@MedicineId", currentMax + 1);
                command.Parameters.AddWithValue("@MedicineName", medicine.Name);
                command.Parameters.AddWithValue("@MedicineDescription", medicine.Description);
                command.Parameters.AddWithValue("@RobotId", robotId);
                command.ExecuteNonQuery();
            }
        }
        connection.Close();

    }

    public void RemoveReminder(Reminder reminder, int robotId)
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"DELETE FROM [Reminder] WHERE ReminderId = @ReminderId AND RobotId = @RobotId";
            command.Parameters.AddWithValue("@ReminderId", reminder.Id);
            command.Parameters.AddWithValue("@RobotId", robotId);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    public void RemoveMedicine(Medicine medicine, int robotId)
    {
        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"DELETE FROM [Medicine] WHERE MedicineId = @MedicineId AND RobotId = @RobotId";
            command.Parameters.AddWithValue("@MedicineId", medicine.Id);
            command.Parameters.AddWithValue("@RobotId", robotId);
            command.ExecuteNonQuery();
        }

        connection.Close();
    }

    public List<Reminder> GetReminders()
    {
        List<Reminder> reminders = [];
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"Select * FROM [Reminder]";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                reminders.Add(new Reminder(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4))
                {
                    RobotId = reader.GetInt32(5)
                });
            }
        }
        connection.Close();
        return reminders;
    }
}
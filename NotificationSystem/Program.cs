using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddTransient<NotificationService>();
services.AddTransient<iMessageService, EmailService>();
services.AddTransient<ILogger, ConsoleLogger>();


var serviceProvider = services.BuildServiceProvider();
var notificationService = serviceProvider.GetRequiredService<NotificationService>();

notificationService.Notify();

Console.ReadLine();

class NotificationService
{
    private readonly iMessageService _messageService;

    public NotificationService(iMessageService messageService)
    {
        _messageService = messageService;
    }

    public void Notify()
    {

        _messageService.SendMessage("Message");
    }

    public void NotifyAll()
    {
        _messageService.SendMessage("Message");
    }
}

interface iMessageService
{
    void SendMessage(string message);
}

class EmailService : iMessageService
{
    private readonly ILogger _logger;
    private readonly IDatabase _database;
    public EmailService(ILogger logger, IDatabase database) 
    {
        _logger = logger;
        _database = database;
    }
    public void SendMessage(string message)
    {
        Console.WriteLine("Email - " + message);

        _logger.Log("Message sent");

        _database.Save();
    }

}

class WhatsAppService : iMessageService
{
    public void SendMessage(string message)
    {
        Console.WriteLine("WhatsApp - " + message);
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}

public class PostgresDatabase : IDatabase
{
    private readonly ILogger _logger;
    public PostgresDatabase(ILogger logger)
    {
        _logger = logger;
    }
    public void Save()
    {
        Console.WriteLine("Data saved");
        _logger.Log("Data Saved!");

    }
}
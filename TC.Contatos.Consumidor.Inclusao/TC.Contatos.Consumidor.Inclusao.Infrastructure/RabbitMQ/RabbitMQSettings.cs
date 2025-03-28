﻿namespace Infrastructure.RabbitMQ;

public class RabbitMQSettings
{
    public required string VirtualHost { get; set; }
    public required string Queue { get; set; }
    public required string HostName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

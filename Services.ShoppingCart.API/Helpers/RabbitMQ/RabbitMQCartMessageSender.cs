using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Services.ShoppingCart.API.Models.Cart;

namespace Services.ShoppingCart.API.Helpers.RabbitMQ;

public class RabbitMQCartMessageSender : IRabbitMQCartMessageSender
{
    private readonly string _hostName = "localhost";
    private readonly string _userName = "guest";
    private readonly string _password = "guest";
    private IConnection _connection;

    public void SendMessage(CartDto cartDto, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        };

        _connection = factory.CreateConnection();

        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queueName, false, false, false, null);

        var json = JsonSerializer.Serialize(cartDto);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }
}
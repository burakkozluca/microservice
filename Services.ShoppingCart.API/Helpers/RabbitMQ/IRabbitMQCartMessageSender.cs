using Services.ShoppingCart.API.Models.Cart;

namespace Services.ShoppingCart.API.Helpers.RabbitMQ;

public interface IRabbitMQCartMessageSender
{
    void SendMessage(CartDto cartDto, string queueName);
}
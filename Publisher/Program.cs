using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Conexão RabbitMQ em localhost

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            //Abertura de conexão com um nó RabbitMQ

            using (var connection = factory.CreateConnection())
           
            // Criação de canal onde vamos definir uma fila, uma mensagem e publicar a mensagem

           /*queue => nome da fila 
             durable => se igual a true a fila permanece ativa após servidor ser reiniciado
             exclusive => se igual a true ela só pode ser acessada via conexão atual e são excluídas ao fechar a conexão
             autoDelete => se igual a true será deletada automaticamente após os consumidores usar a fila
            
            */

            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "saudacao_1",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null

                                    );

                string message = "Bem-vindo ao RabbitMQ";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "saudacao_1",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($" [x] Enviada: {message}");

            }
            Console.ReadLine();

        }
    }
}

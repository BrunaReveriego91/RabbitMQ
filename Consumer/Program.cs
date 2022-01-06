using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
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

                //Solicita a entrega das mensagens de forma assíncrona e fornece um
                //retorno de chamada

                var consumer = new EventingBasicConsumer(channel);


                /* Recebe a mensagem da fila e converte para um string e imprime no console a mensagem */

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Recebida : {message}");
                };

                /* Indicamos o consumo da mensagem */

                channel.BasicConsume(queue: "saudacao_1",
                                     autoAck: true,
                                     consumer: consumer);

            }
            Console.ReadLine();

        }
    }
}

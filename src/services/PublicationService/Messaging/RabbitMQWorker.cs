﻿using Newtonsoft.Json;
using PublicationService.DTOs;
using PublicationService.Interfaces;
using PublicationService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQLibrary;
using System.Text;

namespace PublicationService.Messaging
{
    public class RabbitMQWorker : BackgroundService
    {
        private readonly ILogger<RabbitMQWorker> _logger;
        private readonly TwitterService _twitterService;

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQWorker(ILogger<RabbitMQWorker> logger, TwitterService twitterService)
        {
            _logger = logger;
            _twitterService = twitterService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            string? password = Environment.GetEnvironmentVariable("redactieportaal_rabbitmq_pass") ?? throw new ArgumentNullException("No Password Found");

            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = password,
                DispatchConsumersAsync = true
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "news-item-exchange",
                                     durable: true,
                                     type: ExchangeType.Topic);

            _channel.QueueDeclare(queue: "news-item-queue",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var routingKeys = new[]
            {
                RoutingKeyType.NewsItemDeleted,
                RoutingKeyType.NewsItemCreated,
                RoutingKeyType.NewsItemDispose,
                RoutingKeyType.NewsItemArchive,
                RoutingKeyType.NewsItemUpdated,
                RoutingKeyType.NewsItemPublishTwitter
            };
            foreach (var routingKey in routingKeys)
            {
                _channel.QueueBind(queue: "news-item-queue",
                                   exchange: "news-item-exchange",
                                   routingKey: routingKey);
            }

            _channel.BasicQos(prefetchSize: 0,
                              prefetchCount: 1,
                              global: false);

            _logger.LogInformation("RabbitMQWorker started");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"Received message: {message} with routingkey: {eventArgs.RoutingKey}");

                switch (eventArgs.RoutingKey)
                {
                    case RoutingKeyType.NewsItemPublishTwitter:
                        _logger.LogInformation("NewsItem will be published to Twitter.");
                        var publishNewsItemDTO = JsonConvert.DeserializeObject<PublishNewsItemDTO>(message);

                        await _twitterService.PublishNewsItem(publishNewsItemDTO);

                        break;
                    default:
                        break;
                }

                await Task.Delay(1000);
            };

            _channel.BasicConsume(queue: "news-item-queue",
                                  autoAck: true,
                                  consumer: consumer);

            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();

            _logger.LogInformation("RabbitMQWorker stopped");

            return base.StopAsync(cancellationToken);
        }
    }
}

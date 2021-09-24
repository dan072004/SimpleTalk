using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ST_KafkaConsumer.Handlers
{
	public class KafkaConsumerHandler : IHostedService
	{
		private readonly string topic = "simpletalk_topic";
		public Task StartAsync(CancellationToken cancellationToken)
		{
			//var server = Environment.GetEnvironmentVariable("KAFKA_SERVER") ?? "localhost";
			var conf = new ConsumerConfig
			{
				GroupId = "st_consumer_group",
				BootstrapServers = $"kafka-broker-1:9092",
				AutoOffsetReset = AutoOffsetReset.Earliest
			};
			using (var builder = new ConsumerBuilder<Ignore,
				string>(conf).Build())
			{
				builder.Subscribe(topic);
				var cancelToken = new CancellationTokenSource();
				try
				{
					while (true)
					{
						var consumer = builder.Consume(cancelToken.Token);
						Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
					}
				}
				catch (Exception)
				{
					builder.Close();
				}
			}
			return Task.CompletedTask;
		}
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
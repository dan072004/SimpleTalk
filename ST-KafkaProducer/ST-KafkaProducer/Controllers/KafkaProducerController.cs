using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace ST_KafkaProducer.Controllers
{
	[Route("api/kafka")]
	[ApiController]
	public class KafkaProducerController : ControllerBase
	{
		private readonly ProducerConfig config = new ProducerConfig
			{ BootstrapServers = "kafka-broker-1:9092" };
		private readonly string topic = "simpletalk_topic";
		[HttpPost]
		public IActionResult Post([FromQuery] string message)
		{
			var result = Created(string.Empty, SendToKafka(topic, message));
			return result;
		}

		private Object SendToKafka(string topic, string message)
		{
			using (var producer =
				new ProducerBuilder<Null, string>(config).Build())
			{
				try
				{
					return producer.ProduceAsync(topic, new Message<Null, string> { Value = message })
						.GetAwaiter()
						.GetResult();
				}
				catch (Exception e)
				{
					Console.WriteLine($"Oops, something went wrong: {e}");
				}
			}
			return null;
		}
	}
}

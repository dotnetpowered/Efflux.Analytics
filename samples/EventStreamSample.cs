using System;
using System.Threading.Tasks;
using Efflux.Analytics;
using Microsoft.StreamProcessing;
using System.Reactive.Linq;

namespace Efflux.Samples.Analytics
{
    public class EventStreamSample
    {
        // Event Stream
        // ------------------------------------------------------------
        public static async Task EventStreamDemo(ITopicFactory factory)
        {
            var topic3 = factory.OpenTopic<Person>("event-stream-demo");

            await topic3.AppendAsync(new Person() { FullName = "Mad Dog" });
            var consumer3 = await topic3.CreateConsumerAsync("consumer3");

            Console.WriteLine($"Starting consumer at {consumer3.CurrentOffset}");

            var stream = consumer3.ToStreamEventObserable();

            // Partionable stream using message group meta data???
            //var input = stream.ToPartitionedStreamable(p => p.Payload.FullName, e=>e.EndTime);

            var input = stream.ToStreamable();
            var output = input.Max(p => p.Age);

            var task = stream.StartObserving();

            Console.WriteLine("Output =");
            output.ToStreamEventObservable().ForEachAsync(e => Console.WriteLine(e)).Wait();

        }
    }
}

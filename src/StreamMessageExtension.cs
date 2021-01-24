using System;
using System.Text.Json;
using Microsoft.StreamProcessing;

namespace Efflux.Analytics
{
    public static class StreamMessageExtension
    {
        public static StreamEvent<T> ToStreamEvent<T>(this EffluxMessage message)
        {
            return StreamEvent.CreatePoint<T>(
                startTime: message.MetaData.Timestamp.Ticks,
                message.PayloadAs<T>());
        }

        public static ObservableEventStream<T> ToStreamEventObserable<T>(
            this Efflux.TypedTopicConsumer<T> consumer
        ) where T: class
        {
            return new ObservableEventStream<T>(consumer);
        }
    }
}

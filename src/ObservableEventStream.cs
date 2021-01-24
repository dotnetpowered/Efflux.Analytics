using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.StreamProcessing;

namespace Efflux.Analytics
{
    public class ObservableEventStream<T> : IObservable<StreamEvent<T>> where T: class
    {
        TypedTopicConsumer<T> consumer;
        List<IObserver<StreamEvent<T>>> observers = new List<IObserver<StreamEvent<T>>>();

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<StreamEvent<T>>> _observers;
            private IObserver<StreamEvent<T>> _observer;
            
            public Unsubscriber(List<IObserver<StreamEvent<T>>> observers, IObserver<StreamEvent<T>> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }

        public ObservableEventStream(TypedTopicConsumer<T> consumer)
        {
            this.consumer = consumer;
        }

        public IDisposable Subscribe(IObserver<StreamEvent<T>> observer)
        {
            observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public Task StartObserving()
        {
            return Task.Run(async () =>
            {
                while (1 == 1)
                {
                    var result = await consumer.ReadAsync();
                    if (result.EndOfStream)
                    {
                        foreach (var observer in observers)
                        {
                            observer.OnCompleted();
                        }
                        return;
                    }
                    else
                    {
                        var streamEvent = StreamEvent.CreatePoint<T>(
                                startTime: result.MetaData.Timestamp.Ticks,
                                result.MessageData);
                        foreach (var observer in observers)
                        {
                            observer.OnNext(streamEvent);
                        }
                    }
                }
            });
        }
    }
}

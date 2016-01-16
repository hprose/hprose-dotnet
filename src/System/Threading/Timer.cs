#if Core

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Threading {
    public delegate void TimerCallback(object state);

    public sealed class Timer : CancellationTokenSource, IDisposable {

        public Timer(TimerCallback callback, object state, int dueTime, int period) {

            Task.Delay(dueTime, Token).ContinueWith(async (t, s) => {
                var tuple = (Tuple<TimerCallback, object>) s;
                if (period == -1 || period == 0) {
                    tuple.Item1(tuple.Item2);
                }
                else {
                    while (!IsCancellationRequested) {
                        tuple.Item1(tuple.Item2);
                        await Task.Delay(period, Token).ConfigureAwait(false);
                    }
                }
            }, Tuple.Create(callback, state), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        public new void Dispose() { base.Cancel(); }
    }
}

#endif

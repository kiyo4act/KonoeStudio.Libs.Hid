using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KonoeStudio.Libs.Hid
{
    public static class ExtendMethods
    {
        // Reference: ASYNC AND CANCELLATION SUPPORT FOR WAIT HANDLES
        // https://thomaslevesque.com/2015/06/04/async-and-cancellation-support-for-wait-handles/
        internal static async Task<bool> WaitOneAsync(this WaitHandle handle, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (handle == null)
            {
                throw new ArgumentNullException($"{nameof(handle)} is null");
            }

            RegisteredWaitHandle? registeredHandle = null;
            CancellationTokenRegistration tokenRegistration = default;
            try
            {
                var tcs = new TaskCompletionSource<bool>();
                registeredHandle = ThreadPool.RegisterWaitForSingleObject(
                    handle,
                    (state, timedOut) => ((TaskCompletionSource<bool>)state).TrySetResult(!timedOut),
                    tcs,
                    millisecondsTimeout,
                    true);
                tokenRegistration = cancellationToken.Register(
                    state => ((TaskCompletionSource<bool>)state).TrySetCanceled(),
                    tcs);
                return await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                registeredHandle?.Unregister(null);
                tokenRegistration.Dispose();
            }
        }

        internal static Task<bool> WaitOneAsync(this WaitHandle handle, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return handle.WaitOneAsync((int)timeout.TotalMilliseconds, cancellationToken);
        }

        internal static Task<bool> WaitOneAsync(this WaitHandle handle, CancellationToken cancellationToken)
        {
            return handle.WaitOneAsync(Timeout.Infinite, cancellationToken);
        }

        internal static byte[] GetWholeData(this IHidReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException($"{nameof(report)} is null");
            }

            byte[] result = new byte[report.Data.Count + 1];
            result[0] = report.ReportId;
            Array.Copy(report.Data.ToArray(), 0, result, 1, report.Data.Count);

            return result;
        }

        internal static void ForAll<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            foreach (var item in sequence)
            {
                action(item);
            }
        }

        internal static async Task ForAllAsync<T>(this IAsyncEnumerable<T> sequence, Action<T> action)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            await foreach (var item in sequence)
            {
                action(item);
            }
        }
    }
}

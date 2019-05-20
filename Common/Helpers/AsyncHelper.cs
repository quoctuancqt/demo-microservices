namespace Common.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
          TaskFactory(CancellationToken.None,
                      TaskCreationOptions.None,
                      TaskContinuationOptions.None,
                      TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return _myTaskFactory
              .StartNew(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            _myTaskFactory
              .StartNew(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunAsync(Func<Task> func)
        {
            _myTaskFactory
              .StartNew(func)
              .ContinueWith(t =>
              {
                  throw t.Exception;
              }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}

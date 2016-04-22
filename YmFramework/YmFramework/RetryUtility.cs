using System;
using System.Threading;

namespace YmFramework
{
    public static class RetryUtility
    {
        public static TResult RetryAction<TResult>(Func<TResult> action, int numRetries, int retryTimeout)
        {
            if (action == null)
                throw new ArgumentNullException("action"); 

            TResult result = default(TResult);
            do
            {
                try
                {
                    result = action.Invoke();
                    numRetries = 0;
                }
                catch
                {
                    if (numRetries <= 0)
                        throw;  // avoid silent failure
                    else
                        Thread.Sleep(retryTimeout);
                }
            } while (numRetries-- > 0);

            return result;
        }
    }
}

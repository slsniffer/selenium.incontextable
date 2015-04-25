using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TeamSL.Selenium.Incontextable.Extensions
{
    public static class WebDriverWaitExtensions
    {
        /// <summary>
        /// Perform wait-until chain.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="driver">The driver.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="poolingTime">The pooling time.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="WebDriverTimeoutException"></exception>
        public static TResult WaitUntil<TResult>(this IWebDriver driver, Func<IWebDriver, TResult> condition, int seconds = 5, int poolingTime = 500, string message = null)
        {
            try
            {
                return driver.Wait(seconds, poolingTime).Until(condition);
            }
            catch (WebDriverTimeoutException exc)
            {
                if (!String.IsNullOrEmpty(message))
                {
                    throw new WebDriverTimeoutException(message, exc);
                }
                throw;
            }
        }

        /// <summary>
        /// Tries the wait until.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="poolingTime">The pooling time.</param>
        /// <returns>True if element found, otherwise false.</returns>
        public static bool TryWaitUntil(this IWebDriver driver, Func<IWebDriver, bool> condition, int seconds = 5, int poolingTime = 500)
        {
            try
            {
                return driver.Wait(seconds, poolingTime).Until(condition);
            }
            catch (WebDriverTimeoutException) { }

            return false;
        }

        /// <summary>
        /// Waits the specified seconds.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="seconds">The seconds. Default is 5 seconds.</param>
        /// <param name="pollingInterval">The polling interval. Default is 500 miliseconds.</param>
        /// <returns></returns>
        public static WebDriverWait Wait(this IWebDriver driver, int seconds = 5, int pollingInterval = 500)
        {
            var result = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            if (pollingInterval != 500) result.PollingInterval = TimeSpan.FromMilliseconds(pollingInterval);
            result.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            return result;
        }
    }
}
using OpenQA.Selenium;
using System.Globalization;
using TeamSL.Selenium.Incontextable.Context;
using TeamSL.Selenium.Incontextable.Elements;

namespace TeamSL.Selenium.Incontextable.Extensions
{
    public static class WebDriverExecuteExtensions
    {
        /// <summary>
        /// Executes the script in current context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="driver">The driver.</param>
        /// <param name="script">The script.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static T ExecuteScript<T>(this IWebDriver driver, string script, params object[] args)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)driver;

            var executeScript = javaScriptExecutor.ExecuteScript(script, args);
            if (executeScript == null)
                return default(T);

            return (T)executeScript;
        }

        /// <summary>
        /// Executes the script in container context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="driver">The driver.</param>
        /// <param name="container">The container.</param>
        /// <param name="script">The script.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static T ExecuteScript<T>(this IWebDriver driver, IDomContainer container, string script, params object[] args)
        {
            var contextable = ((IContextable) container);

            var context = contextable.FindContext();

            context.SwitchContext();
            var result = driver.ExecuteScript<T>(script, args);
            context.SwitchToDefault();

            return result;
        }

        /// <summary>
        /// Writes debug information to browser console.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="message">The message.</param>
        public static void WriteToConsole(this IWebDriver driver, string message)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)driver;

            javaScriptExecutor.ExecuteScript(string.Format(CultureInfo.InvariantCulture, "console.log('{0}')", message));
        }
    }
}
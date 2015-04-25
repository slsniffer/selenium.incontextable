using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Extensions
{
    public static class WebDriverExistExtensions
    {
        public static bool Exists(this IWebDriver element, By by, out IWebElement foundElement)
        {
            try
            {
                foundElement = element.FindElement(@by);
            }
            catch (NoSuchElementException)
            {
                foundElement = null;
                return false;
            }

            return true;
        }

        public static bool Exists(this IWebDriver element, By by)
        {
            try
            {
                element.FindElement(@by);
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }
    }
}
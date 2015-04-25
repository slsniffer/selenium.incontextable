using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Extensions
{
    public static class WebDriverFindExtensions
    {
        public static IWebElement FindByCss(this IWebDriver driver, string cssSelector)
        {
            return driver.FindElement(By.CssSelector(cssSelector));
        }

        public static IWebElement FindByXPath(this IWebDriver driver, string xpath)
        {
            return driver.FindElement(By.XPath(xpath));
        }

        public static IWebElement FindById(this IWebDriver driver, string elementId)
        {
            return driver.FindElement(By.Id(elementId));
        }
    }
}
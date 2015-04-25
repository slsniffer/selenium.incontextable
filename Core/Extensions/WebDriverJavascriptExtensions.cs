using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Extensions
{
    public static class WebDriverJavascriptExtensions
    {
        public static void ScrollToView(this IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.document.body.scrollIntoView()");
        }

        public static void ScrollToView(this IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public static void ScrollInJScrolPanel(this IWebDriver driver, IWebElement element, string scrollPaneSelector)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("var pane = $('{0}');var api = pane.data('jsp');api.scrollTo({1},{2});", scrollPaneSelector, element.Location.X, element.Location.Y));
        }

        public static void ScrollInJScrolPanel(this IWebDriver driver, int x, int y, string scrollPaneSelector)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("var pane = $('{0}');var api = pane.data('jsp');api.scrollTo({1},{2});", scrollPaneSelector, x, y));
        }

        public static void RemoveElementByCssPatch(this IWebDriver driver, string css)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("$('{0}').remove();", css));
        }

        public static void MouseOver(this IWebDriver driver, string css)
        {
            ((IJavaScriptExecutor) driver).ExecuteScript(string.Format("$('{0}').first().mouseover();", css));
        }
    }
}

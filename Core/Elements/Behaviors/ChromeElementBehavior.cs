using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Elements.Behaviors
{
    public class ChromeElementBehavior : BaseElementBehavior, IElementBehavior
    {
        public ChromeElementBehavior(IWebElement element, IWebDriver driver) : base(element, driver)
        {
        }

        public override void Scroll()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _element);
        }

        public override void Click()
        {
            Scroll();
            _element.Click();
        }

        public override void SendKeys(string text)
        {
            Scroll();
            _element.SendKeys(text);
        }

        public override void Clear()
        {
            Scroll();
            _element.Clear();
        }

        public override void ClearAndSend(string text)
        {
            Scroll();
            _element.Clear();
            _element.SendKeys(text);
        }
    }
}
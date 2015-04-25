using System;
using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Elements.Behaviors
{
    public class FirefoxElementBehavior : BaseElementBehavior, IElementBehavior
    {
        public FirefoxElementBehavior(IWebElement element, IWebDriver driver)
            :base(element, driver)
        {
        }

        public bool Displayed
        {
            get { return _element.Displayed; }
        }

        public override void Scroll()
        {
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("$(arguments[0]).focus();", _element);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("$ is not defined"))
                {
                    throw new ElementNotVisibleException("Couldn't find element after jQuery focus", exception);
                }
                throw;
            }
        }
    }
}
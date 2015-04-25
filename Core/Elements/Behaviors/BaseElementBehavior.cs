using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TeamSL.Selenium.Incontextable.Extensions;

namespace TeamSL.Selenium.Incontextable.Elements.Behaviors
{
    public abstract class BaseElementBehavior
    {
        protected readonly IWebElement _element;
        protected readonly IWebDriver _driver;

        protected BaseElementBehavior(IWebElement element, IWebDriver driver)
        {
            _element = element;
            _driver = driver;
        }

        public virtual void ClearAndSend(string text)
        {
            if (!_element.Displayed)
            {
                Scroll();
            }
            _element.Clear();
            _element.SendKeys(text);
        }
        public virtual void ClickAndAccept()
        {
            Click();
            var alert = _driver.SwitchTo().Alert();
            Thread.Sleep(1000);
            alert.Accept();
        }
        public abstract void Scroll();

        public Actions Actions()
        {
            return new Actions(_driver);
        }

        public virtual void Click()
        {
            if (!_element.Displayed)
            {
                Scroll();
            }
            _element.Click();
        }

        public virtual void SendKeys(string text)
        {
            if (!_element.Displayed)
            {
                Scroll();
            }
            _element.SendKeys(text);
        }

        public virtual void Clear()
        {
            if (!_element.Displayed)
            {
                Scroll();
            }
            _element.Clear();
        }

        public virtual void MouseOver()
        {
            try
            {
                Actions().MoveToElement(_element).Build().Perform();
            }
            catch (InvalidOperationException) // Safari workaround
            {
                _driver.ExecuteScript<string>(
                    "if(document.createEvent){var evObj = document.createEvent('MouseEvents');evObj.initEvent('mouseover', true, false); arguments[0].dispatchEvent(evObj);} else if(document.createEventObject) { arguments[0].fireEvent('onmouseover');}",
                    _element);
            }
        }

        public virtual void SetVisibilityTo(bool visible)
        {
            _driver.ExecuteScript<string>("arguments[0].style.display = '" + (visible ? "block" : "none") + "';", _element);
        }
    }
}
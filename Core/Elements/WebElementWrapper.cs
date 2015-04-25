using OpenQA.Selenium;
using TeamSL.Selenium.Incontextable.Elements.Behaviors;

namespace TeamSL.Selenium.Incontextable.Elements
{
    public interface IWebElementWrapper : IContextableWebElement
    {
        void ClickAndAccept();
        void MouseOver();
        void Focus();
        void SetVisibilityTo(bool visible = true);
        void ClearAndSend(string elementValue);
    }

    public class WebElementWrapper : ContextableWebElement, IWebElementWrapper
    {
        public WebElementWrapper(IDomContainer parent, IWebElement element, IElementBehavior behavior)
            : base(parent, element, behavior)
        {
        }

        public void ClickAndAccept()
        {
            _behavior.ClickAndAccept();
        }

        public void ClearAndSend(string elementValue)
        {
            _behavior.ClearAndSend(elementValue);
        }

        public void MouseOver()
        {
            _behavior.MouseOver();
        }

        public void Focus()
        {
            _behavior.Actions().MoveToElement(_element).Build().Perform();
        }

        public void SetVisibilityTo(bool visible = true)
        {
            _behavior.SetVisibilityTo(visible);
        }
    }
}
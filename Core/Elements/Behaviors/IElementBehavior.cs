using OpenQA.Selenium.Interactions;

namespace TeamSL.Selenium.Incontextable.Elements.Behaviors
{
    public interface IElementBehavior
    {
        void Click();
        void Scroll();
        void SendKeys(string keys);
        void Clear();
        void ClickAndAccept();
        void ClearAndSend(string text);
        Actions Actions();
        void MouseOver();
        void SetVisibilityTo(bool visible);
    }
}
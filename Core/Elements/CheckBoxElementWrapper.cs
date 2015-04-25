using OpenQA.Selenium;
using TeamSL.Selenium.Incontextable.Elements.Behaviors;

namespace TeamSL.Selenium.Incontextable.Elements
{
    public interface ICheckBoxElementWrapper : IWebElementWrapper
    {
        void SetChecked(bool isChecked);
        void Check();
        void UnCheck();
    }

    public class CheckBoxElementWrapper : WebElementWrapper, ICheckBoxElementWrapper
    {
        public CheckBoxElementWrapper(IDomContainer parent, IWebElement element, IElementBehavior behavior)
            : base(parent, element, behavior)
        {
        }

        public void SetChecked(bool isChecked)
        {
            if (isChecked)
            {
                Check();
            }
            else
            {
                UnCheck();
            }
        }

        public void Check()
        {
            if (!_element.Displayed)
                Scroll();

            if (!_element.Selected)
                _element.Click();
        }

        public void UnCheck()
        {
            if (!_element.Displayed)
                Scroll();

            if (_element.Selected)
                _element.Click();
        }
    }
}
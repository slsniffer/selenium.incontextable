using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TeamSL.Selenium.Incontextable.Elements.Behaviors;

namespace TeamSL.Selenium.Incontextable.Elements
{
    public interface ISelectElementWrapper : IWebElementWrapper
    {
        void SelectByText(string text);
        void SelectByPartialText(string text);
        void SelectByIndex(int index);
        void SelectByValue(string value);
        string SelectedItemText { get; }
    }

    public class SelectElementWrapper : WebElementWrapper, ISelectElementWrapper
    {
        public string SelectedItemText
        {
            get
            {
                var selectedOption = _selectElement.SelectedOption;
                return (selectedOption == null) ? null : selectedOption.Text;
            }
        }

        private readonly SelectElement _selectElement;

        public SelectElementWrapper(IDomContainer parent, IWebElement element, IElementBehavior behavior)
            : base(parent, element, behavior)
        {
            _selectElement = new SelectElement(element);
        }

        public void SelectByText(string text)
        {
            _selectElement.SelectByText(text);
        }

        public void SelectByPartialText(string text)
        {
            var options = _selectElement.Options;
            foreach (var option in options)
            {
                if (option.Text.Contains(text))
                {
                    _selectElement.SelectByText(option.Text);
                    break;
                }
            }
        }

        public void SelectByIndex(int index)
        {
            _selectElement.SelectByIndex(index);
        }

        public void SelectByValue(string value)
        {
            _selectElement.SelectByValue(value);
        }
    }
}
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using TeamSL.Selenium.Incontextable.Elements.Behaviors;
using TeamSL.Selenium.Incontextable.Utils;

namespace TeamSL.Selenium.Incontextable.Elements
{
    public interface IContextableWebElement : IWebElement
    {
        IDomContainer Parent { get; }
        string CssClass { get; }
        void SetFinder(By finder);
        void Scroll();

        [SkipContext]
        void ScrollWithoutContext();
    }

    public class ContextableWebElement : IContextableWebElement
    {
        private readonly IDomContainer _parent;
        protected IWebElement _element;
        protected IElementBehavior _behavior;
        private By _finder;
        public string TagName { get { return _element.TagName; } }
        public string Text { get { return _element.Text; } }
        public Point Location { get { return _element.Location; } }
        public Size Size { get { return _element.Size; } }
        public bool Displayed
        {
            get
            {
                try
                {
                    return _element.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            }
        }
        public bool Selected { get { return _element.Selected; } }
        public bool Enabled { get { return _element.Enabled; } }
        public string CssClass { get { return _element.GetAttribute("class"); } }

        public ContextableWebElement(IDomContainer parent, IWebElement element, IElementBehavior behavior)
        {
            _parent = parent;
            _element = element;
            _behavior = behavior;
        }

        [SkipContext]
        protected IWebElement Element
        {
            get
            {
                if (_finder != null)
                {
                    return _parent.FindChild(_finder);
                }
                return _element;
            }
        }

        IDomContainer IContextableWebElement.Parent { get { return _parent; } }

        public IWebElement FindElement(By @by)
        {
            return _element.FindElement(@by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return _element.FindElements(@by);
        }

        public void Clear()
        {
            _behavior.Clear();
        }

        public void SendKeys(string text)
        {
            _behavior.SendKeys(text);
        }

        public void Submit()
        {
            _element.Submit();
        }

        public void Scroll()
        {
            _behavior.Scroll();
        }

        [SkipContext]
        public void ScrollWithoutContext()
        {
            _behavior.Scroll();
        }

        public void Click()
        {
            _behavior.Click();
        }

        [SkipContext]
        public void SetFinder(By finder)
        {
            _finder = finder;
        }

        public string GetAttribute(string attributeName)
        {
            return Element.GetAttribute(attributeName);
        }

        public string GetCssValue(string propertyName)
        {
            return _element.GetCssValue(propertyName);
        }
    }
}
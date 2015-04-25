using System;
using System.Threading;
using Autofac;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TeamSL.Selenium.Incontextable.Context;
using TeamSL.Selenium.Incontextable.Extensions;

namespace TeamSL.Selenium.Incontextable.Elements
{
    public interface IDomContainer
    {
        void Reload();
        IWebElement FindChild(By by);
        void ScrollTo(string css);
        void SetFinder(By finder);
        void SetDefaultContext(IWebDriver driver, IWebElement element);
    }

    public class DomContainer : IDomContainer, IContextable
    {
        private readonly IComponentContext _context;

        public DomContainer(IComponentContext context)
        {
            _context = context;
            _factory = _context.Resolve<ItemFactory>();
        }

        public IComponentContext ComponentContext { get; set; }

        private By _finder;
        protected readonly ItemFactory _factory;
        IDomContainer IContextable.Parent { get; set; }
        IContext IContextable.Context { get; set; }
        protected IWebElement Element { get; set; }
        protected IWebDriver Driver { get { return ((IContextable)this).Context.Driver; } }

        public void Reload()
        {
            IContextable contextable = this;
            var context = contextable.FindContext();
            if (context != null)
            {
                context.SwitchContext();
                ReloadInternal();
                context.SwitchToDefault();
            }
            else
            {
                ReloadInternal();
            }
        }

        private void ReloadInternal()
        {
            if (_finder != null)
            {
                Element = Driver.FindElement(_finder);
            }
            Init();
        }

        public void SetFinder(By finder)
        {
            _finder = finder;
        }

        protected virtual void Init()
        {
        }

        IContext IContextable.FindContext()
        {
            var contextable = (IContextable)this;
            while (true)
            {
                if (contextable.Context is FrameContext || contextable.Context is WindowContext)
                {
                    return contextable.Context;
                }

                if (contextable.Parent != null)
                {
                    contextable = (IContextable)contextable.Parent;
                }
                else if (contextable.Context is BaseContext)
                {
                    return null;
                }
                else
                {
                    return contextable.Context;
                }
            }
        }

        public void SetDefaultContext(IWebDriver driver, IWebElement element)
        {
            var contextable = (IContextable)this;
            contextable.Context = new BaseContext(driver);
            contextable.SetElement(element);
        }

        void IContextable.SetElement(IWebElement element)
        {
            Element = element;
            Init();
        }

        /// <summary>
        /// Find child and scroll to it
        /// </summary>
        public void ScrollTo(string css)
        {
            var element = FindChild(By.CssSelector(css));
            Driver.ScrollToView(element);
        }

        public IWebElement FindChild(By by)
        {
            return Element.FindElement(@by);
        }

        protected bool Exists(By by)
        {
            IWebElement foundElement;
            return ExistsInternal(@by, out foundElement);
        }

        protected bool Exists(By by, out IWebElement foundElement, bool waitElement = false, int milliseconds = 5000, bool global = false)
        {
            foundElement = null;
            if (!WaitElement(@by, waitElement, milliseconds))
            {
                return false;
            }

            return ExistsInternal(@by, out foundElement, global);
        }

        private bool WaitElement(By @by, bool elementToWait, int milliseconds)
        {
            try
            {
                if (elementToWait)
                {
                    var wait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(milliseconds));
                    wait.Until(p => p.FindElement(@by));
                }
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            return true;
        }

        protected bool ExistsOnPage(By by)
        {
            IWebElement foundElement;
            return ExistsOnPage(@by, out foundElement);
        }

        protected bool ExistsOnPage(By by, out IWebElement foundElement)
        {
            return ExistsInternal(@by, out foundElement, true);
        }

        private bool ExistsInternal(By by, out IWebElement foundElement, bool globalFind = false)
        {
            try
            {
                foundElement = globalFind ? Driver.FindElement(@by) : Element.FindElement(@by);
            }
            catch (NoSuchElementException)
            {
                foundElement = null;
                return false;
            }
            catch (StaleElementReferenceException) // workarround for Safari
            {
                foundElement = null;
                return false;
            }

            return true;
        }

        protected TResult CreateFoundItem<TResult>(By finder, bool waitElement = false, int milliseconds = 5000) where TResult : IDomContainer
        {
            IWebElement itemElement;
            TResult result = default(TResult);
            if (Exists(finder, out itemElement, waitElement, milliseconds))
            {
                result = _factory.CreateItem<TResult>(this, itemElement);
                result.SetFinder(finder);
            }

            return result;
        }

        protected TResult CreateFoundItem<TResult>(By finder, bool global, bool waitElement = false, int milliseconds = 5000 ) where TResult : IDomContainer
        {
            IWebElement itemElement;
            TResult result = default(TResult);
            if (Exists(finder, out itemElement, waitElement, milliseconds, global))
            {
                result = _factory.CreateItem<TResult>(this, itemElement);
                result.SetFinder(finder);
            }

            return result;
        }

        protected TResult CreateFoundPage<TResult>() where TResult : IDomContainer
        {
            var finder = By.TagName("body");
            IWebElement itemElement;
            if (ExistsOnPage(finder, out itemElement))
            {
                var result = _factory.CreatePage<TResult>(Driver);
                result.SetFinder(finder);
                return result;
            }

            return default(TResult);
        }

        protected TWebElement CreateWebElement<TWebElement>(IWebElement element)
            where TWebElement : IWebElementWrapper
        {
            return _factory.CreateWebElement<TWebElement>(this, element);
        }

        protected IWebElementWrapper CreateWebElement(IWebElement element)
        {
            return _factory.CreateWebElement(this, element);
        }

        protected ICheckBoxElementWrapper CreateCheckboxElement(IWebElement element)
        {
            return _factory.CreateCheckboxElement(this, element);
        }

        protected ISelectElementWrapper CreateSelectElement(IWebElement element)
        {
            return _factory.CreateSelectElement(this, element);
        }

        protected void Select(IWebElement element, string option)
        {
            var select = CreateSelectElement(element);
            @select.SelectByText(option);
        }

        protected TWebElement CreateFoundElement<TWebElement>(By finder)
            where TWebElement : IWebElementWrapper
        {
            IWebElement itemElement;
            TWebElement elementWrapper = default(TWebElement);
            if (Exists(finder, out itemElement))
            {
                elementWrapper = CreateWebElement<TWebElement>(itemElement);
                elementWrapper.SetFinder(finder);
            }

            return elementWrapper;
        }

        protected IWebElementWrapper CreateFoundWebElement(By finder)
        {
            return CreateFoundElement<IWebElementWrapper>(finder);
        }

        protected ISelectElementWrapper CreateFoundSelectElement(By finder)
        {
            return CreateFoundElement<ISelectElementWrapper>(finder);
        }

        protected ICheckBoxElementWrapper CreateFoundCheckboxElement(By finder)
        {
            return CreateFoundElement<ICheckBoxElementWrapper>(finder);
        }

        protected IWebElementWrapper CreateFoundSelectOrWebElement(By finder)
        {
            IWebElement itemElement;
            IWebElementWrapper elementWrapper = null;
            if (Exists(finder, out itemElement))
            {
                elementWrapper = itemElement.TagName == "select"
                                     ? CreateSelectElement(itemElement)
                                     : CreateWebElement(itemElement);
                elementWrapper.SetFinder(finder);
            }

            return elementWrapper;
        }

        protected WebDriverWait Wait(int seconds, int poolingInterval = 500, Type[] ignoreExceptionTypes = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds)) { PollingInterval = TimeSpan.FromMilliseconds(poolingInterval) };
            if (ignoreExceptionTypes != null)
            {
                wait.IgnoreExceptionTypes(ignoreExceptionTypes);
            }
            return wait;
        }

        protected TResult WindowAppearButtonClick<TResult>(IWebElementWrapper button) where TResult : IDomContainer
        {
            int countBefore = Driver.WindowHandles.Count;
            button.MouseOver();
            button.Click();

            Driver.WaitUntil(p => p.WindowHandles.Count > countBefore, 20, 1000, "3-d Party window didn't open after click.");
            Thread.Sleep(2000);

            return _factory.CreateWindowItem<TResult>(this);
        }

        protected TResult WindowAppearButtonClickWithAction<TResult>(IWebElementWrapper button, Action action) where TResult : IDomContainer
        {
            int countBefore = Driver.WindowHandles.Count;
            action.Invoke();
            Driver.WaitUntil(p => p.WindowHandles.Count > countBefore, 20, 1000, "3-d Party window didn't open after click.");
            Thread.Sleep(2000);

            return _factory.CreateWindowItem<TResult>(this);
        }

        protected void Confirm()
        {
            Driver.SwitchTo().Alert().Accept();
        }

        protected void MouseOver(IWebElement element)
        {
            new Actions(Driver).MoveToElement(element).Build().Perform();
        }
    }
}
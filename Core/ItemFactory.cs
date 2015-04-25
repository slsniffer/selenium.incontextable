using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TeamSL.Selenium.Incontextable.Context;
using TeamSL.Selenium.Incontextable.Elements;
using TeamSL.Selenium.Incontextable.Elements.Behaviors;
using TeamSL.Selenium.Incontextable.Extensions;

namespace TeamSL.Selenium.Incontextable
{
    public class ItemFactory
    {
        private readonly IComponentContext _context;

        public ItemFactory(IComponentContext context)
        {
            _context = context;
        }

        public TInterface CreatePage<TInterface>(IWebDriver driver, IWebElement pageMarkup = null)
            where TInterface : IDomContainer
        {
            var obj = _context.Resolve<TInterface>();
            var bodyFinder = By.CssSelector("html, body");
            var element = driver.WaitUntil(p => p.FindElement(bodyFinder), 10, 1000, "Window not loaded");
            var contextable = obj as IContextable;
            if (contextable != null)
            {
                contextable.Context = new BaseContext(driver);
                contextable.SetElement(pageMarkup ?? element);
            }
            obj.SetFinder(bodyFinder);

            return obj;
        }

        public ISelectElementWrapper CreateSelectElement(IDomContainer parent, IWebElement element)
        {
            return CreateWebElement<ISelectElementWrapper>(parent, element);
        }

        public TInterface CreateItem<TInterface>(IDomContainer parent, IWebElement element, Action<TInterface> preInitialization = null, Action<TInterface> postInitialization = null)
            where TInterface : IDomContainer
        {
            TInterface obj = _context.Resolve<TInterface>();

            IContextable contextable = obj as IContextable;
            if (contextable != null)
            {
                contextable.Parent = parent;
                contextable.Context = new BaseContext(((IContextable)parent).Context.Driver);

                if (preInitialization != null)
                {
                    preInitialization(obj);
                }

                contextable.SetElement(element);

                if (postInitialization != null)
                {
                    postInitialization(obj);
                }
            }

            return obj;
        }

        public TInterface CreateItem<TInterface>(IWebDriver driver, IWebElement element, Action<TInterface> additionalInitialization = null)
            where TInterface : IDomContainer
        {
            TInterface obj = _context.Resolve<TInterface>();

            IContextable contextable = obj as IContextable;
            if (contextable != null)
            {
                contextable.Context = new BaseContext(driver);
                contextable.SetElement(element);
                if (additionalInitialization != null)
                {
                    additionalInitialization(obj);
                }
            }

            return obj;
        }

        public IWebElementWrapper CreateWebElement(IDomContainer parent, IWebElement element)
        {
            return CreateWebElement<IWebElementWrapper>(parent, element);
        }

        public ICheckBoxElementWrapper CreateCheckboxElement(IDomContainer parent, IWebElement element)
        {
            return CreateWebElement<ICheckBoxElementWrapper>(parent, element);
        }

        public TInterface CreateIframeItem<TInterface>(IDomContainer parent, IWebElement frameElement)
            where TInterface : IDomContainer
        {
            IContextable parentContextable = (IContextable)parent;
            IWebDriver iframeDriver = parentContextable.Context.Driver.SwitchTo().Frame(frameElement);

            var wait = new WebDriverWait(parentContextable.Context.Driver, TimeSpan.FromSeconds(10));
            var finder = By.TagName("body");
            IWebElement element = wait.Until(d => iframeDriver.FindElement(finder));

            TInterface obj = _context.Resolve<TInterface>();

            obj.SetFinder(finder);
            IContextable contextable = obj as IContextable;
            if (contextable != null)
            {
                contextable.Parent = parent;
                contextable.Context = new FrameContext(parentContextable.Context.Driver, frameElement);
                contextable.SetElement(element);
            }

            parentContextable.Context.Driver.SwitchTo().DefaultContent();

            return obj;
        }

        public TInterface CreateWindowItem<TInterface>(IDomContainer parent)
            where TInterface : IDomContainer
        {
            var parentContextable = (IContextable)parent;
            IWebDriver driver = parentContextable.Context.Driver;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var currentHandle = driver.CurrentWindowHandle;
            wait.Until(p => p.WindowHandles.Last() != string.Empty);
            var windowHandle = driver.WindowHandles.Last();

            IWebDriver windowDriver = driver.SwitchTo().Window(windowHandle);

            wait.Until(d => !windowDriver.Url.Contains("about:blank"));
            By finder = By.TagName("body");
            IWebElement element = wait.Until(d => windowDriver.FindElement(finder));

            TInterface obj = _context.Resolve<TInterface>();
            obj.SetFinder(finder);

            IContextable contextable = obj as IContextable;
            if (contextable != null)
            {
                contextable.Parent = parent;
                contextable.Context = new WindowContext(windowDriver, windowHandle, currentHandle);
                contextable.SetElement(element);
            }

            driver.SwitchTo().Window(currentHandle);

            return obj;
        }

        internal T CreateWebElement<T>(IDomContainer parent, IWebElement element)
            where T : IContextableWebElement
        {
            var driver = ((IContextable)parent).Context.Driver;

            var behavior = _context.Resolve<IElementBehavior>(new Parameter[]
                {
                    new NamedParameter("element", element),
                    new NamedParameter("driver", driver)
                });

            var webElement = _context.Resolve<T>(new Parameter[]
                {
                    new NamedParameter("parent", parent),
                    new NamedParameter("element", element),
                    new NamedParameter("behavior", behavior)
                });

            return webElement;
        }
    }
}
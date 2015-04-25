using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Context
{
    internal interface IContext
    {
        IWebDriver Driver { get; }
        void SwitchContext();
        void SwitchToDefault();
    }

    internal class BaseContext : IContext
    {
        public BaseContext(IWebDriver driver)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; private set; }

        public virtual void SwitchToDefault()
        {
            Driver.SwitchTo().DefaultContent();
        }

        public virtual void SwitchContext()
        {
        }
    }
}
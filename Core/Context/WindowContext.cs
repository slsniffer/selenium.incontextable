using System;
using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Context
{
    internal class WindowContext : BaseContext
    {
        private readonly string _currentHandle;
        private readonly string _parentHandle;

        public WindowContext(IWebDriver driver, string currentHandle, string parentHandle)
            : base(driver)
        {
            _currentHandle = currentHandle;
            _parentHandle = parentHandle;
        }

        public override void SwitchContext()
        {
            Driver.SwitchTo().Window(_currentHandle);
        }

        public override void SwitchToDefault()
        {
            try
            {
                Driver.SwitchTo().Window(_parentHandle);
            }
            catch (Exception e)
            {
                if ((e is InvalidOperationException && e.Message.Contains("has no driver"))
                    || e is WebDriverException && e.Message.Contains("response from server"))
                {
                    Driver.SwitchTo().Window(_parentHandle);
                    return;
                }

                throw;
            }
        }
    }
}
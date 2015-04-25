using OpenQA.Selenium;

namespace TeamSL.Selenium.Incontextable.Context
{
    internal class FrameContext : BaseContext
    {
        private readonly IWebElement _frameElement;

        public FrameContext(IWebDriver driver, IWebElement frameElement)
            : base(driver)
        {
            _frameElement = frameElement;
        }

        public override void SwitchContext()
        {
            Driver.SwitchTo().Frame(_frameElement);
        }
    }
}
using OpenQA.Selenium;
using TeamSL.Selenium.Incontextable.Elements;

namespace TeamSL.Selenium.Incontextable.Context
{
    internal interface IContextable
    {
        IDomContainer Parent { get; set; }

        IContext Context { get; set; }

        void SetElement(IWebElement element);

        IContext FindContext();
    }
}
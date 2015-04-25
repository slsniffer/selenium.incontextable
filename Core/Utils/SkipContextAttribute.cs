using System;

namespace TeamSL.Selenium.Incontextable.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class SkipContextAttribute : Attribute
    {
    }
}
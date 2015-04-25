using System.Linq;
using Castle.DynamicProxy;
using TeamSL.Selenium.Incontextable.Elements;
using TeamSL.Selenium.Incontextable.Utils;

namespace TeamSL.Selenium.Incontextable.Context
{
    public class ContextInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var contextChanged = false;
            IContext context = null;
            var invocationTarget = invocation.InvocationTarget as IContextableWebElement;
            if (invocationTarget != null)
            {
                var skipContextSwitch = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(SkipContextAttribute), true).Any();
                var currentElement = (IContextable)invocationTarget.Parent;
                context = currentElement.FindContext();
                if (!skipContextSwitch && context != null)
                {
                    contextChanged = true;
                    context.SwitchContext();
                }
            }

            invocation.Proceed();

            if (contextChanged)
            {
                context.SwitchToDefault();
            }
        }
    }
}

using Autofac;
using AutofacContrib.DynamicProxy;
using TeamSL.Selenium.Incontextable.Context;
using TeamSL.Selenium.Incontextable.Elements;
using TeamSL.Selenium.Incontextable.Elements.Behaviors;

namespace TeamSL.Selenium.Incontextable
{
    public class RegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ContextInterceptor>().AsSelf();
            builder.RegisterType<WebElementWrapper>().As<IWebElementWrapper>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ContextInterceptor));
            builder.RegisterType<SelectElementWrapper>().As<ISelectElementWrapper>().EnableInterfaceInterceptors().InterceptedBy(typeof(ContextInterceptor));
            builder.RegisterType<CheckBoxElementWrapper>().As<ICheckBoxElementWrapper>().EnableInterfaceInterceptors().InterceptedBy(typeof(ContextInterceptor));

            builder.RegisterType<FirefoxElementBehavior>().As<IElementBehavior>();

            builder.RegisterType<ItemFactory>().As<ItemFactory>();
        }
    }
}

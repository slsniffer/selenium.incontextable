using Autofac;
using OpenQA.Selenium;
using TeamSL.Selenium.Incontextable.Elements;

namespace TeamSL.Selenium.Incontextable.Sample.Wrappers
{
    public interface IRsdnPage : IDomContainer
    {
        IRsdnMenuFrame Menu { get; }
        IRsdnHeaderFrame Header { get; }
        IRsdnContentFrame Content { get; }
    }

    public class RsdnPage : DomContainer, IRsdnPage
    {
        public IRsdnMenuFrame Menu { get; private set; }
        public IRsdnHeaderFrame Header { get; private set; }
        public IRsdnContentFrame Content { get; private set; }

        public RsdnPage(IComponentContext context)
            : base(context)
        {
        }

        // Инициализация объектов на странице
        protected override void Init()
        {
            base.Init();

            Header = _factory.CreateIframeItem<IRsdnHeaderFrame>(this, Driver.FindElement(By.CssSelector("frame[name='frmTop']")));
            Menu = _factory.CreateIframeItem<IRsdnMenuFrame>(this, Driver.FindElement(By.CssSelector("frame[name='frmTree']")));
            Content = _factory.CreateIframeItem<IRsdnContentFrame>(this, Driver.FindElement(By.CssSelector("frame[name='frmMain']")));
        }
    }

    public interface IRsdnContentFrame : IDomContainer
    {
        IWebElementWrapper SearchInput { get; }
        IWebElementWrapper SearchButton { get; }
        void Search(string query);
    }

    public class RsdnContentFrame : DomContainer, IRsdnContentFrame
    {
        public RsdnContentFrame(IComponentContext context)
            : base(context)
        {
        }

        public IWebElementWrapper SearchInput { get; private set; }
        public IWebElementWrapper SearchButton { get; private set; }

        protected override void Init()
        {
            base.Init();

            SearchInput = CreateFoundWebElement(By.CssSelector("input.query-text"));
            SearchButton = CreateFoundWebElement(By.CssSelector("input.submit"));
        }

        public void Search(string query)
        {
            SearchInput.SendKeys(query);
            SearchButton.Click();
            Reload();
        }
    }

    public interface IRsdnHeaderFrame : IDomContainer
    {
        IRsdnHeaderFrameLoggedInState LoggedInState { get; }
        IRsdnHeaderFrameLoggedOutState LoggedOutState { get; }
        bool IsLoggedIn { get; }
        IWebElementWrapper SearchButton { get; }
        void Login(string login, string password);
        void GoToSearch();
    }

    public class RsdnHeaderFrame : DomContainer, IRsdnHeaderFrame
    {
        public RsdnHeaderFrame(IComponentContext context)
            : base(context)
        {
        }

        public IRsdnHeaderFrameLoggedInState LoggedInState { get; private set; }
        public IRsdnHeaderFrameLoggedOutState LoggedOutState { get; private set; }

        bool IRsdnHeaderFrame.IsLoggedIn
        {
            get { return LoggedOutState == null; }
        }

        public IWebElementWrapper SearchButton { get; private set; }

        public void Login(string login, string password)
        {
            LoggedOutState.Login(login, password);
            Reload();
        }

        public void GoToSearch()
        {
            SearchButton.Click();
        }

        protected override void Init()
        {
            base.Init();

            LoggedInState = CreateFoundItem<IRsdnHeaderFrameLoggedInState>(By.CssSelector("div#profile-title"));
            LoggedOutState = CreateFoundItem<IRsdnHeaderFrameLoggedOutState>(By.CssSelector("div#login-box"));

            SearchButton = CreateFoundWebElement(By.CssSelector("a[href='/rsdnsearch']"));
        }
    }

    public interface IRsdnHeaderFrameLoggedOutState : IDomContainer
    {
        IWebElementWrapper UserName { get; }
        IWebElementWrapper Password { get; }
        IWebElementWrapper LoginButton { get; }
        void Login(string login, string password);
    }

    public class RsdnHeaderFrameLoggedOutState : DomContainer, IRsdnHeaderFrameLoggedOutState
    {
        public RsdnHeaderFrameLoggedOutState(IComponentContext context)
            : base(context)
        {
        }

        public IWebElementWrapper UserName { get; private set; }
        public IWebElementWrapper Password { get; private set; }
        public IWebElementWrapper LoginButton { get; private set; }
        protected override void Init()
        {
            base.Init();

            UserName = CreateFoundWebElement(By.CssSelector("input#userName"));
            Password = CreateFoundWebElement(By.CssSelector("input#password"));
            LoginButton = CreateFoundWebElement(By.CssSelector("button"));
        }

        public void Login(string login, string password)
        {
            UserName.SendKeys(login);
            Password.SendKeys(password);
            LoginButton.Click();
        }
    }

    public interface IRsdnHeaderFrameLoggedInState : IDomContainer
    {
    }

    public class RsdnHeaderFrameLoggedInState : DomContainer, IRsdnHeaderFrameLoggedInState
    {
        public RsdnHeaderFrameLoggedInState(IComponentContext context)
            : base(context)
        {
        }
    }

    public interface IRsdnMenuFrame : IDomContainer
    {
        IWebElementWrapper ForumsLink { get; }
        void GoToForums();
    }

    public class RsdnMenuFrame : DomContainer, IRsdnMenuFrame
    {
        public RsdnMenuFrame(IComponentContext context)
            : base(context)
        {
        }

        protected override void Init()
        {
            base.Init();

            ForumsLink = CreateFoundWebElement(By.CssSelector("a[href='/forum/mainlist']"));
        }

        public IWebElementWrapper ForumsLink { get; private set; }

        public void GoToForums()
        {
            ForumsLink.Click();
        }
    }
}

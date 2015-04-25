using Autofac;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using TeamSL.Selenium.Incontextable.Sample.Wrappers;

namespace TeamSL.Selenium.Incontextable.Sample
{
    [TestFixture]
    public class RsdnTest
    {
        private FirefoxDriver _driver;
        private IContainer _container;
        private ItemFactory _factory;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<RegistrationModule>();

            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                   .Where(t => t.Name.ToLower().Contains("rsdn"))
                   .AsImplementedInterfaces();

            _container = builder.Build();

            _factory = _container.Resolve<ItemFactory>();

            _driver = new FirefoxDriver();
        }

        [SetUp]
        public void SetUp()
        {
            _driver.Navigate().GoToUrl("http://rsdn.ru");
        }

        [Test]
        public void FirstTest()
        {
            // Инициализируем элементы страницы
            var page = _factory.CreatePage<IRsdnPage>(_driver);

            // Логинимся на сайт
            page.Header.Login("***", "***");

            // Переходим на страницу поиска
            page.Header.GoToSearch();
            page.Content.Reload();

            // Выполняем поиск по сайт по слову
            page.Content.Search("Selenium");

            // Переходим на страницу форумов
            page.Menu.GoToForums();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _driver.Close();
        }
    }
}

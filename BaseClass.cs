using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using NUnit; 
using System;
using System.IO;

namespace DemoQATests
{
    [Parallelizable]
    [TestFixture]
    public class BaseClass
    {
        public IWebDriver driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

            Console.WriteLine(TestContext.CurrentContext.ToString());
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();

            string host = root.GetSection("hub_host").Value;
            string browser = root.GetSection("browser").Value;
            string baseOs = root.GetSection("os").Value;

            string completeUrl = "http://" + host + ":4444/wd/hub";

            var chromeOptions = new ChromeOptions();
            var firefoxOptions = new FirefoxOptions();
            var edgeOptions = new EdgeOptions();

            if (browser == "chrome" || browser == null)
            {
                if (baseOs == "windows")
                {
                    chromeOptions.AddArgument("disable-extensions");
                    chromeOptions.AddArgument("disable-infobars");
                    chromeOptions.AddArgument("no-sandbox");
                    chromeOptions.AddArgument("incognito");
                    chromeOptions.AddArgument("disable-dev-shm-usage");
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddArgument("start-maximized");
                    chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                }
                else if (baseOs == "linux")
                {
                    string driverPath = "/opt/selenium/";
                    string driverExecutableFileName = "chromedriver";
                    chromeOptions.AddArguments("no-sandbox");
                    chromeOptions.BinaryLocation = "/opt/google/chrome/chrome";
                    //chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);
                }

                //driver = new RemoteWebDriver(new Uri(completeUrl), chromeOptions);
                driver = new ChromeDriver(chromeOptions);

                //IDevTools devTools = driver as IDevTools;
                //DevToolsSession session = new DevToolsSession("https://demoqa.com/browser-windows");
                //session.Domains;
            }
            else if (browser == "firefox")
            {
                if (baseOs == "windows")
                {
                    firefoxOptions.AddArgument("disable-extensions");
                }
                else if (baseOs == "linux")
                {
                    string driverPath = "/opt/selenium/";
                    string driverExecutableFileName = "firefoxdriver";
                    firefoxOptions.AddArguments("disable-extensions");
                    firefoxOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(driverPath, driverExecutableFileName);
                }
                driver = new RemoteWebDriver(new Uri(completeUrl), firefoxOptions);
            }
            else if (browser == "edge")
            {
                if (baseOs == "windows")
                {
                    firefoxOptions.AddArgument("disable-extensions");
                }
                else if (baseOs == "linux")
                {
                    string driverPath = "/opt/selenium/";
                    string driverExecutableFileName = "firefoxdriver";
                    edgeOptions.AddArguments("disable-extensions");
                    edgeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    EdgeDriverService service = EdgeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);
                }
                //driver = new RemoteWebDriver(new Uri(completeUrl), edgeOptions);
                driver = new EdgeDriver(edgeOptions);
            }

            driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
            driver.Close();
        }
    }
}
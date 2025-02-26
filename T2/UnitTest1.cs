using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace T2
{
    public class Tests
    {
        WebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        [Test]
        [Ignore("Ignore")]
        public void Test1()
        {
            const string complete = "100%";
            const string start = "0%";

            //1 Go to specified URL
            driver.Navigate().GoToUrl("https://demoqa.com/");

            //3. Select the 'Widgets' tab
            driver.FindElement(By.XPath("//div[@class = 'card-body']/h5[text() = 'Widgets']")).Click();

            //4. Select the 'Progress Bar' tab
            driver.FindElement(By.XPath("//li[@class = 'btn btn-light ']/span[@class = 'text' and text() = 'Progress Bar']")).Click();

            //5. Press the 'Start button'
            driver.FindElement(By.Id("startStopButton")).Click();

            //6. Wait until progress bar reaches 100% and click reset
            wait.Until(driver => driver.FindElement(By.XPath("//div[@id = 'progressBar']/div")).Text == complete);
            driver.FindElement(By.Id("resetButton")).Click();

            //7. Verify that the progress bar is empty (0%)
            Assert.IsTrue(driver.FindElement(By.XPath("//div[@id = 'progressBar']/div")).Text == start);
        }

        
        [Test]
        [Ignore("Ignore")]
        public void Test2()
        {
            const int repetitions = 8;

            //1 Go to specified URL
            driver.Navigate().GoToUrl("https://demoqa.com/");

            //3. Select the 'Elements' tab
            driver.FindElement(By.XPath("//div[@class = 'card-body']/h5[text() = 'Elements']")).Click();

            //4. Select the 'Web Tables' tab
            driver.FindElement(By.XPath("//li[@class = 'btn btn-light ']/span[@class = 'text' and text() = 'Web Tables']")).Click();

            //5. Populate table until there are enough records for pagination
            for (int i = 0; i < repetitions; ++i)
            {
                driver.FindElement(By.Id("addNewRecordButton")).Click();

                driver.FindElement(By.Id("firstName")).SendKeys("Name");
                driver.FindElement(By.Id("lastName")).SendKeys("Surname");
                driver.FindElement(By.Id("userEmail")).SendKeys("name.surname@gmail.com");
                driver.FindElement(By.Id("age")).SendKeys("20");
                driver.FindElement(By.Id("salary")).SendKeys("10000");
                driver.FindElement(By.Id("department")).SendKeys("Department");

                driver.FindElement(By.XPath("//button[@id = 'submit' and text() = 'Submit']")).Click();
            }

            //6. Select the next page by clicking 'Next'
            driver.FindElement(By.XPath("//button[@type = 'button' and text() = 'Next']")).Click();

            //We assert that the page counter actually works and we can come back to the first page
            Assert.IsTrue(driver.FindElement(By.XPath("//input[@aria-label='jump to page']")).GetAttribute("value") == "2");
            Assert.IsTrue(driver.FindElement(By.XPath("//span[@class = '-totalPages']")).Text == "2");

            //7. Delete the 11-th record to bring us back one page
            driver.FindElement(By.Id("delete-record-11")).Click();

            //8. Ensure that the pagination automatically moves to the first page and that the number of pages has reduced to one
            Assert.IsTrue(driver.FindElement(By.XPath("//span[@class = '-totalPages']")).Text == "1");

            var pageInput = driver.FindElement(By.XPath("//input[@aria-label='jump to page']"));
            //pageInput.Click();
            //pageInput.SendKeys(Keys.Tab); //It refreshes but at what cost!!!!
            Assert.IsTrue(pageInput.GetAttribute("value") == "1");
        }
    }
}
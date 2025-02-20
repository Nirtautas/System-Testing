using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static System.Net.Mime.MediaTypeNames;

namespace T3
{
    public class Tests
    {
        IWebDriver driver;
        WebDriverWait wait;
        CredentialManager credentialManager;

        //Register the user before any tests
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            credentialManager = new CredentialManager(driver, wait);
            credentialManager.Register();
            driver.Quit();
        }

        //New driver instance for each test
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        [TestCase("data1.txt")]
        [TestCase("data2.txt")]
        public void Test1(string dataPath)
        {
            //1. Open the website
            driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

            //2. Click 'Log in'
            driver.FindElement(By.ClassName("ico-login")).Click();

            //3. Fill in email and password, then press 'Log in'
            driver.FindElement(By.Id("Email")).SendKeys(credentialManager.email);
            driver.FindElement(By.Id("Password")).SendKeys(credentialManager.password);
            driver.FindElement(By.CssSelector(".button-1.login-button")).Click();

            //4. Select 'Digital downloads' in the left sidebar
            driver.FindElement(By.CssSelector("ul.list a[href='/digital-downloads']")).Click();

            //5. Add products to the cart by reading from a text file
            IEnumerable<string> cartProducts = ReadTestData(dataPath);
            
            foreach (string cartProduct in cartProducts)
            {
                string initialCartQty = driver.FindElement(By.ClassName("cart-qty")).Text;
                driver.FindElement(By.XPath($"//div[@class='item-box' and //a/text()='{cartProduct}']/descendant::input[@class='button-2 product-box-add-to-cart-button']")).Click();
                wait.Until(driver =>
                {
                    string updatedCartQty = driver.FindElement(By.ClassName("cart-qty")).Text;
                    return updatedCartQty != initialCartQty;
                });
            }

            //6. Open shopping cart
            driver.FindElement(By.ClassName("cart-label")).Click();

            //7. Check 'I agree' and click 'checkout'
            driver.FindElement(By.Id("termsofservice")).Click();
            driver.FindElement(By.Id("checkout")).Click();

            var paymentNextButton = WaitUntilVisibleCssSelector(driver, ".button-1.new-address-next-step-button");
            var savedAddressBox = driver.FindElements(By.Id("billing-address-select"));
            if (savedAddressBox.Count == 0)
            {
                //8. In 'Billing address' fill in a new address, then click continue
                var dropdown = driver.FindElement(By.Id("BillingNewAddress_CountryId"));
                var selectDropdownElement = new SelectElement(dropdown);
                selectDropdownElement.SelectByValue("156");

                driver.FindElement(By.Id("BillingNewAddress_City")).SendKeys("Vilnius");
                driver.FindElement(By.Id("BillingNewAddress_Address1")).SendKeys("Didlaukio g. 47");
                driver.FindElement(By.Id("BillingNewAddress_ZipPostalCode")).SendKeys("24592");
                driver.FindElement(By.Id("BillingNewAddress_PhoneNumber")).SendKeys("865197462");
            }
            paymentNextButton.Click();

            //9., 10., 11. press confirm buttons
            WaitUntilVisibleCssSelector(driver, ".button-1.payment-method-next-step-button").Click();
            WaitUntilVisibleCssSelector(driver, ".button-1.payment-info-next-step-button").Click();
            WaitUntilVisibleCssSelector(driver, ".button-1.confirm-order-next-step-button").Click();

            //12. Ensure that the order is successfully placed
            var orderConfirmation = WaitUntilVisibleXPathSelector(driver, "//div[@class='title']/strong");
            Assert.IsTrue(orderConfirmation.Text == "Your order has been successfully processed!");
        }

        //Function to read test data from a text file
        public IEnumerable<string> ReadTestData(string dataPath)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataPath);
            IEnumerable<string> data = File.ReadAllLines(fullPath);
            return data;
        }

        //Function to wait until a certain element is displayed as denoted by the css selector
        public IWebElement WaitUntilVisibleCssSelector(IWebDriver driver, string cssSelector)
        {
            var visibleElement = wait.Until(driver =>
            {
                var element = driver.FindElement(By.CssSelector(cssSelector));
                return element.Displayed ? element : null;
            });
            return visibleElement;
        }

        //Function to wait until a certain element is displayed as denoted by the xpath selector
        public IWebElement WaitUntilVisibleXPathSelector(IWebDriver driver, string xpathSelector)
        {
            var visibleElement = wait.Until(driver =>
            {
                var element = driver.FindElement(By.XPath(xpathSelector));
                return element.Displayed ? element : null;
            });
            return visibleElement;
        }
    }
}
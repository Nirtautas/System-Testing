using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace T1
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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        [Test]
        public void Test1()
        {
            const string expectedSubtotal = "1002600.00";

            //1. Navigate to website
            driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

            //2. Go to giftcards category in the left menu
            driver.FindElement(By.CssSelector("ul.list a[href='/gift-cards']")).Click();

            //3. Select good which costs more than 99
            driver.FindElement(By.XPath("//h2[@class='product-title' and ../descendant::div[@class='prices']/span/text() > 99]/a")).Click();

            //4. Enter Names
            driver.FindElement(By.CssSelector(".recipient-name")).SendKeys("Ponas Automatas");
            driver.FindElement(By.CssSelector(".sender-name")).SendKeys("Ponas Neautomatas");

            //Selenium does not support compound class names
            //5. Set qty to 5000
            driver.FindElement(By.Name("addtocart_4.EnteredQuantity")).Clear();
            driver.FindElement(By.Name("addtocart_4.EnteredQuantity")).SendKeys("5000");

            //6. Add to cart, wait for update
            var initialText = driver.FindElement(By.ClassName("cart-qty")).Text;
            driver.FindElement(By.Id("add-to-cart-button-4")).Click();
            wait.Until(driver => driver.FindElement(By.ClassName("cart-qty")).Text != initialText);

            //7. Add to wishlist, wait for update
            initialText = driver.FindElement(By.ClassName("wishlist-qty")).Text;
            driver.FindElement(By.Id("add-to-wishlist-button-4")).Click();
            wait.Until(driver => driver.FindElement(By.ClassName("wishlist-qty")).Text != initialText);

            //8. Go to jewelry tab in left menu
            driver.FindElement(By.CssSelector("ul.list a[href='/jewelry']")).Click();

            //9. Go to Create your own jewelry
            driver.FindElement(By.CssSelector("a[href='/create-it-yourself-jewelry']")).Click();

            //10. Select values: Material - Silver 1mm, length in cm 80, pendant - star
            var dropdown = driver.FindElement(By.Id("product_attribute_71_9_15"));
            var selectDropdownElement = new SelectElement(dropdown);
            selectDropdownElement.SelectByValue("47");

            driver.FindElement(By.Id("product_attribute_71_10_16")).SendKeys("80");
            driver.FindElement(By.Id("product_attribute_71_11_17_50")).Click();

            //11. Set qty to 26
            driver.FindElement(By.Id("addtocart_71_EnteredQuantity")).Clear();
            driver.FindElement(By.Id("addtocart_71_EnteredQuantity")).SendKeys("26");

            //12. Add to cart, wait for update
            initialText = driver.FindElement(By.ClassName("cart-qty")).Text;
            driver.FindElement(By.Id("add-to-cart-button-71")).Click();
            wait.Until(driver => driver.FindElement(By.ClassName("cart-qty")).Text != initialText);

            //13. Add to wishlist, wait for update
            driver.FindElement(By.Id("add-to-wishlist-button-71")).Click();
            initialText = driver.FindElement(By.ClassName("wishlist-qty")).Text;
            wait.Until(driver => driver.FindElement(By.ClassName("wishlist-qty")).Text != initialText);

            //14. Go to wishlist
            driver.FindElement(By.CssSelector("a[href='/wishlist'] span.cart-label")).Click();

            //15. Press checkmark add to cart for both goods
            var addToCartCheckbox = driver.FindElements(By.CssSelector("input[type='checkbox'][name='addtocart']"));
            foreach (var checkbox in addToCartCheckbox)
                checkbox.Click();

            //16. Press add to cart, wait for update
            initialText = driver.FindElement(By.ClassName("cart-qty")).Text;
            driver.FindElement(By.Name("addtocartbutton")).Click();
            wait.Until(driver => driver.FindElement(By.ClassName("cart-qty")).Text != initialText);

            //17. Check if sub-total is correct
            Assert.IsTrue(driver.FindElement(By.ClassName("product-price")).Text == expectedSubtotal);
        }
    }
}
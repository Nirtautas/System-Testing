using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace T4
{
    public class Tests
    {
        private ChromeDriver driver;
        private WebDriverWait wait;

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

        /*
        -------------------+--------------------------------------------------------------------------
        ID                 | 1
        -------------------+--------------------------------------------------------------------------
        Name               | AddFiveProductsToCompareList_AfterFifthIsAdded_FirstIsRemoved
        -------------------+--------------------------------------------------------------------------
        Designed by / date | Nirtautas Šadauskas, 2025-03-06
        -------------------+--------------------------------------------------------------------------
        Description        | Tests whether the first product is correctly removed from the product
                           | compare list after adding the fifth product to the list.
        -------------------+--------------------------------------------------------------------------
        Pre-conditions     | ChromeDriver object is instantiated, driver (web) window is maximized,
                           | product_list1.txt, which contains test data, exists in the project build
                           | directory.
        -------------------+--------------------------------------------------------------------------
        Test data          | product_list1.txt - Text file containing 5 product names contained in the
                           | first page of 'Apparel & Shoes' category.
                           |
                           | Contents:
                           |    Blue and green Sneaker
                           |    Blue Jeans
                           |    Casual Golf Belt
                           |    Custom T-shirt
                           |    Green and blue Sneaker
        -------------------+--------------------------------------------------------------------------
        Test Steps         | 1. Open the website https://demowebshop.tricentis.com/.
                           | 2. For each product name in file product_list1.txt (5 products total).
                           |     2.1. In the sidebar manu, select 'Apparel & Shoes'.
                           |     2.2. Click on the product with the given name.
                           |     2.3. In the product page, click 'Add to compare list' button.
                           |          (This will redirect to 'Compare products')
                           |     2.4. In the 'Compare products' page, ASSERT that the added product
                           |          name is in the list.
                           | 3. ASSERT that the name of the first product ('Blue and green Sneaker')
                           |    is no longer present in the comparison list.
        -------------------+--------------------------------------------------------------------------
        Expected results   | 1. Assert.True(nameToCheck.Count() == 0, ...) in foreach loop passes
                           | ensuring that each of the 5 products with names from product_list1.txt
                           | are added to the comparison list correctly.
                           | 
                           | 2. Assert.True(nameToCheck.Count() == 0, ...) after foreach loop passes
                           | ensuring that the first product with name from product_list1.txt is
                           | removed from the comparison list after the 5th product has beed added.
        -------------------+--------------------------------------------------------------------------
        Post-conditions    | The ChromeDriver object instance is correctly disposed of by calling the
                           | Quit() method.
        -------------------+--------------------------------------------------------------------------
         */

        [Test]
        [TestCase("product_list1.txt")]
        public void AddFiveProductsToCompareList_AfterFifthIsAdded_FirstIsRemoved(string dataPath)
        {
            IReadOnlyCollection<IWebElement> nameToCheck;

            //1. Open the website https://demowebshop.tricentis.com/.
            driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

            IEnumerable<string> productNames = ReadTestData(dataPath);
            var firstProductName = productNames.First();

            //2. For each product name in file product_list1.txt (5 products total).
            foreach (string productName in productNames)
            {
                //2.1. In the sidebar manu, select 'Apparel & Shoes'.
                driver.FindElement(By.CssSelector("ul.list a[href='/apparel-shoes']")).Click();

                //2.2.Click on the product with the given name.
                driver.FindElement(By.XPath($"//h2[@class = 'product-title']/a[text() = '{productName}']")).Click();

                //2.3. In the product page, click 'Add to compare list' button. (This will redirect to 'Compare products')
                driver.FindElement(By.ClassName("compare-products")).Click();

                //2.4. In the 'Compare products' page, ASSERT that the added product name is in the list.
                nameToCheck = driver.FindElements(By.XPath($"//td/a[text() = '{productName}']"));
                Assert.True(nameToCheck.Count() > 0, $"Item '{productName}' was not added to the compare list!");
            }

            // 3. ASSERT that the name of the first product ('Blue and green Sneaker') is no longer present in the comparison list.
            nameToCheck = driver.FindElements(By.XPath($"//td/a[text() = '{firstProductName}']"));
            Assert.True(!nameToCheck.Any(), $"Item '{firstProductName}' was not removed from the compare list!");
        }
        
        public IEnumerable<string> ReadTestData(string dataPath)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataPath);
            IEnumerable<string> data = File.ReadAllLines(fullPath);
            return data;
        }
    }
}
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Text;

namespace T3
{
    public class CredentialManager
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private Random random;

        public string email = "Placeholder123.";
        public string password = "placeholder@gmail.com";

        private const int emailRndSubstringLengthDefault = 8;

        private const int passwordLengthDefault = 6;

        public CredentialManager(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
            this.random = new Random();
            GenerateEmail();
            GeneratePassword();
        }

        //Register the user
        public void Register() {
            if (driver != null) {
                //1. Go to website
                driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");

                //2. Click 'Log in'
                driver.FindElement(By.ClassName("ico-login")).Click();

                //3. Click 'Register' in the 'New customer' section
                driver.FindElement(By.CssSelector(".button-1.register-button")).Click();

                //4. Fill in registration form fields
                driver.FindElement(By.Id("gender-male")).Click();
                driver.FindElement(By.Id("FirstName")).SendKeys("Name");
                driver.FindElement(By.Id("LastName")).SendKeys("Surname");
                driver.FindElement(By.Id("Email")).SendKeys(email);
                driver.FindElement(By.Id("Password")).SendKeys(password);
                driver.FindElement(By.Id("ConfirmPassword")).SendKeys(password);

                //5. Click 'Register'
                driver.FindElement(By.Id("register-button")).Click();

                //6. Click 'Continue'
                driver.FindElement(By.CssSelector(".button-1.register-continue-button")).Click();
            }
        }

        //Function to generate an email using a substring of a GUID
        public void GenerateEmail(int emailRndSubstringLength = emailRndSubstringLengthDefault) {
            var emailId = Guid.NewGuid().ToString().Substring(0, emailRndSubstringLengthDefault);
            email = $"test{emailId}@mail.com";
        }

        //Generate password using random characters from a list
        public void GeneratePassword(int passwordLength = passwordLengthDefault) {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";
            StringBuilder passwordBuilder = new StringBuilder();

            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(validCharacters.Length);
                passwordBuilder.Append(validCharacters[index]);
            }
            
            password = passwordBuilder.ToString();
        }
    }
}

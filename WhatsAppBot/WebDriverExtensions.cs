using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace WhatsAppBot
{
    public static class WebDriverExtentions
    {

        public static string SecureGetAttribute(this IWebElement element, string attName) 
        {
            try
            {
                var value = element?.GetAttribute(attName);
                return value;
            }
            catch 
            {
                return null;
            }
        }

        public static void ClearTextByKey(this IWebElement element)
        {
            try { element.Click(); } catch { }
            int i = 0;
            var k = 0;
            while (i++ < 70 && !string.IsNullOrEmpty(element.Text))
            {
                while (k++ < 2) { element.SendKeys(Keys.ArrowRight); }
                k = 0;
                element.SendKeys(Keys.Backspace);
            }
        }
        public static List<string> GetElementAttributes(this IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            var attributesAndValues = (Dictionary<string, object>)ex.ExecuteScript("var items = { }; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;", element);
            var attributes = attributesAndValues.Keys.ToList();
            return attributes;
        }
        public static IWebElement SecureFind(this IWebDriver driver, By selector, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));
        }

        public static SelectElement SecureFindSelect(this IWebDriver driver, By selector, TimeSpan? waitTime = null)
        {
            var element = SecureFind(driver, selector, waitTime);
            return new SelectElement(element);
        }
        public static void SecureSelectDropDownItem(this IWebDriver driver, By selector, string itemText, TimeSpan? waitTime = null)
        {
            var select = driver.SecureFindSelect(selector, waitTime);
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(select.WrappedElement, itemText));
            select.SelectByText(itemText);
        }
        public static void SecureSelectDropDownItem(this IWebDriver driver, By selector, object itemValue, TimeSpan? waitTime = null)
        {
            var select = SecureFindSelect(driver, selector, waitTime);
            select.SelectByValue(itemValue.ToString());
        }
        public static void SecureSelectDropDownItem(this IWebDriver driver, By selector, int itemIndex, TimeSpan? waitTime = null)
        {
            var select = SecureFindSelect(driver, selector, waitTime);
            select.SelectByIndex(itemIndex);
        }
        public static void SecureFindAndClick(this IWebDriver driver, By selector, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            var element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));
            element.Click();
        }
        public static void SecureMouseHoverElement(this IWebDriver driver, By selector, TimeSpan? waitTime = null)
        {
            var element = SecureFind(driver, selector, waitTime);
            var action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }

        public static void SecureFindAndSendKeys(this IWebDriver driver, By selector, string keys, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            var element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));
            element.SendKeys(keys);
        }
        public static void SecureFindAndSetText(this IWebDriver driver, By selector, string keys, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            var element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));
            element.Clear();
            element.SendKeys(keys);
        }
        public static IWebElement[] GetTableRows(this IWebDriver driver, By selector, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            var table = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));
            return table.FindElements(By.CssSelector("tr")).ToArray();
        }

        public static void SecureExecuteInframeAndBack(this IWebDriver driver, By selector, Action<IWebDriver> actionInFrame, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            var frame = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));
            driver.SwitchTo().Frame(frame);
            actionInFrame.Invoke(driver);
            driver.SwitchTo().DefaultContent();
        }

        public static IAlert SecureGetAlert(this IWebDriver driver, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
        }
        public static object ExecuteJs(this IWebDriver driver, string jsCommand, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            var returnNull = false;
            var obj = wait.Until(d =>
            {
                var o = ((IJavaScriptExecutor)d).ExecuteScript(jsCommand);
                if (o == null)
                {
                    returnNull = true;
                    return 1;
                }
                return o;
            });

            if (returnNull) return null;

            return obj;
        }

        public static void DropImage(this IWebDriver driver, string dropBoxId, string filePath)
        {

            var javascriptDriver = driver as IJavaScriptExecutor;

            var inputId = " 7a44090d-ca21-4b71-b2d4-7d40bf7a9cb2" + "FileUpload";

            javascriptDriver.ExecuteScript(inputId + " = window.$('<input id=\"" + inputId + "\"/>').attr({type:'file'}).appendTo('body');");

            driver.FindElement(By.Id(inputId)).SendKeys(filePath);

            javascriptDriver.ExecuteScript("e = $.Event('drop'); e.originalEvent = {dataTransfer : { files : " + inputId + ".get(0).files } }; $('#" + dropBoxId + "').trigger(e);");

        }


        public static void SecureGoToUrl(this IWebDriver driver, string url, TimeSpan? waitTime = null)
        {
            driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            var load = wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            if (!load)
            {
                throw new Exception($"A pagina {url} não carregou a tempo");
            }

        }

        public static FileInfo DownloadFile(this IWebDriver driver, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));

            return wait.Until(d =>
            {
                if (!driver.Url.StartsWith("chrome://downloads"))
                {
                    driver.Url = "chrome://downloads/";
                }

                var infos = driver.GetDownloadsInfos(waitTime);

                var limitSpan = waitTime ?? TimeSpan.FromMinutes(1);

                while (infos.ToList().Exists(dic => dic["state"].ToString().ToLower() == "in_progress"))
                {
                    limitSpan.Add(-TimeSpan.FromSeconds(1));
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

                    if (limitSpan.TotalSeconds == 0)
                    {
                        throw new Exception("DownloadFile Timespam dead");
                    }

                    infos = driver.GetDownloadsInfos(waitTime);
                }

                var downloadInfos = (ReadOnlyCollection<object>)driver.ExecuteJs(@"
                        return document.querySelector('downloads-manager')
                        .shadowRoot.querySelector('#downloadsList')
                        .items.filter(e => e.state === 'COMPLETE')
                        .map(e => e.filePath || e.file_path || e.fileUrl || e.file_url);", TimeSpan.FromMinutes(15));

                return downloadInfos
                    .Select(fp => new FileInfo(fp.ToString()))
                    .Where(f => f.Exists)
                    .OrderByDescending(f => f.CreationTime)
                    .FirstOrDefault();
            });

        }

        public static IEnumerable<Dictionary<string, object>> GetDownloadsInfos(this IWebDriver driver, TimeSpan? waitTime = null)
        {
            TimeSpan.FromSeconds(5);
            var wait = new WebDriverWait(driver, waitTime ?? TimeSpan.FromMinutes(1));
            return wait.Until(d =>
            {
                if (!driver.Url.StartsWith("chrome://downloads"))
                {
                    driver.Url = "chrome://downloads/";
                }


                var ret = (ReadOnlyCollection<object>)driver.ExecuteJs(@"
                        return document.querySelector('downloads-manager')
                        .shadowRoot.querySelector('#downloadsList').items;", TimeSpan.FromMinutes(15));


                return ret.Select(i => (Dictionary<string, object>)i);

            });

        }

        public static object GetAllAttributes(this IWebDriver driver, IWebElement element)
        {
            return ((IJavaScriptExecutor)driver).ExecuteScript(
                     "return arguments[0];", element);
        }

    }
}

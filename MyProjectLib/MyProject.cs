using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProjectLib
{
    public class MyProject
    {
		private IWebDriver driver;

		public MyProject(IWebDriver driver)
		{
			this.driver = driver;
		}

		public bool runSerf(String str, String sum, String[] pool)
		{
			try
			{
				driver.Manage().Window.Maximize();//1. Развернуть на весь экран

				driver.Navigate().GoToUrl("https://yandex.ru/");//2. Зайти на yandex.ru

				driver.FindElement(By.CssSelector("[data-id='market']")).Click();//3. Перейти в яндекс маркет

				driver.FindElement(By.CssSelector("[class='n-w-tab n-w-tab_type_navigation-menu']")).Click();//4. Выбрать раздел электроника

				driver.FindElement(By.PartialLinkText(str)).Click();//5. Перейти в указанный раздел
				
				driver.FindElement(By.CssSelector("[class='OcaftndW9c _2bjY2zQo59 _4WmLhr2Vhx _2Kihe5N2Sn']")).Click();//6. Зайти в расширеный поиск

				driver.FindElement(By.Name("glf-pricefrom-var")).SendKeys(sum);//7. Задать параметр поиска от Х рублей

				foreach(String c in pool)//8. Выбрать производителей
				{
					driver.FindElement(By.LinkText(c)).Click();
				}

				driver.FindElement(By.CssSelector("[class='button button_size_l button_theme_pseudo i-bem button_action_show-filtered n-filter-panel-extend__controll-button_size_big button_js_inited']")).Click();//9. Нажать кнопку Применить
				
				if(check("[class='radio-button__radio radio-button__radio_side_left radio-button__radio_checked_yes']"))
				{
					driver.FindElement(By.CssSelector("[value='list']")).Click();
				}

				WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("n-snippet-card2__title")));

				int count = driver.FindElements(By.ClassName("n-snippet-card2__title")).Count;
				if (count == 12)//10. Проверить, что элементов на странице 12.
				{
					MessageBox.Show("Count = " + count);
				}
				else MessageBox.Show("Count = " + count + " != 12");

				String name = driver.FindElements(By.ClassName("n-snippet-card2__title")).ElementAt(0).GetAttribute("textContent");//11. Запомнить первый элемент в списке.
				name = name.Substring(name.IndexOf(' ') + 1);

				driver.FindElement(By.Id("header-search")).SendKeys(name);//12. В поисковую строку ввести запомненное значение.

				wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText(name.ToLower())));
				driver.FindElement(By.LinkText(name.ToLower())).Click();
				//driver.FindElement(By.CssSelector("[class='search2__button']")).Click();

				String title = driver.FindElement(By.CssSelector("[class='n-title__text']")).GetAttribute("textContent");
				title = title.Substring(title.IndexOf(' ') + 1);
				
				if (name.Equals(title)) return true;
				else return false;//13. Найти и проверить, что наименование товара соответствует запомненному значению.
			}
			catch
			{
				return false;
			}
		}
		private bool check(String str)
		{
			try
			{
				driver.FindElement(By.CssSelector(str));
				return true;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}
	}
}

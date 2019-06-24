using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProjectLib
{
    public class MyProject
    {
		private IWebDriver driver;
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public MyProject(IWebDriver driver)
		{
			this.driver = driver;
		}

		public bool runSerf(String str, String sum, String[] pool)
		{
			logger.Info("Старт программы");
			try
			{
				driver.Manage().Window.Maximize();//1. Развернуть на весь экран

				driver.Navigate().GoToUrl("https://yandex.ru/");//2. Зайти на yandex.ru

				Clicker(By.CssSelector("[data-id='market']"));//3. Перейти в яндекс маркет

				Clicker(By.CssSelector("[class='n-w-tab n-w-tab_type_navigation-menu']"));//4. Выбрать раздел электроника

				Clicker(By.PartialLinkText(str));//5. Перейти в указанный раздел
				
				Clicker(By.CssSelector("[class='OcaftndW9c _2bjY2zQo59 _4WmLhr2Vhx _2Kihe5N2Sn']"));//6. Зайти в расширеный поиск

				driver.FindElement(By.Name("glf-pricefrom-var")).SendKeys(sum);//7. Задать параметр поиска от Х рублей
				logger.Info("Задан параметр поиска " + sum);

				foreach (String c in pool)//8. Выбрать производителей
				{
					Clicker(By.LinkText(c));
				}

				Clicker(By.CssSelector("[class='button button_size_l button_theme_pseudo i-bem button_action_show-filtered n-filter-panel-extend__controll-button_size_big button_js_inited']"));//9. Нажать кнопку Применить
				
				if(check("[class='radio-button__radio radio-button__radio_side_left radio-button__radio_checked_yes']"))
				{
					Clicker(By.CssSelector("[value='list']"));
				}

				WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("n-snippet-card2__title")));

				int count = driver.FindElements(By.ClassName("n-snippet-card2__title")).Count;
				if (count == 12)//10. Проверить, что элементов на странице 12.
				{
					logger.Info("Проверка на количество элементов выполнена успешно (12)");
				}
				else logger.Warn("Проверка на количество элементов провалена. Элементов - " + count);

				String name = driver.FindElements(By.ClassName("n-snippet-card2__title")).ElementAt(0).GetAttribute("textContent");//11. Запомнить первый элемент в списке.
				name = name.Substring(name.IndexOf(' ') + 1);
				logger.Info("Получен первый элемент списка - " + name);

				driver.FindElement(By.Id("header-search")).SendKeys(name);//12. В поисковую строку ввести запомненное значение.
				logger.Info("Полученный элемент введен в поисковую строку");

				wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText(name.ToLower())));
				Clicker(By.LinkText(name.ToLower()));
				//driver.FindElement(By.CssSelector("[class='search2__button']")).Click();

				String title = driver.FindElement(By.CssSelector("[class='n-title__text']")).GetAttribute("textContent");
				title = title.Substring(title.IndexOf(' ') + 1);
				logger.Info("Получено имя найденного продукта - " + title);

				if (name.Equals(title))
				{
					logger.Info("Тестирование прошло успешно");
					MessageBox.Show("Тестирование прошло успешно");
					return true;
				}
				else
				{
					logger.Error("Несоответствие значений");
					MessageBox.Show("Несоответствие значений");
					return false;
				}//13. Найти и проверить, что наименование товара соответствует запомненному значению.
			}
			catch (Exception exception)
			{
				logger.Error(exception);
				MessageBox.Show(exception.ToString());
				String[] names = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\logs\\");
				Process.Start(names[0]);
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
		private void Clicker(By by)
		{
			driver.FindElement(by).Click();
			logger.Info("Нажатие на элемент" + by.ToString());
		}
	}
}

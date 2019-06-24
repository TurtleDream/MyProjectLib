using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MyProjectLib.Tests
{
	[TestClass]
	public class MyProjectTests
	{
		[TestMethod]
		public void testCase1_trueReturned()
		{
			IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver();
			bool expected = true;
			String[] pool =
			{
				"LG",
				"Samsung"
			};

			MyProject myProject = new MyProject(driver);
			bool actual = myProject.runSerf("Телевизоры", "20000", pool);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void testCase2_trueReturned()
		{
			IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver();
			bool expected = true;
			String[] pool =
			{
				"Beats"
			};

			MyProject myProject = new MyProject(driver);
			bool actual = myProject.runSerf("Наушники", "5000", pool);

			Assert.AreEqual(expected, actual);
		}
	}
}

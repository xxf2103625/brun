using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrunWebTest.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BrunWebTest.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        ILoggerFactory loggerFactory;
        public HomeControllerTests()
        {
            loggerFactory = LoggerFactory.Create(m => m.AddConsole());

        }
        [TestMethod()]
        public void IndexTest()
        {
            var _logger = GetLogger<HomeController>();
            var control = new HomeController(_logger);
            IActionResult result = control.Index();
            Debug.WriteLine("IndexTest");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            //Assert.Fail();
        }
        private ILogger<T> GetLogger<T>() where T : class
        {
            return loggerFactory.CreateLogger<T>();
        }
    }
}
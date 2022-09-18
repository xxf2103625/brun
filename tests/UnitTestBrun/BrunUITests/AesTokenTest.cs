using BrunUI.Auths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun.BrunUITests
{
    [TestClass]
    public class AesTokenTest
    {
        [TestMethod]
        public void TestAes()
        {
            //32位
            string myKey = "ephxyemahezvxpaosanckaytzpilamco";
            //16位
            //string iv= "ephxephxephxephx";
            
            //string s = "送到房间啊撒旦解放 阿里山的空间发阿斯顿";
            string s = System.Text.Json.JsonSerializer.Serialize(new BrunUI.Auths.BrunUser()
            {
                UserName = "admin",
                Password = "brun",
                Roles = new List<string>() { "admin", "create" }
            });

            //var token = AesTokenHelper.AesEncrypt(s, myKey, iv);
            //Console.WriteLine(token);
            //Assert.IsNotNull(token);
            //var str = AesTokenHelper.AesDecrypt(token, myKey, iv);
            //Console.WriteLine(token);
            //Console.WriteLine(str);
            //Assert.AreEqual(s, str); ;
            {
                var user = new BrunUI.Auths.BrunUser()
                {
                    UserName = "admin",
                    Password = "brun",
                    Roles = new List<string>() { "admin", "create" }
                };
                var userToken = AesTokenHelper.GetToken(user, myKey);
                Console.WriteLine(userToken);
                var getUser = AesTokenHelper.GetUser(userToken, myKey);
                Assert.AreEqual("admin", getUser.UserName);
                Assert.AreEqual("brun", getUser.Password);
                Assert.IsTrue(getUser.Roles.Contains("admin") && getUser.Roles.Contains("create"));
            }
            
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Haraba.GoProxy.Tests
{
    public class MainTests
    {
        private const string GoProxyUrl = "http://localhost:8000/";
        private const string ChromeUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36";
        private const string ChromeJa3 = "771,4865-4866-4867-49195-49199-49196-49200-52393-52392-49171-49172-156-157-47-53,0-23-65281-10-11-35-16-5-13-18-51-45-43-27-21,29-23-24,0";
        //private const string ChromeJa3 = "1234";
        //private const string Url = "https://ja3er.com/json";
        //private const string Url = "https://www.g2.com/";
        //private const string Url = "https://www.c-and-a.com/img/product/q_auto:good,b_rgb:E0DEDA,c_scale,w_477/v1691581791/productimages/2209221-1-08.jpg";
        //private const string Url = "https://p5s.ru/upload/iblock/a82/a82d81b72125d6e177f5b38254dcdcc3.png";
        private const string Url = "https://www.sportsdirect.com/images/products/21181918_l.jpg";
        
        [Test]
        public async Task GetResponse_ShouldApplyJA3()
        {
            var response = await GoHttpRequest.Create(GoProxyUrl)
                .WithJa3(ChromeJa3)
                .WithUserAgent(ChromeUserAgent)
                .WithHeader("Accept", "*")
                .WithHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7")
                .WithHeader("Accept-Encoding", "gzip, deflate, br")
                .GetResponseAsync(Url);
            
            //await File.WriteAllBytesAsync(@"saved.png", Convert.FromBase64String(response.Payload.Text));
            await File.WriteAllBytesAsync(@"saved.jpg", Convert.FromBase64String(response.Payload.Text));
            //File.WriteAllText(@"d:\Projects\Study\Haraba.GoProxy-main\g2.html", response.Payload.Text);
            
            Assert.AreEqual(true, response.Success);
            //Assert.IsTrue(response.Payload.Text.Contains(ChromeJa3));
        }

        [Test]
        public void GetResponse_ShouldApplyJA3Sync()
        {
            var response = GoHttpRequest.Create(GoProxyUrl)
                .WithJa3(ChromeJa3)
                .WithUserAgent(ChromeUserAgent)
                .WithHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9")
                .WithHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7")
                .WithHeader("Accept-Encoding", "gzip, deflate, br")
                .GetResponse("https://ja3er.com/json");

            Assert.AreEqual(true, response.Success);
            Assert.IsTrue(response.Payload.Text.Contains(ChromeJa3));
        }
        
        [Test]
        public async Task GetResponse_ShouldNotApplyJA3()
        {
            var response = await GoHttpRequest.Create(GoProxyUrl)
                .WithUserAgent(ChromeUserAgent)
                .GetResponseAsync("https://ja3er.com/json");
            
            Assert.AreEqual(true, response.Success);
            Assert.IsFalse(response.Payload.Text.Contains(ChromeJa3));
        }
        
        [Test]
        public async Task GetResponse_TestUrl()
        {
            var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(Url);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody?.Take(50));
                Assert.True(response.IsSuccessStatusCode);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
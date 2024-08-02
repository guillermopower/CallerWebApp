using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CallerWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallerController : Controller
    {
        [HttpGet("GetXml3rdParties")]
        public ActionResult<Task<string>> GetXml3rdParties()
        {
            string url = "https://data.cityofnewyork.us/api/views/c3uy-2p5r/rows.xml";
            string finalValue = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response =  client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Now we have the XML content in responseBody
                    // Next step is to parse this XML
                    finalValue = ParseAndProcessXml(responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return Ok(finalValue);
        }

        static string ParseAndProcessXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return JsonConvert.SerializeXmlNode(doc);
        }
    }
}

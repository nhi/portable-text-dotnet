using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace NHI.PortableText.Example.Controllers
{
    /// <summary>
    /// Controller which takes html input and returns Portable Text
    /// </summary>
    [Route("/")]
    [ApiController]
    public class PortableTextController : ControllerBase
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Create a new controller instance
        /// </summary>
        public PortableTextController(ILogger<PortableTextController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns a form for inputting HTML source
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<form action='/' method='post'><h3>Enter HTML to convert to Portable Text:</h3><textarea cols='80' rows='40' name='html'>&ltp&gtWelcome to &ltb&gtportable text&lt/b&gt&lt/p&gt</textarea><br/><br/><input type='submit'></form>"
            };
        }

        /// <summary>
        /// Takes HTML input and returns Portable Text
        /// </summary>
        [HttpPost]
        public ActionResult Index([FromForm] string html)
        {
            BlockConverter bc = new BlockConverter(new BlockConverterSettings 
            { 
                Logger = _logger,
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                }
            });

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = $"<pre>{bc.SerializeHtml(html)}</pre>"
            };
        }
    }
}

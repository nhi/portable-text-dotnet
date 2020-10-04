using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PortableText.Model;

namespace PortableText.Example.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortableTextController : ControllerBase
    {

        [HttpPost]
        public ActionResult<IEnumerable<BlockModel>> GetPortableText([FromBody] string html)
        {
            BlockConverter bc = new BlockConverter();
            return Ok(bc.ConvertHtml(html));
        }
    }
}

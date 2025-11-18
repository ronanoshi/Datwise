using Microsoft.AspNetCore.Mvc;
using Datwise.Services;
using Datwise.Contracts;

namespace Datwise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IExampleService _service;
        public ValuesController(IExampleService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _service.GetExample(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
  public class PruebaController : ControllerBase
    {
        [HttpGet("prueba")]
        public string pruebaApi()
        {
            return "Estoy probando la api";
        }
    }
}

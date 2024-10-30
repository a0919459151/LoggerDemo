using LoggerDemo.Clients.DogClient;
using LoggerDemo.Excptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoggerDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogController : Controller
    {
        private readonly DogClient _dogClient;

        public DogController(DogClient dogClient)
        {
            _dogClient = dogClient;
        }

        [HttpGet("GetDog")]
        public async Task<IActionResult> GetDog()
        {
            var res = await _dogClient.GetDogAsync();

            return Ok(res);
        }
    }
}

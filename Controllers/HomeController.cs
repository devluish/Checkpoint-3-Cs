using Microsoft.AspNetCore.Mvc;
using CP3_C_.Models;
using PratoService.Services.Prato;
using System.Threading.Tasks;

namespace CP3_C_.Controllers
{
    public class HomeController : Controller
    {
        private readonly PratoService _pratoService;

        public HomeController(PratoService pratoService)
        {
            _pratoService = pratoService;
        }

        public async Task<IActionResult> Index()
        {
            var pratos = await _pratoService.GetPratos();
            return View(pratos);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using CP3_C_.Models;
using PratoService.Services.Prato;
using System.Linq;
using System.Threading.Tasks;

namespace checkpoint_DB.Controllers
{
    public class PratoController : Controller
    {
        private readonly PratoService _pratoService;

        public PratoController(PratoService pratoService)
        {
            _pratoService = pratoService;
        }

        public async Task<IActionResult> Index()
        {
            var pratos = await _pratoService.GetPratos();
            return View(pratos);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var prato = await _pratoService.GetPratoPorId(id);
            return View(prato);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var prato = await _pratoService.GetPratoPorId(id);
            return View(prato);
        }

        public async Task<IActionResult> Remover(int id)
        {
            await _pratoService.RemoverPrato(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(PratoCriacaoDto pratoCriacaoDto, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                await _pratoService.CriarPrato(pratoCriacaoDto, foto);
                return RedirectToAction("Index");
            }
            return View(pratoCriacaoDto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PratosModel pratoModel, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                await _pratoService.EditarPrato(pratoModel, foto);
                return RedirectToAction("Index");
            }
            return View(pratoModel);
        }
    }
}

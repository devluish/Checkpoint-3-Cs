using CP3_C_.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PratoInterface.Services.Prato
{
    public interface PratoInterface
    {
        Task<PratosModel> CriarPrato(PratoCriacaoDto pratoCriacaoDto, IFormFile foto);
        Task<List<PratosModel>> GetPratos();
        Task<PratosModel> GetPratoPorId(int id);
        Task<PratosModel> EditarPrato(PratosModel prato, IFormFile? foto);
        Task<PratosModel> RemoverPrato(int id);
        Task<List<PratosModel>> GetPratosFiltro(string? pesquisar);
    }
}

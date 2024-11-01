using checkpoint_DB.Data;
using CP3_C_.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PratoInterface.Services.Prato;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PratoService.Services.Prato
{
    public class PratoService : PratoInterface
    {
        private readonly DataBaseContext _context;
        private readonly string _sistema;

        public PratoService(DataBaseContext context, IWebHostEnvironment sistema)
        {
            _context = context;
            _sistema = sistema.WebRootPath;
        }

        public string GeraCaminhoArquivo(IFormFile foto)
        {
            var codigoUnico = Guid.NewGuid().ToString();
            var nomeCaminhoImagem = foto.FileName.Replace(" ", "").ToLower() + codigoUnico + ".png";

            var caminhoParaSalvarImagens = _sistema + "\\imagem\\";

            if (!Directory.Exists(caminhoParaSalvarImagens))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagens);
            }

            using (var stream = File.Create(caminhoParaSalvarImagens + nomeCaminhoImagem))
            {
                foto.CopyToAsync(stream).Wait();
            }

            return nomeCaminhoImagem;
        }

        public async Task<PratosModel> CriarPrato(PratoCriacaoDto pratoCriacaoDto, IFormFile foto)
        {
            try
            {
                var nomeCaminhoImagem = GeraCaminhoArquivo(foto);

                var prato = new PratosModel
                {
                    Nome = pratoCriacaoDto.Nome,
                    Descricao = pratoCriacaoDto.Descricao,
                    Valor = pratoCriacaoDto.Valor,
                    Capa = nomeCaminhoImagem
                };

                _context.Add(prato);
                await _context.SaveChangesAsync();

                return prato;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PratosModel>> GetPratos()
        {
            try
            {
                return await _context.Pratos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PratosModel> GetPratoPorId(int id)
        {
            try
            {
                return await _context.Pratos.FirstOrDefaultAsync(prato => prato.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PratosModel> EditarPrato(PratosModel pratoModel, IFormFile? foto)
        {
            try
            {
                var pratoBanco = await _context.Pratos.AsNoTracking().FirstOrDefaultAsync(pratoBD => pratoBD.Id == pratoModel.Id);

                var nomeCaminhoImagem = "";

                if (foto != null)
                {
                    string caminhoCapaExistente = _sistema + "\\imagem\\" + pratoBanco.Capa;

                    if (File.Exists(caminhoCapaExistente))
                    {
                        File.Delete(caminhoCapaExistente);
                    }

                    nomeCaminhoImagem = GeraCaminhoArquivo(foto);
                }

                pratoBanco.Nome = pratoModel.Nome;
                pratoBanco.Descricao = pratoModel.Descricao;
                pratoBanco.Valor = pratoModel.Valor;

                if (nomeCaminhoImagem != "")
                {
                    pratoBanco.Capa = nomeCaminhoImagem;
                }
                else
                {
                    pratoBanco.Capa = pratoModel.Capa;
                }

                _context.Update(pratoBanco);
                await _context.SaveChangesAsync();

                return pratoBanco;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoverPrato(int id)
        {
            try
            {
                var prato = await _context.Pratos.FirstOrDefaultAsync(pratobanco => pratobanco.Id == id);

                if (prato == null)
                {
                    return false;
                }

                _context.Remove(prato);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PratosModel>> GetPratosFiltro(string? pesquisar)
        {
            try
            {
                return await _context.Pratos.Where(pratoBanco => pratoBanco.Nome.Contains(pesquisar)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

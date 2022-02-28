using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using proeventos.Domain;
using proeventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using proeventos.Application.Dtos;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using proeventos.api.Extensions;
using Microsoft.AspNetCore.Authorization;
using proeventos.Persistence.Models;

namespace proeventos.api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;
        private readonly IWebHostEnvironment _hostEnveronment;
        private readonly IAccountService _accountService;
        public PalestrantesController(IPalestranteService palestranteService, 
                                      IWebHostEnvironment hostEnveronment,
                                      IAccountService accountService)
        {
            _palestranteService = palestranteService;
            _hostEnveronment = hostEnveronment;
            _accountService = accountService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, 
                                       palestrantes.PageSize, 
                                       palestrantes.TotalCount, 
                                       palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar palestrantes. Erro: {ex.Message}");
            }        
        }

        [HttpGet]
        public async Task<IActionResult> GetPalestrantes()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if (palestrante == null) return NoContent();
                

                return Ok(palestrante);
            }
            catch (Exception ex)
            {               
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar palestrante. Erro: {ex.Message}");
            }     
        }


        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null)
                    palestrante = await _palestranteService.AddPalestrante(User.GetUserId(), model);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao inserir palestrante. Erro: {ex.Message}");
            }     
        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestranteService.UpdatePalestrante(User.GetUserId(), model);
                if (palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar palestrante. Erro: {ex.Message}");
            }     
        }


    }
}

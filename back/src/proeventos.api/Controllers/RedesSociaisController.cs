using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using proeventos.Application.Dtos;
using proeventos.Application.Contratos;
using proeventos.api.Extensions;

namespace proeventos.api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;
        public RedesSociaisController(IRedeSocialService redeSocialService, 
                                      IEventoService eventoService, 
                                      IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;

        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if (await NaoAutorEvento(eventoId)) return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar Redes Sociais. Erro: {ex.Message}");
            }        
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null) return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar Redes Sociais. Erro: {ex.Message}");
            }        
        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if (await NaoAutorEvento(eventoId)) return Unauthorized();

                var redeSociais = await _redeSocialService.SaveByEvento(eventoId, models);
                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao salvar Redes Sociais do Evento. Erro: {ex.Message}");
            }     
        }


        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null) return Unauthorized();

                var redeSociais = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao salvar Redes Sociais do Palestrante. Erro: {ex.Message}");
            }     
        }


        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (await NaoAutorEvento(eventoId)) return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if(redeSocial == null) return NoContent();

                if (await _redeSocialService.DeleteByEvento(eventoId, redeSocialId))
                    return Ok(new {message = "Deletado Rede Social do Evento"});                
                else
                     return BadRequest($"Erro ao Deletar Rede Social do Evento");
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar Rede Social do Evento. Erro: {ex.Message}");
            }    
        }


        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
                if(redeSocial == null) return NoContent();

                if (await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId))
                    return Ok(new {message = "Deletado Rede Social do Palestrante"});                
                else
                     return BadRequest($"Erro ao Deletar Rede Social do Palestrante");
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar Rede Social do Palestrante. Erro: {ex.Message}");
            }    
        }

        [NonAction]
        private async Task<bool> NaoAutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null)
                return true;
            return false;
        }
    }
}

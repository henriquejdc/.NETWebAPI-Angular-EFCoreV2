using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using proeventos.Persistence;
using proeventos.Domain;
using proeventos.Persistence.Contextos;
using proeventos.Application.Contratos;
using Microsoft.AspNetCore.Http;

namespace proeventos.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(true);
                if (eventos == null) return NotFound("Nenhum Evento Encontrado");

                return Ok(eventos);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar eventos. Erro: {ex.Message}");
            }        
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Console.Write(id);
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(id, true);
                if (evento == null) return NotFound($"Nenhum Evento id: {id} Encontrado");

                return Ok(evento);
            }
            catch (Exception ex)
            {               
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar evento. Erro: {ex.Message}");
            }     
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var evento = await _eventoService.GetAllEventosByTemaAsync(tema, true);
                if (evento == null) return NotFound($"Nenhum Evento tema: {tema} Encontrado");

                return Ok(evento);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar evento. Erro: {ex.Message}");
            }     
        }


        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                var evento = await _eventoService.AddEventos(model);
                if (evento == null) return BadRequest($"Erro ao tentar adicionar.");

                return Ok(evento);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao inserir evento. Erro: {ex.Message}");
            }     
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento model)
        {
            try
            {
                var evento = await _eventoService.UpdateEventos(id, model);
                if (evento == null) return BadRequest($"Erro ao tentar atualizar.");

                return Ok(evento);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar evento. Erro: {ex.Message}");
            }     
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _eventoService.DeleteEventos(id))
                    return Ok("Deletado");                
                else
                     return BadRequest($"Erro ao deletar atualizar.");
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar evento. Erro: {ex.Message}");
            }    
        }

    }
}

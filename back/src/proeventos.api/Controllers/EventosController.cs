using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using proeventos.Application.Contratos;
using proeventos.Application.Dtos;
using proeventos.api.Extensions;
using proeventos.Persistence.Models;
using proeventos.api.helpers;

namespace proeventos.api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly string _destino = "Images";
        private readonly IEventoService _eventoService;
        private readonly IAccountService _accountService;
        private readonly IUtil _util;
        public EventosController(IEventoService eventoService, 
                                 IAccountService accountService, 
                                 IUtil util)
        {
            _eventoService = eventoService;
            _accountService = accountService;
            _util = util;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
                if (eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

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
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();
                

                return Ok(evento);
            }
            catch (Exception ex)
            {               
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar evento. Erro: {ex.Message}");
            }     
        }


        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);

                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    evento.ImagemURL = await _util.SaveImage(file, _destino);
                }
                var EventoRetorno = await _eventoService.UpdateEventos(User.GetUserId(), eventoId, evento);

                ;
                return Ok(EventoRetorno);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao inserir imagem do evento. Erro: {ex.Message}");
            }     
        }


        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventoService.AddEventos(User.GetUserId(), model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao inserir evento. Erro: {ex.Message}");
            }     
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventoService.UpdateEventos(User.GetUserId(), id, model);
                if (evento == null) return NoContent();

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
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if(evento == null) return NoContent();

                if (await _eventoService.DeleteEventos(User.GetUserId(), id)){
                    
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new {message = "Deletado"});      
                }          
                else
                     return BadRequest($"Erro ao deletar.");
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar evento. Erro: {ex.Message}");
            }    
        }

        // [NonAction]
        // public async Task<string> SaveImage(IFormFile imageFile)
        // {
        //     string imageName = new String(
        //         Path.GetFileNameWithoutExtension(imageFile.FileName)
        //         .Take(10)
        //         .ToArray()).Replace(' ','-');
            
        //     imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

        //     var imagePath = Path.Combine(_hostEnveronment.ContentRootPath, 
        //     @"Resources/images", imageName);

        //     using(var fileStream = new FileStream(imagePath, FileMode.Create))
        //     {
        //         await imageFile.CopyToAsync(fileStream);
        //     }

        //     Console.WriteLine(imageName);
        //     return imageName;
        // }

        // [NonAction]
        // public void DeleteImage(string imageName)
        // {
        //     var imagePath = Path.Combine(_hostEnveronment.ContentRootPath, 
        //     @"Resources/images", imageName);
        //     if (System.IO.File.Exists(imagePath)){
        //         System.IO.File.Delete(imagePath);
        //     }
        // }


    }
}

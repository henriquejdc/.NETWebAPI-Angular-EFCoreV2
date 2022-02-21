using System;
using System.Threading.Tasks;
using AutoMapper;
using proeventos.Application.Contratos;
using proeventos.Application.Dtos;
using proeventos.Domain;
using proeventos.Persistence.Contratos;
using proeventos.Persistence.Models;

namespace proeventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersistence _geralPersistence;
        private readonly IEventoPersistence _eventoPersistence;
        private readonly IMapper _mapper;
        public EventoService(IGeralPersistence geralPersistence, 
                             IEventoPersistence eventoPersistence,
                             IMapper mapper)
        {
            _geralPersistence = geralPersistence;
            _eventoPersistence = eventoPersistence;
            _mapper = mapper;

        }

        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            try
            {   
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;
                _geralPersistence.Add<Evento>(evento);
                
                if (await _geralPersistence.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersistence.GetEventoByIdAsync(userId, evento.Id, false);
                    
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> UpdateEventos(int userId, int eventoId, EventoDto model)
        {   
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, false);
                if ( evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralPersistence.Update<Evento>(evento);

                if (await _geralPersistence.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersistence.GetEventoByIdAsync(userId, evento.Id, false);
                    
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEventos(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, false);
                if ( evento == null) throw new Exception("Evento Não Existe!");

                _geralPersistence.Delete<Evento>(evento);
                return await _geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId, 
                                                                  PageParams pageParams, 
                                                                  bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersistence.GetAllEventosAsync(userId, pageParams, includePalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<PageList<EventoDto>>(eventos);

                resultado.CurrentPage = eventos.CurrentPage;                
                resultado.TotalPages = eventos.TotalPages;                
                resultado.PageSize = eventos.PageSize;                
                resultado.TotalCount = eventos.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, 
                                                        int EventoId, 
                                                        bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersistence.GetEventoByIdAsync(userId, EventoId, includePalestrantes);
                if (evento == null) return null;
                
                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

    }
}
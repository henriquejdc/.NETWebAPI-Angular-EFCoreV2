using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using proeventos.Application.Contratos;
using proeventos.Application.Dtos;
using proeventos.Domain;
using proeventos.Persistence.Contratos;

namespace proeventos.Application
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialPersistence _redeSocialPersistence;
        private readonly IMapper _mapper;
        public RedeSocialService(IRedeSocialPersistence redeSocialPersistence,
                                 IMapper mapper)
        {
            _redeSocialPersistence = redeSocialPersistence;
            _mapper = mapper;

        }

        public async Task AddRedeSocial(int Id, RedeSocialDto model, bool isEvento)
        {
            try
            {   
                var redeSocial = _mapper.Map<RedeSocial>(model);
                
                if (isEvento)
                {
                    redeSocial.EventoId = Id;
                    redeSocial.PalestranteId = null;
                } 
                else
                {
                    redeSocial.EventoId = null;
                    redeSocial.PalestranteId = Id;
                }

                _redeSocialPersistence.Add<RedeSocial>(redeSocial);

                await _redeSocialPersistence.SaveChangesAsync();

            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {   
            try
            {
                var redeSociais = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
                if ( redeSociais == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {   
                        var redeSocial = redeSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);

                        model.EventoId = eventoId;

                        _mapper.Map(model, redeSocial);

                        _redeSocialPersistence.Update<RedeSocial>(redeSocial);

                        await _redeSocialPersistence.SaveChangesAsync();
                    }

                }

                var redeSocialRetorno = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
                    
                return _mapper.Map<RedeSocialDto[]>(redeSocialRetorno);

            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }        

        public async Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models)
        {   
            try
            {
                var redeSociais = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
                if ( redeSociais == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else                    
                    {
                        var RedeSocial = redeSociais.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);
                        model.PalestranteId = palestranteId;

                        _mapper.Map(model, RedeSocial);

                        _redeSocialPersistence.Update<RedeSocial>(RedeSocial);

                        await _redeSocialPersistence.SaveChangesAsync();
                    }

                }


                var redeSocialRetorno = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
                    
                return _mapper.Map<RedeSocialDto[]>(redeSocialRetorno);

            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersistence.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if ( redeSocial == null) throw new Exception("Rede Social do Evento Não Existe!");

                _redeSocialPersistence.Delete<RedeSocial>(redeSocial);
                return await _redeSocialPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersistence
                                        .GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if ( redeSocial == null) throw new Exception("Rede Social do Palestrante Não Existe!");

                _redeSocialPersistence.Delete<RedeSocial>(redeSocial);
                return await _redeSocialPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redeSociais = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
                if (redeSociais == null) return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(redeSociais);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redeSociais = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
                if (redeSociais == null) return null;

                var resultado = _mapper.Map<RedeSocialDto[]>(redeSociais);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersistence.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null) return null;
                
                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersistence
                                        .GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redeSocial == null) return null;
                
                var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

    }
}
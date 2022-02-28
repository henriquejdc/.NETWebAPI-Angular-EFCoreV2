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
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersistence _palestrantePersistence;
        private readonly IMapper _mapper;
        public PalestranteService(IPalestrantePersistence palestrantePersistence,
                                  IMapper mapper)
        {
            _palestrantePersistence = palestrantePersistence;
            _mapper = mapper;

        }

        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model)
        {
            try
            {   
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;
                _palestrantePersistence.Add<Palestrante>(palestrante);
                
                if (await _palestrantePersistence.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersistence.GetPalestranteByUserIdAsync(userId, false);
                    
                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {   
            try
            {
                var palestrante = await _palestrantePersistence.GetPalestranteByUserIdAsync(userId, false);
                if ( palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestrantePersistence.Update<Palestrante>(palestrante);

                if (await _palestrantePersistence.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersistence.GetPalestranteByUserIdAsync(userId, false);
                    
                    return _mapper.Map<PalestranteDto>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, 
                                                                            bool includeEventos = false)
        {
            try
            {
                var palestrantes = await _palestrantePersistence.GetAllPalestrantesAsync(pageParams, includeEventos);
                if (palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                resultado.CurrentPage = palestrantes.CurrentPage;                
                resultado.TotalPages = palestrantes.TotalPages;                
                resultado.PageSize = palestrantes.PageSize;                
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, 
                                                                      bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestrantePersistence.GetPalestranteByUserIdAsync(userId, includeEventos);
                if (palestrante == null) return null;
                
                var resultado = _mapper.Map<PalestranteDto>(palestrante);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

    }
}
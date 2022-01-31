namespace proeventos.Application.Dtos
{
    public class RedeSocialDto
    {
        public int Id {get; set;}
        public string Nome {get; set;}
        public string Url {get; set;}
        public int EventoId {get; set;}
        public EventoDto Evento {get; set;}
        public int PalestranteId {get; set;}
        public PalestranteDto Palestrante {get; set;}
        
    }
}
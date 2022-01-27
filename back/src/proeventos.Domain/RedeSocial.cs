namespace proeventos.Domain
{
    public class RedeSocial
    {
        public int Id {get; set;}

        public string Nome {get; set;}
        public string Url {get; set;}
        public int? EventoId {get; set;}
        public Evento Evento {get; set;}
        public int? PalestranteId {get; set;}
        public Palestrante Palestrante {get; set;}

    }
}
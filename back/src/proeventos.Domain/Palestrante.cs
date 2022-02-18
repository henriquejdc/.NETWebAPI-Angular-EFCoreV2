using System.Collections.Generic;
using proeventos.Domain.Identity;

namespace proeventos.Domain
{
    public class Palestrante
    {
        public int Id {get; set;}
        public string MiniCurriculo {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public IEnumerable<RedeSocial> RedesSociais {get; set;}
        public IEnumerable<PalestranteEvento> PalestranteEventos {get; set;}

    }
}
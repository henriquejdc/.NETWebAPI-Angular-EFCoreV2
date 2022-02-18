using System;
using System.Collections.Generic;
using proeventos.Domain.Identity;

namespace proeventos.Domain
{
    // [Table("Evento")]
    public class Evento
    {
        // [Key]
        public int Id { get; set;}
        
        // [Required]
        public string Local { get; set;}

        public DateTime? DataEvento { get; set;}

        public string Tema { get; set;}

        // [NotMapped] caso nao seja um campo que sera usado
        public int QtdPessoas { get; set;}

        public string ImagemURL { get; set;}

        public string Telefone { get; set;}

        public string Email { get; set;}
        public int UserId { get; set;}
        public User User { get; set;}

        public IEnumerable<Lote> Lotes { get; set;}

        public IEnumerable<RedeSocial> RedesSociais { get; set;}

        public IEnumerable<PalestranteEvento> PalestranteEventos {get; set;}

    }
}

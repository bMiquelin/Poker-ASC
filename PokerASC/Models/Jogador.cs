using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PokerASC.Models
{
    public class Jogador
    {
        public Jogador()
        {
            if (CartasNaMao == null)
                CartasNaMao = new List<Carta>();
        }
        [Key]
        public int Id { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual List<Carta> CartasNaMao { get; set; }
        
        public Score lastScore { get; set; } //lastScore é um cache do score, para deixar a lógica mais simples.
        public Score GetScore() {
            lastScore = CartasNaMao.GetScore();
            return lastScore;
        }
    }
}

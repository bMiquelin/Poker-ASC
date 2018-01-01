using PokerASC.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PokerASC.Models
{
    public enum Naipe
    {
        [Description("diamonds")]
        Ouros,
        [Description("spades")]
        Espadas,
        [Description("hearts")]
        Copas,
        [Description("clubs")]
        Paus
    }

    public enum Rank
    {
        [Description("A")]
        A,
        [Description("2")]
        _2,
        [Description("3")]
        _3,
        [Description("4")]
        _4,
        [Description("5")]
        _5,
        [Description("6")]
        _6,
        [Description("7")]
        _7,
        [Description("8")]
        _8,
        [Description("9")]
        _9,
        [Description("9")]
        _10,
        [Description("J")]
        J,
        [Description("Q")]
        Q,
        [Description("K")]
        K,
    }

    public class Carta
    {
        [Key]
        public int Id { get; set; }
        public Rank Rank { get; set; }
        public Naipe Naipe { get; set; }
        public virtual Jogador Jogador { get; set; }

        public static bool Equals(Carta carta1, Carta carta2)
        {
            return carta1.Naipe == carta2.Naipe && carta1.Rank == carta2.Rank;
        }

        public static IEnumerable<Carta> NovoDeck()
        {
            foreach(Rank r in Enum.GetValues(typeof(Rank)))
                foreach(Naipe n in Enum.GetValues(typeof(Naipe)))
                    yield return new Carta {
                        Naipe = n,
                        Rank = r
                    };
        }
        public static IEnumerable<Carta> NovoDeckEmbaralhado()
        {
            return NovoDeck().Embaralhar();
        }
    }
}

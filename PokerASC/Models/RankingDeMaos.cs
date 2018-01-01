using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PokerASC.Helpers;

namespace PokerASC.Models
{
    public enum Score
    {
        [Description("Nenhum")]
        Nenhum,
        [Description("Um Par")]
        UmPar,
        [Description("Dois Pares")]
        DoisPares,
        [Description("Trinca")]
        Trinca,
        [Description("Sequência")]
        Sequencia,
        [Description("Flush")]
        Flush,
        [Description("Full House")]
        FullHouse,
        [Description("Quadra")]
        Quadra,
        [Description("Straight Flush")]
        StraightFlush,
        [Description("Royal Flush")]
        RoyalFlush,
    }

    public static class RankingDeMaos
    {
        #region "Cálculo de Regras"
        private static bool UmPar(IEnumerable<Carta> cartas)
        {
            return cartas.GroupBy(c => c.Rank).Any(c => c.Count() == 2);
        }
        private static bool DoisPares(IEnumerable<Carta> cartas)
        {
            return cartas.GroupBy(c => c.Rank).Where(c => c.Count() > 1).Count() > 1;
        }
        private static bool Trinca(IEnumerable<Carta> cartas)
        {
            return cartas.GroupBy(c => c.Rank).Any(c => c.Count() == 3);
        }
        private static bool Sequencia(IEnumerable<Carta> cartas)
        {
            var ordenado = cartas.ToList().OrderBy(c => c.Rank).ToList();

            //!!!Exceção O "A" pode vir no final caso seja na sequência [10 J Q K A]
            if (string.Join(string.Empty, ordenado.Select(c => c.Rank.GetDescription()).ToArray()) == "A10JQK")
                return true;

            for (int i = 0; i < ordenado.Count() - 2; i++)
            {
                if ((int)ordenado[i].Rank != (int)ordenado[i + 1].Rank + 1)
                    return false;
            }
            return true;
        }
        private static bool Flush(IEnumerable<Carta> cartas)
        {
            return cartas.GroupBy(c => c.Naipe).Any(c => c.Count() == 5);
        }
        private static bool FullHouse(IEnumerable<Carta> cartas)
        {
            return Trinca(cartas) && UmPar(cartas);
        }
        private static bool Quadra(IEnumerable<Carta> cartas)
        {
            return cartas.GroupBy(c => c.Rank).Any(c => c.Count() == 4);
        }
        private static bool StraightFlush(IEnumerable<Carta> cartas)
        {
            return Sequencia(cartas) && Flush(cartas);
        }
        private static bool RoyalFlush(IEnumerable<Carta> cartas)
        {
            return StraightFlush(cartas) && cartas.Any(c => c.Rank == Rank.A) && cartas.Any(c => c.Rank == Rank._10);
        }

        #endregion
        
        public static Score GetScore(this IEnumerable<Carta> cartas)
        {
            if (cartas.Count() == 0)
                return Score.Nenhum;
            else if (RoyalFlush(cartas))
                return Score.RoyalFlush;
            else if (StraightFlush(cartas))
                return Score.StraightFlush;
            else if (Quadra(cartas))
                return Score.Quadra;
            else if (FullHouse(cartas))
                return Score.FullHouse;
            else if (Flush(cartas))
                return Score.Flush;
            else if (Sequencia(cartas))
                return Score.Sequencia;
            else if (Trinca(cartas))
                return Score.Trinca;
            else if (DoisPares(cartas))
                return Score.DoisPares;
            else if (UmPar(cartas))
                return Score.UmPar;
            else
                return Score.Nenhum;
        }
    }
}
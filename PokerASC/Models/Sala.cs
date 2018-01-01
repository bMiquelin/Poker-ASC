using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PokerASC.Helpers;

namespace PokerASC.Models
{
    public class Sala
    {
        public Sala()
        {
            if (Jogadores == null) Jogadores = new List<Jogador>();
        }

        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public virtual List<Jogador> Jogadores { get; set; }
        public void Reset()
        {
            //Tira as cartas dos jogadores
            foreach(Jogador j in Jogadores)
                j.CartasNaMao = new List<Carta>();

            //Gera um novo deck p/ a sala
            var deck = Carta.NovoDeckEmbaralhado().ToList();

            //Cada jogador recebe 5 cartas
            foreach(Jogador jogador in Jogadores)
            {
                jogador.CartasNaMao = deck.Take(5).ToList();
                foreach(Carta c in jogador.CartasNaMao)
                {
                    deck.Remove(c);
                    c.Jogador = jogador;
                }
            }
        }

        public List<Carta> GeraDeck()
        {
            //Cria um novo deck embraralhado
            var deck = Carta.NovoDeckEmbaralhado().ToList();
            //Verifica quais cartas estão nas mãos dos jogadores
            var cartasComJogadores = Jogadores.SelectMany(j => j.CartasNaMao);
            //Retira as cartas que forem do mesmo naipe/rank do deck.
            return deck.Where(carta => !cartasComJogadores.Any(cJogador => Carta.Equals(cJogador, carta))).ToList();
        }

        public Jogador GetVencedor()
        {
            if (Jogadores == null || Jogadores.Count() == 0) throw new IndexOutOfRangeException("Não há nenhum jogador");

            //Crítério de Vitória: Jogador com a melhor mão de acordo com o ranking de mãos
            var maiorScore = Jogadores.Max(p => p.GetScore());
            var vencedores = Jogadores.Where(p => p.lastScore == maiorScore);
            if (vencedores.Count() == 1) return vencedores.First();

            //Empate 1 - Jogador com a maior carta. Ex: [2♦, 4♣, 6♣, 8♠, 2♥] = 8♠ = 8
            var maiorCarta = vencedores.Max(v => v.CartasNaMao.Max(c => (int)c.Rank));
            vencedores = vencedores.Where(v => v.CartasNaMao.Max(c => (int)c.Rank) == maiorCarta);
            if (vencedores.Count() == 1) return vencedores.First();

            //Empate 2 - Jogador com o maior nº na soma do "rank". Ex: [2♦, 4♣, 6♣, 8♠, 2♥] = 2+4+6+8+2 = 22
            var maiorSomaDeRank = vencedores.Max(v => v.CartasNaMao.Sum(c => (int)c.Rank));
            vencedores = vencedores.Where(v => v.CartasNaMao.Sum(c => (int)c.Rank) == maiorSomaDeRank);
            if (vencedores.Count() == 1) return vencedores.First();

            //Empate 3 - Aleatório (Muito raro)
            return vencedores.Embaralhar().First();
        }
    }
}
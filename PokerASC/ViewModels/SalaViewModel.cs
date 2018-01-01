using PokerASC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerASC.ViewModels
{
    public class SalaViewModel
    {
        public Sala Sala { get; set; }
        public int idJogador { get; set; }
        public int Capacidade { get; set; }
    }
}
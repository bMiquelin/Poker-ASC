using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerASC.Models
{
    public static class Helper
    {
        public static string ObterPrimeiroNome(this string nome)
        {
            return nome.Split(' ')[0];
        }
    }
}
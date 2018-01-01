using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PokerASC.Models;
using PokerASC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace PokerASC.Controllers
{
    [Authorize]
    public class SalaController : Controller
    {
        private const int CAPACIDADE_SALA = 7;
        private ApplicationDbContext db;

        public SalaController(ApplicationDbContext _db)
        {
            db = _db;
        }
        
        public ActionResult Reset(int id)
        {
            var sala = db.Sala.Single(g => g.Id == id);
            bool jogadorEstaNaPartida = sala.Jogadores.Any(j => j.Usuario.Id == User.Identity.GetUserId());

            if (!jogadorEstaNaPartida) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Jogador não está na partida");

            db.Carta.RemoveRange(sala.Jogadores.SelectMany(j => j.CartasNaMao));
            sala.Reset();            
            db.SaveChanges();
            return RedirectToAction("Join", new { id });
        }

        public ActionResult Vencedor(int id)
        {
            Sala sala = db.Sala.SingleOrDefault(s => s.Id == id);
            var vencedor = sala.GetVencedor();
            return View(vencedor);
        }

        public ActionResult Sair(int id, bool vencedor = false)
        {
            Sala sala = db.Sala.SingleOrDefault(s => s.Id == id);
            if(sala != null && sala.Jogadores.Any(j => j.Usuario.Id == User.Identity.GetUserId()))
            {
                sala.Jogadores.RemoveAll(j => j.Usuario.Id == User.Identity.GetUserId());
                db.SaveChanges();
                if(sala.Jogadores.Count() == 0)
                {
                    sala.Ativo = false;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Join(int id)
        {
            Sala sala;

            if (id != 0)
            {
                sala = db.Sala.SingleOrDefault(s => s.Ativo && s.Id == id);
                if (sala == null) return RedirectToAction("Index", "Home"); //Sala Inexistente (Bad Argument)
            }
            else
            {
                sala = new Sala {
                    Nome = $"Nova Sala {DateTime.Now.ToShortTimeString()}",
                    Ativo = true
                };
            }

            Jogador jogador = sala.Jogadores.SingleOrDefault(p => p.Usuario.Id == User.Identity.GetUserId());
            if (jogador == null)
            {
                //O jogador não está na sala. Entrando agora.
                if(sala.Jogadores.Count >= CAPACIDADE_SALA) return RedirectToAction("Index", "Home");

                var usuarioLogado = new UserManager<Usuario>(new UserStore<Usuario>(db)).FindById(User.Identity.GetUserId());
                jogador = new Jogador
                {
                    Usuario = usuarioLogado
                };
                sala.Jogadores.Add(jogador);
            }

            if (sala.Id == 0)
            {
                //Se a sala for nova, gera carta dos jogadores.
                sala.Reset();
                db.Sala.Add(sala);
            }
            db.SaveChanges();
            return View(new SalaViewModel {Sala = sala, idJogador = jogador.Id, Capacidade = CAPACIDADE_SALA });
        }
    }
}
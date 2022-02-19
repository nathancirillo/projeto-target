using System;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using MeuProjeto.Models;
using System.Text;
using System.Collections.Generic;
using PagedList;

namespace MeuProjeto.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index(int? pagina)
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {
                var users = session.Query<TUsers>().ToList();
                ViewBag.Role = RetornaProfissao(string.Empty);
                int pageSize = 4;
                int pageNumber = (pagina ?? 1);
                return View(users.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Editar(int userID)
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {             
                var user = session.Get<TUsers>(userID);
                ViewBag.Role = RetornaProfissao(user.Role);
                return View(user); 
            }
        }

        private List<SelectListItem> RetornaProfissao(string selectedRole)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Professor", Value = "Professor", Selected = "Professor".Equals(selectedRole)});
            items.Add(new SelectListItem { Text = "Pintor", Value = "Pintor", Selected = "Pintor".Equals(selectedRole) });
            items.Add(new SelectListItem { Text = "Cantor", Value = "Cantor", Selected = "Cantor".Equals(selectedRole) });
            items.Add(new SelectListItem { Text = "Doutor", Value = "Doutor", Selected = "Doutor".Equals(selectedRole) });
            return items;
        }


        [HttpPost]
        public ActionResult Editar(TUsers model)
        {
            try
            {
                ViewBag.Role = RetornaProfissao(string.Empty);
                if (ValidarCampos(model))
                { 
                    using (ISession session = NHibertnateSession.OpenSession())
                    {
                        var user = session.Get<TUsers>(model.Id);
                    
                        user.UserName = model.UserName;
                        user.FullName = model.FullName;
                        user.Date = model.Date;
                        user.Active = model.Active;
                        user.Country = model.Country;
                        user.Role = model.Role;

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(user);
                            transaction.Commit();
                        }
                    }
                    TempData["Alerta"] = "Dados alterados com sucesso!"; 
                    return RedirectToAction("Index");
                }
                TempData["Alerta"] = "Preencha os campos corretamente!"; 
                return View(); 
            }
            catch(Exception ex)
            {
                TempData["Alerta"] = "Ocorreu um erro na edição do dado!";
                return View(); 
            }
        }

        private bool ValidarCampos(TUsers user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                return false;
            if (string.IsNullOrWhiteSpace(user.FullName))
                return false;
            if (user.Date == null)
                return false;
            if (string.IsNullOrWhiteSpace(user.Country))
                return false;
            if (string.IsNullOrWhiteSpace(user.Role))
                return false;

            return true; 
        }

        public ActionResult Cancelar()
        {
            return RedirectToAction("Index");
        }
        
        
        public ActionResult Excluir(int userID)
        {
            try
            {
                using (ISession session = NHibertnateSession.OpenSession())
                {
                    var user = session.Get<TUsers>(userID);
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(user);
                        transaction.Commit();
                        TempData["Alerta"] = string.Format("Usuario {0} excluido com sucesso!", user.UserName); 
                    }
                }                
            }
            catch (Exception ex)
            {
                TempData["Alerta"] = "Ocorreu um erro durante a tentativa de excluir o usuario!";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Cadastrar()
        {
            ViewBag.Role = RetornaProfissao(string.Empty);
            return View(); 
        }

        [HttpPost]
        public ActionResult Cadastrar(TUsers user)
        {            
            try
            {
                ViewBag.Role = RetornaProfissao(string.Empty);
                if (ValidarCampos(user))
                { 
                    using (ISession session = NHibertnateSession.OpenSession())
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(user);
                            transaction.Commit(); 
                        }
                    }
                    TempData["Alerta"] = "Usuario cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }

                TempData["Alerta"] = "Preencha todos os campos corretamente!"; 
                return View();
            }
            catch (Exception ex)
            {
                TempData["Alerta"] = "Ocorreu um erro ao tentar cadastrar o usuário!";
                return View(); 
            }
        }

        public ActionResult Buscar(string username, string role)
        {            
            using (ISession session = NHibertnateSession.OpenSession())
            {
                var user = session.Query<TUsers>().ToList();

                if(!string.IsNullOrWhiteSpace(username))
                    user = (List<TUsers>)user.Where(x => x.UserName.ToLower() == username.ToLower()).ToList();

                if (!string.IsNullOrWhiteSpace(role))
                    user = (List<TUsers>)user.Where(x => x.Role.ToLower() == role.ToLower()).ToList();

                int pageSize = 4;
                int pageNumber = 1;

                return PartialView("_busca", user.ToPagedList(pageNumber, pageSize));                    
            }           
        }

    }
}
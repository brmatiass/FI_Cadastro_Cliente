using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using System.Reflection;
using System.Net.Sockets;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CarregarIdClient(long IdClient)
        {
            List<BeneficiariosModel> listaBeneficiarios = (List<BeneficiariosModel>)Session["Beneficiarios"];
            foreach (var beneficiario in listaBeneficiarios)
            {
                beneficiario.IdClient = IdClient;
            }

            return RedirectToAction("Incluir");
        }

        public ActionResult Incluir(BeneficiariosModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            var validaCPF = bo.ValidarCPF(model.CPF);

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            else if (!validaCPF)
            {
                Response.StatusCode = 400;
                return Json("CPF Invalido");
            }

            else
            {
                model.Id = bo.Incluir(new Beneficiarios()
                {
                    Nome = model.Nome,
                    CPF = model.CPF
                });
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long IdClient)
        {
            List<BeneficiariosModel> models = Session["Beneficiarios"] as List<BeneficiariosModel>;

            if (models == null)
            {
                models = new List<BeneficiariosModel>();
            }

            BoBeneficiario bo = new BoBeneficiario();
            List<Beneficiarios> beneficiario = bo.Listar(IdClient);

            Models.BeneficiariosModel model = null;

            if (beneficiario != null)
            {
                foreach (var b in beneficiario)
                {
                    model = new BeneficiariosModel()
                    {
                        Id = b.Id,
                        Nome = b.Nome,
                        CPF = Convert.ToUInt64(b.CPF).ToString(@"000\.000\.000\-00"),
                        IdClient = b.IdClient
                    };
                    models.Add(model);
                    models = models.GroupBy(mod => mod.CPF).Select(g => g.First()).ToList();

                }
            }

            Session["Beneficiarios"] = models;
            return Json(new { models = models }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Armazenar(BeneficiariosModel model)
        {
            List<BeneficiariosModel> beneficiarios = Session["Beneficiarios"] as List<BeneficiariosModel>;

            if (beneficiarios == null)
            {
                beneficiarios = new List<BeneficiariosModel>();
            }

            BoBeneficiario bo = new BoBeneficiario();

            var validaCPF = bo.ValidarCPF(model.CPF);

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            else if (!validaCPF)
            {
                Response.StatusCode = 400;
                return Json("CPF Invalido");
            }

            else
            {
                model.Id = bo.Incluir(new Beneficiarios()
                {
                    Nome = model.Nome,
                    CPF = model.CPF
                });
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public ActionResult RemoverBeneficiario(string CPF)
        {
            List<BeneficiariosModel> beneficiarios = Session["Beneficiarios"] as List<BeneficiariosModel>;

            if (beneficiarios == null)
            {
                beneficiarios = new List<BeneficiariosModel>();
            }

            var beneficiarioParaRemover = beneficiarios.Find(be => be.CPF == CPF);
            if (!(beneficiarioParaRemover == null))
            {
                beneficiarios.Remove(beneficiarioParaRemover);
            }

            Session["Beneficiarios"] = beneficiarios;
            return Json(new { beneficiarios = beneficiarios }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excluir(long IdClient)
        {
            BoBeneficiario bo = new BoBeneficiario();

            List<Beneficiarios> beneficiariosBD = new List<Beneficiarios>();

            List<BeneficiariosModel> beneficiarios = Session["Beneficiarios"] as List<BeneficiariosModel>;

            foreach (var beneficiario in beneficiarios)
            {
                if (beneficiario.Id == 0)
                {
                    beneficiario.Id = bo.Incluir(new Beneficiarios()
                    {
                        Nome = beneficiario.Nome,
                        CPF = new string(beneficiario.CPF.Where(char.IsDigit).ToArray()),
                        IdClient = beneficiario.IdClient
                    });
                }
                else
                {
                    bo.Alterar(new Beneficiarios()
                    {
                        Nome = beneficiario.Nome,
                        CPF = new string(beneficiario.CPF.Where(char.IsDigit).ToArray()),
                        Id = beneficiario.Id
                    });
                }

            }

            beneficiariosBD = bo.Listar(IdClient);

            foreach (var objeto in beneficiariosBD)
            {
                if (!beneficiarios.Any(o => o.Id == objeto.Id))
                {
                    bo.Excluir(objeto.Id);
                }
            }
            beneficiariosBD = bo.Listar(IdClient);
            Session["Beneficiarios"] = beneficiariosBD;
            return Json(new { beneficiarios = beneficiarios }, JsonRequestBehavior.AllowGet);
        }

        public bool VerificarExistenciaCPF(string CPF, List<BeneficiariosModel> listaBeneficiarios)
        {
            return listaBeneficiarios.Any(ben => ben.CPF == CPF);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApp_Desafio_FrontEnd.ApiClients.Desafio_API;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        public ChamadosController(IHostingEnvironment hostEnvironment, IConfiguration configuration)
        {
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        private ChamadosApiClient CriarChamadosApiClient()
        {
            return new ChamadosApiClient(_configuration["DesafioApi:BaseUrl"], _configuration["DesafioApi:TokenAutenticacao"]);
        }

        private DepartamentosApiClient CriarDepartamentosApiClient()
        {
            return new DepartamentosApiClient(_configuration["DesafioApi:BaseUrl"], _configuration["DesafioApi:TokenAutenticacao"]);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public IActionResult Listar()
        {
            // Busca de dados está na Action Datatable()
            return View();
        }

        [HttpGet]
        public IActionResult Datatable()
        {
            try
            {
                var chamadosApiClient = CriarChamadosApiClient();
                var lstChamados = chamadosApiClient.ChamadosListar();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    length = lstChamados.Count,
                    data = lstChamados
                };

                return Ok(dataTableVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult SolicitantesAutocomplete()
        {
            try
            {
                var chamadosApiClient = CriarChamadosApiClient();
                var lstChamados = chamadosApiClient.ChamadosListar();

                var lstSolicitantes = lstChamados
                    .Select(c => c.Solicitante)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .OrderBy(s => s)
                    .ToList();

                return Ok(lstSolicitantes);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var chamadoVM = new ChamadoViewModel()
            {
                DataAbertura = DateTime.Now
            };
            ViewData["Title"] = "Cadastrar Novo Chamado";

            try
            {
                var departamentosApiClient = CriarDepartamentosApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View("Cadastrar", chamadoVM);
        }

        [HttpPost]
        public IActionResult Cadastrar(ChamadoViewModel chamadoVM)
        {
            if (!ModelState.IsValid)
            {
                var mensagem = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                return BadRequest(new ResponseViewModel(mensagem ?? "Dados inválidos.", AlertTypes.warning));
            }

            try
            {
                var chamadosApiClient = CriarChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoGravar(chamadoVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado gravado com sucesso!",
                                AlertTypes.success,
                                this.RouteData.Values["controller"].ToString(),
                                nameof(this.Listar)));
                else
                    throw new ApplicationException($"Falha ao gravar o Chamado.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Cadastrar Novo Chamado";

            try
            {
                var chamadosApiClient = CriarChamadosApiClient();
                var chamadoVM = chamadosApiClient.ChamadoObter(id);

                var departamentosApiClient = CriarDepartamentosApiClient();
                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();

                return View("Cadastrar", chamadoVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpDelete]
        public IActionResult Excluir([FromRoute] int id)
        {
            try
            {
                var chamadosApiClient = CriarChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Chamados",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Chamado {id}.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Report()
        {
            string mimeType = string.Empty;
            int extension = 1;
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptChamados.rdlc");
            //
            // ... parameters
            //
            LocalReport localReport = new LocalReport(path);

            // Carrega os dados que serão apresentados no relatório
            var chamadosApiClient = CriarChamadosApiClient();
            var lstChamados = chamadosApiClient.ChamadosListar();

            localReport.AddDataSource("dsChamados", lstChamados);

            // Renderiza o relatório em PDF
            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            //return File(reportResult.MainStream, "application/pdf");
            return File(reportResult.MainStream, "application/octet-stream", "rptChamados.pdf");
        }

    }
}

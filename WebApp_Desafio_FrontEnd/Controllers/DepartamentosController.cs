using Microsoft.AspNetCore.Mvc;
using System;
using WebApp_Desafio_FrontEnd.ApiClients.Desafio_API;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class DepartamentosController : Controller
    {
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
                var departamentosApiClient = new DepartamentosApiClient();
                var lstDepartamentos = departamentosApiClient.DepartamentosListar();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    length = lstDepartamentos.Count,
                    data = lstDepartamentos
                };

                return Ok(dataTableVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var departamentoVM = new DepartamentoViewModel();
            ViewData["Title"] = "Cadastrar Novo Departamento";

            return View("Cadastrar", departamentoVM);
        }

        [HttpPost]
        public IActionResult Cadastrar(DepartamentoViewModel departamentoVM)
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentoGravar(departamentoVM);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Departamento gravado com sucesso!",
                                AlertTypes.success,
                                this.RouteData.Values["controller"].ToString(),
                                nameof(this.Listar)));
                else
                    throw new ApplicationException($"Falha ao gravar o Departamento.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Cadastrar Novo Departamento";

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var departamentoVM = departamentosApiClient.DepartamentoObter(id);

                return View("Cadastrar", departamentoVM);
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
                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentoExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Departamento {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Departamentos",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Departamento {id}.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }
    }
}

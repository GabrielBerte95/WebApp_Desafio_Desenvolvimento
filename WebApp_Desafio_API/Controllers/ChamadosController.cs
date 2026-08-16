using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp_Desafio_API.Extensions;
using WebApp_Desafio_API.ViewModels;
using WebApp_Desafio_BackEnd.Business;

namespace WebApp_Desafio_API.Controllers
{
    /// <summary>
    /// ChamadosController
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : Controller
    {
        private readonly ChamadosBLL bll;

        public ChamadosController(ChamadosBLL bll)
        {
            this.bll = bll;
        }

        /// <summary>
        /// Lista todos os chamados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ChamadoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Listar")]
        public IActionResult Listar()
        {
            try
            {
                var _lst = this.bll.ListarChamados();

                var lst = from chamado in _lst
                          select new ChamadoResponse()
                          {
                              id = chamado.ID,
                              assunto = chamado.Assunto,
                              solicitante = chamado.Solicitante,
                              idDepartamento = chamado.IdDepartamento,
                              departamento = chamado.Departamento,
                              dataAbertura = chamado.DataAbertura
                          };

                return Ok(lst);
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }

        /// <summary>
        /// Obtém dados de um chamado específico
        /// </summary>
        /// <param name="idChamado">O ID do chamado a ser obtido</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ChamadoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Obter")]
        public IActionResult Obter([FromQuery] int idChamado)
        {
            try
            {
                var _chamado = this.bll.ObterChamado(idChamado);

                var chamado = new ChamadoResponse()
                              {
                                  id = _chamado.ID,
                                  assunto = _chamado.Assunto,
                                  solicitante = _chamado.Solicitante,
                                  idDepartamento = _chamado.IdDepartamento,
                                  departamento = _chamado.Departamento,
                                  dataAbertura = _chamado.DataAbertura
                              };

                return Ok(chamado);
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }

        /// <summary>
        /// Grava os dados de um chamado
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Gravar")]
        public IActionResult Gravar([FromBody] ChamadoRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentException("Request não informado.");

                var resultado = this.bll.GravarChamado(request.id,
                                                       request.assunto,
                                                       request.solicitante,
                                                       request.idDepartamento,
                                                       request.dataAbertura);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }
        
        /// <summary>
        /// Exclui um chamado específico
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("Excluir")]
        public IActionResult Excluir([FromQuery] int idChamado)
        {
            try
            {
                var resultado = this.bll.ExcluirChamado(idChamado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }
    }
}

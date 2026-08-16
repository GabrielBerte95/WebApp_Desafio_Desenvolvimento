using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApp_Desafio_API.Extensions
{
    /// <summary>
    /// Extensões para tratamento padronizado de exceções nos controllers da API.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Trata uma exceção lançada durante o processamento de uma action e
        /// devolve o StatusCode correspondente com a mensagem da exceção:
        /// ArgumentException -> 400, ApplicationException -> 422, demais -> 500.
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ObjectResult ExceptionProcess(this ControllerBase controllerBase, Exception ex)
        {
            if (ex is ArgumentException)
                return controllerBase.StatusCode(400, ex.Message);

            if (ex is ApplicationException)
                return controllerBase.StatusCode(422, ex.Message);

            return controllerBase.StatusCode(500, ex.Message);
        }
    }
}

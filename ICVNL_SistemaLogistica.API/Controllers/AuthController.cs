using ICVNL_SistemaLogistica.API.Models;
using ICVNL_SistemaLogistica.API.Token;
using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ICVNL_SistemaLogistica.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var responseAPI = new RequestApi<TokenResponse>();
            if (login.Username == "")
            {
                return Unauthorized();
            }
            if (login.Password == "")
            {
                return Unauthorized();
            }
            var passEncrypt = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(login.Password));

            var dbResponseUserAPI = new Usuarios_API_BL().GetUserAPI_Dev(login.Username, passEncrypt);
            if (dbResponseUserAPI.ExecutionOK)
            {
                responseAPI.ExecutionOK = true;
                responseAPI.Data = new TokenResponse()
                {
                    access_token = new TokenGenerator().GenerateTokenJwt(login.Username)
                };
                responseAPI.Message = "Token obtenido";
                responseAPI.NumRows = 1;
                return Ok(responseAPI);
            }
            else
            {
                responseAPI.ExecutionOK = false;
                responseAPI.Data = new TokenResponse()
                {
                    access_token = ""
                };
                responseAPI.Message = "Usuario no autorizado o inexistente";
                return Ok(responseAPI);
            }
        }
    }
}

using ICVNL_SistemaLogistica.Web.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using System.Net.Http.Headers;

namespace ICVNL_SistemaLogistica.Procesos.Service
{
    public class ApiService
    {
        public static async Task<DBResponse<Token>> PostLoginObj<T>(ApiBaseLogin baseLogin)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var url = $"{baseLogin.UrlBase}{baseLogin.Prefijo}{baseLogin.Controlador}";

                var req = new HttpRequestMessage(HttpMethod.Post, url);

                req.Headers.Add("Accept", "application/x-www-form-urlencoded");
                // This is the important part:
                req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", baseLogin.UsuarioApi },
                    { "password", baseLogin.PasswordApi }
                }); 
                var cliente = new HttpClient();
                HttpResponseMessage resp = await cliente.SendAsync(req); 
                Token objRespuesta = new Token();
                objRespuesta = JsonConvert.DeserializeObject<Token>(resp.Content.ReadAsStringAsync().Result);
                if (objRespuesta.access_token != null)
                {
                    return new DBResponse<Token>
                    {
                        ExecutionOK = true,
                        Message = "",
                        Data = objRespuesta
                    };
                }
                else
                {
                    return new DBResponse<Token>
                    {
                        ExecutionOK = false,
                        Message = "",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new DBResponse<Token>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<DBResponseApi> ApiPostAlmacenesListadoRequest<T>(string urlBase, string prefix, string controller, string token, PostAlmacenesListado postObject)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var usuarioJson = JsonConvert.SerializeObject(postObject, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var contentString = String.Format("{0}", usuarioJson);

                var content = new StringContent(contentString, Encoding.UTF8, "application/json");

                //Envio de la solicitud al servicio
                var client = new HttpClient();
                TimeSpan time = TimeSpan.FromSeconds(180);
                client.Timeout = time;
                //client.BaseAddress = new Uri(urlBase);
                var url = "";

                url = $"{urlBase}{prefix}{controller}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Headers.Add("Authorization", "Bearer " + token);

                requestMessage.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Obtenemos la respuesta
                var answer = await response.Content.ReadAsStringAsync();
                //Evaluamos el resultado
                //En caso de no haber comunicacion
                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = answer
                    };
                }

                //Si hubo comunicación pasamos la respuesta a objeto
                ResponseListadoAlmacenes obj2 = JsonConvert.DeserializeObject<ResponseListadoAlmacenes>(answer);
                if (obj2.Acknowledge == 1)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = true,
                        Message = "",
                        Data = obj2
                    };
                }
                else
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = obj2.MessageText,
                        Data = null
                    };
                }

            }
            catch (TaskCanceledException exTask)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = exTask.Message
                };
            }
            catch (Exception ex)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }
        public static async Task<DBResponseApi> ApiPostNotasEntradasRequest<T>(string urlBase, string prefix, string controller, string token, PostNotaEntrada postObject)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var usuarioJson = JsonConvert.SerializeObject(postObject, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var contentString = String.Format("{0}", usuarioJson);

                var content = new StringContent(contentString, Encoding.UTF8, "application/json");

                //Envio de la solicitud al servicio
                var client = new HttpClient();
                TimeSpan time = TimeSpan.FromSeconds(180);
                client.Timeout = time;
                //client.BaseAddress = new Uri(urlBase);
                var url = "";

                url = $"{urlBase}{prefix}{controller}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Headers.Add("Authorization", "Bearer " + token);

                requestMessage.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Obtenemos la respuesta
                var answer = await response.Content.ReadAsStringAsync();
                //Evaluamos el resultado
                //En caso de no haber comunicacion
                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = answer
                    };
                }

                //Si hubo comunicación pasamos la respuesta a objeto
                ResponseNotasEntradas obj2 = JsonConvert.DeserializeObject<ResponseNotasEntradas>(answer);
                if (obj2.Acknowledge == 1)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = true,
                        Message = "",
                        Data = obj2
                    };
                }
                else
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = obj2.MessageText,
                        Data = null
                    };
                }

            }
            catch (TaskCanceledException exTask)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = exTask.Message
                };
            }
            catch (Exception ex)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<DBResponseApi> ApiPostOrdenesCompraRequest<T>(string urlBase, string prefix, string controller, string token, PostOrdenCompra postObject)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var usuarioJson = JsonConvert.SerializeObject(postObject, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var contentString = String.Format("{0}", usuarioJson);

                var content = new StringContent(contentString, Encoding.UTF8, "application/json");

                //Envio de la solicitud al servicio
                var client = new HttpClient();
                TimeSpan time = TimeSpan.FromSeconds(180);
                client.Timeout = time;
                //client.BaseAddress = new Uri(urlBase);
                var url = "";

                url = $"{urlBase}{prefix}{controller}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Headers.Add("Authorization", "Bearer " + token);

                requestMessage.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Obtenemos la respuesta
                var answer = await response.Content.ReadAsStringAsync();
                //Evaluamos el resultado
                //En caso de no haber comunicacion
                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = answer
                    };
                }

                //Si hubo comunicación pasamos la respuesta a objeto
                ResponseOrdenesCompra obj2 = JsonConvert.DeserializeObject<ResponseOrdenesCompra>(answer);
                if (obj2.Acknowledge == 1)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = true,
                        Message = "",
                        Data = obj2
                    };
                }
                else
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = obj2.MessageText,
                        Data = null
                    };
                }

            }
            catch (TaskCanceledException exTask)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = exTask.Message
                };
            }
            catch (Exception ex)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<DBResponseApi> ApiPostPolizasRequest<T>(string urlBase, string prefix, string controller, string token, PostPolizas postObject)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var usuarioJson = JsonConvert.SerializeObject(postObject, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var contentString = String.Format("{0}", usuarioJson);

                var content = new StringContent(contentString, Encoding.UTF8, "application/json");

                //Envio de la solicitud al servicio
                var client = new HttpClient();
                TimeSpan time = TimeSpan.FromSeconds(180);
                client.Timeout = time;
                //client.BaseAddress = new Uri(urlBase);
                var url = "";

                url = $"{urlBase}{prefix}{controller}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Headers.Add("Authorization", "Bearer " + token);

                requestMessage.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Obtenemos la respuesta
                var answer = await response.Content.ReadAsStringAsync();
                //Evaluamos el resultado
                //En caso de no haber comunicacion
                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = answer
                    };
                }

                //Si hubo comunicación pasamos la respuesta a objeto
                ResponsePolizas obj2 = JsonConvert.DeserializeObject<ResponsePolizas>(answer);
                if (obj2.Acknowledge == 1)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = true,
                        Message = "",
                        Data = obj2
                    };
                }
                else
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = obj2.MessageText,
                        Data = null
                    };
                }

            }
            catch (TaskCanceledException exTask)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = exTask.Message
                };
            }
            catch (Exception ex)
            {
                return new DBResponseApi
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }
    }
}

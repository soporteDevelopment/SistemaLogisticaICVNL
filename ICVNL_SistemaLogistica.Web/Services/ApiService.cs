using ICVNL_SistemaLogistica.Web.Entities.Services;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Services
{
    public class ApiService
    {
        public static async Task<DBResponseApi> PostLoginObj<T>(ApiBaseLogin baseLogin)
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
                    { "password",  Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(baseLogin.PasswordApi))}
                });

                var cliente = new HttpClient();
                HttpResponseMessage resp = await cliente.SendAsync(req);

                System.Web.Script.Serialization.JavaScriptSerializer serializador = new System.Web.Script.Serialization.JavaScriptSerializer();
                serializador.MaxJsonLength = 2147483647;
                Token objRespuesta = new Token();
                objRespuesta = serializador.Deserialize<Token>(resp.Content.ReadAsStringAsync().Result);
                if (objRespuesta.access_token != null)
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = true,
                        Message = "",
                        Data = objRespuesta
                    };
                }
                else
                {
                    return new DBResponseApi
                    {
                        ExecutionOK = false,
                        Message = "",
                        Data = null
                    };
                }

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
        public static async Task<DBResponseApi> ApiPostUsuarioRequest<T>(string urlBase, string prefix, string controller, string token, PostUsuario postObject)
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
                ResponseUsuario obj2 = JsonConvert.DeserializeObject<ResponseUsuario>(answer);
                if (obj2.acknowledge == 1)
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
                        Message = obj2.messageText,
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
        public static async Task<DBResponseApi> ApiPostCuentasContablesRequest<T>(string urlBase, string prefix, string controller, string token, PostCuenta postObject)
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
                ResponseCuenta obj2 = JsonConvert.DeserializeObject<ResponseCuenta>(answer);
                if (obj2.acknowledge == 1)
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
                        Message = obj2.messageText,
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
        public static async Task<DBResponseApi> ApiPostCentroCostosRequest<T>(string urlBase, string prefix, string controller, string token, PostCentroDeCosto postObject)
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
                ResponseCentroDeCosto obj2 = JsonConvert.DeserializeObject<ResponseCentroDeCosto>(answer);
                if (obj2.acknowledge == 1)
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
                        Message = obj2.messageText,
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

        public static async Task<DBResponseApi> ApiPostAlmacenRequest<T>(string urlBase, string prefix, string controller, string token, PostAlmacen postObject)
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

                var answe_ = answer;

                //Si hubo comunicación pasamos la respuesta a objeto
                ResponseAlmacen obj2 = JsonConvert.DeserializeObject<ResponseAlmacen>(answer);
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

        public static async Task<DBResponseApi> ApiPostArticuloRequest<T>(string urlBase, string prefix, string controller, string token, PostArticulo postObject)
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
                ResponseArticulo obj2 = JsonConvert.DeserializeObject<ResponseArticulo>(answer);
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
        public static async Task<DBResponseApi> ApiPostProveedorRequest<T>(string urlBase, string prefix, string controller, string token, PostProveedor postObject)
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
                ResponseProveedor obj2 = JsonConvert.DeserializeObject<ResponseProveedor>(answer);
                if (obj2.acknowledge == 1)
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
                        Message = obj2.messageText,
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

        public static async Task<DBResponseApi> ApiPostPermisosUsuarioRequest<T>(string urlBase, string prefix, string controller, string token, PostAcceso postObject)
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
                Entities.Services.Respuesta.Acceso obj2 = JsonConvert.DeserializeObject<Entities.Services.Respuesta.Acceso>(answer);
                if (obj2.acknowledge == 1)
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
                        Message = obj2.messageText,
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

        public static async Task<DBResponseApi> ApiPostNotasEntradasRequest<T>(string urlBase, string prefix, string controller, string token, PostNotaEntradaInd postObject)
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

    }
}
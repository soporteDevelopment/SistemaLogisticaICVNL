using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class AlgoritmoGeneracionPlacas_BL
    {

        public List<string> listadoPlacasGenerado = new List<string>();
        public int primerOrdenRecursivo = 1;
        public int primerOrdenRecursivoLetra = 1;

        public DBResponse<string> GetSeriePlaca(string NumeroPlaca, TiposPlacas TipoPlaca)
        {
            DBResponse<string> response = new DBResponse<string>();
            try
            {
                var serie = "";
                char[] splitPlaca = NumeroPlaca.ToCharArray();
                char[] splitOrdenSerie = TipoPlaca.OrdenSeriePlaca.ToCharArray();
                for (int i = 0; i < splitOrdenSerie.Length; i++)
                {
                    var valorPos = int.Parse(splitOrdenSerie[i].ToString()) - 1;
                    serie += splitPlaca[valorPos];
                }
                if (serie.Length > 0)
                {
                    response.Data = serie;
                    response.ExecutionOK = true;
                }
                else 
                {
                    response.Data = "";
                    response.ExecutionOK = false;
                    response.Message = "Serie no encontrada";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        public DBResponse<List<Placas_Generadas>> GeneraPlacasRangos(GenerarPlacasRangos GenerarPlacasRangos)
        {
            var response_ = new DBResponse<List<Placas_Generadas>>();
            var ListadoRangosInicial = new List<Placas_Orden>();
            var listadoRangoFinalOrden = new List<Placas_Orden>();
            var ListadoRangosInicialOrdenado = new List<Placas_Orden>();
            var ListadoRangosFinalOrdenado = new List<Placas_Orden>();
            var diccionarioLetras = ListadoLetras();
            try
            {
                response_.Data = new List<Placas_Generadas>();
                response_.Data.Add(new Placas_Generadas()
                {
                    IdTipoPlaca = GenerarPlacasRangos.TipoPlaca.IdTipoPlaca,
                    NumeroPlaca = GenerarPlacasRangos.RangoInicial
                });

                char[] splitPlacaInicial = GenerarPlacasRangos.RangoInicial.ToCharArray();
                char[] splitOrden = GenerarPlacasRangos.TipoPlaca.OrdenPlaca.ToCharArray();
                for (int i = 0; i < splitPlacaInicial.Length; i++)
                {
                    ListadoRangosInicial.Add(new Placas_Orden()
                    {
                        Valor = splitPlacaInicial[i].ToString(),
                        Orden = int.Parse(splitOrden[i].ToString()),
                        OrdenOriginal = i + 1
                    });
                }

                char[] splitPlacaFinal = GenerarPlacasRangos.RangoFinal.ToCharArray();
                for (int i = splitPlacaFinal.Length - 1; i > 0; i--)
                {
                    listadoRangoFinalOrden.Add(new Placas_Orden()
                    {
                        Valor = splitPlacaFinal[i].ToString(),
                        Orden = int.Parse(splitOrden[i].ToString()),
                    });
                }

                ListadoRangosInicialOrdenado = ListadoRangosInicial.OrderBy(x => x.Orden).ToList();
                ListadoRangosFinalOrdenado = listadoRangoFinalOrden.OrderBy(x => x.Orden).ToList();
                listadoPlacasGenerado.Add(GenerarPlacasRangos.RangoInicial);

                var listPlacas = GeneraPlacaRecursivo(ListadoRangosInicialOrdenado, ListadoRangosFinalOrdenado, GenerarPlacasRangos.RangoFinal);
                response_.Data = new List<Placas_Generadas>();
                if (listPlacas.Count > 0)
                {
                    response_.ExecutionOK = true;
                    foreach (var item in listPlacas)
                    {
                        response_.Data.Add(new Placas_Generadas()
                        {
                            IdTipoPlaca = GenerarPlacasRangos.TipoPlaca.IdTipoPlaca,
                            NumeroPlaca = item
                        });
                    }
                }                

            }
            catch (Exception)
            {

                throw;

            }

            return response_;
        }

        public List<string> GeneraPlacaRecursivo(List<Placas_Orden> ListadoRangosInicialOrdenado, List<Placas_Orden> ListadoRangosFinalOrdenado, string RangoFinal)
        {
            var listPlacas = new List<string>();
            var PlacaGenerada = "";
            var diccionarioLetras = ListadoLetras();
            int[] ListNumerosOrdenesDesc = ListadoRangosInicialOrdenado.OrderByDescending(x => x.Orden).Select(s => s.Orden).ToArray();

            var objPrimerEnOrden = ListadoRangosInicialOrdenado.Where(x => x.Orden == primerOrdenRecursivo).FirstOrDefault();
            var objIndex = ListadoRangosInicialOrdenado.FindIndex(x => x.Orden == primerOrdenRecursivo);


            if (EsLetra(objPrimerEnOrden.Valor))
            {
                var posicionDiccionarInicial = diccionarioLetras.Where(x => x.Letra == objPrimerEnOrden.Valor).FirstOrDefault();
                var listadoDiccionario = diccionarioLetras.Where(x => x.Posicion >= posicionDiccionarInicial.Posicion).ToList();


                for (int i = 0; i < listadoDiccionario.Count; i++)
                {
                    if (listadoDiccionario[i].Posicion == 23)
                    {
                        ListadoRangosInicialOrdenado[objIndex].Valor = listadoDiccionario[i].Letra;
                        string[] ListGenerada = ListadoRangosInicialOrdenado.OrderBy(x => x.OrdenOriginal).Select(s => s.Valor).ToArray();
                        PlacaGenerada = string.Join("", ListGenerada);
                        if (!listadoPlacasGenerado.Any(x => x == PlacaGenerada))
                        {
                            listadoPlacasGenerado.Add(PlacaGenerada);
                        }
                        if (PlacaGenerada == RangoFinal)
                        {
                            break;
                        }
                        else
                        {
                            if (ListadoRangosFinalOrdenado.Any(x => x.Valor == objPrimerEnOrden.Valor && x.Orden == 1))
                            {
                                for (int y = 0; y < ListadoRangosFinalOrdenado.Count; y++)
                                {
                                    if (EsLetra(ListadoRangosFinalOrdenado[y].Valor) && EsLetra(ListadoRangosInicialOrdenado[y].Valor))
                                    {
                                        if (ListadoRangosFinalOrdenado[y].Valor == ListadoRangosInicialOrdenado[y].Valor)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        primerOrdenRecursivo = ListadoRangosInicialOrdenado[y].Orden + 1;
                                        GeneraPlacaRecursivo(ListadoRangosInicialOrdenado, ListadoRangosFinalOrdenado, RangoFinal);
                                    }
                                }
                            }
                        }
                        ListadoRangosInicialOrdenado[objIndex].Valor = "A";

                        primerOrdenRecursivo++;
                        var objPrimerEnOrdenNext = ListadoRangosInicialOrdenado.Where(x => x.Orden == primerOrdenRecursivo).FirstOrDefault();
                        if (objPrimerEnOrdenNext != null)
                        {
                            var objIndexNext = ListadoRangosInicialOrdenado.FindIndex(x => x.Orden == primerOrdenRecursivo);
                            var posicionDiccionarNext = diccionarioLetras.Where(x => x.Letra == objPrimerEnOrdenNext.Valor).FirstOrDefault();
                            if (posicionDiccionarNext != null)
                            {
                                var listadoDiccionarioNext = diccionarioLetras.Where(x => x.Posicion == (posicionDiccionarNext.Posicion + 1)).FirstOrDefault();
                                if (listadoDiccionarioNext != null)
                                {
                                    ListadoRangosInicialOrdenado[objIndexNext].Valor = listadoDiccionarioNext.Letra;
                                    if (primerOrdenRecursivo >= 2)
                                    {
                                        primerOrdenRecursivo = 1;
                                    }
                                    else
                                    {
                                        primerOrdenRecursivo--;
                                    }
                                }
                            }
                        }
                        GeneraPlacaRecursivo(ListadoRangosInicialOrdenado, ListadoRangosFinalOrdenado, RangoFinal);
                        break;
                    }
                    else
                    {
                        ListadoRangosInicialOrdenado[objIndex].Valor = listadoDiccionario[i].Letra;
                        string[] ListGenerada = ListadoRangosInicialOrdenado.OrderBy(x => x.OrdenOriginal).Select(s => s.Valor).ToArray();
                        PlacaGenerada = string.Join("", ListGenerada);
                        if (!listadoPlacasGenerado.Any(x => x == PlacaGenerada))
                        {
                            listadoPlacasGenerado.Add(PlacaGenerada);
                        }
                        if (PlacaGenerada == RangoFinal)
                        {
                            break;
                        }
                        else
                        {
                            if (ListadoRangosFinalOrdenado.Any(x => x.Valor == objPrimerEnOrden.Valor && x.Orden == 1))
                            {
                                for (int y = 0; y < ListadoRangosFinalOrdenado.Count; y++)
                                {
                                    if (EsLetra(ListadoRangosFinalOrdenado[y].Valor) && EsLetra(ListadoRangosInicialOrdenado[y].Valor))
                                    {
                                        if (ListadoRangosFinalOrdenado[y].Valor == ListadoRangosInicialOrdenado[y].Valor)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        primerOrdenRecursivo = ListadoRangosInicialOrdenado[y].Orden + 1;
                                        GeneraPlacaRecursivo(ListadoRangosInicialOrdenado, ListadoRangosFinalOrdenado, RangoFinal);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var rangoInicia = ListadoRangosInicialOrdenado.Where(x => x.Orden == primerOrdenRecursivo).FirstOrDefault();
                var indexRangoInicia = ListadoRangosInicialOrdenado.FindIndex(x => x.Orden == primerOrdenRecursivo);
                var numero = 0;
                if (int.TryParse(rangoInicia.Valor, out numero))
                {
                    for (int xx = indexRangoInicia; xx < 10; xx++)
                    {
                        string[] ValidaPlacaList = ListadoRangosInicialOrdenado.OrderBy(x => x.OrdenOriginal).Select(s => s.Valor).ToArray();
                        PlacaGenerada = string.Join("", ValidaPlacaList);
                        if (PlacaGenerada == RangoFinal)
                        {
                            break;
                        }
                        var isNumberVal = 0;
                        if (int.TryParse(ListadoRangosInicialOrdenado[indexRangoInicia].Valor, out isNumberVal))
                        {
                            if (isNumberVal == 9)
                            {
                                var ResponseNueva = EsNueve(ListadoRangosInicialOrdenado, indexRangoInicia);
                                if (ResponseNueva.ExecutionOK)
                                {
                                    List<Placas_Orden> nuevoListado = new List<Placas_Orden>();
                                    foreach (var item in ResponseNueva.Data.Where(x => x.EsNueve == true))
                                    {
                                        if (!EsLetra(ListadoRangosInicialOrdenado[item.Posicion + 1].Valor))
                                        {
                                            ListadoRangosInicialOrdenado[item.Posicion].Valor = "0";
                                        }
                                    }


                                    var nexValueIncrement = ResponseNueva.Data.Where(x => x.EsNueve == false).FirstOrDefault();
                                    if (nexValueIncrement == null)
                                    {
                                        var ultimaPos = ResponseNueva.Data.LastOrDefault().Posicion + 1;
                                        if (EsLetra(ListadoRangosInicialOrdenado[ultimaPos].Valor))
                                        {
                                            var posicionDiccionarNext = diccionarioLetras.Where(x => x.Letra == ListadoRangosInicialOrdenado[ultimaPos].Valor).FirstOrDefault();
                                            if (posicionDiccionarNext != null)
                                            {
                                                var listadoDiccionarioNext = diccionarioLetras.Where(x => x.Posicion == (posicionDiccionarNext.Posicion + 1)).FirstOrDefault();
                                                if (listadoDiccionarioNext != null)
                                                {
                                                    ListadoRangosInicialOrdenado[ultimaPos].Valor = listadoDiccionarioNext.Letra;
                                                    for (int postition = ultimaPos - 1; postition > 0; postition--)
                                                    {
                                                        ListadoRangosInicialOrdenado[postition].Valor = postition == 0 ? "1" : "0";
                                                    }
                                                    GeneraPlacaRecursivo(ListadoRangosInicialOrdenado, ListadoRangosFinalOrdenado, RangoFinal);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var numeroSig = (int.Parse(ListadoRangosInicialOrdenado[nexValueIncrement.Posicion].Valor) + 1);
                                        ListadoRangosInicialOrdenado[nexValueIncrement.Posicion].Valor = numeroSig.ToString();
                                    }

                                    string[] ListGenerada = ListadoRangosInicialOrdenado.OrderBy(x => x.OrdenOriginal).Select(s => s.Valor).ToArray();
                                    PlacaGenerada = string.Join("", ListGenerada);
                                    if (!listadoPlacasGenerado.Any(x => x == PlacaGenerada))
                                    {
                                        listadoPlacasGenerado.Add(PlacaGenerada);
                                    }
                                    if (PlacaGenerada == RangoFinal)
                                    {
                                        break;
                                    }
                                    GeneraPlacaRecursivo(ListadoRangosInicialOrdenado, ListadoRangosFinalOrdenado, RangoFinal);
                                }
                            }
                            else
                            {
                                var numeroSig = (int.Parse(ListadoRangosInicialOrdenado[indexRangoInicia].Valor) + 1);
                                ListadoRangosInicialOrdenado[indexRangoInicia].Valor = numeroSig.ToString();
                            }

                            string[] ListGenerada_ = ListadoRangosInicialOrdenado.OrderBy(x => x.OrdenOriginal).Select(s => s.Valor).ToArray();
                            PlacaGenerada = string.Join("", ListGenerada_);
                            if (!listadoPlacasGenerado.Any(x => x == PlacaGenerada))
                            {
                                listadoPlacasGenerado.Add(PlacaGenerada);
                            }
                            if (PlacaGenerada == RangoFinal)
                            {
                                break;
                            }

                        }
                    }
                }
            }

            if (listadoPlacasGenerado.Count > 0)
            {
                listPlacas = listadoPlacasGenerado;
            }

            return listPlacas;
        }

        public static DBResponse<List<ValoresConsecutivo>> EsNueve(List<Placas_Orden> ListadoRangosInicialOrdenado, int empezar)
        {
            var dbResponse = new DBResponse<List<ValoresConsecutivo>>();
            dbResponse.Data = new List<ValoresConsecutivo>();
            for (int i = 0; i < ListadoRangosInicialOrdenado.Count; i++)
            {
                var isNumber = 0;
                if (int.TryParse(ListadoRangosInicialOrdenado[i].Valor, out isNumber))
                {
                    if (isNumber == 9)
                    {
                        dbResponse.Data.Add(new ValoresConsecutivo()
                        {
                            Posicion = i,
                            EsNueve = true
                        });
                        dbResponse.ExecutionOK = true;
                    }
                    else
                    {
                        dbResponse.Data.Add(new ValoresConsecutivo()
                        {
                            Posicion = i,
                            EsNueve = false
                        });
                        dbResponse.ExecutionOK = true;
                    }
                }
            }
            return dbResponse;
        }

        public DBResponse<List<ErroresGeneradosPlacas>> Validaciones(GenerarPlacasRangos GenerarPlacasRangos)
        {
            var response_ = new DBResponse<List<ErroresGeneradosPlacas>>();
            var listadoRangoInicialOrden = new List<Placas_Orden>();
            var listadoRangoFinalOrden = new List<Placas_Orden>();
            var listadoMascaraOrden = new List<Placas_Orden>();
            var diccionarioLetras = ListadoLetras();
            response_.Data = new List<ErroresGeneradosPlacas>();
            try
            {
                if (GenerarPlacasRangos.RangoInicial.Length != GenerarPlacasRangos.RangoFinal.Length)
                {
                    response_.Data.Add(new ErroresGeneradosPlacas()
                    {
                        ConsecutivoError = response_.Data.Count + 1,
                        Error = "Las Longitudes del Rango Inicial y Rango Final no son iguales",
                        EsEvento = 0,
                        Prioridad = 0
                    });
                }
                else
                {
                    if (GenerarPlacasRangos.RangoInicial.Length != GenerarPlacasRangos.TipoPlaca.MascaraPlaca.Length)
                    {
                        response_.Data.Add(new ErroresGeneradosPlacas()
                        {
                            ConsecutivoError = response_.Data.Count + 1,
                            Error = "Las Longitud del Rango Inicial y la Longitud de la Mascara del Tipo de Placa no sin iguales",
                            EsEvento = 0,
                            Prioridad = 0
                        });
                    }
                    else if (GenerarPlacasRangos.RangoFinal.Length != GenerarPlacasRangos.TipoPlaca.MascaraPlaca.Length)
                    {
                        response_.Data.Add(new ErroresGeneradosPlacas()
                        {
                            ConsecutivoError = response_.Data.Count + 1,
                            Error = "Las Longitud del Rango Final y la Longitud de la Mascara del Tipo de Placa no sin iguales",
                            EsEvento = 0,
                            Prioridad = 0
                        });
                    }
                    else if (GenerarPlacasRangos.RangoFinal.Length != GenerarPlacasRangos.TipoPlaca.OrdenPlaca.Length)
                    {
                        response_.Data.Add(new ErroresGeneradosPlacas()
                        {
                            ConsecutivoError = response_.Data.Count + 1,
                            Error = "Las Longitud del Rango Final y la Longitud del Orden del Tipo de Placa no sin iguales",
                            EsEvento = 0,
                            Prioridad = 0
                        });
                    }
                    else if (GenerarPlacasRangos.RangoFinal.Length != GenerarPlacasRangos.TipoPlaca.OrdenPlaca.Length)
                    {
                        response_.Data.Add(new ErroresGeneradosPlacas()
                        {
                            ConsecutivoError = response_.Data.Count + 1,
                            Error = "Las Longitud del Rango Final y la Longitud del Orden del Tipo de Placa no sin iguales",
                            EsEvento = 0,
                            Prioridad = 0
                        });
                    }
                }

                var serieInicial = "";
                var serieFinal = "";

                char[] splitPlacaInicial = GenerarPlacasRangos.RangoInicial.ToCharArray();
                char[] splitOrden = GenerarPlacasRangos.TipoPlaca.OrdenPlaca.ToCharArray();
                char[] splitOrdenSerie = GenerarPlacasRangos.TipoPlaca.OrdenSeriePlaca.ToCharArray();
                for (int i = splitPlacaInicial.Length - 1; i > 0; i--)
                {
                    listadoRangoInicialOrden.Add(new Placas_Orden()
                    {
                        Valor = splitPlacaInicial[i].ToString(),
                        Orden = int.Parse(splitOrden[i].ToString()),
                    });
                }
                char[] splitPlacaFinal = GenerarPlacasRangos.RangoFinal.ToCharArray();
                for (int i = splitPlacaFinal.Length - 1; i > 0; i--)
                {
                    listadoRangoFinalOrden.Add(new Placas_Orden()
                    {
                        Valor = splitPlacaFinal[i].ToString(),
                        Orden = int.Parse(splitOrden[i].ToString()),
                    });
                }

                for (int i = 0; i < splitOrdenSerie.Length; i++)
                {
                    var valorPos = int.Parse(splitOrdenSerie[i].ToString()) - 1;
                    serieInicial += splitPlacaInicial[valorPos];
                }

                for (int i = 0; i < splitOrdenSerie.Length; i++)
                {
                    var valorPos = int.Parse(splitOrdenSerie[i].ToString()) - 1;
                    serieFinal += splitPlacaFinal[valorPos];
                }

                char[] splitMascara = GenerarPlacasRangos.TipoPlaca.MascaraPlaca.ToCharArray();
                for (int i = splitMascara.Length - 1; i > 0; i--)
                {
                    listadoMascaraOrden.Add(new Placas_Orden()
                    {
                        Valor = splitMascara[i].ToString(),
                        Orden = int.Parse(splitOrden[i].ToString()),
                    });
                }

                if (serieInicial != serieFinal)
                {
                    response_.Data.Add(new ErroresGeneradosPlacas()
                    {
                        ConsecutivoError = response_.Data.Count + 1,
                        Error = "Placas de Diferente Serie",
                        EsEvento = 1,
                        Prioridad = 1
                    });
                }

                int[] ListNumerosOrdenesDesc = listadoRangoInicialOrden.OrderByDescending(x => x.Orden).Select(s => s.Orden).ToArray();
                int[] ListNumerosOrdenes = listadoRangoInicialOrden.OrderBy(x => x.Orden).Select(s => s.Orden).ToArray();

                for (int desc = 0; desc < ListNumerosOrdenesDesc.Length; desc++)
                {
                    var index = ListNumerosOrdenesDesc[desc];
                    var posicionMascara = listadoMascaraOrden.Where(x => x.Orden == index).FirstOrDefault();
                    var rangoInicial = listadoRangoInicialOrden.Where(x => x.Orden == index).FirstOrDefault();
                    var rangoFinal = listadoRangoFinalOrden.Where(x => x.Orden == index).FirstOrDefault();

                    if (posicionMascara.Valor == "A")
                    {
                        if (EsLetra(rangoInicial.Valor) && EsLetra(rangoFinal.Valor))
                        {
                            var posicionDiccionarInicial = diccionarioLetras.Where(x => x.Letra == rangoInicial.Valor).FirstOrDefault();
                            var posicionDiccionarFinal = diccionarioLetras.Where(x => x.Letra == rangoFinal.Valor).FirstOrDefault();
                            if (posicionDiccionarFinal.Posicion < posicionDiccionarInicial.Posicion)
                            {
                                response_.Data.Add(new ErroresGeneradosPlacas()
                                {
                                    ConsecutivoError = response_.Data.Count + 1,
                                    Error = "Rango de Placas Incorrectas",
                                    EsEvento = 3, 
                                    Prioridad = 1
                                });
                                break;
                            }
                        }
                        else
                        {
                            response_.Data.Add(new ErroresGeneradosPlacas()
                            {
                                ConsecutivoError = response_.Data.Count + 1,
                                Error = "la Cadena de la estructura de la placa del Rango Inicial no corresponde con la estructura del Rango Final",
                                EsEvento = 0,
                                Prioridad = 0
                            });
                            break;
                        }
                    }
                    else if (posicionMascara.Valor == "N")
                    {
                        if (int.Parse(rangoFinal.Valor) < int.Parse(rangoInicial.Valor))
                        {
                            var rangoInicialAnt = listadoRangoInicialOrden.Where(x => x.Orden == (index + 1)).FirstOrDefault();
                            var rangoFinalAnt = listadoRangoFinalOrden.Where(x => x.Orden == (index + 1)).FirstOrDefault();
                            if (EsLetra(rangoInicialAnt.Valor) && EsLetra(rangoFinalAnt.Valor))
                            {
                                var posicionDiccionarInicial = diccionarioLetras.Where(x => x.Letra == rangoInicialAnt.Valor).FirstOrDefault();
                                var posicionDiccionarFinal = diccionarioLetras.Where(x => x.Letra == rangoFinalAnt.Valor).FirstOrDefault();
                                if (posicionDiccionarFinal.Posicion < posicionDiccionarInicial.Posicion)
                                {
                                    response_.Data.Add(new ErroresGeneradosPlacas()
                                    {
                                        ConsecutivoError = response_.Data.Count + 1,
                                        Error = "Rango de Placas Incorrectas",
                                        EsEvento = 3,
                                        Prioridad = 1
                                    });
                                    break;
                                }
                            }
                            else
                            {
                                if (int.Parse(rangoFinalAnt.Valor) < int.Parse(rangoInicialAnt.Valor))
                                {
                                    response_.Data.Add(new ErroresGeneradosPlacas()
                                    {
                                        ConsecutivoError = response_.Data.Count + 1,
                                        Error = "Rango de Placas Incorrectas",
                                        EsEvento = 3,
                                        Prioridad = 1
                                    });
                                    break;
                                }
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        response_.Data.Add(new ErroresGeneradosPlacas()
                        {
                            ConsecutivoError = response_.Data.Count + 1,
                            Error = "La Mascara solo debe tener las letras A y N",
                            EsEvento = 0,
                            Prioridad = 0
                        });
                    }
                }

                if (response_.Data.Count > 0)
                {
                    response_.ExecutionOK = false;
                    response_.Message = "";
                }
                else
                {
                    response_.ExecutionOK = true;
                }
            }
            catch (Exception)
            {

                throw;

            }

            return response_;
        }
        private static bool EsLetra(string letra)
        {
            foreach (Char ch in letra)
            {
                if (!Char.IsLetter(ch) && ch != 32)
                {
                    return false;
                }
            }
            return true;
        }
        public List<DiccionarioLetraAbcedario> ListadoLetras()
        {
            var listado = new List<DiccionarioLetraAbcedario>();
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "A", Posicion = 1 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "B", Posicion = 2 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "C", Posicion = 3 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "D", Posicion = 4 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "E", Posicion = 5 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "F", Posicion = 6 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "G", Posicion = 7 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "H", Posicion = 8 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "J", Posicion = 9 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "K", Posicion = 10 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "L", Posicion = 11 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "M", Posicion = 12 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "N", Posicion = 13 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "P", Posicion = 14 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "R", Posicion = 15 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "S", Posicion = 16 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "T", Posicion = 17 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "U", Posicion = 18 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "V", Posicion = 19 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "W", Posicion = 20 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "X", Posicion = 21 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "Y", Posicion = 22 });
            listado.Add(new DiccionarioLetraAbcedario() { Letra = "Z", Posicion = 23 });

            return listado;
        }
    }
}

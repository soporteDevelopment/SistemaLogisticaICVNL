﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web
{
    public static class Notificaciones
    {
        public static void ShowNotificacion(this Controller controller, string tipo, string titulo,
            string mensaje, string segundosVisible, string segundosRetardo)
        {
            if (controller.TempData.ContainsKey("messages"))
            {
                try
                {
                    (controller.TempData["messages"] as Dictionary<string, string[]>).Add(
                        tipo,
                        new string[]
                        {
                        titulo,
                        mensaje,
                        segundosVisible,
                        segundosRetardo
                        });
                }
                catch (Exception ex)
                {
                    controller.TempData["messages"] = new Dictionary<string, string[]>();
                    (controller.TempData["messages"] as Dictionary<string, string[]>).Add(
                        tipo,
                        new string[]
                        {
                        titulo,
                        mensaje,
                        segundosVisible,
                        segundosRetardo
                        });
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                controller.TempData["messages"] = new Dictionary<string, string[]>
                {
                    [tipo] = new string[]
                                {
                                    titulo,
                                    mensaje,
                                    segundosVisible,
                                    segundosRetardo
                                }
                };
            }
        }
    }
}
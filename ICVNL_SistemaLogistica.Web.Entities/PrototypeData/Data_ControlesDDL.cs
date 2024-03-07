using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.PrototypeData
{
    public class Data_ControlesDDL
    {
        public IEnumerable<dynamic> getDataDefault()
        {
            var dynamicObj = new List<DynamicDDL>();

            dynamicObj.Add(new DynamicDDL() { Valor = "0", Texto = "Seleccione" }); 

            return dynamicObj;
        }

        public IEnumerable<dynamic> GetEstatusActivoInactivo()
        {
            var dynamicObj = new List<DynamicDDL>();

            dynamicObj.Add(new DynamicDDL() { Valor = "-1", Texto = "Seleccione" });
            dynamicObj.Add(new DynamicDDL() { Valor = "1", Texto = "Activo" });
            dynamicObj.Add(new DynamicDDL() { Valor = "0", Texto = "Inactivo" });

            return dynamicObj;
        }

        public IEnumerable<dynamic> getEstatusNotasEntradasPlacas()
        {
            var dynamicObj = new List<DynamicDDL>();

            dynamicObj.Add(new DynamicDDL() { Valor = "0", Texto = "Seleccione" });
            dynamicObj.Add(new DynamicDDL() { Valor = "1", Texto = "Notas de Entrada con todas sus placas identificadas" });
            dynamicObj.Add(new DynamicDDL() { Valor = "2", Texto = "Notas de Entrada con placas pendientes de ser identificadas" });

            return dynamicObj;
        }

        public IEnumerable<dynamic> getTipoEventos()
        {
            var dynamicObj = new List<DynamicDDL>();

            dynamicObj.Add(new DynamicDDL() { Valor = "0", Texto = "Seleccione" });
            dynamicObj.Add(new DynamicDDL() { Valor = "1", Texto = "Placas de Diferentes Serie" });
            dynamicObj.Add(new DynamicDDL() { Valor = "2", Texto = "Cantidad de Placas no Coincide con el Rango " });
            dynamicObj.Add(new DynamicDDL() { Valor = "2", Texto = "Rango de Placas Fuera del Rango " });

            return dynamicObj;
        }
                       
        public IEnumerable<dynamic> getTiposEstatusPlacas()
        {
            var dynamicObj = new List<DynamicDDL>();

            dynamicObj.Add(new DynamicDDL() { Valor = "0", Texto = "Seleccione" });
            dynamicObj.Add(new DynamicDDL() { Valor = "1", Texto = "Disponible" });
            dynamicObj.Add(new DynamicDDL() { Valor = "2", Texto = "Obsoleta" });
            dynamicObj.Add(new DynamicDDL() { Valor = "3", Texto = "Rechazada" }); 


            return dynamicObj;
        }

        public IEnumerable<dynamic> getInstruccionesRealizadasDDL()
        {
            var dynamicObj = new List<DynamicDDL>();

            dynamicObj.Add(new DynamicDDL() { Valor = "0", Texto = "Seleccione" }); 
            dynamicObj.Add(new DynamicDDL() { Valor = "1", Texto = "Select" });
            dynamicObj.Add(new DynamicDDL() { Valor = "2", Texto = "Update" });
            dynamicObj.Add(new DynamicDDL() { Valor = "3", Texto = "Delete" });
            dynamicObj.Add(new DynamicDDL() { Valor = "4", Texto = "Insert" });

            return dynamicObj;
        }
    }
}

using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_BitacoraEventosModel
    {
        public string FechaEvento { get; set; }
        public string FechaEventoStr { get; set; }
        public string LugarEvento { get; set; }
        public string Evento { get; set; }
        public string Usuario { get; set; }
        public string InstruccionRealizada { get; set; }
        public string IP_Usuario { get; set; }
        public string JsonObject { get; set; }

        public static Listado_BitacoraEventosModel operator +(Listado_BitacoraEventosModel _BitacoraEventosModel, BitacoraEventos bitacoraEventos)
        {
            _BitacoraEventosModel.FechaEvento = bitacoraEventos.FechaEvento.ToString("dd/MM/yyyy hh:mm:ss tt");
            _BitacoraEventosModel.FechaEventoStr = bitacoraEventos.FechaEvento.ToString("yyyyMMddHHmmss");
            _BitacoraEventosModel.LugarEvento = bitacoraEventos.LugarEvento;
            _BitacoraEventosModel.Evento = bitacoraEventos.InstruccionRealizada;
            _BitacoraEventosModel.Usuario = bitacoraEventos.Usuario;
            _BitacoraEventosModel.InstruccionRealizada = bitacoraEventos.Evento;
            _BitacoraEventosModel.IP_Usuario = bitacoraEventos.IP_Usuario;
            _BitacoraEventosModel.JsonObject = bitacoraEventos.JsonObject;
            return _BitacoraEventosModel;
        }
    }
}
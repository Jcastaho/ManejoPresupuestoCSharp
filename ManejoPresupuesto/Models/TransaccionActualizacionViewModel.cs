namespace ManejoPresupuesto.Models
{
    public class TransaccionActualizacionViewModel : TransaccionCreacionViewModel
    {
        public decimal MontoAnterior { get; set; }
        public int CuentaAnteriorId { get; set; }
        public string UrlRetorno { get; set; }
    }
}

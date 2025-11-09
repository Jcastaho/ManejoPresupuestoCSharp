namespace ManejoPresupuesto.Models
{
    public class ReporteMensualViewModel
    {
        public IEnumerable<ResultadoObtenerPorMes> TransaccionesPorMes { get; set; }
        public decimal Ingreso => TransaccionesPorMes.Sum(x => x.Ingreso);
        public decimal Gastos => TransaccionesPorMes.Sum(x => x.Gasto);
        public decimal Total => Ingreso - Gastos;
        public int anio { get; set; }
    }
}

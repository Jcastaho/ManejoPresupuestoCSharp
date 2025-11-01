using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models
{
    public class TransaccionCreacionViewModel : Transaccion
    {
        public IEnumerable<SelectListItem> Categorias { get; set; }
        public IEnumerable<SelectListItem> Cuentas { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backengv2.Controllers.Login
{
    [ApiController] // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")] // Define la ruta base para el controlador (en este caso, "api/customers")
    public class CustomersController : ControllerBase // Controlador base para manejar operaciones relacionadas con clientes
    {
        private readonly BaseContext _context; // Contexto de la base de datos

        public CustomersController(BaseContext context)
        {
            _context = context; // Inicializa el contexto de la base de datos
        }

        [HttpGet, Authorize] // Define la acción HTTP GET y requiere autorización JWT
        public IEnumerable<string> Get()
        {
            // Retorna una lista de nombres de clientes (en este caso, nombres de ejemplo)
            return new string[]
            {
                "Jhon Doe",
                "Chandrashekhar Singh"
            };
        }
    }
}

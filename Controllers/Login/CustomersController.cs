using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backengv2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backengv2.Controllers.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
     private readonly BaseContext _context;
      public CustomersController(BaseContext context)
      {
          _context = context;
      }
      [HttpGet, Authorize]
      public IEnumerable<string> Get()
      {
        return new string []
        {
            " Jhon Doe ",
            " Chandrashekhar Singh "
        };
      }
      
    }
}
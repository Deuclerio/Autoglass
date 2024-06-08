using Microsoft.AspNetCore.Mvc;

namespace Autoglass.Api.Controllers
{
    [Route("api/[controller]/")]    
    public class BaseController : Controller
    {
        public BaseController()
        {
        }
    }
}

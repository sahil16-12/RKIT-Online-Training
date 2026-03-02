using System.Web.Http;

namespace Authentication_Demo.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new
            {
                Message = "You accessed a protected endpoint!",
                User = User.Identity.Name
            });
        }
    }
}

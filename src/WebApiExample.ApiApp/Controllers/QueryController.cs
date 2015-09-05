using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WebApiExample.Contracts;

namespace WebApiExample.ApiApp.Controllers
{
    [Route("Account")]
    public class QueryController : Controller
    {
        [HttpPost]
        public Task<QueryResult> Post([FromBody]Query query)
        {
            return Task.FromResult(new QueryResult { Results = query.Filters.Select(x => x.GetType().ToString()).ToArray() });
        }
    }
}

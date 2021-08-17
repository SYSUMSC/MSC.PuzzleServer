using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSC.Server.Middlewares;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MSC.Server.Controllers
{
    [RequireSignedIn]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    [SwaggerTag("管理员数据交互接口")]
    public class PuzzleController : ControllerBase
    {
    }
}

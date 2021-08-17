using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Utils
{
    public record BadRequestResponse(string Title, int Status = 400);

}

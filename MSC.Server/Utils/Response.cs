using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Utils.Response
{
    public record BadRequestResponse(string Title, int Status = 400);
    public record PuzzleResponse(int Id, int Status = 200);

}

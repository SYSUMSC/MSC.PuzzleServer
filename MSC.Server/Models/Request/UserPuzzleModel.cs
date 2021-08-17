using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models.Request
{
    public class UserPuzzleModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int SolvedCount { get; set; }
    }
}

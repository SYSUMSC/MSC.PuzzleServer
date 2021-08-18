using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSC.Server.Models;
using MSC.Server.Repositories.Interface;

namespace MSC.Server.Repositories
{
    public class ProcessRepository : RepositoryBase, IProcessRepository
    {
        public ProcessRepository(AppDbContext context) : base(context) { }
    }
}

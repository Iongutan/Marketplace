using Marketplace.Data;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.BusinessLogic.DBModel
{
    public class UserContext : BusinessContext
    {
        public UserContext(DbContextOptions<BusinessContext> options) : base(options)
        {
        }
    }
}

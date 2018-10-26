using System.Data.Entity;

namespace Sklep_CMS.Models.Data
{
    public class CmsDataBase : DbContext
    {
        public DbSet<PageDTO> Pages{ get; set; }
        public DbSet<SidebarDTO> Sidebar{ get; set; }
    }
}
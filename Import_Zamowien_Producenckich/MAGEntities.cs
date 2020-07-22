using System.Data.Entity;

namespace Import_Zamowien_Producenckich
{
    public partial class CDNXL_MAGEntities : DbContext
    {
        public CDNXL_MAGEntities(string connectionString, bool autoDetectChangesEnabled = true, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true) : base("name=CDNXL_MAGEntities")
        {
            this.Database.Connection.ConnectionString = connectionString;
            this.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            this.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
            this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
    }
}

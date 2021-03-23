namespace DataAnalysisSystem.RepositoryInterfaces.DataAccessLayerAbstract
{
    public interface DbContextAbstract
    {
        public string ReadConnectionString(string path);
        public void InitializeContext(string connectionString);
    }
}

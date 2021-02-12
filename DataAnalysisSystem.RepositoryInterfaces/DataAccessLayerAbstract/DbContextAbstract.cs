namespace DataAnalysisSystem.RepositoryInterfaces.DataAccessLayerAbstract
{
    public interface DbContextAbstract
    {
        string ReadConnectionString(string path);
        void InitializeContext(string connectionString);
    }
}

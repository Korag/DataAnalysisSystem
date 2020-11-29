namespace DataAnalysisSystem.RepositoryInterfaces
{
    public interface DbContext
    {
        string ReadConnectionString(string path);
        void InitializeContext(string connectionString);
    }
}

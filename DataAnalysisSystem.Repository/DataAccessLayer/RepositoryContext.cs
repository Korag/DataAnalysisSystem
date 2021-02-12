using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;

namespace DataAnalysisSystem.Repository.DataAccessLayer
{
    public class RepositoryContext
    {
        public readonly IUserRepository userRepository;
        public readonly IDatasetRepository datasetRepository;
        public readonly IAnalysisRepository analysisRepository;

        public RepositoryContext(IUserRepository userRepository,
                                 IDatasetRepository datasetRepository,
                                 IAnalysisRepository analysisRepository){

            this.userRepository = userRepository;
            this.datasetRepository = datasetRepository;
            this.analysisRepository = analysisRepository;
        }
    }
}

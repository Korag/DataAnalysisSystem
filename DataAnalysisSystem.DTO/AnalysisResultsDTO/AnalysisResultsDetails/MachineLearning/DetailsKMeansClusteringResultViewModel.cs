using DataAnalysisSystem.DataEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsKMeansClusteringResultViewModel
    {
        public DetailsKMeansClusteringResultViewModel(AnalysisResults analysisResults)
        {
            KMeansClusteringResult result = analysisResults.KMeansClusteringResult;

            if (result == null)
            {
                IsNull = true;
                return;
            }

            this.ClustersAssignment = new List<string>();

            List<double> labels = new List<double>();

            this.OXAttributeName = JsonConvert.SerializeObject(result.OXAttributeName);
            this.OYAttributeName = JsonConvert.SerializeObject(result.OYAttributeName);

            var dataGroupedByClusterNumber = result.Clusters.GroupBy(z => z.ClusterNumber).OrderBy(z=> z.Key).ToList();

            foreach (var clusterPointsGroup in dataGroupedByClusterNumber)
            {
                List<ChartPoint> clusterPoints = new List<ChartPoint>();

                foreach (var singlePoint in clusterPointsGroup)
                {
                    ChartPoint cp = new ChartPoint
                    {
                        x = Math.Round(singlePoint.OXValue, 4),
                        y = Math.Round(singlePoint.OYValue, 4)
                    };

                    clusterPoints.Add(cp);
                    labels.Add(cp.x);
                }

                ClustersAssignment.Add(JsonConvert.SerializeObject(clusterPoints));
            }

            labels = labels.OrderBy(z => z).ToList();
            labels = labels.Distinct().ToList();
            Labels = JsonConvert.SerializeObject(labels);
        }

        public bool IsNull { get; set; }

        public string OXAttributeName { get; set; }
        public string OYAttributeName { get; set; }

        public IList<string> ClustersAssignment { get; set; }
        public string Labels { get; set; }
    }
}

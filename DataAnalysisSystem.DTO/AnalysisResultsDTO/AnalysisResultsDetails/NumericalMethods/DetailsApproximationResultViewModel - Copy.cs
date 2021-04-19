using DataAnalysisSystem.DataEntities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails
{
    public class DetailsApproximationResultViewModel2
    {
        public DetailsApproximationResultViewModel2(AnalysisResults analysisResults)
        {
            ApproximationResult result = analysisResults.ApproximationResult;

            this.AttributeName = new List<string>();
            this.InX = new List<string>();
            this.InY = new List<string>();
            this.OutX = new List<string>();
            this.OutY = new List<string>();

            foreach (var numberColumn in result.NumberColumns)
            {
                if (numberColumn.ColumnSelected)
                {
                    AttributeName.Add(JsonConvert.SerializeObject(numberColumn.AttributeName));

                    InX.Add(JsonConvert.SerializeObject(numberColumn.InX));
                    InY.Add(JsonConvert.SerializeObject(numberColumn.InY));

                    OutX.Add(JsonConvert.SerializeObject(numberColumn.OutX));
                    OutY.Add(JsonConvert.SerializeObject(numberColumn.OutY));
                }
            }
        }

        public IList<string> AttributeName { get; set; }
        public IList<string> InX { get; set; }
        public IList<string> InY { get; set; }

        public IList<string> OutX { get; set; }
        public IList<string> OutY { get; set; }

    }
}
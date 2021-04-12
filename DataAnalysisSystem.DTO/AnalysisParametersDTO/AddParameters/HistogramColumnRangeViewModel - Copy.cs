using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class HistogramColumnRangeViewModel
    {
        public string AttributeName { get; set; }

        [Range(typeof(int), "0", "2147483647")]
        public int Range { get; set; }

        public bool PrepareHistogram { get; set; }
    }
}

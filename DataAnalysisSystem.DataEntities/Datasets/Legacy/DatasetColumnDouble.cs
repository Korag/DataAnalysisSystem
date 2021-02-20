using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetColumnDouble : DatasetColumnAbstract
    {
        public DatasetColumnDouble(string attributeName, string typeOfAttribute = "double") : base(attributeName, typeOfAttribute)
        {
            this.AttributeValue = new List<double>();
        }

        public ICollection<double> AttributeValue { get; set; }
    }
}

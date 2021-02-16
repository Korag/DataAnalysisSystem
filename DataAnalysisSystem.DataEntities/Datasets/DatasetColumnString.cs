using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetColumnString : DatasetColumnAbstract
    {
        public DatasetColumnString(string attributeName, string typeOfAttribute) : base(attributeName, typeOfAttribute)
        {
            this.AttributeValue = new List<string>();
        }

        public ICollection<string> AttributeValue { get; set; }
    }
}

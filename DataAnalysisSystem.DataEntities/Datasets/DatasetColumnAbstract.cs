namespace DataAnalysisSystem.DataEntities
{
    public abstract class DatasetColumnAbstract
    {
        public DatasetColumnAbstract(string attributeName, string typeOfAttribute)
        {
            this.AttributeName = attributeName;
            this.AttributeDataType = typeOfAttribute;
        }

        public string AttributeName { get; set; }
        public string AttributeDataType { get; set; }
    }
}

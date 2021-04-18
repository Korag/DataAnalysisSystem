using System;

namespace DataAnalysisSystem.DataAnalysisMethods.Helpers
{
    public class DynamicTypeProperty
    {
        public DynamicTypeProperty(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
    }
}

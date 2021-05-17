using DataAnalysisSystem.DataAnalysisMethods.Helpers;
using DataAnalysisSystem.DataEntities;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class RegressionMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            AnalysisResults results = new AnalysisResults();
            results.RegressionResult = new RegressionResult();

            var properties = new List<DynamicTypeProperty>();
            List<string> attributeNamesWithoutFirstPredictionAttribute = new List<string>();
            List<string> attributeNamesWithoutSecondPredictionAttribute = new List<string>();

            foreach (var numberColumn in datasetContent.NumberColumns)
            {
                properties.Add(new DynamicTypeProperty(
                                   numberColumn.AttributeName,
                                   typeof(float)
                ));

                if (properties.Last().Name == "default")
                {
                    properties.Last().Name = "defaultName";
                    numberColumn.AttributeName = "defaultName";
                }

                DatasetColumnSelectColumnForParametersTypeDouble columnNumberParameters = parameters.RegressionParameters.NumberColumns.Where(z => z.PositionInDataset == numberColumn.PositionInDataset).FirstOrDefault();

                if (columnNumberParameters.ColumnSelected)
                {
                    if (String.IsNullOrEmpty(results.RegressionResult.OXAttributeName))
                    {
                        results.RegressionResult.OXAttributeName = numberColumn.AttributeName;
                        results.RegressionResult.OXCoordinatePosition = numberColumn.PositionInDataset;

                        attributeNamesWithoutSecondPredictionAttribute.Add(numberColumn.AttributeName);
                    }
                    else
                    {
                        results.RegressionResult.OYAttributeName = numberColumn.AttributeName;
                        results.RegressionResult.OYCoordinatePosition = numberColumn.PositionInDataset;

                        attributeNamesWithoutFirstPredictionAttribute.Add(numberColumn.AttributeName);
                    }
                }
                else
                {
                    attributeNamesWithoutFirstPredictionAttribute.Add(numberColumn.AttributeName);
                    attributeNamesWithoutSecondPredictionAttribute.Add(numberColumn.AttributeName);
                }
            }

            var dynamicType = DynamicType.CreateDynamicType(properties);
            var schema = SchemaDefinition.Create(dynamicType);
            var dynamicList = DynamicType.CreateDynamicList(dynamicType);

            var addAction = DynamicType.GetAddAction(dynamicList);

            var firstColumn = datasetContent.NumberColumns.FirstOrDefault();
            int datasetLength = 0;

            if (firstColumn != null)
            {
                datasetLength = firstColumn.AttributeValue.Count();
            }
            else
            {
                datasetLength = datasetContent.StringColumns.FirstOrDefault().AttributeValue.Count();
            }

            for (int i = 0; i < datasetLength; i++)
            {
                List<object> singleRowValues = new List<object>();

                foreach (var numberColumn in datasetContent.NumberColumns)
                {
                    singleRowValues.Add(Convert.ToSingle(numberColumn.AttributeValue[i]));
                }

                addAction.Invoke(singleRowValues.ToArray());
            }

            var mlContext = new MLContext(seed: 0);

            var dataType = mlContext.Data.GetType();
            var loadMethodGeneric = dataType.GetMethods().First(method => method.Name == "LoadFromEnumerable" && method.IsGenericMethod);
            var loadMethod = loadMethodGeneric.MakeGenericMethod(dynamicType);
            var trainData = (IDataView)loadMethod.Invoke(mlContext.Data, new[] { dynamicList, schema });

            string featuresColumnName = "Features";
            string outputColumnName = "Label";

            var pipelineFirst = mlContext.Transforms
                .CopyColumns(outputColumnName, results.RegressionResult.OXAttributeName)
                .Append(mlContext.Transforms.Concatenate(featuresColumnName, attributeNamesWithoutFirstPredictionAttribute.ToArray()))
                .Append(mlContext.Regression.Trainers.Sdca(outputColumnName, featuresColumnName));

            var pipelineSecond = mlContext.Transforms
                .CopyColumns(outputColumnName, results.RegressionResult.OYAttributeName)
                .Append(mlContext.Transforms.Concatenate(featuresColumnName, attributeNamesWithoutSecondPredictionAttribute.ToArray()))
                .Append(mlContext.Regression.Trainers.Sdca(outputColumnName, featuresColumnName));

            var modelForFirstAttributePrediction = pipelineFirst.Fit(trainData);
            var modelForSecondAttributePrediction = pipelineSecond.Fit(trainData);

            var predictionsForFirstAttribute = modelForFirstAttributePrediction.Transform(trainData);
            var metricsForFirstAttribute = mlContext.Regression.Evaluate(predictionsForFirstAttribute, "Label", "Score");

            results.RegressionResult.OXPredictionRegressionMetrics = new RegressionMetric()
            {
                LossFunction = metricsForFirstAttribute.LossFunction,
                MeanAbsoluteError = metricsForFirstAttribute.MeanAbsoluteError,
                MeanSquaredError = metricsForFirstAttribute.MeanSquaredError,
                RootMeanSquaredError = metricsForFirstAttribute.RootMeanSquaredError,
                RSquared = metricsForFirstAttribute.RSquared
            };

            var predictionsForSecondAttribute = modelForSecondAttributePrediction.Transform(trainData);
            var metricsForSecondAttribute = mlContext.Regression.Evaluate(predictionsForSecondAttribute, "Label", "Score");

            results.RegressionResult.OYPredictionRegressionMetrics = new RegressionMetric()
            {
                LossFunction = metricsForSecondAttribute.LossFunction,
                MeanAbsoluteError = metricsForSecondAttribute.MeanAbsoluteError,
                MeanSquaredError = metricsForSecondAttribute.MeanSquaredError,
                RootMeanSquaredError = metricsForSecondAttribute.RootMeanSquaredError,
                RSquared = metricsForSecondAttribute.RSquared
            };

            var dataViewSchema = trainData.Schema;

            dynamic dynamicPredictionEngineForFirstAttribute;
            var genericPredictionMethodForFirstAttribute = mlContext.Model.GetType().GetMethod("CreatePredictionEngine", new[] { typeof(ITransformer), typeof(DataViewSchema) });
            var predictionMethodForFirstAttribute = genericPredictionMethodForFirstAttribute.MakeGenericMethod(dynamicType, typeof(RegressionPrediction));
            dynamicPredictionEngineForFirstAttribute = predictionMethodForFirstAttribute.Invoke(mlContext.Model, new object[] { modelForFirstAttributePrediction, dataViewSchema });

            var predictMethodForFirstAttribute = dynamicPredictionEngineForFirstAttribute.GetType().GetMethod("Predict", new[] { dynamicType });

            dynamic dynamicPredictionEngineForSecondAttribute;
            var genericPredictionMethodForSecondAttribute = mlContext.Model.GetType().GetMethod("CreatePredictionEngine", new[] { typeof(ITransformer), typeof(DataViewSchema) });
            var predictionMethodForSecondAttribute = genericPredictionMethodForSecondAttribute.MakeGenericMethod(dynamicType, typeof(RegressionPrediction));
            dynamicPredictionEngineForSecondAttribute = predictionMethodForSecondAttribute.Invoke(mlContext.Model, new object[] { modelForSecondAttributePrediction, dataViewSchema });

            var predictMethodForSecondAttribute = dynamicPredictionEngineForSecondAttribute.GetType().GetMethod("Predict", new[] { dynamicType });

            List<double> OXAttributeValues = datasetContent.NumberColumns.Where(z => z.PositionInDataset == results.RegressionResult.OXCoordinatePosition).FirstOrDefault().AttributeValue.ToList();
            List<double> OYAttributeValues = datasetContent.NumberColumns.Where(z => z.PositionInDataset == results.RegressionResult.OYCoordinatePosition).FirstOrDefault().AttributeValue.ToList();

            for (int i = 0; i < datasetLength; i++)
            {
                RegressionPrediction predictForFirstAttribute = predictMethodForFirstAttribute.Invoke(dynamicPredictionEngineForFirstAttribute, new[] { dynamicList.ElementAt(i) });
                RegressionPrediction predictForSecondAttribute = predictMethodForSecondAttribute.Invoke(dynamicPredictionEngineForSecondAttribute, new[] { dynamicList.ElementAt(i) });

                RegressionPredictedValue predictedValue = new RegressionPredictedValue()
                {
                    OXValueFromSource = OXAttributeValues[i],
                    OYValueFromSource = OYAttributeValues[i],

                    OXValuePredicted = Convert.ToDouble(predictForFirstAttribute.Score),
                    OYValuePredicted = Convert.ToDouble(predictForSecondAttribute.Score),
                };

                results.RegressionResult.PredictionResults.Add(predictedValue);
            }

            return results;
        }
    }
}
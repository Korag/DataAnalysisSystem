using DataAnalysisSystem.DataAnalysisMethods.Helpers;
using DataAnalysisSystem.DataEntities;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class KMeansClusteringMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            AnalysisResults results = new AnalysisResults();
            results.KMeansClusteringResult = new KMeansClusteringResult();

            var properties = new List<DynamicTypeProperty>();
            List<string> attributeNames = new List<string>();

            foreach (var numberColumn in datasetContent.NumberColumns)
            {
                properties.Add(new DynamicTypeProperty(
                                   numberColumn.AttributeName,
                                   typeof(float)
                ));

                attributeNames.Add(numberColumn.AttributeName);


                DatasetColumnSelectColumnForParametersTypeDouble columnNumberParameters = parameters.KMeansClusteringParameters.NumberColumns.Where(z => z.PositionInDataset == numberColumn.PositionInDataset).FirstOrDefault();
                DatasetContentKMeansClusteringResultsTypeDouble singleNumberColumnResult = new DatasetContentKMeansClusteringResultsTypeDouble(numberColumn.AttributeName, numberColumn.PositionInDataset, columnNumberParameters.ColumnSelected);
                results.KMeansClusteringResult.NumberColumns.Add(singleNumberColumnResult);

                if (columnNumberParameters.ColumnSelected)
                {
                    if (String.IsNullOrEmpty(results.KMeansClusteringResult.OXAttributeName))
                    {
                        results.KMeansClusteringResult.OXAttributeName = numberColumn.AttributeName;
                        results.KMeansClusteringResult.OXCoordinatePosition = numberColumn.PositionInDataset;
                    }
                    else
                    {
                        results.KMeansClusteringResult.OYAttributeName = numberColumn.AttributeName;
                        results.KMeansClusteringResult.OYCoordinatePosition = numberColumn.PositionInDataset;
                    }
                }
            }

            foreach (var stringColumn in datasetContent.StringColumns)
            {
                DatasetColumnSelectColumnForParametersTypeString columnStringParameters = parameters.KMeansClusteringParameters.StringColumns.Where(z => z.PositionInDataset == stringColumn.PositionInDataset).FirstOrDefault();
                DatasetContentKMeansClusteringResultsTypeString singleStringColumnResult = new DatasetContentKMeansClusteringResultsTypeString(stringColumn.AttributeName, stringColumn.PositionInDataset, columnStringParameters.ColumnSelected);
                results.KMeansClusteringResult.StringColumns.Add(singleStringColumnResult);
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
            var pipeline = mlContext.Transforms
                .Concatenate(featuresColumnName, attributeNames.ToArray())
                .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: parameters.KMeansClusteringParameters.ClustersNumber));

            var model = pipeline.Fit(trainData);

            VBuffer<float>[] centroids = default;
            var lastTransformer = model.LastTransformer;
            KMeansModelParameters kparams = (KMeansModelParameters)lastTransformer.GetType().GetProperty("Model").GetValue(lastTransformer);
            kparams.GetClusterCentroids(ref centroids, out int k);

            var dataViewSchema = trainData.Schema;

            dynamic dynamicPredictionEngine;
            var genericPredictionMethod = mlContext.Model.GetType().GetMethod("CreatePredictionEngine", new[] { typeof(ITransformer), typeof(DataViewSchema) });
            var predictionMethod = genericPredictionMethod.MakeGenericMethod(dynamicType, typeof(ClusterPrediction));
            dynamicPredictionEngine = predictionMethod.Invoke(mlContext.Model, new object[] { model, dataViewSchema });

            var predictMethod = dynamicPredictionEngine.GetType().GetMethod("Predict", new[] { dynamicType });

            List<double> OXAttributeValues = datasetContent.NumberColumns.Where(z => z.PositionInDataset == results.KMeansClusteringResult.OXCoordinatePosition).FirstOrDefault().AttributeValue.ToList();
            List<double> OYAttributeValues = datasetContent.NumberColumns.Where(z => z.PositionInDataset == results.KMeansClusteringResult.OYCoordinatePosition).FirstOrDefault().AttributeValue.ToList();

            results.KMeansClusteringResult.Clusters = new List<ClusterMemberData>();

            for (int i = 0; i < datasetLength; i++)
            {
                ClusterPrediction predict = predictMethod.Invoke(dynamicPredictionEngine, new[] { dynamicList.ElementAt(i) });

                ClusterMemberData singleMember = new ClusterMemberData();
                singleMember.ClusterNumber = Convert.ToInt32(predict.PredictedLabel);
                singleMember.OXValue = OXAttributeValues[i];
                singleMember.OYValue = OYAttributeValues[i];
                singleMember.DistancesToClusters = new double[predict.Score.Length];

                for (int j = 0; j < predict.Score.Length; j++)
                {
                    singleMember.DistancesToClusters[j] = Convert.ToDouble(predict.Score[j]);
                }

                results.KMeansClusteringResult.Clusters.Add(singleMember);
            }

            results.KMeansClusteringResult.CentroidsPoints = new List<IList<double>>(parameters.KMeansClusteringParameters.ClustersNumber);

            for (int i = 0; i < parameters.KMeansClusteringParameters.ClustersNumber; i++)
            {
                List<float> centroidCoordsFt = centroids[i].GetValues().ToArray().ToList();
                results.KMeansClusteringResult.CentroidsPoints.Add(new List<double>());

                foreach (var coord in centroidCoordsFt)
                {
                    results.KMeansClusteringResult.CentroidsPoints[i].Add(Convert.ToDouble(coord));
                }
            }

            return results;
        }
    }
}

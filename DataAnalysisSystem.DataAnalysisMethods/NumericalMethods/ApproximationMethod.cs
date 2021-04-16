using DataAnalysisSystem.DataEntities;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class ApproximationMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            AnalysisResults results = new AnalysisResults();
            results.ApproximationResult = new ApproximationResult();

            results.ApproximationResult.ApproximationPointsNumber = parameters.ApproximationParameters.ApproximationPointsNumber;

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

            IEnumerable<double> inX = Enumerable.Range(0, datasetLength).Select(z => Convert.ToDouble(z));

            foreach (var numberColumn in datasetContent.NumberColumns)
            {
                DatasetColumnSelectColumnForParametersTypeDouble columnNumberParameters = parameters.ApproximationParameters.NumberColumns.Where(z => z.PositionInDataset == numberColumn.PositionInDataset).FirstOrDefault();
                DatasetContentApproximationResultsTypeDouble singleNumberColumnResult = new DatasetContentApproximationResultsTypeDouble(numberColumn.AttributeName, numberColumn.PositionInDataset, columnNumberParameters.ColumnSelected);

                if (columnNumberParameters.ColumnSelected)
                {
                    var inY = numberColumn.AttributeValue;

                    singleNumberColumnResult.InX = inX.ToList();
                    singleNumberColumnResult.InY = inY;

                    var spline = CubicSpline.InterpolateNatural(inX, inY);

                    var outX = new DenseVector(parameters.ApproximationParameters.ApproximationPointsNumber);
                    var outY = new DenseVector(outX.Count);
                    var dydx = new DenseVector(outX.Count);

                    for (int i = 0; i < outX.Count; i++)
                    {
                        outX[i] = (inX.Last() * i) / (outX.Count - 1);
                        outY[i] = spline.Interpolate(outX[i]);
                        //dydx[i] = spline.Differentiate(outX[i]);
                    }

                    singleNumberColumnResult.OutX = outX.ToList();
                    singleNumberColumnResult.OutY = outY.ToList();
                    //singleNumberColumnResult.DYDX = dydx.ToList();
                }
                else
                {
                    singleNumberColumnResult.InX = null;
                    singleNumberColumnResult.InY = null;
                    singleNumberColumnResult.OutX = null;
                    singleNumberColumnResult.OutY = null;
                    //singleNumberColumnResult.DYDX = null;
                }

                results.ApproximationResult.NumberColumns.Add(singleNumberColumnResult);
            }

            foreach (var stringColumn in datasetContent.StringColumns)
            {
                DatasetColumnSelectColumnForParametersTypeString columnStringParameters = parameters.ApproximationParameters.StringColumns.Where(z => z.PositionInDataset == stringColumn.PositionInDataset).FirstOrDefault();
                DatasetContentApproximationResultsTypeString singleStringColumnResult = new DatasetContentApproximationResultsTypeString(stringColumn.AttributeName, stringColumn.PositionInDataset, columnStringParameters.ColumnSelected);
                results.ApproximationResult.StringColumns.Add(singleStringColumnResult);
            }

            return results;
        }
    }
}

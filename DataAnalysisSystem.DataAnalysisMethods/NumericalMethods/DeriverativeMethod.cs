using DataAnalysisSystem.DataEntities;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class DeriverativeMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            AnalysisResults results = new AnalysisResults();
            results.DeriverativeResult = new DeriverativeResult();

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
                DatasetColumnSelectColumnForParametersTypeDouble columnNumberParameters = parameters.DeriverativeParameters.NumberColumns.Where(z => z.PositionInDataset == numberColumn.PositionInDataset).FirstOrDefault();
                DatasetContentDeriverativeResultsTypeDouble singleNumberColumnResult = new DatasetContentDeriverativeResultsTypeDouble(numberColumn.AttributeName, numberColumn.PositionInDataset, columnNumberParameters.ColumnSelected);

                if (columnNumberParameters.ColumnSelected)
                {
                    var inY = numberColumn.AttributeValue;

                    singleNumberColumnResult.InX = inX.ToList();
                    singleNumberColumnResult.InY = inY;

                    var spline = CubicSpline.InterpolateNatural(inX, inY);

                    var outX = new DenseVector(parameters.DeriverativeParameters.ApproximationPointsNumber);
                    var outY = new DenseVector(outX.Count);

                    var dydx = new DenseVector(outX.Count);
                    var dy2dx2 = new DenseVector(outX.Count);

                    for (int i = 0; i < outX.Count; i++)
                    {
                        outX[i] = (inX.Last() * i) / (outX.Count - 1);
                        outY[i] = spline.Interpolate(outX[i]);

                        dydx[i] = spline.Differentiate(outX[i]);
                        dy2dx2[i] = spline.Differentiate2(outX[i]);
                    }

                    singleNumberColumnResult.OutX = outX.ToList();
                    singleNumberColumnResult.OutY = outY.ToList();

                    singleNumberColumnResult.DYDX = dydx.ToList();
                    singleNumberColumnResult.DY2DX2 = dy2dx2.ToList();
                }
                else
                {
                    singleNumberColumnResult.InX = null;
                    singleNumberColumnResult.InY = null;
                   
                    singleNumberColumnResult.OutX = null;
                    singleNumberColumnResult.OutY = null;

                    singleNumberColumnResult.DYDX = null;
                    singleNumberColumnResult.DY2DX2 = null;
                }

                results.DeriverativeResult.NumberColumns.Add(singleNumberColumnResult);
            }

            foreach (var stringColumn in datasetContent.StringColumns)
            {
                DatasetColumnSelectColumnForParametersTypeString columnStringParameters = parameters.DeriverativeParameters.StringColumns.Where(z => z.PositionInDataset == stringColumn.PositionInDataset).FirstOrDefault();
                DatasetContentDeriverativeResultsTypeString singleStringColumnResult = new DatasetContentDeriverativeResultsTypeString(stringColumn.AttributeName, stringColumn.PositionInDataset, columnStringParameters.ColumnSelected);
                results.DeriverativeResult.StringColumns.Add(singleStringColumnResult);
            }

            return results;
        }
    }
}

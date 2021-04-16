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

            results.DeriverativeResult.ApproximationPointsNumber = parameters.DeriverativeParameters.ApproximationPointsNumber;

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

                    var outX = new DenseVector(parameters.ApproximationParameters.ApproximationPointsNumber);
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

                    singleNumberColumnResult.DYDX = dydx.ToList();
                    singleNumberColumnResult.DY2DX2 = dy2dx2.ToList();
                }
                else
                {
                    singleNumberColumnResult.InX = null;
                    singleNumberColumnResult.InY = null;
                   
                    singleNumberColumnResult.OutX = null;
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

            #region ToDO
            //foreach (var numberColumn in datasetContent.NumberColumns)
            //{
            //    DatasetColumnSelectColumnForParametersTypeDouble columnNumberParameters = parameters.DeriverativeParameters.NumberColumns.Where(z => z.PositionInDataset == numberColumn.PositionInDataset).FirstOrDefault();
            //    //DatasetContentApproximationResultsTypeDouble singleNumberColumnResult = new DatasetContentApproximationResultsTypeDouble(numberColumn.AttributeName, numberColumn.PositionInDataset, columnNumberParameters.ColumnSelected);

            //}

            //foreach (var stringColumn in datasetContent.StringColumns)
            //{
            //    DatasetColumnSelectColumnForParametersTypeString columnStringParameters = parameters.DeriverativeParameters.StringColumns.Where(z => z.PositionInDataset == stringColumn.PositionInDataset).FirstOrDefault();
            //    //DatasetContentApproximationResultsTypeString singleStringColumnResult = new DatasetContentApproximationResultsTypeString(stringColumn.AttributeName, stringColumn.PositionInDataset, columnStringParameters.ColumnSelected);
            //    //results.ApproximationResult.StringColumns.Add(singleStringColumnResult);
            //}

            //double result;
            //double estimatedError;

            //Func<double, double> fCentral = Math.Cos;

            //result = fCentral.CentralDerivative(1.0);
            //result = fCentral.CentralDerivative(1.0, out estimatedError);
            //Func<double, double> fForward = x => (x + 2) * (x + 2) * Math.Sqrt(x + 2);

            //// Calculating the derivative using central 
            //// differences returns NaN (Not a Number):
            //result = fForward.CentralDerivative(-2.0, out estimatedError);

            //result = fForward.ForwardDerivative(-2.0, out estimatedError);

            //Func<double, double> fBackward = x => x > 0.0 ? 1.0 : Math.Sin(x);
            //result = fBackward.BackwardDerivative(0.0, out estimatedError);

            //Func<double, double> dfCentral = fCentral.GetNumericalDifferentiator();
            //Func<double, double> dfForward = fForward.GetForwardDifferentiator();
            //Func<double, double> dfBackward = fBackward.GetBackwardDifferentiator();
            #endregion

            return results;
        }
    }
}

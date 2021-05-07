
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace CurveChartImageCreator
{
    public class GraphicCurve
    {
        public static bool LinearFreqAxis;
        public static string HeaderCaption;
        public static string LastError;
        public static double YMin4Graph = 1.0;
        public static double YMax4Graph = -1.0;

        //draw curves to bitmap file

        internal static void WriteFile(IList<FreqCrv> targetCurves, IList<FreqCrv> simCurves, Stream stream, uint width = 300, uint height = 300)
        {
            try
            {
                var dWidth = width; //px
                var dHeight = height; // px
                const double dBorder = 30.0; // px
                const double dFontSize = 10.0; // px
                const float fLineWidth = 1.6f; // px

                double dXMin = 0; double dXMax = 0; double dYMin = 0; double dYMax = 0;
                CalculateXandYRange(targetCurves, simCurves, ref dXMin, ref dXMax, ref dYMin, ref dYMax);

                double dXScale = 0; double dYScale = 0;
                CalculateXScaleAndYScale(dWidth, dHeight, dBorder, dXMax, dXMin, dYMax, dYMin, ref dXScale, ref dYScale);

                var graphicCurveResources = GraphicCurveResources.Create(dWidth, dHeight, dFontSize, fLineWidth);

                DrawAxisFrame(dBorder, dYMin, dYMax, graphicCurveResources);

                DrawXAxis(dBorder, dXScale, dXMin, graphicCurveResources);

                DrawYAxis(dBorder, dYMin, dYMax, dYScale, graphicCurveResources);

                DrawTargets(targetCurves, dXMin, dYMin, dXScale, dYScale, dBorder, graphicCurveResources);

                DrawSimCurves(simCurves, dXMin, dYMin, dYScale, dXScale, dBorder, graphicCurveResources);

                if (stream != null)
                {
                    graphicCurveResources.MGraph.Save(stream, ImageFormat.Png);
                }

                graphicCurveResources.Dispose();
            }
            catch (Exception exc)
            {
                if (LastError != null) LastError += "\n";
                LastError += exc.Message;
                if (exc.InnerException != null)
                    LastError += exc.InnerException.Message;
                throw new Exception(LastError);
            }
        }

        private static void CalculateXScaleAndYScale(double dWidth, double dHeight, double dBorder, double dXMax,
            double dXMin, double dYMax, double dYMin, ref double dXScale, ref double dYScale)
        {
            if (LinearFreqAxis)
                dXScale = (dWidth - 2.0 * dBorder) / (dXMax - dXMin);   //linear x axis
            else
            {
                if (Math.Abs(dXMin) < Math.Pow(10, -15))
                    throw new ArgumentException("Minimum x value must be greater than 0 for a logarithmic x axis");

                dXScale = (dWidth - 2.0 * dBorder) / Math.Log10(dXMax / dXMin);   //logarithmic x axis
            }

            // Set deltaY to 10 if (dYMax - dYMin) is 0 in order to avoid an exception
            var deltaY = (dYMax - dYMin);
            var deltaYResulting = (deltaY == 0) ? 10 : deltaY;

            dYScale = (dHeight - 2.0 * dBorder) / deltaYResulting;
        }

        private static void CalculateXandYRange(
            IList<FreqCrv> targetCurves,
            IList<FreqCrv> simCurves,
            ref double dXMin,
            ref double dXMax,
            ref double dYMin,
            ref double dYMax
            )
        {
            dXMin = ( AreCurvesAvailable(targetCurves) || AreCurvesAvailable(simCurves) ) ? 0 : 1;
            dXMax = ( AreCurvesAvailable(targetCurves) || AreCurvesAvailable(simCurves) ) ? 0 : 10; 
            dYMin = 0;
            dYMax = 0;

            if (AreCurvesAvailable(targetCurves))
            {
                dXMin = targetCurves[0][0].X;
                dXMax = dXMin;
                dYMin = targetCurves[0][0].Y;
                dYMax = dYMin;

                foreach (var crv in targetCurves)
                {
                    foreach (var pt in crv)
                    {
                        if (pt.X < dXMin)
                        {
                            dXMin = pt.X;
                        }
                        if (pt.X > dXMax)
                        {
                            dXMax = pt.X;
                        }
                        if (pt.Y < dYMin)
                        {
                            dYMin = pt.Y;
                        }
                        if (pt.Y > dYMax)
                        {
                            dYMax = pt.Y;
                        }
                    }
                }
            }
            if (AreCurvesAvailable(simCurves))
            {
                if(!AreCurvesAvailable(targetCurves))
                {
                    dXMin = simCurves[0][0].X;
                    dXMax = dXMin;
                    dYMin = simCurves[0][0].Y;
                    dYMax = dYMin;
                }
                
                foreach (var crv in simCurves)
                {
                    foreach (var pt in crv)
                    {
                        if (pt.X < dXMin)
                        {
                            dXMin = pt.X;
                        }
                        if (pt.X > dXMax)
                        {
                            dXMax = pt.X;
                        }
                        if (pt.Y < dYMin)
                        {
                            dYMin = pt.Y;
                        }
                        if (pt.Y > dYMax)
                        {
                            dYMax = pt.Y;
                        }
                    }
                }
            }

            if (YMax4Graph <= YMin4Graph) //autoscale
            {
                dYMin = 10.0 * Math.Floor(dYMin / 10.0);
                dYMax = 10.0 * Math.Ceiling(dYMax / 10.0);
            }
            else
            {
                dYMin = YMin4Graph;
                dYMax = YMax4Graph;
            }
        }

        private static void DrawAxisFrame(
            double dBorder,
            double dYMin,
            double dYMax,
            GraphicCurveResources graphicCurveResources)
        {
            var dX1 = dBorder;
            var dY1 = dBorder;
            var dX2 = graphicCurveResources.DWidth - dBorder;
            var dY2 = dBorder;
            DrawLine(dX1, dY1, dX2, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);
            dX1 = dX2;
            dY1 = dY2;
            dX2 = graphicCurveResources.DWidth - dBorder;
            dY2 = graphicCurveResources.DHeight - dBorder;
            DrawLine(dX1, dY1, dX2, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);
            dX1 = dX2;
            dY1 = dY2;
            dX2 = dBorder;
            dY2 = graphicCurveResources.DHeight - dBorder;
            DrawLine(dX1, dY1, dX2, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);
            dX1 = dX2;
            dY1 = dY2;
            dX2 = dBorder;
            dY2 = dBorder;
            DrawLine(dX1, dY1, dX2, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);
            dX1 = 1.5 * dBorder;
            dY1 = graphicCurveResources.DHeight - 0.5 * dBorder;
            DrawStringY(HeaderCaption, dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry,
                graphicCurveResources.Graph, graphicCurveResources.DHeight);

            //min y
            dX1 = 0.02 * dBorder;
            dY1 = 1.0 * dBorder;
            DrawStringY(dYMin.ToString(CultureInfo.InvariantCulture), dX1, dY1, graphicCurveResources.Font,
                graphicCurveResources.BrushGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);

            //max y
            dX1 = 0.02 * dBorder;
            dY1 = graphicCurveResources.DHeight - 1.0 * dBorder;
            DrawStringY(dYMax.ToString(CultureInfo.InvariantCulture), dX1, dY1, graphicCurveResources.Font,
                graphicCurveResources.BrushGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);
        }

        private static void DrawXAxis(
            double dBorder,
            double dXScale,
            double dXMin,
            GraphicCurveResources graphicCurveResources)
        {
            var dHeight = graphicCurveResources.DHeight;

            //x-axis:  125Hz  250Hz  500Hz  1k  2k  4k  8k
            var dY1 = dBorder;
            var dY2 = dHeight - dBorder;
            var dX1 = Value2PixelX(125.0, dBorder, dXScale, dXMin);
            DrawStringX("125", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
            DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
            dX1 = Value2PixelX(250.0, dBorder, dXScale, dXMin);
            DrawStringX("250", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
            DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
            dX1 = Value2PixelX(500.0, dBorder, dXScale, dXMin);
            DrawStringX("500", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
            DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
            dX1 = Value2PixelX(1000.0, dBorder, dXScale, dXMin);
            DrawStringX("1k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
            DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
            dX1 = Value2PixelX(2000.0, dBorder, dXScale, dXMin);
            DrawStringX("2k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
            DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
            dX1 = Value2PixelX(4000.0, dBorder, dXScale, dXMin);
            DrawStringX("4k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
            DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
            dX1 = Value2PixelX(8000.0, dBorder, dXScale, dXMin);
            DrawStringX("8k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
            DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);

            //linear x-axis only:  5k 6k 7k 10k 11k
            if (LinearFreqAxis)
            {
                dX1 = Value2PixelX(5000.0, dBorder, dXScale, dXMin);
                DrawStringX("5k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
                dX1 = Value2PixelX(6000.0, dBorder, dXScale, dXMin);
                DrawStringX("6k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
                dX1 = Value2PixelX(7000.0, dBorder, dXScale, dXMin);
                DrawStringX("7k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
                dX1 = Value2PixelX(10000.0, dBorder, dXScale, dXMin);
                DrawStringX("10k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
                dX1 = Value2PixelX(11000.0, dBorder, dXScale, dXMin);
                DrawStringX("11k", dX1, dY1, graphicCurveResources.Font, graphicCurveResources.BrushGry, graphicCurveResources.Graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, dHeight);
            }
        }

        private static void DrawYAxis(
            double dBorder,
            double dYMin,
            double dYMax,
            double dYScale,
            GraphicCurveResources graphicCurveResources
            )
        {
            //y-axis:  10db steps
            var dX1 = dBorder;
            var dX2 = graphicCurveResources.DWidth - dBorder;
            for (var i = (int)Math.Ceiling(0.1 + dYMin / 10.0); 1.0 + 10.0 * i < dYMax; i++)
            {
                double dValue = 10.0 * i;
                var dY1 = dBorder + dYScale * (dValue - dYMin);
                var dY2 = dY1;
                DrawLine(dX1, dY1, dX2, dY2, graphicCurveResources.PenGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);
                DrawStringY(dValue.ToString(CultureInfo.InvariantCulture), 0.02 * dBorder, dY1, graphicCurveResources.Font,
                    graphicCurveResources.BrushGry, graphicCurveResources.Graph, graphicCurveResources.DHeight);
            }
        }

        private static void DrawTargets(
            IList<FreqCrv> targetCurves,
            double dXMin,
            double dYMin,
            double dXScale,
            double dYScale,
            double dBorder,
            GraphicCurveResources graphicCurveResources)
        {
            if (targetCurves != null)
            {
                //targets
                foreach (var crv in targetCurves)
                {
                    if ((crv.CurveType != null) && (crv.CurveType == TCurveType.MPO_Target))
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenYellow, graphicCurveResources.Graph,
                            crv, dYScale, dXScale, dBorder);
                    else
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenRed, graphicCurveResources.Graph,
                            crv, dYScale, dXScale, dBorder);
                }
            }
        }

        private static void DrawSimCurves(
             IList<FreqCrv> simCurves,
             double dXMin,
             double dYMin,
             double dYScale,
             double dXScale,
             double dBorder,
             GraphicCurveResources graphicCurveResources)
        {
            if (simCurves != null)
            {
                //sim curves
                foreach (var crv in simCurves)
                {
                    if (crv.CurveType == TCurveType.Level_Low || crv.CurveType == TCurveType.Level_Medium ||
                        crv.CurveType == TCurveType.Level_High)
                    {
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenBlk,
                            graphicCurveResources.Graph, crv, dYScale, dXScale, dBorder);
                    }
                    else if (crv.CurveType == TCurveType.Fog)
                    {
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenOrange,
                            graphicCurveResources.Graph, crv, dYScale, dXScale, dBorder);
                    }
                    else if (crv.CurveType == TCurveType.TinnitusNoiserSimulation ||
                             crv.CurveType == TCurveType.TinnitusNoiserBroadbandLevel)
                    {
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenLGrn,
                            graphicCurveResources.Graph, crv, dYScale, dXScale, dBorder);
                    }
                    else if (crv.CurveType == TCurveType.CgmNoiseBroadbandLevel ||
                             crv.CurveType == TCurveType.CgmReferenceCurve ||
                             crv.CurveType == TCurveType.CriticalGain ||
                             crv.CurveType == TCurveType.CriticalGainFollowUp ||
                             crv.CurveType == TCurveType.CriticalGainMeasured ||
                             crv.CurveType == TCurveType.CriticalGainStatistical)
                    {
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenBlue,
                            graphicCurveResources.Graph, crv, dYScale, dXScale, dBorder);
                    }
                    else if (crv.CurveType == TCurveType.Effective_MPO)
                    {
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenDarkBlue,
                            graphicCurveResources.Graph, crv, dYScale, dXScale, dBorder);
                    }
                    else
                    {
                        DrawCurve(graphicCurveResources.DHeight, dXMin, dYMin, graphicCurveResources.PenGry,
                            graphicCurveResources.Graph, crv, dYScale, dXScale, dBorder);
                    }
                }
            }
        }

        internal static void WriteLegend(Stream stream, uint width = 300, uint height = 300)
        {
            try
            {
                var dWidth = width; //px
                var dHeight = height; // px
                const double dBorder = 30.0; // px
                const double dFontSize = 10.0; // px
                const float fLineWidth = 1.6f; // px

                //drawing resources
                var mGraph = new Bitmap((int)dWidth, (int)dHeight, PixelFormat.Format32bppArgb);
                var font = new Font("Lucida Sans Unicode", (int)dFontSize);
                var graph = Graphics.FromImage(mGraph);
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                var brushBlk = new SolidBrush(Color.Black);
                var brushRed = new SolidBrush(Color.Red);
                var brushLGrn = new SolidBrush(Color.DarkGreen);
                var brushGry = new SolidBrush(Color.DarkGray);
                var brushYellow = new SolidBrush(Color.FromArgb(255, 255, 230, 0));
                var brushOrange = new SolidBrush(Color.Orange);
                var penBlk = new Pen(brushBlk, fLineWidth);
                var penRed = new Pen(brushRed, fLineWidth);
                var penLGrn = new Pen(brushLGrn, fLineWidth);
                var penGry = new Pen(brushGry, fLineWidth);
                var penYellow = new Pen(brushYellow, fLineWidth);
                var penOrange = new Pen(brushOrange, fLineWidth);

                graph.FillRectangle(Brushes.White, 0.0f, 0.0f, dWidth, dHeight);

                double dX = dBorder;
                double dX2 = dX + 3 * dBorder;
                double dY = dHeight - dBorder;

                DrawLine(dX, dY, dX2, dY, penYellow, graph, dHeight);
                DrawStringY("MPO Target", dX2 + dBorder, dY, font, brushYellow, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penRed, graph, dHeight);
                DrawStringY("Target", dX2 + dBorder, dY, font, brushRed, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penBlk, graph, dHeight);
                DrawStringY("Level Curve", dX2 + dBorder, dY, font, brushBlk, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penOrange, graph, dHeight);
                DrawStringY("Fog Curve", dX2 + dBorder, dY, font, brushOrange, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penLGrn, graph, dHeight);
                DrawStringY("Tinn. Curve", dX2 + dBorder, dY, font, brushLGrn, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penGry, graph, dHeight);
                DrawStringY("Other Curve", dX2 + dBorder, dY, font, brushGry, graph, dHeight);

                if (stream != null)
                    mGraph.Save(stream, ImageFormat.Png);

                mGraph.Dispose();
                font.Dispose();
                graph.Dispose();
                brushBlk.Dispose();
                brushRed.Dispose();
                brushLGrn.Dispose();
                brushGry.Dispose();
                brushYellow.Dispose();
                brushOrange.Dispose();
                penBlk.Dispose();
                penRed.Dispose();
                penLGrn.Dispose();
                penGry.Dispose();
                penYellow.Dispose();
                penOrange.Dispose();
            }
            catch (Exception exc)
            {
                if (LastError != null) LastError += "\n";
                LastError += exc.Message;
                if (exc.InnerException != null) LastError += exc.InnerException.Message;
            }
        }

        private static void DrawCurve(double dHeight, double dXMin, double dYMin, Pen penRed, Graphics graph,
                                         FreqCrv crv, double dYScale, double dXScale, double dBorder)
        {
            double dX1 = 0;
            double dY1 = 0;
            for (var ii = 0; ii < crv.Count; ii++)
            {
                if (ii == 0)
                {
                    dX1 = Value2PixelX(crv[ii].X, dBorder, dXScale, dXMin);
                    dY1 = dBorder + dYScale * (crv[ii].Y - dYMin);
                }
                else
                {
                    var dX2 = Value2PixelX(crv[ii].X, dBorder, dXScale, dXMin);
                    var dY2 = dBorder + dYScale * (crv[ii].Y - dYMin);
                    DrawLine(dX1, dY1, dX2, dY2, penRed, graph, dHeight);
                    dX1 = dX2;
                    dY1 = dY2;
                }
            }
        }

        private static void DrawLine(double dX1, double dY1, double dX2, double dY2,
                              Pen pen, Graphics graph, double dHeight)
        {
            graph.DrawLine(pen, (float)dX1, (float)(dHeight - dY1), (float)dX2, (float)(dHeight - dY2));
        }

        private static void DrawStringX(string sString, double dX, double dY,
                                 Font font, Brush brush, Graphics graph, double dHeight)
        {
            //draw the string centered along x-axis
            var fX = (float)dX;
            var fY = (float)dY;
            var pt = new PointF(fX, (float)dHeight - fY);
            var fmt = new StringFormat { Alignment = StringAlignment.Center };
            graph.DrawString(sString, font, brush, pt, fmt);
        }

        private static void DrawStringY(string sString, double dX, double dY,
                                 Font font, Brush brush, Graphics graph, double dHeight)
        {
            //draw the string centered along y-axis
            var fX = (float)dX;
            var fY = (float)dY;
            var pt = new PointF(fX, (float)dHeight - fY);
            var fmt = new StringFormat { LineAlignment = StringAlignment.Center };
            graph.DrawString(sString, font, brush, pt, fmt);
        }

        //helper functions for graphic
        private static double Value2PixelX(double dValue, double dBorder, double dXScale, double dXMin)
        {
            double dPixelPos = 0;

            if (LinearFreqAxis)
            {
                dPixelPos = dBorder + dXScale * (dValue - dXMin);   //linear x axis
            }
            else
            {
                if (dXMin != 0)
                {
                    dPixelPos = dBorder + dXScale * Math.Log10(dValue / dXMin);   //logarithmic x axis
                }

            }
            return dPixelPos;
        }

        private static bool AreCurvesAvailable(IList<FreqCrv> curves)
        {
            var areCurvesAvailable = (curves != null) && (curves.Count > 0) && (curves[0].Count > 0);
            return areCurvesAvailable;
        }

    }
}

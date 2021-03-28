using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CurveChartImageCreator
{
    internal class GraphicCurveResources
    {
        public static GraphicCurveResources Create(uint dWidth, uint dHeight, double dFontSize, float fLineWidth)
        {
            return new GraphicCurveResources(dWidth, dHeight, dFontSize, fLineWidth);
        }

        private GraphicCurveResources(uint dWidth, uint dHeight, double dFontSize, float fLineWidth)
        {
            DWidth = dWidth;
            DHeight = dHeight;
            DFontSize = dFontSize;
            FLineWidth = fLineWidth;

            MGraph = new Bitmap((int)dWidth, (int)dHeight, PixelFormat.Format32bppArgb);
            Font = new Font("Lucida Sans Unicode", (int)dFontSize);
            Graph = Graphics.FromImage(MGraph);
            BrushBlk = new SolidBrush(Color.Black);
            BrushRed = new SolidBrush(Color.Red);
            BrushLGrn = new SolidBrush(Color.DarkGreen);
            BrushGry = new SolidBrush(Color.DarkGray);
            BrushYellow = new SolidBrush(Color.FromArgb(255, 255, 230, 0));
            BrushOrange = new SolidBrush(Color.Orange);
            BrushBlue = new SolidBrush(Color.Blue);
            BrushDarkBlue = new SolidBrush(Color.DarkBlue);
            PenBlk = new Pen(BrushBlk, fLineWidth);
            PenRed = new Pen(BrushRed, fLineWidth);
            PenLGrn = new Pen(BrushLGrn, fLineWidth);
            PenGry = new Pen(BrushGry, 1f);
            PenYellow = new Pen(BrushYellow, fLineWidth);
            PenOrange = new Pen(BrushOrange, fLineWidth);
            PenBlue = new Pen(BrushBlue, fLineWidth);
            PenDarkBlue = new Pen(BrushDarkBlue, fLineWidth);

            Graph.SmoothingMode = SmoothingMode.AntiAlias;
            Graph.FillRectangle(Brushes.White, 0.0f, 0.0f, dWidth, dHeight);
        }

        public Bitmap MGraph { get; }
        public Font Font { get; }
        public Graphics Graph { get; }
        public SolidBrush BrushBlk { get; }
        public SolidBrush BrushRed { get; }
        public SolidBrush BrushLGrn { get; }
        public SolidBrush BrushGry { get; }
        public SolidBrush BrushYellow { get; }
        public SolidBrush BrushOrange { get; }
        public SolidBrush BrushBlue { get; }
        public SolidBrush BrushDarkBlue { get; }
        public Pen PenBlk { get; }
        public Pen PenRed { get; }
        public Pen PenLGrn { get; }
        public Pen PenGry { get; }
        public Pen PenYellow { get; }
        public Pen PenOrange { get; }
        public Pen PenBlue { get; }
        public Pen PenDarkBlue { get; }
        public uint DWidth { get; }
        public uint DHeight { get; }
        public double DFontSize { get; }
        public float FLineWidth { get; }

        public void Dispose()
        {
            MGraph.Dispose();

            Font.Dispose();
            Graph.Dispose();

            BrushBlk.Dispose();
            BrushRed.Dispose();
            BrushLGrn.Dispose();
            BrushGry.Dispose();
            BrushYellow.Dispose();
            BrushOrange.Dispose();
            BrushBlue.Dispose();
            BrushDarkBlue.Dispose();
            PenBlk.Dispose();
            PenRed.Dispose();
            PenLGrn.Dispose();
            PenGry.Dispose();
            PenYellow.Dispose();
            PenOrange.Dispose();
            PenBlue.Dispose();
            PenDarkBlue.Dispose();
        }
    }
}

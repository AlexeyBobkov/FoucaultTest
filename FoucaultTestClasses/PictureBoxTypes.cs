using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace FoucaultTestClasses
{
    public enum UIModeE
    {
        Off,
        SelectMirrorBound,
        ShowZones
    };

    public enum ZoneVisualizationE
    {
        Off,
        ZoneBoundsOnly,
        ActiveOnly
    };

    // options
    public struct Options
    {
        public Color SelectPenColor;
        public Color InactiveZoneColor;
        public Color ActiveZoneColor;
        public double ZoneHeight;
        public int SideTolerance;
        public float ZoneAngle;
        public int TimeAveragingCnt;
        public float AutoPrecision;
        public double AutoStabilizationTime;
    };

    // UI handler interfaces
    public interface IPictureUIHandler : IDisposable
    {
        void OnPaint(PaintEventArgs e);
        void OnMouseDown(MouseEventArgs e);
        void OnMouseMove(MouseEventArgs e);
        void OnMouseUp(MouseEventArgs e);
        bool IsImageUpdateEnabled();
    }
    public interface IPictureUISelMirrorBoundData : IDisposable
    {
        RectangleF MirrorBound { get; set; }
        event EventHandler MirrorBoundChanged;
    }
    public interface IPictureUIUpdateZoneData : IDisposable
    {
        RectangleF MirrorBound { get; set; }
        double[] ZoneBounds { get; set; }
        int ActiveZone { get; set; }
    }

    // Picture interface for zone handler
    public interface IPictureBase
    {
        // conversion
        PointF Client2Image(Point clientPt);
        SizeF Client2Image(Size clientSize);
        RectangleF Client2Image(Rectangle clientRect);
        Point Image2Client(PointF imagePt);
        Size Image2Client(SizeF imageSize);
        Rectangle Image2Client(RectangleF imageRect);
        event EventHandler ConversionChanged;

        // image
        Rectangle ImageBound { get; }

        // windows UI
        bool Capture { get; set; }
        Cursor Cursor { get; set; }
        void Invalidate();
        void Invalidate(Rectangle rc);
        void OnDefaultPaint(PaintEventArgs e);
    }

    public enum CalcBrightnessModeE
    {
        Median = 0,
        Mean = 1,

        Size = 2
    }

    public interface ICalcBrightness : IDisposable
    {
        RectangleF MirrorBoundAbs { get; set; }
        double[] ZoneBounds { get; set; }
        string FloatFormat(CalcBrightnessModeE mode);
        void GetBrightness(Bitmap bitmap, int activeZone, CalcBrightnessModeE mode, ref float l, ref float r);
    }

    public struct ZoneReading
    {
        public List<double> seq_;
    }
}

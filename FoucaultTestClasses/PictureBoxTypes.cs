using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

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
        ActiveOnly,
        ZoneBoundsOnly2,
        ActiveOnly2
    };

    // options
    public struct UIOptions
    {
        public Color selectPenColor_;
        public Color inactiveZoneColor_, activeZoneColor_;
        public double zoneHeight_;
        public int sideTolerance_;
        public float zoneAngle_;
    };

    // UI handler interfaces
    public interface PictureUIHandler : IDisposable
    {
        void OnPaint(PaintEventArgs e);
        void OnMouseDown(MouseEventArgs e);
        void OnMouseMove(MouseEventArgs e);
        void OnMouseUp(MouseEventArgs e);
        bool IsImageUpdateEnabled();
    }
    public interface PictureUISelMirrorBoundData : IDisposable
    {
        RectangleF MirrorBound { get; set; }
        event EventHandler MirrorBoundChanged;
    }
    public interface PictureUIUpdateZoneData : IDisposable
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
}

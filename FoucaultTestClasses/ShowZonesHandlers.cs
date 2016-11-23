using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FoucaultTestClasses
{
    ////////////////////////////////////////////////////////////////////////////////////
    // Show Zone Base UI handler
    public abstract class UIShowZoneBaseHandler : PictureUIHandler, PictureUIUpdateZoneData
    {
        public UIShowZoneBaseHandler(IPictureBase pict, RectangleF mirrorBound, UIOptions options)
        {
            pict_ = pict;
            mirrorBound_ = mirrorBound;
            ClientBound = pict_.Image2Client(mirrorBound_);
            pict_.ConversionChanged += OnConversionChanged;
        }

        public virtual void Dispose()
        {
            if (!disposed_)
            {
                disposed_ = true;
                pict_.ConversionChanged -= OnConversionChanged;
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            if (mirrorBound_.IsEmpty)
            {
                pict_.OnDefaultPaint(e);
                DrawMessage(e.Graphics, "Select Mirror Bound!");
            }
            else if (ZoneBounds == null)
            {
                pict_.OnDefaultPaint(e);
                DrawMessage(e.Graphics, "No zone information loaded!");
            }
            else
                OnPaintZones(e);
        }
        public void OnMouseDown(MouseEventArgs e) { }
        public void OnMouseMove(MouseEventArgs e) { pict_.Cursor = null; }
        public void OnMouseUp(MouseEventArgs e) { }

        public bool IsImageUpdateEnabled() { return true; }

        public double[] ZoneBounds
        {
            get { return zoneBounds_; }
            set
            {
                zoneBounds_ = value;
                OnZoneBoundsChanged();
            }
        }
        public RectangleF MirrorBound
        {
            get { return mirrorBound_; }
            set
            {
                if (mirrorBound_ != value)
                {
                    mirrorBound_ = value;
                    ClientBound = pict_.Image2Client(mirrorBound_);
                    RebuildZoneData();
                    pict_.Invalidate();
                }
            }
        }
        public int ActiveZone
        {
            get { return activeZone_; }
            set
            {
                if (activeZone_ != value)
                {
                    activeZone_ = value;
                    OnActiveZoneChanged();
                }
            }
        }

        protected virtual void OnZoneBoundsChanged()
        {
            RebuildZoneData();
            pict_.Invalidate();
        }
        protected abstract void OnActiveZoneChanged();

        protected abstract void RebuildZoneData();
        protected abstract void OnPaintZones(PaintEventArgs e);

        protected void DrawMessage(Graphics graphics, string text)
        {
            using (Font font = new Font("Arial", 24))
            {
                using (SolidBrush brush = new SolidBrush(System.Drawing.Color.Red))
                {
                    Rectangle imageBound = pict_.ImageBound;
                    SizeF textSize = graphics.MeasureString(text, font);
                    float x = imageBound.Left + imageBound.Width / 2 - textSize.Width / 2;
                    float y = imageBound.Top + imageBound.Height / 2 - textSize.Height / 2;
                    graphics.DrawString(text, font, brush, x, y);
                }
            }
        }

        protected Rectangle ClientBound { get; private set; }

        private IPictureBase pict_;
        private double[] zoneBounds_;
        private int activeZone_;
        private RectangleF mirrorBound_;
        private bool disposed_ = false;

        private void OnConversionChanged(object sender, EventArgs e)
        {
            ClientBound = pict_.Image2Client(mirrorBound_);
            RebuildZoneData();
            pict_.Invalidate();
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////
    // Show Active Zone UI handler
    public class UIShowActiveZoneHandler : UIShowZoneBaseHandler
    {
        public UIShowActiveZoneHandler(IPictureBase pict, RectangleF mirrorBound, UIOptions options)
            : base(pict, mirrorBound, options)
        {
            pict_ = pict;
            options_ = options;
        }

        public override void Dispose()
        {
            base.Dispose();
            DisposeZoneData();
        }

        protected override void OnActiveZoneChanged()
        {
            pict_.Invalidate();
        }

        protected override void RebuildZoneData()
        {
            DisposeZoneData();
            if (ZoneBounds == null || ZoneBounds.Length <= 1 || MirrorBound.IsEmpty)
                zoneData_ = null;
            else
            {
                zoneData_ = new ZoneData[ZoneBounds.Length - 1];

                // ellipse parameters
                int a = ClientBound.Width / 2, b = ClientBound.Height / 2;  // axis
                mirrorCenterX_ = ClientBound.Left + a;                      // center
                mirrorCenterY_ = ClientBound.Top + b;

                for (int i = zoneData_.Length; --i >= 0; )
                {
                    double inner = ZoneBounds[i], outer = ZoneBounds[i + 1];

                    float innerA = (float)(a * inner + 2), innerB = (float)(b * inner + 2);
                    float outerA = (float)(a * outer - 2), outerB = (float)(b * outer - 2);

                    Region r;
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddPie(mirrorCenterX_ - outerA, mirrorCenterY_ - outerB, 2 * outerA, 2 * outerB, 180 - options_.zoneAngle_, 2 * options_.zoneAngle_);
                        path.AddPie(mirrorCenterX_ - outerA, mirrorCenterY_ - outerB, 2 * outerA, 2 * outerB, -options_.zoneAngle_, 2 * options_.zoneAngle_);
                        r = new Region(path);
                    }
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(mirrorCenterX_ - innerA, mirrorCenterY_ - innerB, 2 * innerA, 2 * innerB);
                        r.Exclude(path);
                    }

                    Region r1;
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(ClientBound);
                        r1 = new Region(path);
                    }
                    r1.Exclude(r);
                    zoneData_[i].mask_ = r1;
                }
            }
        }

        protected override void OnPaintZones(PaintEventArgs e)
        {
            Region rClip = e.Graphics.Clip;
            Region zoneMask = zoneData_[ActiveZone].mask_;

            using (Region r = rClip.Clone(), r1 = rClip.Clone())
            {
                r.Exclude(zoneMask);
                e.Graphics.Clip = r;
                pict_.OnDefaultPaint(e);

                r1.Intersect(zoneMask);
                e.Graphics.Clip = r1;
                e.Graphics.FillEllipse(Brushes.Black, ClientBound);

                e.Graphics.Clip = rClip;
            }
        }

        private struct ZoneData
        {
            public Region mask_;
        }

        private IPictureBase pict_;
        private UIOptions options_;
        private int mirrorCenterX_, mirrorCenterY_;
        private ZoneData[] zoneData_;

        private void DisposeZoneData()
        {
            if (zoneData_ != null)
            {
                foreach (ZoneData zd in zoneData_)
                {
                    if (zd.mask_ != null)
                        zd.mask_.Dispose();
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////
    // Show Zone Bounds UI handler
    public class UIShowZoneBoundsHandler : UIShowZoneBaseHandler
    {
        public UIShowZoneBoundsHandler(IPictureBase pict, RectangleF mirrorBound, UIOptions options)
            : base(pict, mirrorBound, options)
        {
            pict_ = pict;
            options_ = options;
        }

        protected override void OnActiveZoneChanged()
        {
            pict_.Invalidate();
        }

        protected override void RebuildZoneData()
        {
            if (ZoneBounds == null || ZoneBounds.Length <= 1 || MirrorBound.IsEmpty)
                zoneData_ = null;
            else
            {
                zoneData_ = new ZoneData[ZoneBounds.Length - 1];

                // ellipse parameters
                int a = ClientBound.Width / 2, b = ClientBound.Height / 2;  // axis
                mirrorCenterX_ = ClientBound.Left + a;                      // center
                mirrorCenterY_ = ClientBound.Top + b;

                for (int i = zoneData_.Length; --i >= 0; )
                {
                    double inner = ZoneBounds[i], outer = ZoneBounds[i + 1];

                    float zoneH = (float)(options_.zoneHeight_ * ClientBound.Height / 2);
                    float innerA = (float)(a * inner + 2), innerB = (float)(b * inner + 2);
                    float outerA = (float)(a * outer - 2), outerB = (float)(b * outer - 2);

                    zoneData_[i].h_ = zoneH;
                    zoneData_[i].innerA_ = innerA;
                    zoneData_[i].innerB_ = innerB;
                    zoneData_[i].outerA_ = outerA;
                    zoneData_[i].outerB_ = outerB;

                    if (zoneH < outerB)
                    {
                        double tmp = zoneH / outerB;
                        zoneData_[i].outerX_ = (float)(outerA * Math.Sqrt(1 - tmp * tmp));
                        if (zoneH < innerB)
                        {
                            tmp = zoneH / zoneData_[i].innerB_;
                            zoneData_[i].innerX_ = (float)(innerA * Math.Sqrt(1 - tmp * tmp));
                        }
                    }
                }
            }
        }

        protected override void OnPaintZones(PaintEventArgs e)
        {
            pict_.OnDefaultPaint(e);
            if (zoneData_ != null)
                for (int i = 0; i < zoneData_.Length; ++i)
                    PaintZone(e.Graphics, i, i == ActiveZone);
        }

        private struct ZoneData
        {
            public float h_;
            public float innerA_, outerA_;
            public float innerB_, outerB_;
            public float innerX_, outerX_;
        }

        private IPictureBase pict_;
        private UIOptions options_;
        private int mirrorCenterX_, mirrorCenterY_;
        private ZoneData[] zoneData_;

        private static void PaintArcs(Graphics g, Pen pen, int centerX, int centerY, float zoneA, float zoneB, float angle)
        {
            g.DrawArc(pen, centerX - zoneA, centerY - zoneB, 2 * zoneA, 2 * zoneB, 180 - angle, 2 * angle);
            g.DrawArc(pen, centerX - zoneA, centerY - zoneB, 2 * zoneA, 2 * zoneB, -angle, 2 * angle);
        }

        private void PaintZone(Graphics g, int i, bool active)
        {
            Pen pen = active ? Pens.Red : Pens.Black;

            float innerA = zoneData_[i].innerA_, innerB = zoneData_[i].innerB_;
            float outerA = zoneData_[i].outerA_, outerB = zoneData_[i].outerB_;

            PaintArcs(g, pen, mirrorCenterX_, mirrorCenterY_, outerA, outerB, options_.zoneAngle_);
            PaintArcs(g, pen, mirrorCenterX_, mirrorCenterY_, innerA, innerB, options_.zoneAngle_);

            double tgA = Math.Tan(options_.zoneAngle_ * Math.PI / 180);
            float innerX = (float)Math.Sqrt(1 / (1 / (innerA * innerA) + (tgA * tgA) / (innerB * innerB)));
            float innerY = (float)(innerX * tgA);
            float outerX = (float)Math.Sqrt(1 / (1 / (outerA * outerA) + (tgA * tgA) / (outerB * outerB)));
            float outerY = (float)(outerX * tgA);

            g.DrawLine(pen, mirrorCenterX_ - innerX, mirrorCenterY_ - innerY, mirrorCenterX_ - outerX, mirrorCenterY_ - outerY);
            g.DrawLine(pen, mirrorCenterX_ - innerX, mirrorCenterY_ + innerY, mirrorCenterX_ - outerX, mirrorCenterY_ + outerY);
            g.DrawLine(pen, mirrorCenterX_ + innerX, mirrorCenterY_ - innerY, mirrorCenterX_ + outerX, mirrorCenterY_ - outerY);
            g.DrawLine(pen, mirrorCenterX_ + innerX, mirrorCenterY_ + innerY, mirrorCenterX_ + outerX, mirrorCenterY_ + outerY);
        }
    }

    
    ////////////////////////////////////////////////////////////////////////////////////
    // Show Active Zone UI handler
    public class UIShowActiveZoneHandler2 : UIShowZoneBaseHandler
    {
        public UIShowActiveZoneHandler2(IPictureBase pict, RectangleF mirrorBound, UIOptions options)
            : base(pict, mirrorBound, options)
        {
            pict_ = pict;
            options_ = options;
        }

        public override void Dispose()
        {
            base.Dispose();
            DisposeZoneData();
        }

        protected override void OnActiveZoneChanged()
        {
            pict_.Invalidate();
        }

        protected override void RebuildZoneData()
        {
            DisposeZoneData();
            if (ZoneBounds == null || ZoneBounds.Length <= 1 || MirrorBound.IsEmpty)
                zoneData_ = null;
            else
            {
                zoneData_ = new ZoneData[ZoneBounds.Length - 1];

                // ellipse parameters
                int a = ClientBound.Width / 2, b = ClientBound.Height / 2;  // axis
                mirrorCenterX_ = ClientBound.Left + a;                      // center
                mirrorCenterY_ = ClientBound.Top + b;

                for (int i = zoneData_.Length; --i >= 0; )
                {
                    double inner = ZoneBounds[i], outer = ZoneBounds[i + 1];

                    float zoneH = (float)(options_.zoneHeight_ * ClientBound.Height / 2);
                    float innerA = (float)(a * inner + 2), innerB = (float)(b * inner + 2);
                    float outerA = (float)(a * outer - 2), outerB = (float)(b * outer - 2);

                    Region r = new Region(new RectangleF(mirrorCenterX_ - outerA, mirrorCenterY_ - zoneH, 2 * outerA, 2 * zoneH));
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(new RectangleF(mirrorCenterX_ - outerA, mirrorCenterY_ - outerB, 2 * outerA, 2 * outerB));
                        r.Intersect(path);
                    }
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(new RectangleF(mirrorCenterX_ - innerA, mirrorCenterY_ - innerB, 2 * innerA, 2 * innerB));
                        r.Exclude(path);
                    }

                    Region r1;
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(ClientBound);
                        r1 = new Region(path);
                    }
                    r1.Exclude(r);
                    zoneData_[i].mask_ = r1;
                }
            }
        }

        protected override void OnPaintZones(PaintEventArgs e)
        {
            Region rClip = e.Graphics.Clip;
            Region zoneMask = zoneData_[ActiveZone].mask_;

            using (Region r = rClip.Clone(), r1 = rClip.Clone())
            {
                r.Exclude(zoneMask);
                e.Graphics.Clip = r;
                pict_.OnDefaultPaint(e);

                r1.Intersect(zoneMask);
                e.Graphics.Clip = r1;
                e.Graphics.FillEllipse(Brushes.Black, ClientBound);

                e.Graphics.Clip = rClip;
            }
        }

        private struct ZoneData
        {
            public Region mask_;
        }

        private IPictureBase pict_;
        private UIOptions options_;
        private int mirrorCenterX_, mirrorCenterY_;
        private ZoneData[] zoneData_;

        private void DisposeZoneData()
        {
            if (zoneData_ != null)
            {
                foreach (ZoneData zd in zoneData_)
                {
                    if (zd.mask_ != null)
                        zd.mask_.Dispose();
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////
    // Show Zone Bounds UI handler
    public class UIShowZoneBoundsHandler2 : UIShowZoneBaseHandler
    {
        public UIShowZoneBoundsHandler2(IPictureBase pict, RectangleF mirrorBound, UIOptions options)
            : base(pict, mirrorBound, options)
        {
            pict_ = pict;
            options_ = options;
        }

        protected override void OnActiveZoneChanged()
        {
            pict_.Invalidate();
        }

        protected override void RebuildZoneData()
        {
            if (ZoneBounds == null || ZoneBounds.Length <= 1 || MirrorBound.IsEmpty)
                zoneData_ = null;
            else
            {
                zoneData_ = new ZoneData[ZoneBounds.Length - 1];

                // ellipse parameters
                int a = ClientBound.Width / 2, b = ClientBound.Height / 2;  // axis
                mirrorCenterX_ = ClientBound.Left + a;                      // center
                mirrorCenterY_ = ClientBound.Top + b;

                for (int i = zoneData_.Length; --i >= 0; )
                {
                    double inner = ZoneBounds[i], outer = ZoneBounds[i + 1];

                    float zoneH = (float)(options_.zoneHeight_ * ClientBound.Height / 2);
                    float innerA = (float)(a * inner + 2), innerB = (float)(b * inner + 2);
                    float outerA = (float)(a * outer - 2), outerB = (float)(b * outer - 2);

                    zoneData_[i].h_ = zoneH;
                    zoneData_[i].innerA_ = innerA;
                    zoneData_[i].innerB_ = innerB;
                    zoneData_[i].outerA_ = outerA;
                    zoneData_[i].outerB_ = outerB;

                    if (zoneH < outerB)
                    {
                        double tmp = zoneH / outerB;
                        zoneData_[i].outerX_ = (float)(outerA * Math.Sqrt(1 - tmp * tmp));
                        if (zoneH < innerB)
                        {
                            tmp = zoneH / zoneData_[i].innerB_;
                            zoneData_[i].innerX_ = (float)(innerA * Math.Sqrt(1 - tmp * tmp));
                        }
                    }
                }
            }
        }

        protected override void OnPaintZones(PaintEventArgs e)
        {
            pict_.OnDefaultPaint(e);
            if (zoneData_ != null)
                for (int i = 0; i < zoneData_.Length; ++i)
                    PaintZone(e.Graphics, i, i == ActiveZone);
        }

        private struct ZoneData
        {
            public float h_;
            public float innerA_, outerA_;
            public float innerB_, outerB_;
            public float innerX_, outerX_;
        }

        private IPictureBase pict_;
        private UIOptions options_;
        private int mirrorCenterX_, mirrorCenterY_;
        private ZoneData[] zoneData_;

        private static void PaintArcs(Graphics g, Pen pen, int centerX, int centerY, float x, float zoneA, float zoneB, float zoneH)
        {
            float angle = (float)(Math.Atan(zoneH / x) * 180 / Math.PI);
            g.DrawArc(pen, centerX - zoneA, centerY - zoneB, 2 * zoneA, 2 * zoneB, 180 - angle, 2 * angle);
            g.DrawArc(pen, centerX - zoneA, centerY - zoneB, 2 * zoneA, 2 * zoneB, -angle, 2 * angle);
        }

        private void PaintZone(Graphics g, int i, bool active)
        {
            Pen pen = active ? Pens.Red : Pens.Black;

            float zoneH = zoneData_[i].h_;
            float innerA = zoneData_[i].innerA_, innerB = zoneData_[i].innerB_;
            float outerA = zoneData_[i].outerA_, outerB = zoneData_[i].outerB_;
            if (zoneH < outerB)
            {
                float outerX = zoneData_[i].outerX_;
                PaintArcs(g, pen, mirrorCenterX_, mirrorCenterY_, outerX, outerA, outerB, zoneH);
                if (zoneH < innerB)
                {
                    float innerX = zoneData_[i].innerX_;
                    PaintArcs(g, pen, mirrorCenterX_, mirrorCenterY_, innerX, innerA, innerB, zoneH);
                    g.DrawLine(pen, mirrorCenterX_ - innerX, mirrorCenterY_ - zoneH, mirrorCenterX_ - outerX, mirrorCenterY_ - zoneH);
                    g.DrawLine(pen, mirrorCenterX_ - innerX, mirrorCenterY_ + zoneH, mirrorCenterX_ - outerX, mirrorCenterY_ + zoneH);
                    g.DrawLine(pen, mirrorCenterX_ + innerX, mirrorCenterY_ - zoneH, mirrorCenterX_ + outerX, mirrorCenterY_ - zoneH);
                    g.DrawLine(pen, mirrorCenterX_ + innerX, mirrorCenterY_ + zoneH, mirrorCenterX_ + outerX, mirrorCenterY_ + zoneH);
                }
                else
                {
                    g.DrawEllipse(pen, mirrorCenterX_ - innerA, mirrorCenterY_ - innerB, 2 * innerA, 2 * innerB);
                    g.DrawLine(pen, mirrorCenterX_ - outerX, mirrorCenterY_ - zoneH, mirrorCenterX_ + outerX, mirrorCenterY_ - zoneH);
                    g.DrawLine(pen, mirrorCenterX_ - outerX, mirrorCenterY_ + zoneH, mirrorCenterX_ + outerX, mirrorCenterY_ + zoneH);
                }
            }
        }
    }
}

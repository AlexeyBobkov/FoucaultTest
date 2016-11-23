using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FoucaultTestClasses
{
    public abstract class CalcBrightnessBase : ICalcBrightness
    {
        public CalcBrightnessBase(RectangleF mirrorBound, double[] zoneBounds)
        {
            mirrorBound_ = mirrorBound;
            zoneBounds_ = zoneBounds;
        }

        public RectangleF MirrorBound
        {
            get { return mirrorBound_; }
            set
            {
                if (mirrorBound_ != value)
                {
                    mirrorBound_ = value;
                    RebuildZoneData();
                }
            }
        }

        public double[] ZoneBounds
        {
            get { return zoneBounds_; }
            set
            {
                zoneBounds_ = value;
                OnZoneBoundsChanged();
            }
        }

        public abstract void Dispose(); 
        public abstract bool GetBrightness(Image image, int activeZone, ref float l, ref float r);

        protected abstract void RebuildZoneData();
        protected virtual void OnZoneBoundsChanged()
        {
            RebuildZoneData();
        }

        private double[] zoneBounds_;
        private RectangleF mirrorBound_; 
    }

    public class CalcBrightness1 : CalcBrightnessBase
    {
        public CalcBrightness1(RectangleF mirrorBound, double[] zoneBounds, CalcOptions calcOptions)
            : base(mirrorBound, zoneBounds)
        {
            options_ = calcOptions;
        }

        public override bool GetBrightness(Image image, int activeZone, ref float l, ref float r)
        {
            return false;
        }

        public override void Dispose()
        {
            DisposeZoneData();
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
                float a = MirrorBound.Width / 2, b = MirrorBound.Height / 2;    // axis
                float mirrorCenterX = MirrorBound.Left + a;                     // center
                float mirrorCenterY = MirrorBound.Top + b;

                for (int i = zoneData_.Length; --i >= 0; )
                {
                    double inner = ZoneBounds[i], outer = ZoneBounds[i + 1];

                    float innerA = (float)(a * inner + 2), innerB = (float)(b * inner + 2);
                    float outerA = (float)(a * outer - 2), outerB = (float)(b * outer - 2);

                    // we assume that the ellipse is almost a circle
                    float angle = (float)((360 * options_.calcBrightnessPixelNum_) / (Math.PI * (outerA * outerA - innerA * innerA)));

                    Region rl, rr;
                    using (var path = new GraphicsPath())
                    {
                        path.AddPie(mirrorCenterX - outerA, mirrorCenterY - outerB, 2 * outerA, 2 * outerB, 180 - angle, 2 * angle);
                        rl = new Region(path);
                    }
                    using (var path = new GraphicsPath())
                    {
                        path.AddPie(mirrorCenterX - outerA, mirrorCenterY - outerB, 2 * outerA, 2 * outerB, -angle, 2 * angle);
                        rr = new Region(path);
                    }
                    using (var path = new GraphicsPath())
                    {
                        path.AddEllipse(mirrorCenterX - innerA, mirrorCenterY - innerB, 2 * innerA, 2 * innerB);
                        rl.Exclude(path);
                        rr.Exclude(path);
                    }

                    zoneData_[i].maskL_ = rl;
                    zoneData_[i].maskR_ = rr;
                }
            }
        }

        private struct ZoneData
        {
            public Region maskL_, maskR_;
        }

        private ZoneData[] zoneData_;
        private CalcOptions options_;

        private float GetRegionBrightness(Image image, Region r)
        {
            return 0F;
        }

        private void DisposeZoneData()
        {
            if (zoneData_ != null)
            {
                foreach (ZoneData zd in zoneData_)
                {
                    if (zd.maskL_ != null)
                        zd.maskL_.Dispose();
                    if (zd.maskR_ != null)
                        zd.maskR_.Dispose();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace FoucaultTestClasses
{
    public abstract class CalcBrightnessBase : ICalcBrightness
    {
        public CalcBrightnessBase(RectangleF mirrorBoundAbs, double[] zoneBounds)
        {
            mirrorBoundAbs_ = mirrorBoundAbs;
            zoneBounds_ = zoneBounds;
        }

        public RectangleF MirrorBoundAbs
        {
            get { return mirrorBoundAbs_; }
            set
            {
                if (mirrorBoundAbs_ != value)
                {
                    mirrorBoundAbs_ = value;
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

        public abstract string FloatFormat(CalcBrightnessModeE mode);
        public abstract void Dispose();
        public abstract void GetBrightness(Bitmap bitmap, int activeZone, CalcBrightnessModeE mode, ref float l, ref float r);

        protected abstract void RebuildZoneData();
        protected virtual void OnZoneBoundsChanged()
        {
            RebuildZoneData();
        }

        private double[] zoneBounds_;
        private RectangleF mirrorBoundAbs_; 
    }


    ////////////////////////////////////////////////////////////////////////////////////
    public abstract class CalcBrightnessPieZonesBase : CalcBrightnessBase
    {
        public CalcBrightnessPieZonesBase(RectangleF mirrorBound, double[] zoneBounds, Options calcOptions)
            : base(mirrorBound, zoneBounds)
        {
            options_ = calcOptions;
            RebuildZoneData();
        }

        public override void GetBrightness(Bitmap bitmap, int activeZone, CalcBrightnessModeE mode, ref float l, ref float r)
        {
            l = GetRegionBrightness(mode, bitmap, zoneData_[activeZone].maskL_, zoneData_[activeZone].boundsL_, zoneData_[activeZone].areaL_);
            r = GetRegionBrightness(mode, bitmap, zoneData_[activeZone].maskR_, zoneData_[activeZone].boundsR_, zoneData_[activeZone].areaR_);
        }

        public override void Dispose()
        {
            DisposeZoneData();
        }

        protected override void RebuildZoneData()
        {
            DisposeZoneData();
            if (ZoneBounds == null || ZoneBounds.Length <= 1 || MirrorBoundAbs.IsEmpty)
                zoneData_ = null;
            else
            {
                zoneData_ = new ZoneData[ZoneBounds.Length - 1];

                // ellipse parameters
                float a = MirrorBoundAbs.Width / 2, b = MirrorBoundAbs.Height / 2;    // axis
                float mirrorCenterX = MirrorBoundAbs.Left + a;                     // center
                float mirrorCenterY = MirrorBoundAbs.Top + b;

                for (int i = zoneData_.Length; --i >= 0; )
                {
                    double inner = ZoneBounds[i], outer = ZoneBounds[i + 1];

                    float innerA = (float)(a * inner), innerB = (float)(b * inner);
                    float outerA = (float)(a * outer), outerB = (float)(b * outer);

                    float angle = (float)options_.ZoneAngle;

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
                    GetRegionParameters(rl, out zoneData_[i].boundsL_, out zoneData_[i].areaL_);
                    GetRegionParameters(rr, out zoneData_[i].boundsR_, out zoneData_[i].areaR_);
                }
            }
        }

        private struct ZoneData
        {
            public Region maskL_, maskR_;
            public Rectangle boundsL_, boundsR_;
            public int areaL_, areaR_;
        }

        private ZoneData[] zoneData_;
        private Options options_;

        private void GetRegionParameters(Region region, out Rectangle bounds, out int area)
        {
            area = 0;
            var rects = region.GetRegionScans(new Matrix());
            if (rects.Length <= 0)
            {
                bounds = new Rectangle();
                return;
            }
            int l = int.MaxValue, t = int.MaxValue, r = int.MinValue, b = int.MinValue;
            foreach (var rcF in rects)
            {
                Rectangle rc = Rectangle.Round(rcF);
                area += rc.Width * rc.Height;
                if(l > rc.Left)
                    l = rc.Left;
                if (t > rc.Top)
                    t = rc.Top;
                if (r < rc.Right)
                    r = rc.Right;
                if (b < rc.Bottom)
                    b = rc.Bottom;
            }
            bounds = new Rectangle(l, t, r - l, b - t);
        }

        protected abstract float GetRegionBrightness(CalcBrightnessModeE mode, Bitmap image, Region region, Rectangle bounds, int area);

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
    ////////////////////////////////////////////////////////////////////////////////////
    public class CalcBrightness : CalcBrightnessPieZonesBase
    {
        public CalcBrightness(RectangleF mirrorBound, double[] zoneBounds, Options calcOptions)
            : base(mirrorBound, zoneBounds, calcOptions)
        {
        }

        public override string FloatFormat(CalcBrightnessModeE mode) { return mode == CalcBrightnessModeE.Median ? "F1" : "F2"; }

        protected override float GetRegionBrightness(CalcBrightnessModeE mode, Bitmap image, Region region, Rectangle bounds, int area)
        {
            switch (mode)
            {
                case CalcBrightnessModeE.Mean:
                    return GetMeanRegionBrightness(image, region, bounds, area);
                default:
                case CalcBrightnessModeE.Median:
                    return GetMedianRegionBrightness(image, region, bounds, area);
            }
        }

        private float GetMeanRegionBrightness(Bitmap image, Region region, Rectangle bounds, int area)
        {
            if (area <= 0)
                return -1;

            // only this format is supported
            System.Diagnostics.Debug.Assert(image.PixelFormat == PixelFormat.Format24bppRgb);

            Int64 sum = 0;
            int area1 = 0;
            try
            {
                Rectangle imageRect = new Rectangle(new Point(0, 0), image.Size);
                bounds.Intersect(imageRect);
                if (bounds.IsEmpty)
                    return -1;

                BitmapData srcData = image.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                int pixelSize = 3;

                RectangleF[] rects = region.GetRegionScans(new Matrix());
                foreach (var rcF in rects)
                {
                    Rectangle rc = Rectangle.Round(rcF);
                    rc.Intersect(imageRect);
                    area1 += rc.Width * rc.Height;

                    int offsetX = rc.Left - bounds.Left, offsetY = rc.Top - bounds.Top;
                    for (int i = 0; i < rc.Height; i++)
                    {
                        unsafe
                        {
                            byte* row = (byte*)srcData.Scan0 + ((i + offsetY) * srcData.Stride) + offsetX * pixelSize;
                            for (int j = 0; j < rc.Width; j++)
                            {
                                sum += row[0] + row[1] + row[2];
                                row += pixelSize;
                            }
                        }
                    }
                }
                image.UnlockBits(srcData);
            }
            catch (InvalidOperationException)
            {
            }
            return area1 > 0 ? ((float)sum) / area1 : -1;
        }

        private float GetMedianRegionBrightness(Bitmap image, Region region, Rectangle bounds, int area)
        {
            if (area <= 0)
                return -1;

            // only this format is supported
            System.Diagnostics.Debug.Assert(image.PixelFormat == PixelFormat.Format24bppRgb);

            int[] pixels = new int[area];
            int idx = 0;
            try
            {
                Rectangle imageRect = new Rectangle(new Point(0, 0), image.Size);
                bounds.Intersect(imageRect);
                if (bounds.IsEmpty)
                    return -1;

                BitmapData srcData = image.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                int pixelSize = 3;

                RectangleF[] rects = region.GetRegionScans(new Matrix());
                foreach (var rcF in rects)
                {
                    Rectangle rc = Rectangle.Round(rcF);
                    rc.Intersect(imageRect);

                    int offsetX = rc.Left - bounds.Left, offsetY = rc.Top - bounds.Top;
                    for (int i = 0; i < rc.Height; i++)
                    {
                        unsafe
                        {
                            byte* row = (byte*)srcData.Scan0 + ((i + offsetY) * srcData.Stride) + offsetX * pixelSize;
                            for (int j = 0; j < rc.Width; j++)
                            {
                                pixels[idx++] = row[0] + row[1] + row[2];
                                row += pixelSize;
                            }
                        }
                    }
                }

                image.UnlockBits(srcData);
            }
            catch (InvalidOperationException)
            {
            }
            if (idx <= 0)
                return -1;

            Array.Sort(pixels, 0, idx);
            if (idx % 2 == 0)
                return (pixels[idx / 2 - 1] + pixels[idx / 2]) / 2.0F;
            else
                return pixels[idx / 2];
        }
    }
}

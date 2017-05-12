using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FoucaultTestClasses
{
    // default UI handler
    public class UIOffHandler : IPictureUIHandler
    {
        public UIOffHandler(IPictureBase pict) { pict_ = pict; }

        public void Dispose() { }
        public void OnPaint(PaintEventArgs e) { pict_.OnDefaultPaint(e); }
        public void OnMouseDown(MouseEventArgs e) { }
        public void OnMouseMove(MouseEventArgs e) { pict_.Cursor = null; }
        public void OnMouseUp(MouseEventArgs e) { }
        public bool IsImageUpdateEnabled() { return true; }

        private IPictureBase pict_;
    }

    // Select new bound UI handler
    public class UISelNewMirrorBoundHandler : IPictureUIHandler, IPictureUISelMirrorBoundData
    {
        public UISelNewMirrorBoundHandler(IPictureBase pict, Options options)
        {
            pict_ = pict;
            pen_ = new Pen(options.SelectPenColor, 1);
            pen_.DashPattern = new float[] { 6.0F, 3.0F };

            using (System.IO.MemoryStream cursorMemoryStream = new System.IO.MemoryStream(Properties.Resources.Selrect))
            {
                selRectCursor_ = new Cursor(cursorMemoryStream);
            }
        }

        public void Dispose() { pen_.Dispose(); selRectCursor_.Dispose(); }

        public void OnPaint(PaintEventArgs e)
        {
            pict_.OnDefaultPaint(e);
            if (!bound_.IsEmpty)
                e.Graphics.DrawRectangle(pen_, bound_);
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            pict_.Capture = true;
            pict_.Cursor = null;
            startPoint_ = new Point(e.X, e.Y);
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            if (pict_.Capture)
            {
                pict_.Cursor = null;
                Invalidate();
                int l = startPoint_.X < e.X ? startPoint_.X : e.X, r = startPoint_.X < e.X ? e.X : startPoint_.X,
                    t = startPoint_.Y < e.Y ? startPoint_.Y : e.Y, b = startPoint_.Y < e.Y ? e.Y : startPoint_.Y;
                bound_ = new Rectangle(l, t, r - l, b - t);
                Invalidate();
            }
            else
            {
                pict_.Cursor = selRectCursor_;
            }
        }
        public void OnMouseUp(MouseEventArgs e)
        {
            pict_.Cursor = selRectCursor_;
            pict_.Capture = false;

            MirrorBound = pict_.Client2Image(bound_);
            if (MirrorBoundChanged != null)
                MirrorBoundChanged(this, new EventArgs());
            Invalidate();
        }
        public bool IsImageUpdateEnabled() { return true; }

        public RectangleF MirrorBound { get; set; }
        public event EventHandler MirrorBoundChanged;

        private IPictureBase pict_;
        private Pen pen_;
        private Point startPoint_;
        private Rectangle bound_;

        private void Invalidate() { pict_.Invalidate(bound_); }
        private Cursor selRectCursor_;
    }

    // Select mirror bound UI handler
    public class UISelMirrorBoundHandler : IPictureUIHandler, IPictureUISelMirrorBoundData
    {
        public UISelMirrorBoundHandler(IPictureBase pict, RectangleF mirrorBound, Options options)
        {
            pict_ = pict;
            options_ = options;
            pen_ = new Pen(options.SelectPenColor, 1);
            pen_.DashPattern = new float[] { 6.0F, 3.0F };
            MirrorBound = mirrorBound;
            bound_ = pict_.Image2Client(mirrorBound);

            pict_.ConversionChanged += OnConversionChanged;
        }

        public void Dispose()
        {
            if (!disposed_)
            {
                disposed_ = true;
                pict_.ConversionChanged -= OnConversionChanged;
                pen_.Dispose();
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            pict_.OnDefaultPaint(e);
            if (!bound_.IsEmpty)
                e.Graphics.DrawRectangle(pen_, bound_);
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            startBound_ = bound_;
            startLocation_ = e.Location;

            Side side = GetSide(startLocation_);
            if (side != Side.None)
            {
                pict_.Capture = isCaptured_ = true;
                side_ = side;
            }
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            if (pict_.Capture)
            {
                Invalidate();
                switch (side_)
                {
                    case Side.UpperLeft:
                        bound_ = new Rectangle(e.X, e.Y, bound_.Right - e.X, bound_.Bottom - e.Y);
                        break;
                    case Side.LowerLeft:
                        bound_ = new Rectangle(e.X, bound_.Top, bound_.Right - e.X, e.Y - bound_.Top);
                        break;
                    case Side.UpperRight:
                        bound_ = new Rectangle(bound_.Left, e.Y, e.X - bound_.Left, bound_.Bottom - e.Y);
                        break;
                    case Side.LowerRight:
                        bound_ = new Rectangle(bound_.Left, bound_.Top, e.X - bound_.Left, e.Y - bound_.Top);
                        break;
                    case Side.Left:
                        bound_ = new Rectangle(e.X, bound_.Top, bound_.Right - e.X, bound_.Height);
                        break;
                    case Side.Right:
                        bound_ = new Rectangle(bound_.Left, bound_.Top, e.X - bound_.Left, bound_.Height);
                        break;
                    case Side.Top:
                        bound_ = new Rectangle(bound_.Left, e.Y, bound_.Width, bound_.Bottom - e.Y);
                        break;
                    case Side.Bottom:
                        bound_ = new Rectangle(bound_.Left, bound_.Top, bound_.Width, e.Y - bound_.Top);
                        break;
                    case Side.Move:
                        bound_ = new Rectangle(startBound_.Left + e.Location.X - startLocation_.X,
                                               startBound_.Top + e.Location.Y - startLocation_.Y,
                                               startBound_.Width, startBound_.Height);
                        break;
                }
                Invalidate();
            }
            else
            {
                Side side = GetSide(e.Location);
                switch (side)
                {
                    case Side.UpperLeft:
                    case Side.LowerRight: pict_.Cursor = Cursors.SizeNWSE; break;
                    case Side.LowerLeft:
                    case Side.UpperRight: pict_.Cursor = Cursors.SizeNESW; break;
                    case Side.Left:
                    case Side.Right: pict_.Cursor = Cursors.SizeWE; break;
                    case Side.Top:
                    case Side.Bottom: pict_.Cursor = Cursors.SizeNS; break;
                    case Side.Move: pict_.Cursor = Cursors.SizeAll; break;
                    default: pict_.Cursor = null; break;
                }
            }
        }
        public void OnMouseUp(MouseEventArgs e)
        {
            if (pict_.Capture)
            {
                MirrorBound = pict_.Client2Image(bound_);
                if (MirrorBoundChanged != null)
                    MirrorBoundChanged(this, new EventArgs());
                pict_.Capture = isCaptured_ = false;
                Invalidate();
            }
        }
        public bool IsImageUpdateEnabled()
        {
            return !isCaptured_;
        }

        public RectangleF MirrorBound
        {
            get { return mirrorBound_; }
            set
            {
                if (mirrorBound_ != value)
                {
                    Invalidate();
                    mirrorBound_ = value;
                    bound_ = pict_.Image2Client(mirrorBound_);
                    Invalidate();
                }
            }
        }
        public event EventHandler MirrorBoundChanged;

        private IPictureBase pict_;
        private Options options_;
        private Pen pen_;
        private RectangleF mirrorBound_;
        private Rectangle bound_;
        private bool isCaptured_ = false;
        private bool disposed_ = false;

        private void OnConversionChanged(object sender, EventArgs e)
        {
            Invalidate();
            bound_ = pict_.Image2Client(MirrorBound);
            Invalidate();
        }

        private void Invalidate() { pict_.Invalidate(bound_); }

        private enum Side { None, Move, UpperLeft, UpperRight, LowerLeft, LowerRight, Left, Right, Top, Bottom };
        private Side side_;
        private Point startLocation_;
        private Rectangle startBound_;

        private Side GetSide(Point pt)
        {
            if (pt.X >= bound_.Left - options_.SideTolerance && pt.X <= bound_.Left + options_.SideTolerance)
            {
                if (pt.Y >= bound_.Top - options_.SideTolerance && pt.Y <= bound_.Top + options_.SideTolerance)
                    return Side.UpperLeft;
                if (pt.Y >= bound_.Bottom - options_.SideTolerance && pt.Y <= bound_.Bottom + options_.SideTolerance)
                    return Side.LowerLeft;
                if (pt.Y <= bound_.Bottom && pt.Y >= bound_.Top)
                    return Side.Left;
            }
            if (pt.X >= bound_.Right - options_.SideTolerance && pt.X <= bound_.Right + options_.SideTolerance)
            {
                if (pt.Y >= bound_.Top - options_.SideTolerance && pt.Y <= bound_.Top + options_.SideTolerance)
                    return Side.UpperRight;
                if (pt.Y >= bound_.Bottom - options_.SideTolerance && pt.Y <= bound_.Bottom + options_.SideTolerance)
                    return Side.LowerRight;
                if (pt.Y <= bound_.Bottom && pt.Y >= bound_.Top)
                    return Side.Right;
            }
            if (pt.X <= bound_.Right && pt.X >= bound_.Left)
            {
                if (pt.Y >= bound_.Top - options_.SideTolerance && pt.Y <= bound_.Top + options_.SideTolerance)
                    return Side.Top;
                if (pt.Y >= bound_.Bottom - options_.SideTolerance && pt.Y <= bound_.Bottom + options_.SideTolerance)
                    return Side.Bottom;
                if (pt.Y >= bound_.Top && pt.Y <= bound_.Bottom)
                    return Side.Move;
            }
            return Side.None;
        }
    }
}

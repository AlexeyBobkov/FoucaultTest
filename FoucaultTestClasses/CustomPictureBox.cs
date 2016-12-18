using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FoucaultTestClasses
{
    // customized picture box control
    public class CustomPictureBox : PictureBox, IPictureBase
    {
        public CustomPictureBox()
        {
            ImageSizeChanged += new EventHandler(UpdateConversion);
            ClientSizeChanged += new EventHandler(UpdateConversion);
            SizeModeChanged += new EventHandler(UpdateConversion);
        }

        public Rectangle ImageBound
        {
            get { return new Rectangle(new Point(xshift_, yshift_), imageSize_); }
        }

        public event EventHandler ImageSizeChanged;
        public new Image Image
        {
            get { return base.Image; }
            set
            {
                if (uiHandler_ != null && !uiHandler_.IsImageUpdateEnabled())
                    return;

                bool sizeChanged = false;
                Image oldImage = base.Image;
                if (oldImage == null)
                {
                    if (value != null)
                        sizeChanged = true;
                    base.Image = value;
                }
                else
                {
                    if (value == null || oldImage.Size != value.Size)
                        sizeChanged = true;
                    base.Image = value;
                    oldImage.Dispose();
                }
                
                if (sizeChanged && this.ImageSizeChanged != null)
                    this.ImageSizeChanged(this, new EventArgs());
            }
        }

        public PointF Client2Image(Point clientPt)
        {
            lock (lockConversion_)
                return new PointF((float)(clientPt.X - xshift_) / (float)imageSize_.Width, (float)(clientPt.Y - yshift_) / (float)imageSize_.Height);
        }
        public SizeF Client2Image(Size clientSize)
        {
            lock (lockConversion_)
                return new SizeF((float)clientSize.Width / (float)imageSize_.Width, (float)clientSize.Height / (float)imageSize_.Height);
        }
        public RectangleF Client2Image(Rectangle clientRect)
        {
            lock (lockConversion_)
                return new RectangleF(Client2Image(clientRect.Location), Client2Image(clientRect.Size));
        }

        public Point Image2Client(PointF imagePt)
        {
            lock (lockConversion_)
                return new Point((int)(imagePt.X * imageSize_.Width) + xshift_, (int)(imagePt.Y * imageSize_.Height) + yshift_);
        }
        public Size Image2Client(SizeF imageSize)
        {
            lock (lockConversion_)
                return new Size((int)(imageSize.Width * imageSize_.Width), (int)(imageSize.Height * imageSize_.Height));
        }
        public Rectangle Image2Client(RectangleF imageRect)
        {
            lock (lockConversion_)
                return new Rectangle(Image2Client(imageRect.Location), Image2Client(imageRect.Size));
        }
        public void OnDefaultPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        void IPictureBase.Invalidate(Rectangle rc)
        {
            rc.Inflate(1, 1);
            base.Invalidate(rc);
        }

        public void SetUIHandler(IPictureUIHandler h)
        {
            if (uiHandler_ != null)
                uiHandler_.Dispose();
            uiHandler_ = h;
            Invalidate();
        }

        public event EventHandler ConversionChanged;
        
        // IMPLEMENTATION

        protected override void OnPaint(PaintEventArgs e)
        {
            if (uiHandler_ != null)
                uiHandler_.OnPaint(e);
            else
                base.OnPaint(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)   { if (uiHandler_ != null) uiHandler_.OnMouseDown(e); }
        protected override void OnMouseMove(MouseEventArgs e)   { if (uiHandler_ != null) uiHandler_.OnMouseMove(e); }
        protected override void OnMouseUp(MouseEventArgs e)     { if (uiHandler_ != null) uiHandler_.OnMouseUp(e); }

        private IPictureUIHandler uiHandler_;

        // conversion
        private object lockConversion_ = new object();
        private Size imageSize_;
        private int xshift_, yshift_;
        private void UpdateConversion(object sender, EventArgs e)
        {
            if (Image == null)
                return;
            lock (lockConversion_)
            {
                switch (SizeMode)
                {
                    case PictureBoxSizeMode.Zoom:
                        {
                            double wfactor = (double)Image.Width / ClientSize.Width;
                            double hfactor = (double)Image.Height / ClientSize.Height;
                            double resizeFactor = Math.Max(wfactor, hfactor);
                            imageSize_ = new Size((int)(Image.Width / resizeFactor), (int)(Image.Height / resizeFactor));
                            xshift_ = (ClientSize.Width - imageSize_.Width) / 2;
                            yshift_ = (ClientSize.Height - imageSize_.Height) / 2;
                        }
                        break;

                    case PictureBoxSizeMode.StretchImage:
                    default:
                        imageSize_ = new Size(ClientSize.Width, ClientSize.Height);
                        xshift_ = yshift_ = 0;
                        break;
                }
            }
            if (ConversionChanged != null)
                ConversionChanged(this, new EventArgs());
        }
    }
}

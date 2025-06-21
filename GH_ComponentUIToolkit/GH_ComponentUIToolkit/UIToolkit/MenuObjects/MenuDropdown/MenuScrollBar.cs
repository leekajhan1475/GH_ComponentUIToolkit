using Grasshopper.GUI.Canvas;
using Grasshopper.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GH_ComponentUIToolkit
{
    internal class MenuScrollBar : GH_CustomAttribute
    {
        private bool _active = false;

        private float _minWidth = 6.5f; // Default minimum width of the scroll bar.

        private float _minHeight = 6f; // Default minimum height of the scroll bar.

        private float _localTop;

        private float _localBottom;

        private float _dragHeight;

        public double _ratio;

        private float _currentHight;

        private int _startIndex;

        private int _endIndex;

        public int numItems;

        public int numVisibleItems;

        private PointF _clickPos;

        private Rectangle _content;

        private Rectangle _drag;

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive
        {
            get => this._active;
            set => this._active = value;
        }

        public float MinWidth
        {
            get => this._minWidth;
            set => this._minWidth = value;
        }

        public float MinHeight
        {
            get => this._minHeight;
            set => this._minHeight = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] VisibleRange => new int[2] { this._startIndex, this._endIndex };

        /// <summary>
        /// 
        /// </summary>
        public MenuScrollBar()
            : base(0, "")
        {
            base.Width = this.MinWidth;
        }

        public void SetClick(PointF click)
        {
            this._clickPos = click;
            this._dragHeight = (base.Height * (float)_ratio);
            float num = this._clickPos.Y - base.Transform.Y;
            this._localTop = num - this._currentHight;
            this._localBottom = this._dragHeight - this._localTop;
        }

        public override void Render(WidgetRenderArgs args)
        {
            if (args.Channel == WidgetChannel.Overlay)
            {
                Graphics graphics = args.Canvas.Graphics;

                // Render scroll bar background
                //      1. Fill background
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 150, 150, 150)), _content);
                //      2. Draw shadow
                int shadowWidth = 1;
                Rectangle rect2 = new Rectangle(this._content.X + shadowWidth,
                                                this._content.Y + shadowWidth,
                                                this._content.Width - shadowWidth * 2, 
                                                this._content.Height - shadowWidth * 2);

                Pen penShadow = new Pen(new SolidBrush(Color.FromArgb(90, 80, 80, 80)));
                penShadow.Width = shadowWidth * 1.8f;
                graphics.DrawRectangle(penShadow, rect2);
                //      3. Draw boundary stroke
                // Pen penBound = new Pen(Brushes.Black);
                // penBound.Width = 0.5f;
                // graphics.DrawRectangle(penBound, this._content);

                // Render drag bar's rectangle
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 200, 200, 200)), this._drag);
                Pen penBound = new Pen(Brushes.Black);
                penBound.Width = 0.6f;
                graphics.DrawRectangle(penBound, this._drag);
            }
        }

        public override SizeF ComputeMinSize()
        {
            return new SizeF(this.MinWidth, this.MinHeight);
        }

        public override void Layout()
        {
            // Set scroll bar slot/track boundary rectangle shape
            //this._content = GH_CustomAttribute.Convert(base.CanvasBoundary);
            float bound1 = 1f;
            this._content = new Rectangle(
                (int)(base.Transform.X - bound1),
                (int)(base.Transform.Y),
                (int)(base.Width + bound1 * 1.2),
                (int)(base.Height)
                );

            // Set drag-bar boundary rectangle
            float bound = bound1 * 0.8f;
            this._drag = new Rectangle((int)(base.Transform.X + bound),
                                  (int)(base.Transform.Y + this._currentHight + bound),
                                  (int)(base.Width),
                                  (int)(((double)(base.Height) * _ratio) - bound));
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            float num = e.CanvasLocation.Y - base.Transform.Y;
            float num2 = num - _localTop;
            float num3 = num + _localBottom;

            if (num2 < 0f)
            {
                _currentHight = 0f;
                _startIndex = 0;
                _endIndex = numVisibleItems;
            }
            else if (num3 > base.Height)
            {
                _currentHight = base.Height - _dragHeight;
                _startIndex = numItems - numVisibleItems;
                _endIndex = numItems;
            }
            else
            {
                _currentHight = num2;
                _startIndex = (int)(_currentHight / base.Height * (float)numItems);
                _endIndex = _startIndex + numVisibleItems;
            }
            return GH_ObjectResponse.Capture;
        }

        public void Update()
        {
            if (_currentHight == 0f)
            {
                _startIndex = 0;
                _endIndex = numVisibleItems;
            }
            else if (_currentHight == base.Height - _dragHeight)
            {
                _startIndex = numItems - numVisibleItems;
                _endIndex = numItems;
            }
            else
            {
                _startIndex = (int)(_currentHight / base.Height * (float)numItems);
                _endIndex = _startIndex + numVisibleItems;
            }
        }

        public void SetSlider(int start, int length)
        {
            _startIndex = start;
            _endIndex = start + length;
            double num = (double)start / (double)numItems * (double)base.Height;
            _currentHight = (float)num;
            _dragHeight = (float)(_ratio * (double)base.Height);
        }

        public override bool Contains(PointF pt)
        {
            return _content.Contains((int)pt.X, (int)pt.Y);
        }
    }
}

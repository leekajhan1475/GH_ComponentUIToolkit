using Grasshopper.GUI.Canvas;
using Grasshopper.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH_ComponentUIToolkit
{
    internal class MenuDropDownWindow : GH_CustomAttribute
    {
        private MenuDropDown _dropMenu;

        private MenuScrollBar _scrollBar;

        private int _tempActive = -1;

        private int _tempStart;

        private double _ratio = 1.0;

        private Rectangle _contentBox;

        private Rectangle _resizeBox;

        private int _resizeBoxSize = 10;

        private bool _resizeActive;

        private int _maxLen;

        public MenuDropDownWindow(MenuDropDown parent)
            : base(0, "")
        {
            _scrollBar = new MenuScrollBar();
            _scrollBar.Width = 5f;
            _scrollBar.ParentAttribute = this;
            _dropMenu = parent;
        }

        public void Update()
        {
            int count = _dropMenu.Items.Count;

            int num = Math.Min(_dropMenu.VisibleItemCount, count);

            if (_dropMenu.LastValidValue > count)
            {
                _dropMenu.Value = -1;
            }

            if (_dropMenu.Items.Count == 0)
            {
                _tempStart = 0;
            }

            _maxLen = num;
            _ratio = (double)num / (double)count;
            int num2 = num * 20;
            base.Height = num2 + _resizeBoxSize;
            _contentBox = new Rectangle((int)base.CanvasPivot.X,
                (int)base.CanvasPivot.Y,
                (int)base.Width,
                num2);

            _scrollBar.Height = num2;
            _scrollBar._ratio = _ratio;
            _scrollBar.numItems = count;
            _scrollBar.numVisibleItems = num;
            _scrollBar.SetSlider(_tempStart, count);
            _scrollBar.Layout();

            PointF transform = new PointF(base.CanvasPivot.X + base.Width - _scrollBar.Width,
                base.CanvasPivot.Y);
            _scrollBar.UpdateBounds(transform, this._scrollBar.Width);
        }

        /// <summary>
        /// Check this method
        /// </summary>
        /// <returns></returns>
        public override SizeF ComputeMinSize()
        {
            if (this._dropMenu != null && !this._dropMenu.IsEmpty)
            {
                foreach (MenuItem item in _dropMenu.Items)
                {
                    MenuItem entry = item;
                }
            }
            return _scrollBar.ComputeMinSize();
        }

        public override void Layout()
        {
            Update();
            _resizeBox = new Rectangle((int)base.Transform.X + (int)base.Width - _resizeBoxSize,
                (int)base.Transform.Y + (int)base.Height - _resizeBoxSize,
                _resizeBoxSize,
                _resizeBoxSize);
        }

        public override void Render(WidgetRenderArgs args)
        {
            if (args.Channel != WidgetChannel.Overlay)
            {
                return;
            }
            Graphics graphics = args.Canvas.Graphics;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;

            // Set drop menu boundary stroke colour and width
            Pen pen = new Pen(Brushes.Black);
            pen.Width = 2f;
            // Render drop menu boundary
            graphics.DrawRectangle(pen, _contentBox);
            // Render drop menu background
            graphics.FillRectangle(Brushes.White, _contentBox);

            int num = 0;
            for (int i = _tempStart; i < _tempStart + _maxLen; i++)
            {
                Brush cellBackgroundColour = Brushes.White;
                Brush textForegroundColour = Brushes.White;
                Font dropdownItemFont = WidgetServer.Default.DropdownFont;
                float locationY = (int)base.Transform.Y + 20 * num + 5;

                if (i == _tempActive)
                {
                    cellBackgroundColour = new SolidBrush(Color.FromArgb(200, 193, 216, 47));
                    textForegroundColour = Brushes.Black;
                    dropdownItemFont = WidgetServer.Default.DropdownActiveFont;
                    locationY = (int)Transform.Y + 20 * num + 3;
                }
                else if (i == _dropMenu.Value)
                {
                    cellBackgroundColour = Brushes.LightGray;
                    textForegroundColour = Brushes.Black;
                }
                else
                {
                    cellBackgroundColour = new SolidBrush(Color.White);
                    textForegroundColour = Brushes.Black;
                }

                Rectangle rect = new Rectangle((int)base.Transform.X, (int)base.Transform.Y + 20 * num, (int)base.Width, 20);
                // Render menu cell
                graphics.FillRectangle(cellBackgroundColour, rect);
                // Render Text
                graphics.DrawString(_dropMenu.Items[i].Content,
                                    dropdownItemFont,
                                    textForegroundColour,
                                    base.Transform.X + base.Width / 2f,
                                    locationY,
                                    stringFormat);
                num++;
            }
            _scrollBar.Render(args);
            //graphics.FillRectangle(new SolidBrush(Color.FromArgb(120, 80, 80, 80)), _resizeBox);
            //graphics.DrawString("+", WidgetServer.Instance.DropdownFont, new SolidBrush(Color.FromArgb(255, 0, 0, 0)), _resizeBox.Location.X + 5, _resizeBox.Location.Y - 3, stringFormat);
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (_resizeActive || _scrollBar.IsActive)
            {
                _scrollBar.IsActive = false;
                _resizeActive = false;
            }
            return GH_ObjectResponse.Capture;
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (_scrollBar.IsActive)
            {
                _scrollBar.RespondToMouseMove(sender, e);
                int[] visibleRange = _scrollBar.VisibleRange;
                _tempStart = visibleRange[0];
            }
            else if (_resizeActive)
            {
                float num = e.CanvasLocation.Y - Transform.Y;
                if (num > 20f)
                {
                    int num2 = (int)(num / 20f);
                    if (num2 + _tempStart < _dropMenu.Items.Count)
                    {
                        _dropMenu.VisibleItemCount = num2;
                        Update();
                    }
                    else if (num2 <= _dropMenu.Items.Count)
                    {
                        _tempStart = _dropMenu.Items.Count - num2;
                        _dropMenu.VisibleItemCount = num2;
                        Update();
                    }
                }
            }
            else if (Contains(e.CanvasLocation))
            {
                _tempActive = _tempStart + (int)((e.CanvasLocation.Y - base.Transform.Y) / 20f);
            }
            else
            {
                _tempActive = -1;
            }
            return GH_ObjectResponse.Capture;
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            _scrollBar.IsActive = false;
            _resizeActive = false;

            if (_scrollBar.Contains(e.CanvasLocation))
            {
                _scrollBar.IsActive = true;
                _scrollBar.SetClick(e.CanvasLocation);
                return GH_ObjectResponse.Capture;
            }

            if (_resizeBox.Contains((int)e.CanvasLocation.X, (int)e.CanvasLocation.Y))
            {
                _resizeActive = true;
                return GH_ObjectResponse.Capture;
            }

            if (_contentBox.Contains((int)e.CanvasLocation.X, (int)e.CanvasLocation.Y))
            {
                _dropMenu.Value = _tempStart + (int)((e.CanvasLocation.Y - base.Transform.Y) / 20f);
                _tempActive = -1;
                _resizeActive = false;
                _dropMenu.HideWindow(fire: true);
                return GH_ObjectResponse.Release;
            }
            _dropMenu.HideWindow(fire: false);
            return GH_ObjectResponse.Release;
        }

        public override bool Contains(PointF pt)
        {
            return base.CanvasBoundary.Contains(pt);
        }
    }
}

using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GH_ComponentUIToolkit
{
    public class MenuDropDown : GH_CustomAttribute
    {
        private MenuDropDownWindow _window;

        public bool expanded;

        private static int default_item_index = 0;

        private int current_value;

        private int last_valid_value;

        private int _visibleItemCount = 4;

        private List<MenuItem> _items;

        private string _emptyText = "empty";

        public int Value
        {
            get
            {
                return current_value;
            }
            set
            {
                current_value = Math.Max(value, 0);
                last_valid_value = ((value >= 0) ? value : 0);
            }
        }

        public int LastValidValue => last_valid_value;

        public List<MenuItem> Items => _items;

        internal bool IsEmpty => _items.Count == 0;

        public int VisibleItemCount
        {
            get
            {
                return _visibleItemCount;
            }
            set
            {
                if (_visibleItemCount < 1)
                {
                    _visibleItemCount = 1;
                }
                else
                {
                    _visibleItemCount = value;
                }
            }
        }

        public event ValueChangeEventHandler ValueChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int FindItemIndex(string name)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Name.Equals(name))
                {
                    return i;
                }
            }
            return -1;
        }

        public override void PostUpdateBounds(out float outHeight)
        {
            _window.Width = base.Width;
            outHeight = ComputeMinSize().Height;
        }

        public MenuDropDown(int index, string id, string tag)
            : base(index, id)
        {
            _items = new List<MenuItem>();
            _window = new MenuDropDownWindow(this);
            _window.ParentAttribute = this;
        }

        public void AddItem(string name, string content)
        {
            MenuItem item = new MenuItem(name, content, _items.Count);
            _items.Add(item);
            Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cont"></param>
        /// <param name="data"></param>
        public void AddItem(string name, string cont, object data)
        {
            MenuItem entry = new MenuItem(name, cont, _items.Count);
            entry.Data = data;
            _items.Add(entry);
            Update();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (this._items.Count == 0)
            {
                current_value = 0;
            }
            this._window.Update();
        }

        public override void Layout()
        {
            _window.UpdateBounds(base.CanvasPivot, base.Width);
            _window.Layout();
        }

        public void Clear()
        {
            _items.Clear();
            Update();
        }

        public override SizeF ComputeMinSize()
        {
            int num = 0;
            int num2 = 0;
            if (IsEmpty)
            {
                Size size = TextRenderer.MeasureText(_emptyText, WidgetServer.Default.DropdownFont);
                num = size.Width + 4 + 10;
                num2 = size.Height + 2;
            }
            else
            {
                foreach (MenuItem item in _items)
                {
                    Size size2 = TextRenderer.MeasureText(item.Content, WidgetServer.Default.DropdownFont);
                    int val = size2.Width + 4 + 10;
                    int val2 = size2.Height + 2;
                    num = Math.Max(num, val);
                    num2 = Math.Max(num2, val2);
                }
            }
            return new SizeF(num, num2);
        }

        public override void Render(WidgetRenderArgs args)
        {
            GH_Canvas canvas = args.Canvas;
            if (args.Channel == WidgetChannel.Overlay)
            {
                if (expanded)
                {
                    _window.Render(args);
                }
            }
            else if (args.Channel == WidgetChannel.Object)
            {
                Graphics graphics = canvas.Graphics;
                float zoom = canvas.Viewport.Zoom;
                int num = 255;
                if (zoom < 1f)
                {
                    float num2 = (zoom - 0.5f) * 2f;
                    num = (int)((float)num * num2);
                }
                if (num < 0)
                {
                    num = 0;
                }
                num = GH_Canvas.ZoomFadeLow;

                SolidBrush brush = new SolidBrush(Color.FromArgb(num, 90, 90, 90));
                SolidBrush brush2 = new SolidBrush(Color.FromArgb(num, 150, 150, 150));
                SolidBrush brush3 = new SolidBrush(Color.FromArgb(num, 0, 0, 0));
                SolidBrush brush4 = new SolidBrush(Color.FromArgb(num, 255, 255, 255));

                Pen pen = new Pen(brush3);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;

                if (IsEmpty)
                {
                    PointF point = new PointF(base.CanvasPivot.X + base.Width / 2f, base.CanvasBoundary.Y + 2f);
                    graphics.DrawRectangle(pen, GH_CustomAttribute.Convert(base.CanvasBoundary));
                    graphics.FillRectangle(brush2, base.CanvasBoundary);
                    graphics.DrawString(_emptyText, WidgetServer.Default.DropdownFont, brush, point, stringFormat);
                }
                else
                {
                    PointF point2 = new PointF(base.CanvasPivot.X + (base.Width - 5f) / 2f, base.CanvasBoundary.Y + 2f);
                    graphics.DrawRectangle(pen, GH_CustomAttribute.Convert(base.CanvasBoundary));
                    graphics.FillRectangle(brush4, base.CanvasBoundary);

                    graphics.DrawString(_items[current_value].Content, 
                        WidgetServer.Default.DropdownFont,
                        new SolidBrush(Color.FromArgb(210, 50, 50, 50)), 
                        point2,
                        stringFormat);
                    
                    // Render expand menu icon
                    //Rectangle rect = new Rectangle((int)(base.CanvasPivot.X + base.Width - 5f), 
                    //                               (int)base.CanvasPivot.Y, 
                    //                               5, 
                    //                               (int)base.Height - 10);

                    //graphics.FillRectangle(brush4, rect);

                    //graphics.DrawRectangle(pen, rect);
                    //graphics.DrawString("+", 
                    //    WidgetServer.Instance.TextFont,
                    //    Brushes.DarkGray,
                    //    new PointF((int)( (base.CanvasPivot.X + base.Width - 5f) ), (int)( base.CanvasPivot.Y + 2 )), 
                    //    stringFormat);

                    // Draw expandable triangle icon
                    PointF[] points = new PointF[3] { new PointF(base.CanvasPivot.X + base.Width - 3f, base.CanvasPivot.Y + base.Height * 0.25f),
                                                      new PointF(base.CanvasPivot.X + base.Width - 8f, base.CanvasPivot.Y + base.Height * 0.75f),
                                                      new PointF(base.CanvasPivot.X + base.Width - 13f, base.CanvasPivot.Y + + base.Height * 0.25f) };

                    graphics.FillPolygon(new SolidBrush(Color.FromArgb(180, 200, 200, 200)), points);
                }
            }
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (IsEmpty)
            {
                return GH_ObjectResponse.Release;
            }
            if (expanded)
            {
                return _window.RespondToMouseUp(sender, e);
            }
            return GH_ObjectResponse.Ignore;
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (expanded)
            {
                return _window.RespondToMouseMove(sender, e);
            }
            return GH_ObjectResponse.Ignore;
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (IsEmpty)
            {
                return GH_ObjectResponse.Handled;
            }
            if (expanded)
            {
                if (_window.Contains(e.CanvasLocation))
                {
                    return _window.RespondToMouseDown(sender, e);
                }
                HideWindow(fire: false);
                return GH_ObjectResponse.Release;
            }
            ShowWindow();
            return GH_ObjectResponse.Capture;
        }

        public void ShowWindow()
        {
            if (!expanded)
            {
                expanded = true;
                TopCollection.ActiveWidget = this;
                Update();
            }
        }

        public void HideWindow(bool fire)
        {
            if (expanded)
            {
                expanded = false;
                TopCollection.ActiveWidget = null;
                TopCollection.MakeAllInActive();
                if (fire && this.ValueChanged != null)
                {
                    this.ValueChanged(this, new EventArgs());
                }
            }
        }

        public override bool Contains(PointF pt)
        {
            return base.CanvasBoundary.Contains(pt);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.CreateChunk("MenuDropDown", Index).SetInt32("ActiveItemIndex", current_value);
            return true;
        }

        public override bool Read(GH_IReader reader)
        {
            GH_IReader val = reader.FindChunk("MenuDropDown", Index);
            try
            {
                current_value = val.GetInt32("ActiveItemIndex");
            }
            catch
            {
                current_value = default_item_index;
            }
            return true;
        }

    }
}

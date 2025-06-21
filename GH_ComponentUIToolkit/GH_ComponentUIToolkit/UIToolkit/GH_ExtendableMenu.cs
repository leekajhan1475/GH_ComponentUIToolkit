using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GH_ComponentUIToolkit
{
    public class GH_ExtendableMenu : GH_CustomAttribute
    {
        private List<ExtendedPlug> _inputs;

        private List<ExtendedPlug> _outputs;

        private string name;

        private GH_MenuCollection collection;

        private GH_Capsule _menu;

        private RectangleF _headBounds;

        private RectangleF _contentBounds;

        private List<GH_CustomAttribute> _controls;

        private GH_CustomAttribute _activeControl;

        private bool _isExpanded;

        private bool _isClicked;

        /// <summary>
        /// Gets or sets the locked (disabled) status of this menu
        /// </summary>
        public int RuntimeStatus
        {
            get => this._runtimeStatus;
            set => this._runtimeStatus = value;
        }

        /// <summary>
        /// Gets or sets the locked (disabled) status of this menu
        /// </summary>
        public bool Locked
        {
            get => this._isLocked;
            set => this._isLocked = value;
        }

        /// <summary>
        /// Gets or sets the locked (disabled) status of this menu
        /// </summary>
        public bool Selected
        {
            get => this._isSelected;
            set => this._isSelected = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ExtendedPlug> Inputs
        {
            get => this._inputs;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ExtendedPlug> Outputs
        {
            get => this._outputs;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExpanded
        {
            get => this._isExpanded;
        }

        /// <summary>
        /// 
        /// </summary>
        public GH_MenuCollection Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }
        public List<GH_CustomAttribute> Controls => _controls;
        public override string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public float TotalHeight
        {
            get
            {
                if (this.IsExpanded)
                {
                    int num = Math.Max(this._inputs.Count, this._outputs.Count) * 20;

                    if (num > 0)
                    {
                        num += 5;
                    }

                    return base.Height + (float)num;
                }
                return base.Height;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="id"></param>
        public GH_ExtendableMenu(int index, string id)
        : base(index, id)
        {
            this._inputs = new List<ExtendedPlug>();
            this._outputs = new List<ExtendedPlug>();
            this._controls = new List<GH_CustomAttribute>();
            this._headBounds = default(RectangleF);
            this._contentBounds = default(RectangleF);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plug"></param>
        public void RegisterInputPlug(ExtendedPlug plug)
        {
            plug.IsMenu = true;
            this._inputs.Add(plug);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plug"></param>
        public void RegisterOutputPlug(ExtendedPlug plug)
        {
            plug.IsMenu = true;
            this._outputs.Add(plug);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Expand()
        {
            if (!this.IsExpanded)
            {
                this._isExpanded = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Collapse()
        {
            if (this.IsExpanded)
            {
                this._isExpanded = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(GH_CustomAttribute control)
        {
            control.ParentAttribute = this;
            this._controls.Add(control);
        }

        /// <summary>
        /// 
        /// </summary>
        public void MakeAllInActive()
        {
            int count = this._controls.Count;

            for (int i = 0; i < count; i++)
            {
                if (this._controls[i].GetType() == typeof(MenuPanel))
                {
                    ((MenuPanel)this._controls[i])._activeControl = null;
                }
            }
            _activeControl = null;
        }

        #region Mouse actions and responses
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (this._headBounds.Contains(e.CanvasLocation) && e.Button == MouseButtons.Left)
            {
                if (this._isClicked) this._isClicked = false;

                return GH_ObjectResponse.Handled;
            }

            if (_activeControl != null)
            {
                GH_ObjectResponse val = _activeControl.RespondToMouseUp(sender, e);

                if ((int)val == 2)
                {
                    _activeControl = null;
                    return val;
                }

                if ((int)val != 0)
                {
                    return val;
                }

                _activeControl = null;
            }
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left && this._headBounds.Contains(e.CanvasLocation))
            {
                if (this.IsExpanded)
                {
                    this._activeControl = null;
                }

                bool toggle = !this._isExpanded;

                this._isExpanded = toggle;

                if (!this._isClicked) this._isClicked = true;

                return GH_ObjectResponse.Handled;
            }

            if (this.IsExpanded)
            {
                if (_contentBounds.Contains(e.CanvasLocation))
                {
                    for (int i = 0; i < this._inputs.Count; i++)
                    {
                        if (this._inputs[i].Parameter.Attributes.Bounds.Contains(e.CanvasLocation))
                        {
                            return this._inputs[i].Parameter.Attributes.RespondToMouseDown(sender, e);
                        }
                    }
                    for (int j = 0; j < _controls.Count; j++)
                    {
                        if (_controls[j].Contains(e.CanvasLocation))
                        {
                            _activeControl = _controls[j];
                            return _controls[j].RespondToMouseDown(sender, e);
                        }
                    }
                }
                else if (this._activeControl != null)
                {
                    this._activeControl.RespondToMouseDown(sender, e);
                    this._activeControl = null;
                    return GH_ObjectResponse.Handled;
                }
            }
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override GH_CustomAttribute IsTtipPoint(PointF pt)
        {
            if (_headBounds.Contains(pt))
            {
                return this;
            }

            if (this.IsExpanded && _contentBounds.Contains(pt))
            {
                int count = _controls.Count;
                for (int i = 0; i < count; i++)
                {
                    if (_controls[i].Contains(pt))
                    {
                        GH_CustomAttribute gH_Attr_Widget = _controls[i].IsTtipPoint(pt);
                        if (gH_Attr_Widget != null)
                        {
                            return gH_Attr_Widget;
                        }
                    }
                }
            }
            return null;
        }
        public override void TooltipSetup(PointF canvasPoint, GH_TooltipDisplayEventArgs e)
        {
            e.Icon = ((Bitmap)null);
            e.Title = ("Menu (" + name + ")");
            e.Text = (_header);
            if (_header != null)
            {
                e.Text = (e.Text + "\n");
            }
            if (this.IsExpanded)
            {
                e.Text = (e.Text + "Click to close menu");
            }
            else
            {
                e.Text = (e.Text + "Click to open menu");
            }
            e.Description = (_description);
        }
        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (_activeControl != null)
            {
                return _activeControl.RespondToMouseMove(sender, e);
            }
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            PointF canvasPivot = base.CanvasPivot;

            if (_headBounds.Contains(e.CanvasLocation))
            {
                return GH_ObjectResponse.Handled;
            }

            if (this.IsExpanded && _contentBounds.Contains(e.CanvasLocation))
            {
                int count = _controls.Count;
                for (int i = 0; i < count; i++)
                {
                    if (_controls[i].Contains(e.CanvasLocation))
                    {
                        return _controls[i].RespondToMouseDoubleClick(sender, e);
                    }
                }
            }

            return GH_ObjectResponse.Ignore;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("Expanded", this.IsExpanded);
            for (int i = 0; i < this._controls.Count; i++)
            {
                this._controls[i].Write(writer);
            }
            return base.Write(writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public override bool Read(GH_IReader reader)
        {
            this._isExpanded = reader.GetBoolean("Expanded");
            for (int i = 0; i < _controls.Count; i++)
            {
                _controls[i].Read(reader);
            }
            return base.Read(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SizeF ComputeMinSize()
        {
            SizeF menuHeadTextSize = GetMenuHeadTextSize();
            float num = menuHeadTextSize.Width;
            float num2 = menuHeadTextSize.Height;
            foreach (GH_CustomAttribute control in _controls)
            {
                SizeF sizeF = control.ComputeMinSize();
                num = Math.Max(sizeF.Width, num);
                if (this.IsExpanded)
                {
                    num2 += sizeF.Height;
                }
            }
            return new SizeF(num, num2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SizeF GetMenuHeadTextSize()
        {
            Size size = TextRenderer.MeasureText(name, WidgetServer.Default.MenuHeaderFont);
            return new SizeF((float)(size.Width + 8), (float)(size.Height + 4));
        }

        /// <summary>
        /// Layout of the expandable menu
        /// </summary>
        public override void Layout()
        {
            SizeF menuHeadTextSize = GetMenuHeadTextSize();

            PointF canvasPivot = base.CanvasPivot;

            float x = canvasPivot.X;

            canvasPivot = base.CanvasPivot;

            this._headBounds = new RectangleF(x, canvasPivot.Y, base.Width, menuHeadTextSize.Height);

            canvasPivot = base.CanvasPivot;

            float x2 = canvasPivot.X;

            canvasPivot = base.CanvasPivot;

            this._contentBounds = new RectangleF(x2, canvasPivot.Y + menuHeadTextSize.Height, base.Width, base.Height - menuHeadTextSize.Height);

            // Create header-bar rectangle
            Rectangle rectangle = new Rectangle((int)_headBounds.X + 3,
                                                (int)_headBounds.Y + 1,
                                                (int)_headBounds.Width - 6,
                                                (int)_headBounds.Height - 2);


            _menu = GH_Capsule.CreateTextCapsule(rectangle,
                                             rectangle,
                                             GH_Palette.White,
                                             name,
                                             WidgetServer.Default.MenuHeaderFont,
                                             GH_Orientation.horizontal_center,
                                             2,
                                             0);

            float num = menuHeadTextSize.Height;

            if (this.IsExpanded)
            {
                canvasPivot = base.CanvasPivot;

                float x3 = canvasPivot.X;

                canvasPivot = base.CanvasPivot;

                PointF transform = new PointF(x3, canvasPivot.Y + menuHeadTextSize.Height);

                foreach (GH_CustomAttribute control in _controls)
                {
                    control.UpdateBounds(transform, base.Width);

                    control.Transform = transform;

                    control.Style = this.Style;

                    control.Palette = this.Palette;

                    control.Layout();

                    num += control.Height;
                }
            }
            base.Height = num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Render(WidgetRenderArgs args)
        {
            GH_Canvas canvas = args.Canvas;

            WidgetChannel channel = args.Channel;

            float zoom = canvas.Viewport.Zoom;

            int num = 255;

            if (zoom < 1f)
            {
                float num2 = (zoom - 0.5f) * 2f;
                num = (int)((float)num * num2);
            }


            // Set header button's colour palette based on mouse action
            Color colour = (this._isClicked) ? Color.FromArgb(220, 73, 82, 18) : Color.FromArgb(220, 193, 216, 47);

            if (this._runtimeStatus == (int)GH_RuntimeMessageLevel.Warning)
            {
                colour = Color.DarkOrange;
            }
            else if (this._runtimeStatus == (int)GH_RuntimeMessageLevel.Error)
            {
                colour = Color.Red;
            }

            // Render menu header bar
            if(!this.Selected && !this.Locked)
            {
                // Render menu header bar
                _menu.Render(canvas.Graphics,
                             new GH_PaletteStyle(colour));
            }
            else
            {
                // Render menu header bar
                _menu.Render(canvas.Graphics,
                             selected: this.Selected,
                             locked: this.Locked,
                             true);
            }

            if (this.IsExpanded && num > 0)
            {
                RenderMenuParameters(canvas, canvas.Graphics);

                foreach (GH_CustomAttribute control in _controls)
                {
                    control.OnRender(args);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="graphics"></param>
        public void RenderMenuParameters(GH_Canvas canvas, Graphics graphics)
        {
            if (Math.Max(this._inputs.Count, this._outputs.Count) != 0)
            {
                int zoomFadeLow = GH_Canvas.ZoomFadeLow;

                if (zoomFadeLow >= 5)
                {
                    StringFormat farCenter = GH_TextRenderingConstants.FarCenter;

                    canvas.SetSmartTextRenderingHint();

                    SolidBrush solidBrush = new SolidBrush(Color.FromArgb(zoomFadeLow, this.Style.Text));

                    List<ExtendedPlug>.Enumerator enumerator = this._inputs.GetEnumerator();

                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            IGH_Param parameter = enumerator.Current.Parameter;

                            RectangleF bounds = parameter.Attributes.Bounds;

                            if (bounds.Width >= 1f)
                            {
                                graphics.DrawString(parameter.NickName, StandardFont.Standard, solidBrush, bounds, farCenter);

                                GH_LinkedParamAttributes obj = (GH_LinkedParamAttributes)parameter.Attributes;

                                FieldInfo field = typeof(GH_LinkedParamAttributes).GetField("m_renderTags", BindingFlags.Instance | BindingFlags.NonPublic);

                                if (field != (FieldInfo)null)
                                {
                                    GH_StateTagList value = (GH_StateTagList)field.GetValue(obj);

                                    if (value != null)
                                    {
                                        value.RenderStateTags(graphics);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }

                    farCenter = GH_TextRenderingConstants.NearCenter;

                    enumerator = this._outputs.GetEnumerator();

                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            IGH_Param parameter2 = enumerator.Current.Parameter;

                            RectangleF bounds2 = parameter2.Attributes.Bounds;

                            if (bounds2.Width >= 1f)
                            {
                                graphics.DrawString(parameter2.NickName, StandardFont.Standard, solidBrush, bounds2, farCenter);

                                GH_LinkedParamAttributes obj2 = (GH_LinkedParamAttributes)parameter2.Attributes;

                                FieldInfo field2 = typeof(GH_LinkedParamAttributes).GetField("m_renderTags", BindingFlags.Instance | BindingFlags.NonPublic);

                                if (field2 != (FieldInfo)null)
                                {
                                    GH_StateTagList value2 = (GH_StateTagList)field2.GetValue(obj2);

                                    if (value2 != null)
                                    {
                                        value2.RenderStateTags(graphics);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                    solidBrush.Dispose();
                }
            }
        }
        public override string GetWidgetDescription()
        {
            string str = base.GetWidgetDescription() + "{\n";
            foreach (GH_CustomAttribute control in _controls)
            {
                str = str + control.GetWidgetDescription() + "\n";
            }
            return str + "}";
        }
    }
}

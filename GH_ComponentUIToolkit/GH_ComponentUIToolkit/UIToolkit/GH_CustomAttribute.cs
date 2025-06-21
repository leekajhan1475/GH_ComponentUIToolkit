using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using System.Drawing;
using System.Windows.Forms;

namespace GH_ComponentUIToolkit
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GH_CustomAttribute
    {
        protected RectangleF _boundary;

        protected RectangleF _canvasBoundary;

        protected GH_CustomAttribute _parent;

        protected PointF _transformation;

        protected GH_PaletteStyle _style;

        protected GH_Palette _palette;

        protected int _index;

        protected bool _enabled = true;

        protected string _description;

        protected string _header;

        protected string _name;

        protected bool _showToolTip = true;

        protected bool _isLocked = false;

        protected bool _isSelected = false;

        protected int _runtimeStatus;

        /// <summary>
        /// 
        /// </summary>
        public RectangleF CanvasBoundary
        {
            get => this._canvasBoundary;
        }

        /// <summary>
        /// 
        /// </summary>
        public RectangleF Boundary
        {
            get => this._boundary;
        }

        /// <summary>
        /// Class Properties
        /// </summary>
        public virtual bool ShowToolTip
        {
            get => this._showToolTip;
            set => this._showToolTip = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get => this._name;
            set => this._name = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Desciption
        {
            get => this._description;
            set => this._description = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Header
        {
            get => this._header;
            set => this._header = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual GH_CustomAttribute ParentAttribute
        {
            get => this._parent;
            set => this._parent = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            get => this._boundary.Width;
            set
            {
                this._boundary.Width = value;
                UpdateCanvasBounds();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Height
        {
            get => this._boundary.Height;
            set
            {
                this._boundary.Height = value;
                UpdateCanvasBounds();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Index
        {
            get => this._index;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual GH_MenuCollection TopCollection
        {
            get
            {
                GH_CustomAttribute gH_CustomAttribute = ParentAttribute._parent;
                while (!(gH_CustomAttribute is GH_ExtendableMenu) && gH_CustomAttribute.ParentAttribute != null)
                {
                    gH_CustomAttribute = gH_CustomAttribute.ParentAttribute;
                }
                if (gH_CustomAttribute != null)
                {
                    return ((GH_ExtendableMenu)gH_CustomAttribute).Collection;
                }
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GH_PaletteStyle Style
        {
            get => this._style;
            set => this._style = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public GH_Palette Palette
        {
            get => this._palette;
            set => this._palette = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public PointF CanvasPivot
        {
            get
            {
                return new PointF(CanvasBoundary.X, CanvasBoundary.Y);
            }
        }

        /// <summary>
        /// Class Methods
        /// </summary>
        public GH_CustomAttribute(int index, string id)
        {
            _index = index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Rectangle Convert(RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        internal PointF Transform
        {
            get => this._transformation;
            set => this._transformation = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        public static RectangleF Shrink(RectangleF rect, float left, float right, float top, float bot)
        {
            return new RectangleF(rect.Left + left, rect.Top + top, rect.Width - (left + right), rect.Height - (top + bot));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void OnRender(WidgetRenderArgs args)
        {
            Render(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public abstract void Render(WidgetRenderArgs args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool Contains(PointF pt)
        {
            return CanvasBoundary.Contains(pt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual GH_ObjectResponse RespondToKeyDown(GH_Canvas sender, KeyEventArgs e)
        {
            return GH_ObjectResponse.Ignore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual bool Write(GH_IWriter writer)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public virtual bool Read(GH_IReader reader)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract SizeF ComputeMinSize();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="width"></param>
        public void UpdateBounds(PointF transform, float width)
        {
            this._transformation = transform;
            this._boundary.Width = width;
            UpdateCanvasBounds();
            PostUpdateBounds(out float outHeight);
            Height = outHeight;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCanvasBounds()
        {
            this._canvasBoundary = new RectangleF(Transform.X, Transform.Y, this.Boundary.Width, this.Boundary.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outHeight"></param>
        public virtual void PostUpdateBounds(out float outHeight)
        {
            outHeight = Height;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Layout()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual GH_CustomAttribute IsTtipPoint(PointF pt)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvasPoint"></param>
        /// <param name="e"></param>
        public virtual void TooltipSetup(PointF canvasPoint, GH_TooltipDisplayEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetWidgetDescription()
        {
            return GetType().Name + " name" + Name + " index:" + Index;
        }
    }
}

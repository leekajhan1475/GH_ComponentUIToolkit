using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Rhino;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GH_ComponentUIToolkit
{
    public class GH_MenuCollection
    {
        private List<GH_ExtendableMenu> _menus;

        private PointF pivot;

        private float width;

        private GH_PaletteStyle style;

        private GH_Palette palette;

        private GH_CustomAttribute _activeWidget;

        private bool _isLocked;

        private bool _isSelected;

        private int _runtimeStatus;

        public int RuntimeStatus
        {
            get => this._runtimeStatus;
            set => this._runtimeStatus = value;
        }

        /// <summary>
        /// Gets or sets the locked status.
        /// </summary>
        public bool IsLocked
        {
            get => this._isLocked;
            set => this._isLocked = value;
        }

        /// <summary>
        /// Gets or sets the selection status.
        /// </summary>
        public bool IsSelected
        {
            get => this._isSelected;
            set => this._isSelected = value;
        }

        public List<GH_ExtendableMenu> Menus
        {
            get
            {
                return _menus;
            }
            set
            {
                _menus = value;
            }
        }
        public PointF Pivot
        {
            get
            {
                return pivot;
            }
            set
            {
                pivot = value;
            }
        }
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        public GH_PaletteStyle Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
            }
        }
        public GH_Palette Palette
        {
            get
            {
                return palette;
            }
            set
            {
                palette = value;
            }
        }
        public GH_CustomAttribute ActiveWidget
        {
            get
            {
                return _activeWidget;
            }
            set
            {
                _activeWidget = value;
            }
        }
        public float Height
        {
            get
            {
                float num = 0f;
                for (int i = 0; i < _menus.Count; i++)
                {
                    num += _menus[i].TotalHeight;
                }
                return num;
            }
        }
        public GH_MenuCollection()
        {
            _menus = new List<GH_ExtendableMenu>();
        }
        public void Merge(GH_MenuCollection other)
        {
            for (int i = 0; i < other._menus.Count; i++)
            {
                AddMenu(other._menus[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="onlyVisible"></param>
        public void GetMenuPlugs(ref List<ExtendedPlug> inputs, ref List<ExtendedPlug> outputs, bool onlyVisible)
        {
            for (int i = 0; i < this._menus.Count; i++)
            {
                switch (onlyVisible)
                {
                    default:
                        if (!this._menus[i].IsExpanded)
                        {
                            break;
                        }
                        goto case false;
                    case false:
                        inputs.AddRange(_menus[i].Inputs);
                        outputs.AddRange(_menus[i].Outputs);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_menu"></param>
        public void AddMenu(GH_ExtendableMenu _menu)
        {
            _menu.Collection = this;
            _menus.Add(_menu);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CollapseAllMenu()
        {
            MakeAllInActive();
            for (int i = 0; i < _menus.Count; i++)
            {
                _menus[i].Collapse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void MakeAllInActive()
        {
            for (int i = 0; i < _menus.Count; i++)
            {
                _menus[i].MakeAllInActive();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            for (int i = 0; i < _menus.Count; i++)
            {
                GH_ObjectResponse val = _menus[i].RespondToMouseDoubleClick(sender, e);
                if ((int)val != 0)
                {
                    return val;
                }
            }
            return 0;
        }
        public GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (_activeWidget != null)
            {
                return _activeWidget.RespondToMouseUp(sender, e);
            }
            for (int i = 0; i < _menus.Count; i++)
            {
                GH_ObjectResponse val = _menus[i].RespondToMouseUp(sender, e);
                if ((int)val != 0)
                {
                    return val;
                }
            }
            return 0;
        }
        public GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (_activeWidget != null)
            {
                return _activeWidget.RespondToMouseMove(sender, e);
            }
            int count = _menus.Count;
            for (int i = 0; i < count; i++)
            {
                GH_ObjectResponse val = _menus[i].RespondToMouseMove(sender, e);
                if ((int)val != 0)
                {
                    return val;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (_activeWidget != null)
            {
                return _activeWidget.RespondToMouseDown(sender, e);
            }

            int count = this._menus.Count;

            for (int i = 0; i < count; i++)
            {
                GH_ObjectResponse val = this._menus[i].RespondToMouseDown(sender, e);

                if ((int)val != 0)
                {
                    return val;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public GH_CustomAttribute IsTtipPoint(PointF pt)
        {
            if (this._activeWidget != null)
            {
                return null;
            }

            int count = this._menus.Count;
            for (int i = 0; i < count; i++)
            {
                GH_CustomAttribute gH_CustomAttribute = this._menus[i].IsTtipPoint(pt);
                if (gH_CustomAttribute != null)
                {
                    return gH_CustomAttribute;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public GH_ObjectResponse RespondToKeyDown(GH_Canvas sender, KeyEventArgs e)
        {
            if (_activeWidget != null)
            {
                return _activeWidget.RespondToKeyDown(sender, e);
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool Write(GH_IWriter writer)
        {
            GH_IWriter val = writer.CreateChunk("Collection");
            for (int i = 0; i < _menus.Count; i++)
            {
                GH_IWriter writer2 = val.CreateChunk("Menu", i);
                _menus[i].Write(writer2);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool Read(GH_IReader reader)
        {
            GH_IReader val = reader.FindChunk("Collection");
            if (val == null)
            {
                RhinoApp.WriteLine("Invalid menu collection");
                return false;
            }
            for (int i = 0; i < _menus.Count; i++)
            {
                GH_IReader val2 = val.FindChunk("Menu", i);
                if (val2 == null)
                {
                    return false;
                }
                _menus[i].Read(val2);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SizeF GetMinLayoutSize()
        {
            float val = 0f;
            float num = 0f;
            foreach (GH_ExtendableMenu menu in _menus)
            {
                SizeF sizeF = menu.ComputeMinSize();
                val = Math.Max(val, sizeF.Width);
                num += sizeF.Height;
            }
            return new SizeF(val, num);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Layout()
        {
            int num = 0;
            int count = _menus.Count;
            foreach (GH_ExtendableMenu menu in _menus)
            {
                menu.UpdateBounds(new PointF(pivot.X, pivot.Y + (float)num), width);
                menu.Style = style;
                menu.Palette = palette;
                menu.Layout();
                num += (int)menu.TotalHeight;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Render(WidgetRenderArgs args)
        {
            if (args.Channel == WidgetChannel.Object)
            {
                foreach (GH_ExtendableMenu menu in _menus)
                {
                    // Set the selected, locked status of each extendable menu
                    menu.Selected = this._isSelected;
                    menu.Locked = this._isLocked;
                    menu.RuntimeStatus = this._runtimeStatus;
                    menu.OnRender(args);
                }
            }
            else if (args.Channel == WidgetChannel.Overlay && _activeWidget != null)
            {
                _activeWidget.OnRender(args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetMenuDescription()
        {
            string str = "collection {\n";
            foreach (GH_ExtendableMenu menu in _menus)
            {
                str = str + menu.GetWidgetDescription() + "\n";
            }
            return str + "}";
        }
    }
}

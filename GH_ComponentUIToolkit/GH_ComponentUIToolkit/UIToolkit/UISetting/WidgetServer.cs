using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using Eto.Forms;
using Grasshopper.Kernel;


namespace GH_ComponentUIToolkit
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WidgetServer
    {
        private int _radioButtonPadding;

        private int _checkBoxPadding;

        private Size _radioButtonSize;

        private Size _checkBoxSize;

        private Font _textFontStyle;

        private Font _dropDownFontStyle;

        private Font _dropDownActiveFontStyle;

        private Font _menuHeaderFontStyle;

        private Font _sliderValueTagFontStyle;

        /// <summary>
        /// Gets the radio-button outline's stroke width. 
        /// </summary>
        public int RadioButtonPadding
        {
            get => this._radioButtonPadding;
            private set => this._radioButtonPadding = value;
        }

        /// <summary>
        ///  Gets the check-box outline's stroke width.
        /// </summary>
        public int CheckBoxPadding
        {
            get => this._checkBoxPadding;
            private set => this._checkBoxPadding = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public Font TextFont
        {
            get => this._textFontStyle;
            private set => this._textFontStyle = value;
        }

        /// <summary>
        /// Gets the standard item's font style in the drop-down list.
        /// </summary>
        public Font DropdownFont
        {
            get => this._dropDownFontStyle;
            private set => this._dropDownFontStyle = value;
        }

        /// <summary>
        /// Gets the active item's font style in the drop-down list.
        /// </summary>
        public Font DropdownActiveFont
        {
            get => this._dropDownActiveFontStyle;
            private set => this._dropDownActiveFontStyle = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public Font MenuHeaderFont
        {
            get => this._menuHeaderFontStyle;
            private set => this._menuHeaderFontStyle = value;
        }

        /// <summary>
        /// Gets the slider's tag font style.
        /// </summary>
        public Font SliderValueTagFont
        {
            get => this._sliderValueTagFontStyle;
            private set => this._sliderValueTagFontStyle = value;
        }

        /// <summary>
        /// Gets the radio-button's size value.
        /// </summary>
        public Size RadioButtonSize
        {
            get => this._radioButtonSize;
            private set => this._radioButtonSize = value;
        }

        /// <summary>
        /// Gets the check-box's size value.
        /// </summary>
        public Size CheckBoxSize
        {
            get => this._checkBoxSize;
            private set => this._checkBoxSize = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public static WidgetServer Default
        {
            get => new WidgetServer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static float GetScalingFactor()
        {
            float num = Screen.PrimaryScreen.LogicalPixelSize;
            if ((double)num > 0.95 && (double)num < 1.05)
            {
                num = 1f;
            }
            return num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="font_size"></param>
        /// <returns></returns>
        public static int ScaleFontSize(int font_size)
        {
            return (int)Math.Round((double)font_size * (double)GH_FontServer.StandardAdjusted.Height / (double)GH_FontServer.Standard.Height);
        }

        /// <summary>
        /// Sets the style of the text font.
        /// </summary>
        /// <param name="style">
        /// Custom font style
        /// </param>
        public void SetTextFont(Font style)
        {
            this.TextFont = style;
        }

        /// <summary>
        /// Sets the style of the text font in drop-down menu.
        /// </summary>
        /// <param name="style">
        /// Custom font style
        /// </param>
        public void SetDropdownTextFont(Font style)
        {
            this.DropdownFont = style;
        }

        /// <summary>
        /// Sets the style of the Active item's text font in drop-down menu.
        /// </summary>
        /// <param name="style">
        /// Custom font style
        /// </param>
        public void SetDropdownActiveTextFont(Font style)
        {
            this.DropdownActiveFont = style;
        }

        /// <summary>
        /// Sets the style of the drop-down menu header's text font.
        /// </summary>
        /// <param name="style">
        /// Custom font style
        /// </param>
        public void SetMenuHeaderTextFont(Font style)
        {
            this.MenuHeaderFont = style;
        }

        /// <summary>
        /// 
        /// </summary>
        private WidgetServer()
        {
            string name1 = "Arial";
            int size1 = WidgetServer.ScaleFontSize(8);
            this._textFontStyle = new Font(new FontFamily(name1), size1, FontStyle.Regular);

            string name2 = "Arial";
            int num2 = WidgetServer.ScaleFontSize(8);
            this._dropDownFontStyle = new Font(new FontFamily(name2), num2, FontStyle.Regular);

            string nameActive = "Arial";
            int numActive = WidgetServer.ScaleFontSize(10);
            this._dropDownActiveFontStyle = new Font(new FontFamily(nameActive), numActive, FontStyle.Bold);

            string name3 = "Arial";
            int num3 = WidgetServer.ScaleFontSize(8);
            this._menuHeaderFontStyle = new Font(new FontFamily(name3), num3, FontStyle.Bold);

            string name4 = "Arial";
            int num4 = WidgetServer.ScaleFontSize(6);
            this._sliderValueTagFontStyle = new Font(new FontFamily(name4), num4, FontStyle.Italic);

            int width = 8;
            int height = 8;
            RadioButtonSize = new Size(width, height);

            RadioButtonPadding = 4;

            int width2 = 8;
            int height2 = 8;
            CheckBoxSize = new Size(width2, height2);

            CheckBoxPadding = 4;
        }
    }
}

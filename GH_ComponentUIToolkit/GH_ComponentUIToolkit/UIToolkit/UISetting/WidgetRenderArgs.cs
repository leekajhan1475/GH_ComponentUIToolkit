using Grasshopper.GUI.Canvas;

namespace GH_ComponentUIToolkit
{
    public class WidgetRenderArgs
    {
        private GH_Canvas _canvas;

        private WidgetChannel _channel;

        /// <summary>
        /// 
        /// </summary>
        public GH_Canvas Canvas
        {
            get => this._canvas;
            private set => this._canvas = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public WidgetChannel Channel
        {
            get => this._channel;
            private set => this._channel = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="channel"></param>
        public WidgetRenderArgs(GH_Canvas canvas, WidgetChannel channel)
        {
            this._canvas = canvas;
            this._channel = channel;
        }
    }
}

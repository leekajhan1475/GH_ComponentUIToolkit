using Grasshopper.Kernel;
using System.Drawing;

namespace GH_ComponentUIToolkit
{
    public class StandardFont
    {
        /// <summary>
        /// Gets the standard font type.
        /// </summary>
        public static Font Standard
        {
            get => GH_FontServer.StandardAdjusted;
        }

        /// <summary>
        /// Gets the large font type. 
        /// </summary>
        public static Font Large
        {
            get => GH_FontServer.LargeAdjusted;
        }
    }
}

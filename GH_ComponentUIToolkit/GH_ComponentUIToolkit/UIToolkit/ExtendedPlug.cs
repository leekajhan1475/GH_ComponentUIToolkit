using Grasshopper.Kernel;
using System.Collections.Generic;

namespace GH_ComponentUIToolkit
{
    /// <summary>
    /// A class that represents input plug object in the extended menu/table
    /// </summary>
    public class ExtendedPlug
    {
        private bool _isMenu;

        private IGH_Param _parameter;

        private EvaluationUnit _unit;

        private List<string> _enums = null;

        /// <summary>
        /// 
        /// </summary>
        public IGH_Param Parameter
        {
            get => this._parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMenu
        {
            get => this._isMenu;
            set => this._isMenu = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public EvaluationUnit Unit
        {
            get => this._unit;
            set => this._unit = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> EnumInput 
        {
            get => this._enums;
            set => this._enums = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public ExtendedPlug(IGH_Param parameter)
        {
            this._parameter = parameter;
        }
    }
}

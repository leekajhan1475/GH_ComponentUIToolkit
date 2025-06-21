using System;
using System.Collections.Generic;

namespace GH_ComponentUIToolkit
{
    public class EvaluationUnitManager
    {
        private GH_SwitchComponent _switchComponent;

        private List<EvaluationUnit> _units;

        /// <summary>
        /// 
        /// </summary>
        public List<EvaluationUnit> Units
        {
            get => this._units;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public EvaluationUnitManager(GH_SwitchComponent component)
        {
            this._units = new List<EvaluationUnit>();
            this._switchComponent = component;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool GetUnit(string name, out EvaluationUnit rc)
        {
            // Check if input name is invalid
            if (string.IsNullOrEmpty(name))
            {
                rc = null;
                return false;
            }

            foreach (EvaluationUnit unit in this._units)
            {
                if (unit.Name.Equals(name))
                {
                    rc = unit;
                    return true;
                }
            }

            rc = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <exception cref="ArgumentException"></exception>
        public void RegisterUnit(EvaluationUnit unit)
        {
            string name = unit.Name;

            if (name != null)
            {
                if (this.GetUnit(name, out EvaluationUnit rc))
                {
                    throw new ArgumentException("Duplicate evaluation unit[" + name + "] detected");
                }

                unit.SwitchComponent = this._switchComponent;
                this._units.Add(unit);
            }
        }
    }
}

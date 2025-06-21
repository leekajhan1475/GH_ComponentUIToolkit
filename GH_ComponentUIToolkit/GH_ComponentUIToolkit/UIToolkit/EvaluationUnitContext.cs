using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH_ComponentUIToolkit
{
    public class EvaluationUnitContext
    {
        private EvaluationUnit _unit;

        private GH_MenuCollection _collection;

        public GH_MenuCollection Collection
        {
            get => this._collection;
            set => this._collection = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        public EvaluationUnitContext(EvaluationUnit unit)
        {
            this._unit = unit;

            _collection = new GH_MenuCollection();
        }
    }
}

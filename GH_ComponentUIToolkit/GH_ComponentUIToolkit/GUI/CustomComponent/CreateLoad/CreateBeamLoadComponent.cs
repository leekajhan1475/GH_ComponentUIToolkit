using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using GH_ComponentUIToolkit;

namespace GH_ComponentUIToolkit.GUI
{
    public class CreateBeamLoadComponent : GH_SwitchComponent
    {
        private List<SubComponent> _subcomponents = new List<SubComponent>();

        public override string UnitMenuName => "Load Type:";

        public override string ToolStripMenuHeader => "Load Type";

        protected override string DefaultEvaluationUnit => this._subcomponents[0].name();

        public override int VisibleItemCount => 3;

        public override Guid ComponentGuid => new Guid("af4e7fc2-96aa-4109-aab1-d74243b0bb15");

        public override GH_Exposure Exposure => (GH_Exposure)2;

        //protected override Bitmap Icon => Resources.Minion_reading;
        //protected override Bitmap Icon => Resources.Minion_Lautsprecher;

        public CreateBeamLoadComponent()
            : base("Create Beam Load", "Load",
              "A Test Component",
              "St7Toolkit", "Load")
        {
            ((GH_Component)this).Hidden = true;
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Beam", "Beam|Id", "description", GH_ParamAccess.item);

            pManager.AddNumberParameter("Load Case", "LCase", "description", GH_ParamAccess.item);

            pManager.AddNumberParameter("Force", "Force", "description", GH_ParamAccess.item);

            pManager[0].Optional = false;

            pManager[1].Optional = false;

            pManager[2].Optional = false;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("St7Load", "Load", "description", GH_ParamAccess.item);
        }

        protected override void RegisterEvaluationUnits(EvaluationUnitManager mngr)
        {
            this._subcomponents.Add(new Constant());
            this._subcomponents.Add(new Linear());
            this._subcomponents.Add(new Polylinear());
            this._subcomponents.Add(new Trapezoidal());

            foreach (SubComponent item in this._subcomponents)
            {
                item.registerEvaluationUnits(mngr);
            }
        }

        protected override void OnComponentLoaded()
        {
            base.OnComponentLoaded();
            foreach (SubComponent item in this._subcomponents)
            {
                item.OnComponentLoaded();
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA, EvaluationUnit unit)
        {
            //IL_0046: Unknown result type (might be due to invalid IL or missing references)
            if (unit == null)
            {
                return;
            }
            foreach (SubComponent item in this._subcomponents)
            {
                if (unit.Name.Equals(item.name()))
                {
                    item.SolveInstance(DA, out var msg, out var level);
                    if (msg != "")
                    {
                        ((GH_ActiveObject)this).AddRuntimeMessage(level, msg + " May cause errors in exported models.");
                    }
                    return;
                }
            }
            throw new Exception("Invalid sub-component");
        }
    }

}

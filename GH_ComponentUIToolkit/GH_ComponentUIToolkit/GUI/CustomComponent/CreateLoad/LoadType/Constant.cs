using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using GH_ComponentUIToolkit;

namespace GH_ComponentUIToolkit.GUI
{
    public class Constant : SubComponent
    {
        public override string name()
        {
            return "Constant";
        }

        public override string display_name()
        {
            return "Constant";
        }

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Creates a square from the input variables");
            //evaluationUnit.Icon = Resources.Minion_reading;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Number(), "T0", "t0", "description", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_Number(), "T1", "t1", "description", GH_ParamAccess.item);

            evaluationUnit.Inputs[0].Parameter.Optional = false;
            evaluationUnit.Inputs[1].Parameter.Optional = false;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {

            msg = "";
            level = (GH_RuntimeMessageLevel)10;

            double val1 = -1;
            double val2 = -1;
            double val3 = -1;
            double val4 = -1;
            double val5 = -1;

            DA.GetData(0, ref val1);
            DA.GetData(1, ref val2);
            DA.GetData(2, ref val3);
            DA.GetData(3, ref val4);
            DA.GetData(4, ref val5);

            double val = val1 + val2 + val3 + val4 + val5;

            DA.SetData(0, val);
        }
    }

}


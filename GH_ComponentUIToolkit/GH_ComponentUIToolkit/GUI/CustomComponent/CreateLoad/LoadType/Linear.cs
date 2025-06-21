using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using GH_ComponentUIToolkit;

namespace GH_ComponentUIToolkit.GUI
{
    public class Linear : SubComponent
    {
        //private static int default_loadcase = 0;
        //private static Vector3d default_gravity = new Vector3d(0.0, 0.0, -1.0);


        public override string name()
        {
            return "Linear";
        }

        public override string display_name()
        {
            return "Linear";
        }

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Creates a square from the input variables");
            //evaluationUnit.Icon = Resources.Minion_Lautsprecher;
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

            //Point3d origin = new Point3d(0, 0, 0);
            //double sidelength = 1.0;
            //Point3d centre = new Point3d(0, 0, 0);

            //DA.GetData(0, ref centre);
            //DA.GetData(1, ref sidelength);

            //Point3d rectanglecorner = new Point3d(centre.X - sidelength/2, centre.Y - sidelength/2, 0);
            //Point3d secrectanglecorner = new Point3d(centre.X + sidelength / 2, centre.Y + sidelength / 2, 0);

            ////Circle circle = new Circle(centre, radius);
            //Rectangle3d square = new Rectangle3d(new Plane(origin, new Vector3d(0, 0, 1)), rectanglecorner, secrectanglecorner);

            //DA.SetData(0, square);
        }
    }

}


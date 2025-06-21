using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using GH_ComponentUIToolkit;

namespace GH_ComponentUIToolkit.GUI
{
    public class Polylinear : SubComponent
    {
        //private static int default_loadcase = 0;
        //private static Vector3d default_gravity = new Vector3d(0.0, 0.0, -1.0);


        public override string name()
        {
            return "Polylinear";
        }

        public override string display_name()
        {
            return "Polylinear";
        }

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Creates a square from the input variables");
            //evaluationUnit.Icon = Resources.Minion_Lautsprecher;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Number(), "Beam Parameter t", "t", "description", GH_ParamAccess.list);

            evaluationUnit.Inputs[0].Parameter.Optional = false;
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


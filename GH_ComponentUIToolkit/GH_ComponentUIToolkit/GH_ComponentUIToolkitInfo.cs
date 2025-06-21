using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GH_ComponentUIToolkit
{
    public class GH_ComponentUIToolkitInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GH_ComponentUIToolkit";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("42cc943a-0923-4e07-803d-7ac0fb9bec8f");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}

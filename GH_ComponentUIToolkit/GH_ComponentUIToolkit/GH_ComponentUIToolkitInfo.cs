using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GH_ComponentUIToolkit
{
    public class GH_ComponentUIToolkitInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get => "GH_ComponentUIToolkit";
        }

        /// <summary>
        /// 24x24 pixel bitmap to represent this .gha library.
        /// </summary>
        public override Bitmap Icon
        {
            get => null;
        }

        /// <summary>
        /// Gets a short string describing the purpose of this .gha library.
        /// </summary>
        public override string Description
        {
            get => "Custom Grasshopper components";
        }

        /// <summary>
        /// Gets a GUID representing this .gha library.
        /// </summary>
        public override Guid Id
        {
            get => new Guid("42cc943a-0923-4e07-803d-7ac0fb9bec8f");
        }

        /// <summary>
        /// Gets the name of this assembly's author
        /// </summary>
        public override string AuthorName
        {
            get => "Zong-Han Chan";
        }

        /// <summary>
        /// Gets the contact info of this assembly's author
        /// </summary>
        public override string AuthorContact
        {
            get => "zonghan.chan@burohappold.com";
        }
    }
}

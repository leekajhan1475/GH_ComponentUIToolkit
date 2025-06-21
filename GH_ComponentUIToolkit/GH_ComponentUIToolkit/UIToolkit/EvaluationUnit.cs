using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace GH_ComponentUIToolkit
{
    public class EvaluationUnit
    {
        private string _name;

        private string _displayName;

        private string _description;

        private bool _isActive;

        private bool _keepLinks;

        private Bitmap _icon;

        private EvaluationUnitContext _context;

        private GH_SwitchComponent _switchComponent;

        private List<ExtendedPlug> _inputs;

        private List<ExtendedPlug> _outputs;

        /// <summary>
        /// Gets or sets the name of this evaluation unit.
        /// </summary>
        public string Name
        {
            get => this._name;
            set => this._name = value;
        }

        /// <summary>
        /// Gets or sets the display name of this evaluation unit.
        /// </summary>
        public string DisplayName
        {
            get => this._displayName;
            set => this._displayName = value;
        }

        /// <summary>
        /// Gets or sets the description of this evaluation unit.
        /// </summary>
        public string Description
        {
            get => this._description;
            set => this._description = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive
        {
            get => this._isActive;
            set => this._isActive = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool KeepLinks
        {
            get => this._keepLinks;
            set => this._keepLinks = value;
        }

        /// <summary>
        /// Gets or sets the icon for this evaluation unit
        /// </summary>
        public Bitmap Icon
        {
            get => this._icon;
            set => this._icon = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public EvaluationUnitContext Context
        {
            get => this._context;
        }

        /// <summary>
        /// 
        /// </summary>
        public GH_SwitchComponent SwitchComponent
        {
            get => this._switchComponent;
            set => this._switchComponent = value;
        }

        /// <summary>
        /// Gets the collection of input parameter plugs of this evaluation unit.
        /// </summary>
        public List<ExtendedPlug> Inputs
        {
            get => this._inputs;
        }

        /// <summary>
        /// Gets the collection of output parameter plugs of this evaluation unit.
        /// </summary>
        public List<ExtendedPlug> Outputs
        {
            get => this._outputs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="icon"></param>
        public EvaluationUnit(string name, 
                              string displayName, // the name that displays
                              string description, // description of this evaluation unit
                              Bitmap icon = null)
        {
            this._name = name;
            this._displayName = displayName;
            this._description = description;
            this._inputs = new List<ExtendedPlug>();
            this._outputs = new List<ExtendedPlug>();
            this._keepLinks = false;
            this._icon = icon;
            this._context = new EvaluationUnitContext(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        private static Type GetGenericType(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                Type right = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == right)
                {
                    return toCheck;
                }
                toCheck = toCheck.BaseType;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="name"></param>
        /// <param name="nickName"></param>
        /// <param name="description"></param>
        /// <param name="access"></param>
        /// <param name="defaultValue"></param>
        public void RegisterInputParam(IGH_Param param, string name, string nickName, string description, GH_ParamAccess access, IGH_Goo defaultValue)
        {            
            param.Name = name;
            param.NickName = nickName;
            param.Description = description;
            param.Access = access;
            try
            {
                if (defaultValue != null && typeof(IGH_Goo).IsAssignableFrom(param.GetType()))
                {
                    //Type genericType = GetGenericType(typeof(GH_PersistentParam), ((object)param).GetType());
                    Type genericType = GetGenericType(typeof(GH_PersistentParam<>), ((object)param).GetType());
                    if (genericType != null)
                    {
                        Type[] genericArguments = genericType.GetGenericArguments();
                        if (genericArguments.Length != 0)
                        {
                            Type type = genericArguments[0];
                            genericType.GetMethod("SetPersistentData", BindingFlags.Instance | BindingFlags.Public, null, new Type[1]
                            {
                            genericArguments[0]
                            }, null).Invoke(param, new object[1]
                            {
                            defaultValue
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            ExtendedPlug extendedPlug = new ExtendedPlug(param);
            extendedPlug.Unit = this;
            this._inputs.Add(extendedPlug);
        }

        public void RegisterInputParam(IGH_Param param, string name, string nickName, string description, GH_ParamAccess access)
        {
              RegisterInputParam(param, name, nickName, description, access, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="name"></param>
        /// <param name="nickName"></param>
        /// <param name="description"></param>
        public void RegisterOutputParam(IGH_Param param, string name, string nickName, string description)
        {
            param.Name = name;
            param.NickName = nickName;
            param.Description = description;
            ExtendedPlug extendedPlug = new ExtendedPlug(param);
            this._outputs.Add(extendedPlug);
        }

        /// <summary>
        /// 
        /// </summary>
        public void NewParameterIds()
        {
            foreach (ExtendedPlug input in this._inputs)
            {
                input.Parameter.NewInstanceGuid();
            }
            foreach (ExtendedPlug output in this._outputs)
            {
                output.Parameter.NewInstanceGuid();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        public void AddMenu(GH_ExtendableMenu menu)
        {
            Context.Collection.AddMenu(menu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool Write(GH_IWriter writer)
        {
            writer.SetString("name", this.Name);

            GH_IWriter val = writer.CreateChunk("params");
            GH_IWriter val2 = val.CreateChunk("input");

            val2.SetInt32("index", 0);
            val2.SetInt32("count", this.Inputs.Count);

            for (int i = 0; i < this._inputs.Count; i++)
            {
                if (this._inputs[i].Parameter.Attributes != null)
                {
                    GH_IWriter val3 = val2.CreateChunk("p", i);
                    this._inputs[i].Parameter.Write(val3);
                }
            }

            // Outputs
            int outputCount = this.Outputs.Count;

            GH_IWriter val4 = val.CreateChunk("output");

            val4.SetInt32("index", 0);
            val4.SetInt32("count", outputCount);

            for (int j = 0; j < outputCount; j++)
            {
                if (this._outputs[j].Parameter.Attributes != null)
                {
                    GH_IWriter val5 = val4.CreateChunk("p", j);
                    this._outputs[j].Parameter.Write(val5);
                }
            }
            GH_IWriter writer2 = writer.CreateChunk("context");
            return this._context.Collection.Write(writer2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool Read(GH_IReader reader)
        {
            int inputCount = this.Inputs.Count;
            int outputCount = this.Outputs.Count;

            if (reader.ChunkExists("params"))
            {
                GH_IReader val = reader.FindChunk("params");

                // Inputs
                if (val.ChunkExists("input") && this._inputs != null)
                {
                    GH_IReader val2 = val.FindChunk("input");
                    int num = -1;
                    if (val2.TryGetInt32("count", ref num) && inputCount == num)
                    {
                        for (int i = 0; i < num; i++)
                        {
                            if (val2.ChunkExists("p", i))
                            {
                                this._inputs[i].Parameter.Read(val2.FindChunk("p", i));
                            }
                            else if (val2.ChunkExists("param", i))
                            {
                                this._inputs[i].Parameter.Read(val2.FindChunk("param", i));
                            }
                        }
                    }
                }

                // Outputs
                if (val.ChunkExists("output") && this._outputs != null)
                {
                    GH_IReader val3 = val.FindChunk("output");
                    int num2 = -1;
                    if (val3.TryGetInt32("count", ref num2) && outputCount == num2)
                    {
                        for (int j = 0; j < num2; j++)
                        {
                            if (val3.ChunkExists("p", j))
                            {
                                this._outputs[j].Parameter.Read(val3.FindChunk("p", j));
                            }
                            else if (val3.ChunkExists("param", j))
                            {
                                this._outputs[j].Parameter.Read(val3.FindChunk("param", j));
                            }
                        }
                    }
                }
            }
            try
            {
                GH_IReader val4 = reader.FindChunk("context");
                if (val4 != null)
                {
                    this._context.Collection.Read(val4);
                }
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("Component error:" + ex.Message + "\n" + ex.StackTrace);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetMenuDescription()
        {
            return Context.Collection.GetMenuDescription();
        }


    }
}

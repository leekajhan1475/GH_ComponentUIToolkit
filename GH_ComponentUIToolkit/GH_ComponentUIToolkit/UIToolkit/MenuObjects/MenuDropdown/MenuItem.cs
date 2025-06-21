using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH_ComponentUIToolkit
{
    public class MenuItem
    {
        private string _content;

        private string _name;

        private int _index;

        private object _data;

        public string Content
        {
            get => this._content;
            set => this._content = value;
        }

        public string Name
        {
            get => this._name;
            set => this._name = value;
        }

        public int Index
        {
            get => this._index;
            set => this._index = value;
        }

        public object Data
        {
            get => this._data;
            set => this._data = value;
        }

        public MenuItem(string name, string content, int ind)
        {
            this._content = content;
            this._name = name;
            this._index = ind;
        }
    }
}

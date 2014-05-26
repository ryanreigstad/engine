using System.IO;

namespace engine.data.config
{
    public class ShaderConfig : AbstractConfig
    {
        public ShaderConfig(FileInfo file)
            : base(file)
        {
        }

        public override AbstractConfig GetDefaults(FileInfo file)
        {
            return new ShaderConfig(file);
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    string oldvalue = _name, newvalue = value;
                    _name = newvalue;
                    NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private string _vertexShader;
        public string VertexShader
        {
            get { return _vertexShader; }
            set
            {
                if (_vertexShader != value)
                {
                    string oldvalue = _vertexShader, newvalue = value;
                    _vertexShader = newvalue;
                    NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private string _fragmentShader;
        public string FragmentShader
        {
            get { return _fragmentShader; }
            set
            {
                if (_fragmentShader != value)
                {
                    string oldvalue = _fragmentShader, newvalue = value;
                    _fragmentShader = newvalue;
                    NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using engine.Libraries;
using OpenTK.Graphics.OpenGL;

namespace engine.Libraries
{
    static class ShaderLibrary
    {
        public static readonly Dictionary<string, int> Shaders = new Dictionary<string, int>();


        public static void RegisterShader(string name, int id)
        {
            Shaders[name] = id;
        }

        public static void LoadShaders(string program, string[] filenames, out List<int> handle)
        {
            ProgramLibrary.RegisterProgram(program, GL.CreateProgram());
            var handles = new List<int>();
            var shaderType = ShaderType.VertexShader;
            foreach (var s in filenames)
            {
                int value;
                if (s.Contains("vert"))
                    shaderType = ShaderType.VertexShader;
                else if (s.Contains("frag"))
                    shaderType = ShaderType.FragmentShader;

                LoadShaderFromFile(s, shaderType, ProgramLibrary.GetProgram(program), out value);
                handles.Add(value);
            }

            GL.LinkProgram(ProgramLibrary.GetProgram(program));
            //debug, can remove
            Console.WriteLine(GL.GetProgramInfoLog(ProgramLibrary.GetProgram(program)));
            /////


            handle = handles;
        }


        public static void LoadShaderFromFile(string filename, ShaderType type, int program, out int address)
        {
            //create the shader
            address = GL.CreateShader(type);
            //load the file from the disk
            using (var sr = new StreamReader(@"C:\Users\Tree\Documents\GitHub\engine\engine\Shaders\" + filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            //compile the shader
            GL.CompileShader(address);
            //atach it to the program
            GL.AttachShader(program, address);
            //debug, can remove
            Console.WriteLine(GL.GetShaderInfoLog(address));
            ////////
        }
    }
}

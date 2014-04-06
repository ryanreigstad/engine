using System;
using System.Collections.Generic;
using System.IO;

using OpenTK.Graphics.OpenGL4;

namespace engine.graphics
{
    public static class ShaderLibrary
    {
        public const string ShaderPath = @"\..\..\Shaders\";
        public const string VertexExt = "vert";
        public const string FragmentExt = "frag";

        public static readonly Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();

        public static Shader GetShader(string name)
        {
            return GetShader<Shader>(name);
        }

        public static T GetShader<T>(string name)
            where T : Shader
        {
            return Shaders.ContainsKey(name) && Shaders[name] is T ? (T) Shaders[name] : null;
        }

        public static T LoadShader<T>(string name, params string[] filenames)
            where T : Shader, new()
        {
            var shader = new T();
            foreach (string file in filenames)
            {
                using (var sr = new StreamReader(Environment.CurrentDirectory + ShaderPath + file))
                {
                    shader.AddProgram(sr.ReadToEnd(), TypeForFile(file));
                }
            }
            shader.Compile();
            Shaders[name] = shader;
            return shader;
        }

        public static ShaderType TypeForFile(string name)
        {
            switch (name.Substring(name.LastIndexOf('.') + 1))
            {
                case VertexExt:
                    return ShaderType.VertexShader;
                case FragmentExt:
                    return ShaderType.FragmentShader;
                default:
                    throw new Exception("unknown shader type: " + name);
            }
        }
    }
}

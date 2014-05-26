using System;
using System.Collections.Generic;
using System.IO;

using engine.core.rendering;
using engine.data;
using engine.data.config;
using engine.data.exceptions;
using engine.util.extensions;

using OpenTK.Graphics.OpenGL4;

namespace engine.core.resources
{
    public static class ShaderLibrary
    {
        private const string VertexExt = ".vert";
        private const string FragmentExt = ".frag";

        private static readonly Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();

        public static Shader GetShader(string name)
        {
            return Shaders.ContainsKey(name) ? Shaders[name] : null;
        }

        public static T GetShader<T>(string name)
            where T : Shader
        {
            return (T) GetShader(name);
        }

        public static T LoadShader<T>(string name, params string[] filenames)
            where T : Shader, new()
        {
            var shader = new T();
            foreach (string file in filenames)
            {
                var fi = Filepaths.GetShaderFileInfo(file);
                using (var sr = new StreamReader(fi.FullName))
                {
                    shader.AddProgram(sr.ReadToEnd(), TypeForFile(fi));
                }
            }
            shader.Compile();
            Shaders[name] = shader;
            return shader;
        }

        public static ShaderType TypeForFile(FileInfo file)
        {
            switch (file.Extension)
            {
                case VertexExt:
                    return ShaderType.VertexShader;
                case FragmentExt:
                    return ShaderType.FragmentShader;
                default:
                    throw new ShaderException(new ArgumentException("Unknown shader extension \"{0}\".".Formatted(file.Name)));
            }
        }
    }
}

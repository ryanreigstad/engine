using System;
using System.IO;
using engine.util.extensions;

namespace engine.data
{
    /// <summary>
    /// This should be how you get your files.
    /// </summary>
    public static class Filepaths
    {
        private const string FilepathFormat = @"{0}{1}\{2}";

        private const string ConfigPath = @"\..\..\..\Configs";
        private const string ModelsPath = @"\..\..\Models";
        private const string TexturesPath = @"\..\..\Textures";
        private const string ShadersPath = @"\..\..\Shaders";

        private static string CurrentPath
        {
            get { return Environment.CurrentDirectory; }
        }

        private static FileInfo GetFileInfo(string path, string filename)
        {
            return new FileInfo(FilepathFormat.Formatted(CurrentPath, path, filename));
        }

        public static FileInfo GetConfigFileInfo(string filename)
        {
            return GetFileInfo(ConfigPath, filename);
        }

        public static FileInfo GetModelFileInfo(string filename)
        {
            return GetFileInfo(ModelsPath, filename);
        }

        public static FileInfo GetTextureFileInfo(string filename)
        {
            return GetFileInfo(TexturesPath, filename);
        }

        public static FileInfo GetShaderFileInfo(string filename)
        {
            return GetFileInfo(ShadersPath, filename);
        }
    }
}

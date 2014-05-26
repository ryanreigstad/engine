using System.IO;
using engine.util.extensions;

namespace engine.data.exceptions
{
    /// <summary>
    /// If you have an exception that you throw a lot, make it here then:
    /// throw new ?T:EngineException?(StandardExceptions.New?YourException?(?string any, string message, object args?))
    /// </summary>
    public static class StandardExceptions
    {
        #region FileNotFoundExceptions

        public static FileNotFoundException NewFileNotFound(string filename)
        {
            return new FileNotFoundException("File '{0}' could not be found.".Formatted(filename));
        }

        public static FileNotFoundException NewFileNotFound(FileInfo file)
        {
            return NewFileNotFound(file.FullName);
        }

        #endregion
    }
}

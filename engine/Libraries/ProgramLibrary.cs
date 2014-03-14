using System.Collections.Generic;

namespace engine.Libraries
{
    static class ProgramLibrary
    {
        public static readonly Dictionary<string, int> Programs = new Dictionary<string, int>();

        public static bool HasProgram(string programName)
        {
            return Programs.ContainsKey(programName);
        }

        public static void RegisterProgram(string name, int id)
        {
            Programs[name] = id;
        }

        public static int GetProgram(string programName)
        {
            return HasProgram(programName) ? Programs[programName] : -1;
        }
    }
}

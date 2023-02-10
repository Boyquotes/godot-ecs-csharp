using System.Runtime.CompilerServices;

namespace GdEcs
{

    [System.AttributeUsage(System.AttributeTargets.Class)]
    internal sealed class ExportCustomNodeAttribute : System.Attribute
    {

        public readonly string? BaseTypePath;
        public readonly string ScriptPath;

        public ExportCustomNodeAttribute(string? baseTypePath = null, [CallerFilePath] string scriptPath = "")
        {
            this.BaseTypePath = baseTypePath;
            this.ScriptPath = scriptPath;
        }

    }

}
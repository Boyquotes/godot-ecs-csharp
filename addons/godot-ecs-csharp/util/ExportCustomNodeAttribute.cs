using System.Runtime.CompilerServices;

namespace GdEcs
{

    [System.AttributeUsage(System.AttributeTargets.Class)]
    internal sealed class ExportCustomNodeAttribute : System.Attribute
    {

        public readonly string? IconName;
        public readonly string? BaseTypePath;
        public readonly string ScriptPath;

        public ExportCustomNodeAttribute(string? iconName = null, string? baseTypePath = null,
            [CallerFilePath] string scriptPath = "")
        {
            this.IconName = iconName;
            this.BaseTypePath = baseTypePath;
            this.ScriptPath = scriptPath;
        }

    }

}
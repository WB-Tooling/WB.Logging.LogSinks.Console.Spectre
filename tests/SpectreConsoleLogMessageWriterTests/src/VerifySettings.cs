using System.Runtime.CompilerServices;
using DiffEngine;
using VerifyTUnit;

namespace ExtensionsTests;

public static class VerifySettingsConfig
{
    [ModuleInitializer]
    public static void Init()
    {
        Verifier.UseSourceFileRelativeDirectory("__snapshots__");
        DiffTools.UseOrder(DiffTool.VisualStudioCode);
    }
}

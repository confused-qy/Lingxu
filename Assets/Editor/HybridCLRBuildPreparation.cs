using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

// Keep native placeholder names in sync when a hot-update assembly is renamed.
internal sealed class HybridCLRBuildPreparation : IPreprocessBuildWithReport
{
    public int callbackOrder => -100;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (!SettingsUtil.Enable)
            return;

        var group = BuildPipeline.GetBuildTargetGroup(report.summary.platform);
        if (PlayerSettings.GetScriptingBackend(group) != ScriptingImplementation.IL2CPP)
            return;

        Il2CppDefGeneratorCommand.GenerateIl2CppDef();
    }
}

// Builds the WebGL player for CI (Vault's unity-build.yml, game-ci buildMethod: VaultWebGLBuild.Build).
// game-ci's own build script uses C# 7, which Unity 2018.2 can't compile, so this project brings its own,
// in C# 4. game-ci passes the output path as -customBuildPath and reads the exit code.
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class VaultWebGLBuild
{
    public static void Build()
    {
        string path = "build/WebGL/WebGL";
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "-customBuildPath") path = args[i + 1];
        }
        // Debug IL2CPP makes Unity 2018.2 pass -disable-O0-optnone, which the CI image's compiler rejects.
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.WebGL, Il2CppCompilerConfiguration.Release);
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        UnityEngine.Debug.Log("VaultWebGLBuild: " + scenes.Length + " scenes -> " + path);
        BuildReport report = BuildPipeline.BuildPlayer(scenes, path, BuildTarget.WebGL, BuildOptions.None);
        UnityEngine.Debug.Log("VaultWebGLBuild: " + report.summary.result + ", " + report.summary.totalErrors + " errors, " + report.summary.totalSize + " bytes");
        EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : 1);
    }
}

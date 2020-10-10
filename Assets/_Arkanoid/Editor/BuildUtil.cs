using UnityEditor;
using UnityEngine;

public class BuildUtil
{
    static void PerformBuild()
    {
        var report = BuildPipeline.BuildPlayer(
            new[] { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/Game.unity" },
            "Build/Win/Test.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None);

        Debug.Log(report);
    }
}

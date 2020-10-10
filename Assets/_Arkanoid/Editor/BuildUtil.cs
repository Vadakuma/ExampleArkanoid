using UnityEditor;
using UnityEngine;

public class BuildUtil
{
    static void PerformBuild()
    {
        var report = BuildPipeline.BuildPlayer(
            new[] { "Assets/_Arkanoid/Scenes/MainMenu.unity", "Assets/_Arkanoid/Scenes/Game.unity" },
            "Build/Win/Test.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None);

        Debug.Log(report);
    }
}

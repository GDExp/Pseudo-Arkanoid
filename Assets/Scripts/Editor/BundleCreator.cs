using UnityEditor;
using UnityEngine;

public class BundleCreator : Editor
{
    [MenuItem("Assets/Build Bundles")]
    static void BuildAssetsBundles()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, BuildTarget.WebGL);
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}

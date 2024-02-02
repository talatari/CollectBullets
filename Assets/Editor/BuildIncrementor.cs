using Source.Build;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    public class BuildIncrementor : IPreprocessBuildWithReport
    {
        private const string PathToBuildAsset = "Assets/Source/Build/Build.asset";
        
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildScriptableObject buildScriptableObject = ScriptableObject.CreateInstance<BuildScriptableObject>();

            switch (report.summary.platform)
            {
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.WebGL:
                    PlayerSettings.macOS.buildNumber = IncrementBuildNumber(PlayerSettings.macOS.buildNumber);
                    buildScriptableObject.BuildNumber = PlayerSettings.macOS.buildNumber;
                    break;

                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = IncrementBuildNumber(PlayerSettings.iOS.buildNumber);
                    buildScriptableObject.BuildNumber = PlayerSettings.iOS.buildNumber;
                    break;

                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode++;
                    buildScriptableObject.BuildNumber = PlayerSettings.Android.bundleVersionCode.ToString();
                    break;

                case BuildTarget.PS4:
                    PlayerSettings.PS4.appVersion = IncrementBuildNumber(PlayerSettings.PS4.appVersion);
                    buildScriptableObject.BuildNumber = PlayerSettings.PS4.appVersion;
                    break;

                case BuildTarget.XboxOne:
                    PlayerSettings.XboxOne.Version = IncrementBuildNumber(PlayerSettings.XboxOne.Version);
                    buildScriptableObject.BuildNumber = PlayerSettings.XboxOne.Version;
                    break;

                case BuildTarget.WSAPlayer:
                    PlayerSettings.WSA.packageVersion = new System.Version(
                        PlayerSettings.WSA.packageVersion.Major, 
                        PlayerSettings.WSA.packageVersion.Minor, 
                        PlayerSettings.WSA.packageVersion.Build + 1);
                    buildScriptableObject.BuildNumber = PlayerSettings.WSA.packageVersion.Build.ToString();
                    break;

                case BuildTarget.Switch:
                    PlayerSettings.Switch.displayVersion = IncrementBuildNumber(PlayerSettings.Switch.displayVersion);
                    PlayerSettings.Switch.releaseVersion = IncrementBuildNumber(PlayerSettings.Switch.releaseVersion);
                    buildScriptableObject.BuildNumber = PlayerSettings.Switch.displayVersion;
                    break;

                case BuildTarget.tvOS:
                    PlayerSettings.tvOS.buildNumber = IncrementBuildNumber(PlayerSettings.tvOS.buildNumber);
                    buildScriptableObject.BuildNumber = PlayerSettings.tvOS.buildNumber;
                    break;
            }

            AssetDatabase.DeleteAsset(PathToBuildAsset);
            AssetDatabase.CreateAsset(buildScriptableObject, PathToBuildAsset);
            AssetDatabase.SaveAssets();
        }

        private string IncrementBuildNumber(string buildNumber)
        {
            int.TryParse(buildNumber, out int outputBuildNumber);
        
            return (outputBuildNumber + 1).ToString();
        }
    }
}
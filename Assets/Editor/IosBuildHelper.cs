using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections.Generic;
 
public class IosBuildHelper : MonoBehaviour
{
#if UNITY_IOS
    [PostProcessBuild]
    static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        // Read plist
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
 
        // Update value
        PlistElementDict rootDict = plist.root;
        
        rootDict.SetString("NSLocationWhenInUseUsageDescription", "Program requires GPS to track your location while using the app");
        rootDict.CreateDict("NSAppTransportSecurity");
        rootDict["NSAppTransportSecurity"].AsDict().SetBoolean("NSAllowsArbitraryLoads", true);
        rootDict["NSAppTransportSecurity"].AsDict().CreateDict("NSExceptionDomains");
        rootDict["NSAppTransportSecurity"]["NSExceptionDomains"].AsDict().SetString("google.com", "");
        

        // Write plist
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}
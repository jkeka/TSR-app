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
        if (rootDict ["NSAppTransportSecurity"] == null) {
          rootDict.CreateDict ("NSAppTransportSecurity");
        }

        rootDict ["NSAppTransportSecurity"].AsDict ().SetBoolean ("NSAllowsArbitraryLoads", true);
        rootDict ["NSAppTransportSecurity"].AsDict ().SetBoolean ("NSAllowsArbitraryLoadsInWebContent", true);

        var exceptionDomains = rootDict ["NSAppTransportSecurity"].AsDict().CreateDict ("NSExceptionDomains");
        var domain = exceptionDomains.CreateDict ("YOURDOMAIN.com");

        domain.SetBoolean ("NSExceptionAllowsInsecureHTTPLoads", true);
        domain.SetBoolean ("NSIncludesSubdomains", true);

        // Write plist
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}
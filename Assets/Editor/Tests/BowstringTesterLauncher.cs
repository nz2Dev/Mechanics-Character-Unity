using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BowstringTester))]
public class BowstringTesterLauncher : Editor {

	public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Stick")) {
            BowstringTester tester = target as BowstringTester;
            tester.Stick();
        }
        if (GUILayout.Button("Release")) {
            BowstringTester tester = target as BowstringTester;
            tester.Release();
        }
    }

}

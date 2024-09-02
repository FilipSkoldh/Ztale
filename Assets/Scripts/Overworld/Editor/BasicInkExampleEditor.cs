using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InkScript))]
[InitializeOnLoad]
public class BasicInkExampleEditor : Editor {
    static bool storyExpanded;
    static BasicInkExampleEditor () {
        InkScript.OnCreateStory += OnCreateStory;
    }

    static void OnCreateStory (Story story) {
        // If you'd like NOT to automatically show the window and attach (your teammates may appreciate it!) ten replace "true" with "false" here. 
        InkPlayerWindow window = InkPlayerWindow.GetWindow(false);
        if(window != null) InkPlayerWindow.Attach(story);
    }
	public override void OnInspectorGUI () {
		Repaint();
		base.OnInspectorGUI ();
		var realTarget = target as InkScript;
		var story = realTarget.inkStory;
		InkPlayerWindow.DrawStoryPropertyField(story, ref storyExpanded, new GUIContent("Story"));
	}
}

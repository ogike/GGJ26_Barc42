using Dialogue;
using Ink.Runtime;
using Ink.UnityIntegration;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueManager))]
[InitializeOnLoad]
public class BasicInkExampleEditor : Editor {
    static bool storyExpanded;
    
    static BasicInkExampleEditor()
    {
        DialogueManager.OnCreateStory += BindInkWindows;
        DialogueManager.OnDestroyStory += OnDestroyStory;
    }
    
    static void BindInkWindows(Story story)
    {
        InkPlayerWindow window = InkPlayerWindow.GetWindow(false);
        InkPlayerWindow.InkPlayerParams inkPlayerParams = new InkPlayerWindow.InkPlayerParams();
        inkPlayerParams.disablePlayControls = false;
        inkPlayerParams.disableUndoHistory = false;
        inkPlayerParams.disableChoices = false;
        inkPlayerParams.disableStateLoading = false;
        inkPlayerParams.disableSettingVariables = false;
        inkPlayerParams.profileOnStart = false;
        if(window != null) InkPlayerWindow.Attach(story, inkPlayerParams);
    }
    
    public override void OnInspectorGUI () {
        Repaint();
        base.OnInspectorGUI ();
        var realTarget = target as DialogueManager;
        var story = realTarget.currentStory;
        InkPlayerWindow.DrawStoryPropertyField(story, ref storyExpanded, new GUIContent("Story"));
    }

    static void OnDestroyStory()
    {
        InkPlayerWindow window = InkPlayerWindow.GetWindow(false);
        if(window != null) InkPlayerWindow.Detach();
    }
}
#if (UNITY_EDITOR)
using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A custom editor window that can be used to manually run functions in scripts.
/// </summary>
public class ScriptDebugger : EditorWindow
{
    private AudioHandler audioHandler;
    private CameraManager cameraManager;
    private CharacterManager characterManager;
    private List<string> validCharacters;

    [MenuItem("Window/Script Debugger")]
    public static void ShowDebugger()
    {
        ScriptDebugger wnd = GetWindow<ScriptDebugger>();
        wnd.titleContent = new GUIContent("Script Debugger");
    }

    public void CreateGUI()
    {
        // Get necessary scripts
        audioHandler = FindFirstObjectByType<AudioHandler>();
        cameraManager = FindFirstObjectByType<CameraManager>();
        characterManager = FindFirstObjectByType<CharacterManager>();
        
        // This is a list of valid characters that can be used in the audio debugging functions
        validCharacters = new List<string> { "spongebob", "patrick", "squidward", "mrkrabs", "plankton", "sandy", "larry", "bubblebuddy", "mrspuff" };

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        VisualElement container = CreateContainer(root);
        CreateCameraControls(container);
        CreateTargetingControls(container);
        CreateAudioControls(container);
        CreateAudioFunctionsControls(container);
        CreateHelperBox(root);
    }

    private VisualElement CreateContainer(VisualElement root)
    {
        VisualElement container = new();
        container.style.marginLeft = 5;
        container.style.marginRight = 5;
        container.style.marginTop = 5;
        container.style.marginBottom = 5;
        root.Add(container);
        return container;
    }

    private void CreateCameraControls(VisualElement container)
    {
        VisualElement cameraControls = new Label("Camera Functions");
        cameraControls.style.unityFontStyleAndWeight = FontStyle.Bold;
        container.Add(cameraControls);
    }

    private void CreateTargetingControls(VisualElement container)
    {
        Foldout characterFoldout = new()
        {
            text = "Targeting Controls",
            value = true // Set to true to make it initially expanded
        };
        container.Add(characterFoldout);

        VisualElement characterContainer = new();
        characterContainer.style.marginBottom = 5;
        characterFoldout.Add(characterContainer);

        foreach (string character in validCharacters)
        {
            characterContainer.Add(CreateDebugButton($"Follow {character}", () => { MoveCamera(character); }));
        }
    }

    private void MoveCamera(string character)
    {
        Debug.Log($"SCRIPT DEBUGGER: Camera now tracking {character}");
        cameraManager.MoveCamera(characterManager.Characters[character]);
    }

    private void CreateAudioControls(VisualElement container)
    {
        VisualElement audioControls = new Label("Audio Functions");
        audioControls.style.unityFontStyleAndWeight = FontStyle.Bold;
        audioControls.style.marginTop = 5;
        container.Add(audioControls);
    }

    private void CreateAudioFunctionsControls(VisualElement container)
    {
        // Load voice line controls
        Foldout loadVoiceLineFoldout = new()
        {
            text = "Load Voice Line Controls",
            value = true // Set to true to make it initially expanded
        };
        container.Add(loadVoiceLineFoldout);

        // Create the dropdown field
        DropdownField dropdown = new("Character", validCharacters, 0);
        loadVoiceLineFoldout.Add(dropdown);

        // Create a text field for the file name
        var fileNameField = new TextField("File Name");
        loadVoiceLineFoldout.Add(fileNameField);

        // Create a button to load the voice line
        loadVoiceLineFoldout.Add(CreateDebugButton("Load voice line", () => { LoadVoiceLine(dropdown.value, fileNameField.value); }));

        // Play voice line controls
        Foldout playVoiceLineFoldout = new()
        {
            text = "Play Voice Line Controls",
            value = true // Set to true to make it initially expanded
        };
        container.Add(playVoiceLineFoldout);

        // Create the dropdown field
        var dropdown1 = new DropdownField("Character", validCharacters, 0);
        playVoiceLineFoldout.Add(dropdown1);

        // Create a button to play the voice line
        playVoiceLineFoldout.Add(CreateDebugButton("Play voice line", () => { PlayVoiceLine(dropdown1.value); }));

		// Create a button to stop all audio sources
		playVoiceLineFoldout.Add(CreateDebugButton("Stop all audio sources", () => { StopAllAudioSources(); }));
	}

	private void StopAllAudioSources()
    {
        // Find all audio sources for each character and stop them from playing
        Debug.Log("SCRIPT DEBUGGER: Stopping all audio sources");
		foreach (KeyValuePair<string, Character> character in characterManager.Characters)
		{
			character.Value.AudioSource.Stop();
		}
	}

	private void LoadVoiceLine(string characterNameParameterLoad, string fileNameParameter)
    {
        Debug.Log($"SCRIPT DEBUGGER: Loading voice line for {characterNameParameterLoad} from file {fileNameParameter}");
        audioHandler.LoadVoiceLine(characterNameParameterLoad, fileNameParameter);
    }

    private void PlayVoiceLine(string characterNameParameterPlay)
    {
        Debug.Log($"SCRIPT DEBUGGER: Playing voice line for {characterNameParameterPlay}");
        audioHandler.PlayVoiceLine(characterNameParameterPlay);
    }

    private Button CreateDebugButton(string buttonText, Action onClickAction)
    {
        Button button = new(onClickAction)
        {
            text = buttonText
        };

        button.SetEnabled(EditorApplication.isPlaying);

        UnityEditor.EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            button.SetEnabled(state == PlayModeStateChange.EnteredPlayMode);
        };

        return button;
    }

    private void CreateHelperBox(VisualElement root)
    {
        HelpBox helpBox = new("You must be in play mode to use these functions.", HelpBoxMessageType.Info);
        root.Add(helpBox);

        helpBox.style.display = EditorApplication.isPlaying ? DisplayStyle.None : DisplayStyle.Flex;

        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            helpBox.style.display = (state == PlayModeStateChange.EnteredPlayMode) ? DisplayStyle.None : DisplayStyle.Flex;
        };
    }
}
#endif

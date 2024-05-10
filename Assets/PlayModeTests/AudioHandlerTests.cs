using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class AudioHandlerTests
{
    private AudioHandler audioHandler;
    private CharacterManager characterManager;

    [SetUp]
    public void Setup()
    {
        // Load the scene
        SceneManager.LoadScene("BikiniBottom");
        // Wait until the scene has loaded and then find the AudioHandler and CharacterManager
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "BikiniBottom")
            {
                // Find the AudioHandler and CharacterManager
                audioHandler = GameObject.Find("Script").GetComponent<AudioHandler>();
                characterManager = GameObject.Find("Script").GetComponent<CharacterManager>();
            }
        };
    }

    // Check that the dictionary is not null after RetrieveAudioSources
    [UnityTest]
    public IEnumerator RetrieveAudioSources_DictionaryIsNotNull()
    {
        Assert.IsNotNull(audioHandler.AudioSources, "AudioSources dictionary is null");
        yield return null;
    }

    // Check that the dictionary has the same count as the Characters dictionary in CharacterManager
    [UnityTest]
    public IEnumerator RetrieveAudioSources_HasSameCountAsCharacters()
    {
        Assert.AreEqual(characterManager.Characters.Count, audioHandler.AudioSources.Count, "AudioSources dictionary does not have the same count as Characters dictionary");
        yield return null;
    }

    // Check that the dictionary has the correct AudioSource for each character
    [UnityTest]
    public IEnumerator RetrieveAudioSources_HasCorrectAudioSources()
    {
        foreach (KeyValuePair<string, Character> character in characterManager.Characters)
        {
            Assert.AreEqual(character.Value.AudioSource, audioHandler.AudioSources[character.Key], $"AudioSource for character {character.Key} is not correct");
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator LoadVoiceLine_CorrectlyLoadsAudioClip()
    {
        audioHandler.LoadVoiceLine("spongebob", "audio1");
        Assert.IsNotNull(audioHandler.AudioSources["spongebob"].clip, "No audio clip loaded for spongebob");
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayVoiceLine_CorrectlyPlaysAudioClip()
    {
        audioHandler.LoadVoiceLine("spongebob", "audio1");
        audioHandler.PlayVoiceLine("spongebob");
        Assert.IsTrue(audioHandler.AudioSources["spongebob"].isPlaying, "Audio clip is not playing for spongebob");
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayVoiceLine_DoesNotPlayIfNoClipLoaded()
    {
        LogAssert.ignoreFailingMessages = true;
        audioHandler.AudioSources["spongebob"].clip = null;
        audioHandler.PlayVoiceLine("spongebob");
        Assert.IsFalse(audioHandler.AudioSources["spongebob"].isPlaying, "Audio clip is playing for spongebob without clip loaded");
        yield return null;
        LogAssert.Expect(LogType.Error, "No voice line loaded for spongebob");
    }
}

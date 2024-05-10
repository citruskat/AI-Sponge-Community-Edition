using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class CharacterManagerTests
{
    private CharacterManager characterManager;
    private readonly int EXPECTED_CHARACTER_COUNT = 9;

    [SetUp]
    public void Setup()
    {
        // Load the scene
        SceneManager.LoadScene("BikiniBottom");
        // Wait until the scene has loaded and then find the CharacterManager
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "BikiniBottom")
            {
                // Find the CharacterManager
                characterManager = GameObject.Find("Script").GetComponent<CharacterManager>();
            }
        };
    }

    // Check that the dictionary is not null
    [UnityTest]
    public IEnumerator SpawnCharacters_DictionaryIsNotNull()
    {
        Assert.IsNotNull(characterManager.Characters, "Characters dictionary is null");
        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnCharacters_HasExpectedCount()
    {
        Assert.AreEqual(EXPECTED_CHARACTER_COUNT, characterManager.Characters.Count, "Characters dictionary does not have the expected count. Is EXPECTED_CHARACTER_COUNT set correctly?");
        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnCharacters_CharactersHaveBeenSpawned()
    {
        // Check that the characters have been spawned
        foreach (KeyValuePair<string, Character> character in characterManager.Characters)
        {
            Assert.IsNotNull(character.Value, $"Character {character.Key} has not been spawned");
        }
        yield return null;
    }
}

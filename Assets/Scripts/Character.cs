using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Character object should have releavant information about each character.
/// These might include, a GameObject ref, tts ID, audio source
/// </summary>
public class Character
{
    private readonly GameObject characterObject;
    private readonly AudioSource audioSource;

    // Enable other classes to read the Transform of the characterObject
    public Transform Transform => characterObject.transform;

    public AudioSource AudioSource => audioSource;

    public Character(GameObject characterPrefab, Vector3 position, Quaternion rotation)
    {
        // Assign the characterObject
        characterObject = GameObject.Instantiate(characterPrefab, position, rotation);

        // Ensure there is an audio source child. This should be the first child, but just in case, loop until we find one.
        foreach (Transform child in characterObject.transform)
        {
            // Temporary AudioSource to hold potential audio sources
            if (child.TryGetComponent<AudioSource>(out AudioSource childAudioSource))
            {
                audioSource = childAudioSource;
                break;
            }
        }

        if (audioSource == null)
        {
            throw new ArgumentException("No AudioSource component found within passed prefab.", characterPrefab.name);
        }
    }
}
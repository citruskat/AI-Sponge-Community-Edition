using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AudioHandler : MonoBehaviour
    {
        private Dictionary<string, AudioSource> audioSources;
        private CharacterManager characterManager;

        public Dictionary<string, AudioSource> AudioSources => audioSources;

        // Populate the audioSources dictionary with the audio sources from each character
        private void RetrieveAudioSources()
        {
            audioSources = new Dictionary<string, AudioSource>();
            foreach (KeyValuePair<string, Character> character in characterManager.Characters)
            {
                audioSources.Add(character.Key, character.Value.AudioSource);
            }
        }

        public void LoadVoiceLine(string character, string fileName)
        {
            audioSources[character].clip = Resources.Load<AudioClip>($"{fileName}");
        }

        public void LoadVoiceLine(string character, AudioClip clip)
        {
            audioSources[character].clip = clip;
        }

        public void PlayVoiceLine(string character)
        {
            // Ensure character has a voice line loaded
            if (audioSources[character].clip == null)
            {
                Debug.LogError($"No voice line loaded for {character}");
                return;
            }

            audioSources[character].Play();
        }

        public void Awake()
        {
            // Get necessary components and assign them
            characterManager = GetComponent<CharacterManager>();
        }

        public void Start()
        {
            // Populate the audioSources dictionary
            RetrieveAudioSources();
        }
    }
}
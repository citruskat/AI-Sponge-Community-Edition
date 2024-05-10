using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] krustyKrab, conchStreet, outsideKrustyKrab, townCenter; // Create multiple spawn locations for different areas. Index 0 MUST be a vCam spawn location. TODO: Add code to ensure this is the case
        [SerializeField]
        private GameObject[] characterPrefabs; // Array defining character prefabs
        [SerializeField]
        private Dictionary<string, Character> characters; // Dictionary to map Characters to strings
        private CameraManager cameraManager; // Reference to the CameraManager

        public Dictionary<string, Character> Characters => characters;

        // Choose a spawn location
        private Transform[] GetSpawnGroup()
        {
            return new System.Random().Next(0, 4) switch
            {
                0 => krustyKrab, // krustyKrab will not be implemented until later. Fall back to outsideKrustyKrab
                1 => conchStreet,
                2 => outsideKrustyKrab,
                3 => townCenter,
                _ => null, // If we return null, the universe has ended
            };
        }

        // Spawn characters at a random spawn point and store references to them in a dictionary
        private void SpawnCharacters()
        {
            // Create our Dictionary object
            characters = new Dictionary<string, Character>();

            Transform[] spawnGroup = GetSpawnGroup();

            // GameObject vCam = GameObject.Find("Virtual Camera"); Comment out unused vCam code in this branch

            // spawnGroup = null; // Debug declaration for manuallly setting location

            if (spawnGroup == krustyKrab)
            {
                Debug.LogWarning("Krusty Krab spawn group not yet implemented. Falling back to Outside Krusty Krab...");
                spawnGroup = outsideKrustyKrab;
            }

            cameraManager.MoveCamera(spawnGroup[0]);

            for (int i = 0; i < characterPrefabs.Length; i++)
            {
                characters[characterPrefabs[i].name] = new(characterPrefabs[i], spawnGroup[i + 1].position, spawnGroup[i + 1].rotation);
            }
        }

        public void Awake()
        {
            cameraManager = GetComponent<CameraManager>();
        }

        public void Start()
        {
            SpawnCharacters();
        }
    }
}
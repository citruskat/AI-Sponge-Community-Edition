#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
	/// <sumarry>
	/// Arbitrary code to take the place of unimplemented methods
	/// </sumarry>
	public class DevelopmentNonsense : MonoBehaviour
	{
		// Declare necessary managers
		private CharacterManager characterManager;
		private AudioHandler audioHandler;
		private CameraManager cameraManager;

		// We need to get our voice line manifest. For now, I will just hardcode one
		public string[] audioManifest;
		private List<VoiceLine> voiceLines;
		private string[] characters;

		public IEnumerator MakeEmYap()
		{
			for (int i = 0; i < audioManifest.Length; i++)
			{
				voiceLines.Add(new VoiceLine(i, audioManifest[i], characters[i]));
				// Debug.Log($"{voiceLines[i].voiceLineID}, {voiceLines[i].voiceLinePath}, {voiceLines[i].character}");
				yield return StartCoroutine(Speak(voiceLines[i]));
			}
		}

		public IEnumerator Speak(VoiceLine line)
		{
			// Debug.Log($"Playing voice line {line.voiceLineID}: {line.voiceLinePath} on {line.character}");
			audioHandler.LoadVoiceLine(line.character, line.voiceLinePath);
			AudioSource source = characterManager.Characters[line.character].AudioSource;
			cameraManager.MoveCamera(characterManager.Characters[line.character]);
			source.Play();
			while (source.isPlaying)
			{
				yield return null;
			}
			yield return new WaitForSeconds(0.5f);
		}

		/* Plan:
		 * We need to use FakeYouService.cs to request voice lines from the FakeYou API, download them, and play them.
		 * We need to implement the following methods:
		 * - RequestVoiceLine()
		 * - Speak()
		 * - CheckCookie()
		   For now, instead of generating voice lines with the API, we will use pre-generated lines.
		   These lines are defined in Assets/Resources/example_voice_lines.json
		 */

		public void Awake()
		{
			characterManager = GetComponent<CharacterManager>();
			audioHandler = GetComponent<AudioHandler>();
			cameraManager = GetComponent<CameraManager>();
		}

		public void Start()
		{
			voiceLines = new List<VoiceLine>();
			audioManifest = FileHandler.ReadFile("Assets/Resources/AUDIO_MANIFEST.csv");
			characters = FileHandler.ReadFile("Assets/Resources/CHARACTER_LIST.csv");
			StartCoroutine(MakeEmYap());
		}
	}

	public class FileHandler
	{
		public static string[] ReadFile(string path)
		{
			string[] values = new string[0];
            using var reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                values = line.Split(',');
            }
			return values;
        }
	}

	public class VoiceLine
	{
		public int voiceLineID; // The order in which the voice line will be played
		public string voiceLinePath; // Where the voice line is located
		public string character; // Who is reading the line

		public VoiceLine(int voiceLineID, string voiceLinePath, string character)
		{
			this.voiceLineID = voiceLineID;
			this.voiceLinePath = voiceLinePath;
			this.character = character;
		}
	}
}
#endif
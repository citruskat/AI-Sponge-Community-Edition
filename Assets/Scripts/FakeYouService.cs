#if UNITY_EDITOR
using System.Net.Http;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Assets.Scripts
{
	/// <summary>
	/// This class will connect with the FakeYou API to request and retrieve TTS audio files.
	/// </summary>
	// Note: plankton voice id is TM:ym446j7wkewg
	public class FakeYouService : MonoBehaviour
	{
		private readonly HttpClientHandler handler = new();
		private HttpClient client;
		private string fakeYouAPIKey;
		private List<AudioRequest> audioRequests;
		private List<AudioClip> audioClips;
		
		public List<AudioClip> AudioClips => audioClips;

		private AudioHandler audioHandler;
		private CameraManager cameraManager;
		private CharacterManager characterManager;

		[SerializeField]
		private int function;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void RequestVoiceLine()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
	/* 		var jsonContent = new
			{
				inference_text = 
				tts_model_token = 
				uuid_idempotency_token = 
			};

			var content = new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");
			var response = await client.PostAsync("https://api.fakeyou.com/tts/inference", content);
			var responseString = await response.Content.ReadAsStringAsync();
			Debug.Log(responseString); */

			// For now, we will just get our voice lines from Assets/Resources/example_voice_lines.json but still use our HttpClient to make the request
			audioRequests = JsonConvert.DeserializeObject<List<AudioRequest>>(File.ReadAllText("Assets/Resources/example_voice_lines.json"));
		}

		private IEnumerator DownloadAudio()
		{
			UnityWebRequest www;
			foreach (var audioRequest in audioRequests)
			{
				www = UnityWebRequestMultimedia.GetAudioClip($"https://storage.googleapis.com/vocodes-public{audioRequest.state.maybe_public_bucket_wav_audio_path}", AudioType.WAV);
				yield return www.SendWebRequest();
				var audioClip = DownloadHandlerAudioClip.GetContent(www);
				audioClips.Add(audioClip);
			}
		}

		private async void CheckCookie()
		{
			var checkKey = await client.GetAsync("https://api.fakeyou.com/v1/billing/active_subscriptions");
			var checkString = await checkKey.Content.ReadAsStringAsync();
			Debug.Log(checkString);
		}

		/* Plan:
		* We need to use FakeYouService.cs to request voice lines from the FakeYou API, download them, and play them.
		* For now, instead of generating voice lines with the API, we will use pre-generated lines.
		* These lines are defined in Assets/Resources/example_voice_lines.json
		* We will pretend that we have already requested the voice lines from the API and have them stored in memory (for now, we will simply load them into that variable from the file.)
		* We need to parse the json response from the API and store essential information (such as the voice line ID, the character, and the path to the audio file, etc) in a list of objects (we will make a class for this.)
		* We will then use this list to play the voice lines similarly to how we did in DevelopmentNonsense.cs
		*/

		public void Awake()
		{
			audioClips = new List<AudioClip>();

			audioHandler = GetComponent<AudioHandler>();
			cameraManager = GetComponent<CameraManager>();
			characterManager = GetComponent<CharacterManager>();

			try
			{
				fakeYouAPIKey = File.ReadAllText("Assets/Resources/FAKEYOU_API_KEY");
				// Debug.Log(fakeYouAPIKey);
				var uri = new System.Uri("https://api.fakeyou.com");
				handler.CookieContainer.Add(uri, new System.Net.Cookie("session", fakeYouAPIKey));
				client = new HttpClient(handler);
				client.DefaultRequestHeaders.Add("Accept", "application/json");
			}
			catch (FileNotFoundException e)
			{
				Debug.LogWarning($"{e.Message} This is expected in a docker. If you are running locally, be sure to add your key.");
			}
		}

		public IEnumerator Start()
		{
			RequestVoiceLine();
			yield return StartCoroutine(DownloadAudio());
		}
	}
}
#endif
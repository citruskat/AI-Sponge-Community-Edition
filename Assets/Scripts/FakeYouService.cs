using System.Net.Http;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading.Tasks;

// API docs: https://docs.fakeyou.com/

namespace Assets.Scripts
{
	/// <summary>
	/// This class will connect with the FakeYou API to request and retrieve TTS audio files.
	/// </summary>
	public class FakeYouService : MonoBehaviour
	{
		private readonly HttpClientHandler handler = new();
		private HttpClient client;
		private string fakeYouAPIKey;
		private List<AudioRequest> audioRequests;
		private List<string> jobTokens;
		private List<AudioClip> audioClips;

		public List<AudioClip> AudioClips => audioClips;

		private AudioHandler audioHandler;
		private CameraManager cameraManager;
		private CharacterManager characterManager;

		[SerializeField]
		private int function;

        private IEnumerator RequestVoiceLine()
        {
	 		var jsonContent = new
			{
				inference_text =  "I have consumed 17 million metal pipes esq.",
				tts_model_token = "TM:ym446j7wkewg",
				uuid_idempotency_token = Guid.NewGuid().ToString()
			};

			var content = new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");
			var request = new UnityWebRequest("https://api.fakeyou.com/tts/inference", "POST")
			{
				uploadHandler = new UploadHandlerRaw(content.ReadAsByteArrayAsync().Result),
				downloadHandler = new DownloadHandlerBuffer()
			};
			request.SetRequestHeader("Content-Type", "application/json");

			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogWarning(request.error);
				yield break;
			}
			else 
			{
				var responseString = request.downloadHandler.text;
				// I am just taking the inference_job_token and storing it in a list. I am ignoring the status content as I don't think it will be important, but in case there are any issues, I am noting that here.
				if (JsonConvert.DeserializeObject<AudioRequest>(responseString).Success != "true")
				{
					Debug.LogWarning("TTS request was not successful");
				}
				jobTokens.Add(JsonConvert.DeserializeObject<AudioRequest>(responseString).Inference_job_token);
				Debug.Log(jobTokens[0]);
			}
		}

		private IEnumerator PollJob()
		{
			UnityWebRequest www;
			foreach (var job in jobTokens)
			{
				www = UnityWebRequest.Get($"https://api.fakeyou.com/tts/job/{job}");
				yield return www.SendWebRequest();
				var jobStatus = www.downloadHandler.text;
				Debug.Log(jobStatus);
			}
		}

		private IEnumerator DownloadAudio()
		{
			UnityWebRequest www;
			foreach (var audioRequest in audioRequests)
			{
				www = UnityWebRequestMultimedia.GetAudioClip($"https://storage.googleapis.com/vocodes-public{audioRequest.State.Maybe_public_bucket_wav_audio_path}", AudioType.WAV);
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

		private void InitializeHttpClient()
		{
			client = new HttpClient(handler);
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			fakeYouAPIKey = File.ReadAllText("Assets/Resources/FAKEYOU_API_KEY");
			var uri = new Uri("https://api.fakeyou.com");
			handler.CookieContainer.Add(uri, new System.Net.Cookie("session", fakeYouAPIKey));
			client = new HttpClient(handler);
			client.DefaultRequestHeaders.Add("Accept", "application/json");
		}

		public void Awake()
		{
			audioClips = new List<AudioClip>();
			audioRequests = new List<AudioRequest>();
			jobTokens = new List<string>();

			audioHandler = GetComponent<AudioHandler>();
			cameraManager = GetComponent<CameraManager>();
			characterManager = GetComponent<CharacterManager>();

			try
			{
				InitializeHttpClient();
			}
			catch (FileNotFoundException e)
			{
				Debug.LogWarning($"{e.Message} This is expected in a docker. If you are running locally, be sure to add your key.");
			}
		}

		public IEnumerator Start()
		{
			yield return StartCoroutine(RequestVoiceLine());
			yield return StartCoroutine(PollJob());
			// yield return StartCoroutine(DownloadAudio());
		}
	}
}

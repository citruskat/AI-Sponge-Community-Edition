using System.Net.Http;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

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
		private List<AudioClip> audioClips;

		public List<AudioClip> AudioClips => audioClips;

		private readonly int RETRY_DELAY = 7;

        public IEnumerator RequestVoiceLine(string inference_text, string tts_model_token)
        {
	 		var jsonContent = new
			{
                inference_text,
				tts_model_token,
				uuid_idempotency_token = Guid.NewGuid().ToString()
			};

			var content = new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");
			var request = new UnityWebRequest("https://api.fakeyou.com/tts/inference", "POST")
			{
				uploadHandler = new UploadHandlerRaw(content.ReadAsByteArrayAsync().Result),
				downloadHandler = new DownloadHandlerBuffer()
			};
			request.SetRequestHeader("Content-Type", "application/json");
			request.SetRequestHeader("session", fakeYouAPIKey);

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
				audioRequests.Add(JsonConvert.DeserializeObject<AudioRequest>(responseString));
				Debug.Log(audioRequests[0]);
			}
		}

		// Poll each job in audioRequests to check if the audio is ready to be downloaded
		// If State.Status is "complete_success", download the audio file
		// Otherwise, poll again after a delay
		public IEnumerator PollJob()
		{
			UnityWebRequest www;
			for (int i = 0; i < audioRequests.Count; i++)
			{
				// If we just created the job request, we will use Inference_job_token. After the first poll, the API will return a State.Job_token which we will use for subsequent polls.
				string job_token = audioRequests[i].Inference_job_token ?? audioRequests[i].State.Job_token;
				www = UnityWebRequest.Get($"https://api.fakeyou.com/tts/job/{job_token}");
				yield return www.SendWebRequest();
				var jobStatus = www.downloadHandler.text;

				Debug.Log(jobStatus);
				audioRequests[i] = JsonConvert.DeserializeObject<AudioRequest>(jobStatus);

				if (audioRequests[i].State.Status == "complete_success")
				{
					// Download the audio file
					www = UnityWebRequestMultimedia.GetAudioClip($"https://storage.googleapis.com/vocodes-public{audioRequests[i].State.Maybe_public_bucket_wav_audio_path}", AudioType.WAV);
					yield return www.SendWebRequest();
					var audioClip = DownloadHandlerAudioClip.GetContent(www);
					audioClips.Add(audioClip);
				}
				else
				{
					yield return new WaitForSeconds(RETRY_DELAY);
					yield return PollJob();
				}
			}
		}

		public async void CheckCookie()
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
			CheckCookie();
			yield return null;
/* 			yield return StartCoroutine(RequestVoiceLine("I am a bee esq.", "TM:ym446j7wkewg"));
			yield return StartCoroutine(RequestVoiceLine("My name is Dr. Jr.", "TM:ym446j7wkewg"));
			yield return StartCoroutine(PollJob());
			audioHandler.LoadVoiceLine("plankton", audioClips[0]);
			yield return StartCoroutine(Speak(audioClips[0], "plankton"));
			audioHandler.LoadVoiceLine("plankton", audioClips[1]);
			yield return StartCoroutine(Speak(audioClips[1], "plankton")); */
		}
	}
}

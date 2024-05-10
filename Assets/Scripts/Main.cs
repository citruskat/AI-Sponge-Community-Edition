using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <sumarry>
    /// A basic class for starting the codebase.
    /// </sumarry>
    public class Main : MonoBehaviour
    {
        private CharacterManager characterManager;
        private AudioHandler audioHandler;
        private CameraManager cameraManager;

        /* Show a loading screen
        Eventually, I would like to implement a main menu scene, but for now I want to indicate to the user when we are loading
        We should also include feedback, such as percentages, ETA, and some fun messages */
        private static void ShowLoadingScreen() { throw new NotImplementedException(); }

        /* Configure an HttpClient to work with the FakeYou and OpenAI APIs
        We should have one that uses a proxy and another without (unless we can dynamically change that */
        private static void ConfigureHttpClient() { throw new NotImplementedException(); }

        /* We need to verify that the stored FakeYou cookie is valid. We can do this by checking what our FakeYou plan is
        If we don't have a FakeYou cookie stored, we need to handle it somehow */
        private static void ConfirmFakeYouCookie() { throw new NotImplementedException(); }

        /* Use file I/O to check for a stored FakeYou cookie.
        If there is not one, we need to fetch one from the /login endpoint */
        private static void ReadStoredFakeYouCookie() { throw new NotImplementedException(); }

        /* If we do not have a FakeYou cookie, we need to get an email and password from the user
        Optimally, this should be done through non-persistent console input as to protect credentials */
        private static void PromptFakeYouCredentialInput() { throw new NotImplementedException(); }

        /* Use the user provided credentials to get a cookie from the /login endpoint
        Once we get our cookie, it should be stored to a file */
        private static void FetchFakeYouCookie() { throw new NotImplementedException(); }

        /* Serialize a JSON object to send to FakeYou. Each object should contain the voice line we need generated */
        private static void CreateTTSRequest() { throw new NotImplementedException(); }

        /* Read the file with queued topics. The first topic should be taken, removed from the file, and passed as a parameter */
        private static void ReadTopicsFromFile() { throw new NotImplementedException(); }

        /* Generate a topic using OpenAI then write the output to a file
        We also need to read a token for OpenAI. I will have to find a way to securely store this */
        private static void GenerateScript() { throw new NotImplementedException(); }

        /* Hide the loading screen */
        private static void HideLoadingScreen() { throw new NotImplementedException(); }

        /* Check voice line status with FakeYou. Once voice lines are downloaded, make characters start talking
        Also get rid of the loading screen */

        /* After the last voice line has been played, start from the beginning */
        private static void StartOver() { throw new NotImplementedException(); }

        public void Awake()
        {
            // Get necessary components and assign them
            characterManager = GetComponent<CharacterManager>();
            audioHandler = GetComponent<AudioHandler>();
            cameraManager = GetComponent<CameraManager>();
        }
    }
}
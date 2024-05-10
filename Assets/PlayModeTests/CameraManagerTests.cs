using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;

public class CameraManagerTests
{
    private CameraManager cameraManager;
    private CharacterManager characterManager;
    private CinemachineVirtualCamera virtualCamera;

    [SetUp]
    public void Setup()
    {
        // Load the scene
        SceneManager.LoadScene("BikiniBottom");
        // Wait until the scene has loaded and then find the CameraManager and CharacterManager
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "BikiniBottom")
            {
                // Find the CameraManager, CharacterManager, and CinemachineVirtualCamera
                cameraManager = GameObject.Find("Script").GetComponent<CameraManager>();
                characterManager = GameObject.Find("Script").GetComponent<CharacterManager>();
                virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            }
        };
    }

    // Check that the virtual camera is not null after Awake
    [UnityTest]
    public IEnumerator Awake_VirtualCameraIsNotNull()
    {
        Assert.IsNotNull(virtualCamera, "VirtualCamera is null");
        yield return null;
    }

    // Check that the camera follows and looks at the correct character after MoveCamera
    [UnityTest]
    public IEnumerator MoveCamera_FollowsAndLooksAtCorrectCharacter()
    {
        Character character = characterManager.Characters["spongebob"];
        cameraManager.MoveCamera(character);
        Assert.AreEqual(character.Transform, virtualCamera.Follow, "Camera does not follow the correct character");
        Assert.AreEqual(character.Transform, virtualCamera.LookAt, "Camera does not look at the correct character");
        yield return null;
    }

    // Check that the camera follows and looks at the correct transform after MoveCamera
    [UnityTest]
    public IEnumerator MoveCamera_FollowsAndLooksAtCorrectTransform()
    {
        Transform transform = characterManager.Characters["spongebob"].Transform;
        cameraManager.MoveCamera(transform);
        Assert.AreEqual(transform, virtualCamera.Follow, "Camera does not follow the correct transform");
        Assert.AreEqual(transform, virtualCamera.LookAt, "Camera does not look at the correct transform");
        yield return null;
    }
}

# AI Sponge Community Edition
Release Branch:
[![Build](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/build.yml) [![Test](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/test.yml/badge.svg)](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/test.yml)

Development Branch:
[![Build](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/build.yml/badge.svg?branch=development)](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/build.yml) [![Test](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/test.yml/badge.svg?branch=development)](https://github.com/citruskat/AI-Sponge-Community-Edition/actions/workflows/test.yml)

## Project Overview
This project aims to allow users to generate their own AI Sponge episodes. This is a Unity based application which utilizes FakeYou and OpenAI.
## Main Features
1.	Character Management: The CharacterManager.cs script is responsible for spawning characters at random spawn points and storing references to them in a dictionary.
2.	Audio Handling: The AudioHandlerTests.cs script is used to manage the audio in the scene. It includes a setup method that loads the scene and finds the AudioHandler and CharacterManager.
3.	Camera Management: The CameraManagerTests.cs script is used to manage the camera in the scene. It includes a setup method that loads the scene and finds the CameraManager, CharacterManager, and CinemachineVirtualCamera.
4.	Script Generation: The Main.cs script includes a method for generating a script using OpenAI and writing the output to a file.
5.	Audio Request: The AudioRequest.cs script includes a property for storing a possible extra status description.
6.	Script Debugging: The ScriptDebugger.cs script includes methods for playing and loading voice lines for debugging purposes.
7.	Voice Line Management: The DevelopmentNonsense.cs script includes a method for creating voice lines and making characters speak.
## Setup
To set up the project, load the "BikiniBottom" scene in Unity. The scripts will automatically find the necessary components (such as the CharacterManager, AudioHandler, and CameraManager) and perform their respective tasks.
## Testing
Unit tests are included in the PlayModeTests directory. These tests cover the CameraManager, CharacterManager, and AudioHandler.
## Future Work
The Main.cs script includes a placeholder for a method to generate a script using OpenAI. This method is not yet implemented and will be part of future work on this project.

/* SAVES AND LOADS GAMES THROUGH PLAYER ID's

**FEATURES**

SAVING:
	1. Sets temporary variables to what is in the gamecontroller, highscore controller, and playercontroller scripts. 
	2. Creates a dat file and names it accordingly to player ID, slot number as a .dat file.
	3. Assigns values of temporary data to PlayerData class.
	4. Writes these values to created file at Application.persistentDataPath.
	5. Serializes and closes the file. 
	6. Sends message that the save was completed.

LOADING:
	1. Checks if file already exists at Application.persistentDataPath.
	2. Opens the .dat file and deserializes it, then closes it again.
	3. Outputs data from save game file to temporary variables.
	4. Assigns temporary variables to the relevant scripts when called.
	5. Sends message when the load was complete.
	
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoad : MonoBehaviour
{
	public bool		loadOnStart;
	public enum 	saveMode {menu, main}
	public saveMode SaveMode;
	public enum 	loadMode {menu, main}
	public loadMode LoadMode;
	public string   Username = "Player";

	[Header ("Window Size")]
	public int Width = 1280;
	public int Height = 720;
	public bool fullscreen = true;

	public qualityPreset Quality;
	public enum qualityPreset
	{
		Low,
		Medium,
		High,
		Ultra
	}

	public PostProcessingProfile postProcessingProfileA;
	public PostProcessingProfile postProcessingProfileB;

	public static SaveAndLoad SaveLoadManager;
	public GameController gameControllerScript;
	public PlayerData 		  playersData;

	void Awake () 
	{
		Username = PlayerPrefs.GetString ("username");

		// Checks if we have a SaveAndLoad class already. Destroys self if it found one.
		if (SaveLoadManager == null)
		{
			SaveLoadManager = this;
		} 
		else if (SaveLoadManager != this) 
		{
		}

		// Checks if username is null or empty string, adds default username.
		if (Username == null || Username == "") 
		{
			PlayerPrefs.SetString ("username", "Player");
			Username = PlayerPrefs.GetString ("username");
			Debug.LogWarning ("Empty username, created default username: \"" + Username + "\".");
			PlayerPrefs.SetString ("username", Username);
		}

		if (LoadMode == loadMode.menu) 
		{
			
		}

		if (LoadMode == loadMode.main) 
		{
			//Load ();
		}
	}

	void Start ()
	{
		FindComponents ();

		if (loadOnStart == true) 
		{
			Load ();
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.F6)) 
		{
			Load ();
		}

		if (Input.GetKeyDown (KeyCode.F9)) 
		{
			Save ();
		}

		if (Input.GetKeyDown (KeyCode.T)) 
		{
			LoadImageEffects ();
			LoadTerrainData ();
		}
	}

	private void FindComponents ()
	{
	}
		
	private void SetStartArraySizes ()
	{
	}
		
	public void SetUsername (string UserName)
	{
		PlayerPrefs.SetString ("username", UserName);
	}
		
	public void Save ()
	{
		if (SaveMode == saveMode.main) 
		{
			// Gets variables from other scripts.
			GrabTemporaryVariables ();
	
			// Creates save data file.
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/" + Username + "_savegame.dat");

			// Does the saving.
			PlayerData data = new PlayerData ();

			SaveStats (data);

			// Serializes and closes the file.
			bf.Serialize (file, data);
			file.Close ();

			Debug.Log ("Successfully saved to " +
			Application.persistentDataPath + "/" +
			PlayerPrefs.GetString ("username") + "_savegame.dat"); 
		}

		if (SaveMode == saveMode.menu) 
		{
			// Creates save data file.
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/" + Username + "_savegame.dat");

			// Does the saving.
			PlayerData data = new PlayerData ();

			SaveMenuContents ();

			// Serializes and closes the file.
			bf.Serialize (file, data);
			file.Close ();

			Debug.Log ("Successfully saved to " +
			Application.persistentDataPath + "/" +
			PlayerPrefs.GetString ("username") + "_savegame.dat"); 
		}
	}

	public void GrabTemporaryVariables ()
	{
		SaveResolution ();
		SaveImageEffects ();
		SaveTerrainData ();
		SaveLevelTime ();
	}

	void SaveResolution ()
	{
		Width = Screen.width;
		Height = Screen.height;
		fullscreen = Screen.fullScreen;
	}

	void SaveImageEffects ()
	{
		switch (Quality) 
		{
		case qualityPreset.Low:
			postProcessingProfileA.antialiasing.enabled = 			false;
			postProcessingProfileA.ambientOcclusion.enabled = 		false;
			postProcessingProfileA.screenSpaceReflection.enabled = 	false;
			postProcessingProfileA.depthOfField.enabled = 			false;
			postProcessingProfileA.motionBlur.enabled = 			false;
			postProcessingProfileA.eyeAdaptation.enabled = 			false;
			postProcessingProfileA.bloom.enabled = 					false;
			postProcessingProfileA.colorGrading.enabled = 			false;
			postProcessingProfileA.userLut.enabled = 				false;
			postProcessingProfileA.chromaticAberration.enabled = 	false;
			postProcessingProfileA.grain.enabled = 					false;
			postProcessingProfileA.vignette.enabled = 				false;
			postProcessingProfileA.dithering.enabled = 				false;
			break;
		case qualityPreset.Medium:
			postProcessingProfileA.antialiasing.enabled = 			false;
			postProcessingProfileA.ambientOcclusion.enabled = 		false;
			postProcessingProfileA.screenSpaceReflection.enabled = 	false;
			postProcessingProfileA.depthOfField.enabled = 			false;
			postProcessingProfileA.motionBlur.enabled = 			false;
			postProcessingProfileA.eyeAdaptation.enabled = 			false;
			postProcessingProfileA.bloom.enabled = 					true;
			postProcessingProfileA.colorGrading.enabled = 			false;
			postProcessingProfileA.userLut.enabled = 				false;
			postProcessingProfileA.chromaticAberration.enabled = 	false;
			postProcessingProfileA.grain.enabled = 					true;
			postProcessingProfileA.vignette.enabled = 				true;
			postProcessingProfileA.dithering.enabled = 				false;
			break;
		case qualityPreset.High:
			postProcessingProfileA.antialiasing.enabled = 			false;
			postProcessingProfileA.ambientOcclusion.enabled = 		false;
			postProcessingProfileA.screenSpaceReflection.enabled = 	false;
			postProcessingProfileA.depthOfField.enabled = 			true;
			postProcessingProfileA.motionBlur.enabled = 			false;
			postProcessingProfileA.eyeAdaptation.enabled = 			false;
			postProcessingProfileA.bloom.enabled = 					true;
			postProcessingProfileA.colorGrading.enabled = 			false;
			postProcessingProfileA.userLut.enabled = 				false;
			postProcessingProfileA.chromaticAberration.enabled = 	true;
			postProcessingProfileA.grain.enabled = 					true;
			postProcessingProfileA.vignette.enabled = 				true;
			postProcessingProfileA.dithering.enabled = 				false;
			break;
		case qualityPreset.Ultra:
			postProcessingProfileA.antialiasing.enabled = 			true;
			postProcessingProfileA.ambientOcclusion.enabled = 		true;
			postProcessingProfileA.screenSpaceReflection.enabled = 	false;
			postProcessingProfileA.depthOfField.enabled = 			true;
			postProcessingProfileA.motionBlur.enabled = 			false;
			postProcessingProfileA.eyeAdaptation.enabled = 			false;
			postProcessingProfileA.bloom.enabled = 					true;
			postProcessingProfileA.colorGrading.enabled = 			false;
			postProcessingProfileA.userLut.enabled = 				false;
			postProcessingProfileA.chromaticAberration.enabled = 	true;
			postProcessingProfileA.grain.enabled = 					true;
			postProcessingProfileA.vignette.enabled = 				true;
			postProcessingProfileA.dithering.enabled = 				false;
			break;
		}
	}

	void SaveTerrainData ()
	{
	}

	void SaveLevelTime ()
	{
	}

	void SaveStats (PlayerData data)
	{
		data.Width = Width;
		data.Height = Height;
		data.fullscreen = fullscreen;
	}

	public void SaveMenuContents ()
	{
	}

	public void Load ()
	{
		if (LoadMode == loadMode.main) 
		{
			if (File.Exists (Application.persistentDataPath + "/" + Username + "_savegame.dat") == true) 
			{
				// Opens the save data.
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/" + Username + "_savegame.dat", FileMode.Open);

				// Processes the save data into memory.
				PlayerData data = (PlayerData)bf.Deserialize (file);
				file.Close ();

				LoadGameContents (data);
				StoreGameContentsInGame ();

				Debug.Log ("Successfully loaded from " +
				Application.persistentDataPath + "/" + 
				PlayerPrefs.GetString ("username") + "_savegame.dat");
			}

			if (File.Exists (Application.persistentDataPath + "/" + Username + "_savegame.dat") == false) 
			{
				Debug.Log ("Unable to load from " +
				Application.persistentDataPath + "/" +
				PlayerPrefs.GetString ("username") + "_savegame.dat");
			}
		}

		if (LoadMode == loadMode.menu) 
		{
			if (File.Exists (Application.persistentDataPath + "/" + Username + "_savegame.dat") == true) 
			{
				// Opens the save data.
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/" + Username + "_savegame.dat", FileMode.Open);

				// Processes the save data into memory.
				PlayerData data = (PlayerData)bf.Deserialize (file);
				file.Close ();

				LoadGameContents (data);
				Invoke ("StoreGameContentsInMenu", 1);

				Debug.Log ("Successfully loaded from " +
				Application.persistentDataPath +
				PlayerPrefs.GetString ("username") + "_savegame.dat");
			}

			if (File.Exists (Application.persistentDataPath + "/" + Username + "_savegame.dat") == false) 
			{
				Debug.Log ("Unable to load from " +
				Application.persistentDataPath + "/" +
				PlayerPrefs.GetString ("username") + "_savegame.dat");
			}
		}
	}

	public void LoadGameContents (PlayerData data)
	{
		// Inputs high score data from PlayersData class.
		Width = data.Width;
		Height = data.Height;
		fullscreen = data.fullscreen;
	}

	public void LoadImageEffects ()
	{
	}

	void LoadTerrainData ()
	{
	}

	public void LoadMenuContents (PlayerData data)
	{
		
	}

	public void StoreGameContentsInGame ()
	{
		ApplyResolution ();

		if (LoadMode == loadMode.main)
		{
			LoadImageEffects ();
			LoadTerrainData ();
		}
	}

	public void StoreGameContentsInMenu ()
	{
		ApplyResolution ();

		if (LoadMode == loadMode.main)
		{
			LoadImageEffects ();
			LoadTerrainData ();
		}
	}

	// Resets if it can save or load.
	public void ResetLoadState ()
	{
		
	}

	public void ResetSavedState ()
	{

	}

	// Resets all stats.
	public void ResetAll ()
	{
		
	}

	public void SetResolutionWidth (int width)
	{
		Width = width;
	}

	public void SetResolutionHeight (int height)
	{
		Height = height;
	}

	public void SetFullscreen (bool Fullscreen)
	{
		fullscreen = Fullscreen;
	}

	public void ApplyResolution ()
	{
		Screen.SetResolution (Width, Height, fullscreen);
	}

	// This is where the saved data gets stored.
	[Serializable]
	public class PlayerData
	{
		[Header ("Window Size")]
		public int Width = 1280;
		public int Height = 720;
		public bool fullscreen = true;

		public qualityPreset Quality;
		public enum qualityPreset
		{
			Low,
			Medium,
			High,
			Ultra
		}
	}
}

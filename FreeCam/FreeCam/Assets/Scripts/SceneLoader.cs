using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public float delay;
	public LoadType sceneLoadType;
	public enum LoadType
	{
		Simple,
		Async
	}

	public string SceneName;
		
	public void ReloadScene ()
	{
		if (sceneLoadType == LoadType.Simple)
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	public void LoadScene (string sceneName)
	{
		if (sceneLoadType == LoadType.Simple) 
		{
			SceneManager.LoadScene (sceneName);
		}

		if (sceneLoadType == LoadType.Async) 
		{
			SceneName = sceneName;
			LoadAsyncSceneDelay ();
		}
	}

	public void LoadAsyncSceneDelay ()
	{
		Invoke ("LoadAsyncSceneNow", delay);
	}

	public void LoadAsyncSceneNow ()
	{
		SceneManager.LoadSceneAsync (SceneName, LoadSceneMode.Single);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomSceneManager
{
    private Dictionary<int, string> _scenes;
    private int _currentSceneIndex;

    // Use this for initialization
    void Start ()
    {
        
	}

    public CustomSceneManager()
    {
        InitializeScenes();

        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        foreach (int key in _scenes.Keys.Where(key => _scenes[key] == currentSceneName))
        {
            _currentSceneIndex = key;
            break;
        }
    }
	
	// Update is called once per frame
	//void Update ()
 //   {
	
	//}

    public void LoadNextScene()
    {
        _currentSceneIndex++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(_scenes[_currentSceneIndex]);
    }

    private void InitializeScenes()
    {
        _scenes = new Dictionary<int, string>();

        _scenes[0] = "1-1";
        _scenes[1] = "1-2";
    }
}

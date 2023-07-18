using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public void LoadSceneByIndex(int scene)
    {
        if(scene == 1)
        {
            sceneLoader.GetComponent<SceneLoader>().LoadGame();
        }
        else if(scene == 3 || scene == 4 || scene == 5)
        {
            sceneLoader.GetComponent<SceneLoader>().LoadMetalScene(scene);
        }
        else if(scene == 6 || scene == 7)
        {
            sceneLoader.GetComponent<SceneLoader>().LoadWhiteCloudScene(scene);
        }
        else if(scene == 0)
        {
            sceneLoader.GetComponent<SceneLoader>().LoadMenu();
        }
    }
}

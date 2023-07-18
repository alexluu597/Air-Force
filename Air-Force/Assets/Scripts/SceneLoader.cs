using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public Animator darkCloudsTransition;
    public Animator whiteCloudsTransition;
    public Animator metalTransition;

    protected const float TransitionTime = 1.5f;

    public void LoadMenu()
    {
        StartCoroutine(Menu());
    }

    IEnumerator Menu()
    {
        darkCloudsTransition.SetTrigger("DarkCloudsStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        darkCloudsTransition.SetTrigger("DarkCloudsStart");

        yield return new WaitForSeconds(TransitionTime);

        gameState.message = "";
        gameState.score = 0;
        gameState.enemiesDefeated = 0;
        gameState.numAmmo[0] = 10;
        gameState.numAmmo[1] = 30;
        gameState.numAmmo[2] = 8;
        gameState.numAmmo[3] = 5;
        gameState.numAmmo[4] = 40;

        SceneManager.LoadScene(1);
    }

    public void LoadWhiteCloudScene(int scene)
    {
        StartCoroutine(WhiteCloudScene(scene));
    }

    IEnumerator WhiteCloudScene(int scene)
    {
        whiteCloudsTransition.SetTrigger("WhiteCloudsStart");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(scene);
    }

    public void LoadMetalScene(int scene)
    {
        StartCoroutine(MetalScene(scene));
    }

    IEnumerator MetalScene(int scene)
    {
        metalTransition.SetTrigger("MetalStart");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(scene);
    }
}

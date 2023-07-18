using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public int[] upgradeLevel;
    public int[] price;
    public string[] maxed;
    public bool[] canBuy;
    public int[] numAmmo;
    public bool[] hasGainedPowerUps;
    public float score;
    public float highScore;
    public float playerHp;
    public float coins;
    public int weaponIndex;
    public int enemiesDefeated;
    public string message;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameState");
        if (objs.Length > 1)
        {
            //I'm not unique, destroy myself
            Destroy(this.gameObject);
        }
        //I'm the only one, make sure I survive between scenes
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < canBuy.Length; i++)
        {
            canBuy[i] = true;
        }
    }
    public void LoadResultsScreen()
    {
        StartCoroutine(ResultsScreen());
    }

    IEnumerator ResultsScreen()
    {
        yield return new WaitForSeconds(2);
        message = "Mission Failed";
        if(score > highScore)
        {
            highScore = score;
        }
        numAmmo[0] = 10;
        for(int i = 0; i < hasGainedPowerUps.Length; i++)
        {
            hasGainedPowerUps[i] = false;
        }
        SceneManager.LoadScene(2);
    }
}

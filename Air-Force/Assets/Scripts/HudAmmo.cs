using UnityEngine;
using UnityEngine.UI;

public class HudAmmo : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    private void Update()
    {
        if(gameState.weaponIndex == 1)
        {
            GetComponent<Text>().text = "Bombs: " + gameState.numAmmo[0];
        }
        else if (gameState.weaponIndex == 2)
        {
            GetComponent<Text>().text = "Flame: " + gameState.numAmmo[1];
        }
        else if (gameState.weaponIndex == 3)
        {
            GetComponent<Text>().text = "Smoke Bombs: " + gameState.numAmmo[2];
        }
        else if (gameState.weaponIndex == 4)
        {
            GetComponent<Text>().text = " ";
        }
        else if (gameState.weaponIndex == 5)
        {
            GetComponent<Text>().text = "Black Hole: " + gameState.numAmmo[3];
        }
        else if (gameState.weaponIndex == 6)
        {
            GetComponent<Text>().text = "Laser: " + gameState.numAmmo[4];
        }
    }
}

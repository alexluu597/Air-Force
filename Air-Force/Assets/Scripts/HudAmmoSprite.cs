using UnityEngine;

public class HudAmmoSprite : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public Sprite[] sprites;

    void Update()
    {
        if (gameState.weaponIndex == 1)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else if (gameState.weaponIndex == 2)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else if (gameState.weaponIndex == 3)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
        else if (gameState.weaponIndex == 4)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[3];
        }
        else if (gameState.weaponIndex == 5)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[4];
        }
        else if (gameState.weaponIndex == 6)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[5];
        }
    }
}

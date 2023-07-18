using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public int[,] shopItems = new int[4, 16];
    public string[] maxed = new string[15];
    public Text CoinsText;

    void Start()
    {
        CoinsText.text = gameState.coins.ToString();

        //ID's
        for (int i = 1; i < 16; i++)
        {
            shopItems[1, i] = i;
        }

        //Price
        shopItems[2, 1] = gameState.price[0];
        shopItems[2, 2] = gameState.price[1];
        shopItems[2, 3] = gameState.price[2];
        shopItems[2, 4] = gameState.price[3];
        shopItems[2, 5] = gameState.price[4];
        shopItems[2, 6] = gameState.price[5];
        shopItems[2, 7] = gameState.price[6];
        shopItems[2, 8] = gameState.price[7];
        shopItems[2, 9] = gameState.price[8];
        shopItems[2, 10] = gameState.price[9];
        shopItems[2, 11] = gameState.price[10];
        shopItems[2, 12] = gameState.price[11];
        shopItems[2, 13] = gameState.price[12];
        shopItems[2, 14] = gameState.price[13];
        shopItems[2, 15] = gameState.price[14];

        //UpgradeLevel
        for (int i = 1; i < 16; i++)
        {
            shopItems[3, i] = gameState.upgradeLevel[i - 1];
        }

        for(int i = 0; i < 15; i++)
        {
            maxed[i] = gameState.maxed[i];
        }
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if(gameState.coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            if(gameState.canBuy[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1])
            {
                gameState.coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                gameState.upgradeLevel[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1]++;
                shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID] = gameState.upgradeLevel[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1];
                CoinsText.text = gameState.coins.ToString();
                gameState.price[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1] *= 2;
                shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID] = gameState.price[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1];
                ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = gameState.upgradeLevel[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1].ToString();
                if (gameState.upgradeLevel[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1] >= 3)
                {
                    gameState.canBuy[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1] = false;
                    gameState.price[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1] = 0;
                    shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID] = gameState.price[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1];
                    gameState.maxed[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1] = "Maxed";
                    maxed[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1] = gameState.maxed[ButtonRef.GetComponent<ButtonInfo>().ItemID - 1];
                }
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBank : MonoBehaviour
{
    private int coins = 900; //how much money the player has (they will start off the game with 1000 coins

    private UIController uiController;

    public AudioClip loseCoinsClip;
    public AudioClip gainCoinsClip;
    private AudioSource audioSource;

    public int Coins
    {
        get { return coins; }
        set { coins = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        uiController.UpdateCoinsText(coins);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCoins(int value) {
        coins += value;
        uiController.UpdateCoinsText(coins);

        audioSource.clip = gainCoinsClip;
        audioSource.Play();




    }

    public void RemoveCoins(int value) {
        coins -= value;

        if (coins < 0) {
            coins = 0;
        }

        //update ui
        uiController.UpdateCoinsText(coins);
        audioSource.clip = loseCoinsClip;
        audioSource.Play();
    }

}

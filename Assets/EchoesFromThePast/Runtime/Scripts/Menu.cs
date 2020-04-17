using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //Play - K-391, AlanWalker
    public void Play(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    //Quit - Cashmere Cat, Ariana Grande
    public void Quit()
    {
        Application.Quit();
    }
}

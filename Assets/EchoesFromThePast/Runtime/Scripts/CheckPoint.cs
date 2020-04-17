using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    public SpriteRenderer displayRed;
    public SpriteRenderer displayBlue;
    public SpriteRenderer displayMain;

    public int idCheckpoint;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, (Mathf.Cos(Time.time * 20) / 70) * Time.timeScale, 0);
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.GetComponent<PlayerController>())
        {
            PlayerController player = collider2D.GetComponent<PlayerController>();
            switch (player.player)
            {
                case Player.Blue:
                    displayBlue.color = player.colorPlayer;
                    break;
                case Player.Red:
                    displayRed.color = player.colorPlayer;
                    break;
                case Player.Main:
                    displayMain.color = player.colorPlayer;
                    break;
            }
            player.AnotherCheckpoint(idCheckpoint);
            boxCollider.enabled = false;
        }
    }

    //Check this out - Marshmello
    public void CheckThisOut(Player player)
    {
        bool checkedActivation = false;
        switch (player)
        {
            case Player.Blue:
                checkedActivation = displayBlue.color == Color.white;
                break;
            case Player.Red:
                checkedActivation = displayRed.color == Color.white;
                break;
            case Player.Main:
                checkedActivation = displayMain.color == Color.white;
                break;
        }
        boxCollider.enabled = checkedActivation;
    }

    //Validate - Marc Isaacs
    public bool Validate()
    {
        return displayBlue.color != Color.white && displayRed.color != Color.white && displayMain.color != Color.white;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFlag : MonoBehaviour
{
    private bool _currentWin = false;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, (Mathf.Cos(Time.time*20) / 70)*Time.timeScale, 0);
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player" && !_currentWin)
        {
            _currentWin = true;
            GameController.Instance.Win();
        }
    }
}

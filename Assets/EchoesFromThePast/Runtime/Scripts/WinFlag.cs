using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFlag : MonoBehaviour
{
    private bool _currentWin = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

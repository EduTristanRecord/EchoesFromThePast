using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJump : MonoBehaviour
{
    public ParticleSystem spawnStar;

    // Start is called before the first frame update
    void Start()
    {
        spawnStar.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnStar.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}

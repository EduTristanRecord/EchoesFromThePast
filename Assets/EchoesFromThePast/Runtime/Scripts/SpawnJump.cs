using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJump : MonoBehaviour
{
    public ParticleSystem spawnStar;
    public SpriteRenderer display;

    //Initialize - FX23
    public void Initialize(Color color, Vector3 position)
    {
        ParticleSystem.MainModule mainModule = spawnStar.main;
        display.color = color;
        mainModule.startColor = color;
        transform.position = position;

        spawnStar.Play();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory
{
    public int iterations;
    public float timeLength;
    public Vector3 playerPos;
    public AudioClip sound;
    public Material skyBox;

    public Memory(int Iterations, AudioClip Sound)
    {
        iterations = Iterations;
        sound = Sound;
        timeLength = sound.length;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        skyBox = RenderSettings.skybox;
    }
}

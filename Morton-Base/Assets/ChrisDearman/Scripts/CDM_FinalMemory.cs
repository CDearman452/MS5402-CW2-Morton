﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets;
using UnityStandardAssets.Characters;
using UnityStandardAssets.Characters.FirstPerson;

public class CDM_FinalMemory : MonoBehaviour
{
    //==============================================================================================================
    // Variables
    //Private
    private bool bl_fadeOut = true;
    private bool bl_fadeIn = false;
    private bool bl_trigger;
    private bool bl_waitForAudio;
    private bool bl_fadeUpEnd = false;

    private float fl_fadeDuration = 2;

    private int in_num = 0;

    private GameObject go_blackOut;

    private Memory mem_01;
    //---------------------------
    // Public
    public GameObject[] go_worldSets;
    public GameObject go_memoryObject;
    public GameObject go_endScreen;
    public Material mt_memorySky;
    public Light lt_memorySpot;
    public AudioClip ac_memorySound;
    public AudioSource as_memory;
    //==============================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        go_blackOut = GameObject.Find("BlackOut");
        mem_01 = new Memory(1, ac_memorySound);
    }
    //==============================================================================================================
    // Update is called once per frame
    void Update()
    {
        if (bl_trigger)
        {
            if (bl_fadeOut)
            {
                float fl_fadeValue = (1 / fl_fadeDuration) * Time.deltaTime;
                Color cl_newFade = go_blackOut.GetComponent<Image>().color;

                if (go_blackOut.GetComponent<Image>().color.a < 1)
                {
                    cl_newFade.a += fl_fadeValue;
                    go_blackOut.GetComponent<Image>().color = cl_newFade;
                }
                else
                {
                    bl_fadeOut = false;
                    go_memoryObject.SetActive(true);
                    go_worldSets[1].SetActive(true);
                    go_worldSets[0].SetActive(false);
                    GameObject.FindGameObjectWithTag("Player").SetActive(false);
                    RenderSettings.skybox = mt_memorySky;
                    lt_memorySpot.intensity = 10;
                    bl_fadeIn = true;
                }
            }
            else if (bl_fadeIn)
            {

                float fl_fadeValue = (1 / fl_fadeDuration) * Time.deltaTime;
                Color cl_newFade = go_blackOut.GetComponent<Image>().color;

                if (go_blackOut.GetComponent<Image>().color.a > 0)
                {
                    cl_newFade.a -= fl_fadeValue;
                    go_blackOut.GetComponent<Image>().color = cl_newFade;
                }
                else
                {
                    while (bl_waitForAudio == false)
                    {
                        if (as_memory.clip != mem_01.sound) as_memory.clip = mem_01.sound;
                        else
                        {
                            if (!as_memory.isPlaying) as_memory.Play();
                            else bl_waitForAudio = true;
                        }
                    }

                    if (bl_waitForAudio)
                    {
                        if (!as_memory.isPlaying)
                        {
                            bl_fadeUpEnd = true;
                            bl_fadeIn = false;
                        }
                    }
                }
            }

            if (bl_fadeUpEnd)
            {
                if (in_num == 0)
                {
                    float fl_fadeValue = (1 / fl_fadeDuration) * Time.deltaTime;
                    Color cl_newFade = go_blackOut.GetComponent<Image>().color;

                    if (go_blackOut.GetComponent<Image>().color.a < 1)
                    {
                        cl_newFade.a += fl_fadeValue;
                        go_blackOut.GetComponent<Image>().color = cl_newFade;
                    }
                    else
                    {
                        go_endScreen.SetActive(true);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        in_num++;
                    }
                }
                else if (in_num == 1)
                {
                    float fl_fadeValue = (1 / fl_fadeDuration) * Time.deltaTime;
                    Color cl_newFade = go_blackOut.GetComponent<Image>().color;

                    if (go_blackOut.GetComponent<Image>().color.a > 0)
                    {
                        cl_newFade.a -= fl_fadeValue;
                        go_blackOut.GetComponent<Image>().color = cl_newFade;
                    }
                    else
                    {
                        GetComponent<CDM_FinalMemory>().enabled = false;
                        in_num++;
                    }
                }
            }
        }
    }
    //==============================================================================================================
    // Trigger for final memory
    private void OnTriggerEnter(Collider _col)
    {
        bl_trigger = true;
    }
}

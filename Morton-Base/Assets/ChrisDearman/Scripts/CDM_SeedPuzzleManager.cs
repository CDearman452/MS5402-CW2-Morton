using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CDM_SeedPuzzleManager : MonoBehaviour
{
    //==============================================================================================================
    // Variables
    // private
    private bool bl_fadeComplete = false;
    private bool bl_fadeIn = false;
    private bool bl_fadeOut = false;
    private bool bl_waitForAudio = false;

    private int in_stage = 0;
    private int in_num = 0;

    private GameObject go_blackOut;
    private GameObject go_player;

    private AudioSource as_memory;
    //---------------------------------------
    // public
    public bool bl_trigger = false;

    public float fl_fadeDuration;

    public GameObject[] go_seedPuzzleObjects;
    public GameObject[] go_worldSets;
    public GameObject[] go_treeSet;
    public GameObject[] go_memoryTableau;
    public Light SkyLightSource;
    public Light MemorySpot;

    public Material mt_memorySky;

    public AudioClip[] MemorySounds;
    //==============================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        go_blackOut = GameObject.Find("BlackOut");
        go_player = GameObject.FindGameObjectWithTag("Player");
        as_memory = GameObject.Find("PickupPos").GetComponent<AudioSource>();
    }
    //==============================================================================================================
    // Update is called once per frame
    void Update()
    {
        //------------------------------------------
        // Trigger each stage when the correct item is used to interact with the target zone
        if (bl_trigger == true)
        {
            if (in_stage <= 0)//------------------
            {
                if (go_player.GetComponent<CDM_3DItemPickup>().go_held == go_seedPuzzleObjects[0]) bl_fadeOut = true;
            }
            else if (in_stage == 1)//------------------
            {
                if (go_player.GetComponent<CDM_3DItemPickup>().go_held == go_seedPuzzleObjects[1]) bl_fadeOut = true;
            }
            else if (in_stage >= 2)//------------------
            {
                if (go_player.GetComponent<CDM_3DItemPickup>().go_held == go_seedPuzzleObjects[2]) bl_fadeOut = true;
            }
            bl_trigger = false;
        }
        //------------------------------------------
        // When screen is black, trigger memory sequence
        if (bl_fadeComplete == true)
        {
            if (in_stage <= 0)//------------------
            {
                if (in_num == 0) MemoryTransition(0);

                MemorySequence(new Memory(1, MemorySounds[0]));
            }
            else if (in_stage == 1)//------------------
            {
                if (in_num == 0) MemoryTransition(0);

                MemorySequence(new Memory(1, MemorySounds[1]));
            }
            else if (in_stage >= 2)//------------------
            {
                if (in_num == 0) MemoryTransition(0);

                MemorySequence(new Memory(1, MemorySounds[2]));
            }
        }
        //------------------------------------------
        if (bl_fadeOut == true) FadeOut(fl_fadeDuration);
        else if (bl_fadeIn == true) FadeIn(fl_fadeDuration);
    }
    //==============================================================================================================
    // Gradually fades the screen to black and returns a bool when complete
    void FadeOut(float fl_frameDuration)
    {
        float fl_fadeValue = (1 / fl_frameDuration) * Time.deltaTime;
        Color cl_newFade = go_blackOut.GetComponent<Image>().color;

        if (go_blackOut.GetComponent<Image>().color.a < 1)
        {
            cl_newFade.a += fl_fadeValue;
            go_blackOut.GetComponent<Image>().color = cl_newFade;
        }
        else
        {
            bl_fadeOut = false;
            bl_fadeComplete = true;
            bl_fadeIn = true;
        }
    }
    //==============================================================================================================
    // Fade in
    void FadeIn (float fl_frameDuration)
    {
        float fl_fadeValue = (1 / fl_frameDuration) * Time.deltaTime;
        Color cl_newFade = go_blackOut.GetComponent<Image>().color;

        if (go_blackOut.GetComponent<Image>().color.a > 0)
        {
            cl_newFade.a -= fl_fadeValue;
            go_blackOut.GetComponent<Image>().color = cl_newFade;
        }
        else
        {
            bl_fadeIn = false;
        }
    }
    //==============================================================================================================
    // Unchild a pickup
    void unchild()
    {
        Debug.Log("Child");
        go_player.GetComponent<CDM_3DItemPickup>().go_held.transform.SetParent(null); // Unchild the pickup
        go_player.GetComponent<CDM_3DItemPickup>().go_held.transform.position = new Vector3(0, 0, -100); // Move the pickup to the set location
        go_player.GetComponent<CDM_3DItemPickup>().go_held.layer = LayerMask.NameToLayer("Default"); // Swap it to the default layermask so that it is no longer on the top render layer
        go_player.GetComponent<CDM_3DItemPickup>().go_held = null;
    }
    //==============================================================================================================
    // Begin or End Memory transition
    void MemoryTransition (int identifier)
    {
        if (identifier == 0)
        {
            unchild(); // Remove Object
            if (go_treeSet[in_stage] != null) go_treeSet[in_stage].SetActive(false); // Deactivate the Current Tree Asset
            go_memoryTableau[in_stage].SetActive(true);
            go_worldSets[1].SetActive(true);
            go_worldSets[0].SetActive(false);
            RenderSettings.skybox = mt_memorySky;
            RenderSettings.sun = null;
            in_num++;
        }
        else if (identifier == 1)
        {
            bl_fadeComplete = false;
            go_memoryTableau[in_stage].SetActive(false);
            go_worldSets[0].SetActive(true);
            go_worldSets[1].SetActive(false);
            go_treeSet[in_stage + 1].SetActive(true);
            in_stage++;
            in_num = 0;
        }
    }
    //==============================================================================================================
    // Run memory based on stage
    void MemorySequence(Memory _mem)
    {
        if (bl_fadeIn == false)
        {
            if (MemorySpot.intensity < 10) MemorySpot.intensity += 0.1f;

            while (bl_waitForAudio == false)
            {
                if (as_memory.clip != _mem.sound) as_memory.clip = _mem.sound;
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
                    float fl_fadeValue = (1 / fl_fadeDuration) * Time.deltaTime;
                    Color cl_newFade = go_blackOut.GetComponent<Image>().color;

                    if (go_blackOut.GetComponent<Image>().color.a < 1)
                    {
                        cl_newFade.a += fl_fadeValue;
                        go_blackOut.GetComponent<Image>().color = cl_newFade;
                    }
                    else
                    {
                        MemoryTransition(1);
                        MemorySpot.intensity = 0;
                        go_player.SetActive(false);
                        go_player.transform.position = _mem.playerPos;
                        go_player.SetActive(true);
                        RenderSettings.skybox = _mem.skyBox;
                        RenderSettings.sun = SkyLightSource;
                        bl_fadeIn = true;
                        bl_fadeComplete = false;
                    }
                }
            }
        }
    }
}

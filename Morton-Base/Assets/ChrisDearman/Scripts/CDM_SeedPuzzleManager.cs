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
    public bool bl_fadeComplete = false;
    
    public bool bl_fadeOut = false;

    public int in_stage = 0;

    private GameObject go_blackOut;
    private GameObject go_player;
    //---------------------------------------
    // public
    public bool bl_trigger = false;

    public float fl_fadeDuration;

    public GameObject[] go_seedPuzzleObjects;
    //==============================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        go_blackOut = GameObject.Find("BlackOut");
        go_player = GameObject.FindGameObjectWithTag("Player");
    }
    //==============================================================================================================
    // Update is called once per frame
    void Update()
    {
        //------------------------------------------
        // Trigger each stage when the correct item is used to interact with the target zone
        if (bl_trigger == true)
        {
            if (in_stage <= 0)
            {
                if (go_player.GetComponent<CDM_3DItemPickup>().go_held == go_seedPuzzleObjects[0])
                {
                    bl_fadeOut = true;
                }
            }
            else if (in_stage == 1)
            {
                if (go_player.GetComponent<CDM_3DItemPickup>().go_held == go_seedPuzzleObjects[1])
                {
                    bl_fadeOut = true;
                }
            }
            else if (in_stage >= 2)
            {
                if (go_player.GetComponent<CDM_3DItemPickup>().go_held == go_seedPuzzleObjects[2])
                {
                    bl_fadeOut = true;
                }
            }
            bl_trigger = false;
        }
        //------------------------------------------
        // When screen is black, trigger memory sequence
        if (bl_fadeComplete == true)
        {
            if (in_stage <= 0)
            {
                unchild();
                // Trigger memory Sequence here
                bl_fadeComplete = false;
                in_stage++;
                // Change Plant Object
            }
            else if (in_stage == 1)
            {
                unchild();
                // Trigger memory Sequence here
                bl_fadeComplete = false;
                in_stage++;
                // Change Plant Object
            }
            else if (in_stage >= 2)
            {
                unchild();
                // Trigger memory Sequence here
                bl_fadeComplete = false;
                in_stage++;
                // Change Plant Object
            }
        }
        //------------------------------------------
        if (bl_fadeOut == true) FadeOut(fl_fadeDuration);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CDM_PaintingPuzzleManager : MonoBehaviour
{
    //==============================================================================================================
    // Variables
    // private
    [SerializeField] private bool bl_puzzleComplete = false;
    private bool[] bl_locksCompleted;
    private bool bl_memComplete = false;
    private bool bl_fadeOut;
    private bool bl_fadeIn;
    private bool bl_waitForAudio = false;
    private int in_locksCompleted = 0;
    private int in_num = 0;
    private GameObject go_blackOut;
    private GameObject go_player;
    private Memory mem_01;
    //---------------------------------------
    // public
    public GameObject[] go_paintings;
    public GameObject[] go_pendants;
    public GameObject[] go_worldSets;
    public GameObject go_memoryObject;
    public GameObject go_door;
    public AudioClip ac_memAudio;
    public AudioSource as_memory;
    public Material mt_memorySky;
    public Light lt_memorySpot;
    public float fl_fadeDuration;
    //==============================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        bl_locksCompleted = new bool[3];
        mem_01 = new Memory(1, ac_memAudio);
        go_blackOut = GameObject.Find("BlackOut");
        go_player = GameObject.FindGameObjectWithTag("Player");
    }
    //==============================================================================================================
    // Update is called once per frame
    void Update()
    {
        if (!bl_puzzleComplete)
        {
            foreach (GameObject _go in go_pendants)
            {
                if (!bl_locksCompleted[Array.IndexOf(go_pendants, _go)] && _go.transform.position == go_paintings[Array.IndexOf(go_pendants, _go)].transform.GetChild(0).transform.position)
                {
                    _go.GetComponent<Collider>().enabled = false;
                    go_paintings[Array.IndexOf(go_pendants, _go)].transform.tag = "Untagged";
                    bl_locksCompleted[Array.IndexOf(go_pendants, _go)] = true;
                }
            }

            in_locksCompleted = 0;
            foreach (bool _bl in bl_locksCompleted)
            {
                if (_bl) in_locksCompleted++;
            }

            if (in_locksCompleted >= 3)
            {
                bl_puzzleComplete = true;
                bl_fadeOut = true;
            }
        }
        else
        {
            if (go_paintings[1].GetComponent<Animator>().GetBool("bl_open") != true) go_paintings[1].GetComponent<Animator>().SetBool("bl_open", true);
            
            if (!bl_memComplete)
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
                    }
                }
                else if (in_num == 0)
                {
                    mem_01.playerPos = go_player.transform.position;
                    go_memoryObject.SetActive(true);
                    go_worldSets[1].SetActive(true);
                    go_worldSets[0].SetActive(false);
                    go_player.SetActive(false);
                    RenderSettings.skybox = mt_memorySky;
                    lt_memorySpot.intensity = 10;
                    bl_fadeIn = true;
                    in_num++;
                }

                if (bl_fadeIn)
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
                        bl_fadeIn = false;
                    }
                }
                else if (in_num == 1)
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
                            float fl_fadeValue = (1 / fl_fadeDuration) * Time.deltaTime;
                            Color cl_newFade = go_blackOut.GetComponent<Image>().color;

                            if (go_blackOut.GetComponent<Image>().color.a < 1)
                            {
                                cl_newFade.a += fl_fadeValue;
                                go_blackOut.GetComponent<Image>().color = cl_newFade;
                            }
                            else
                            {
                                go_memoryObject.SetActive(false);
                                go_worldSets[0].SetActive(true);
                                go_worldSets[1].SetActive(false);
                                go_player.SetActive(true);
                                RenderSettings.skybox = null;
                                lt_memorySpot.intensity = 0;

                                go_door.transform.localPosition = new Vector3(0.139f, 1.988f, -1.686f);
                                go_door.transform.localRotation = Quaternion.Euler(0, -75, 0);

                                go_player.SetActive(false);
                                go_player.transform.position = mem_01.playerPos;
                                go_player.SetActive(true);

                                in_num++;
                            }
                        }
                    }
                }
                else if (in_num == 2)
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
                        bl_memComplete = false;
                        in_num++;
                    }
                }
            }

        }
    }
}

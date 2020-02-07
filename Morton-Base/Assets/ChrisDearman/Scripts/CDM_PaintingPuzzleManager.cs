using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CDM_PaintingPuzzleManager : MonoBehaviour
{
    //==============================================================================================================
    // Variables
    // private
    public bool bl_puzzleComplete = false;
    public bool[] bl_locksCompleted;
    private int in_locksCompleted = 0;
    //---------------------------------------
    // public
    public GameObject[] go_paintings;
    public GameObject[] go_pendants;
    //==============================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        bl_locksCompleted = new bool[3];
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
            if (in_locksCompleted >= 3) bl_puzzleComplete = true;
        }
        else
        {
            go_paintings[1].GetComponent<Animator>().SetBool("bl_open", true);
            GetComponent<CDM_PaintingPuzzleManager>().enabled = false;
        }
    }
}

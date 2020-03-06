using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_SeedPuzzleManager : MonoBehaviour
{
    //==============================================================================================================
    // Variables
    // private
    private int in_stage = 0;
    //---------------------------------------
    // public
    public bool bl_trigger = false;

    public GameObject[] go_seedPuzzleObjects;
    //==============================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //==============================================================================================================
    // Update is called once per frame
    void Update()
    {
        if (bl_trigger == true)
        {
            if (in_stage <= 0)
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<CDM_3DItemPickup>().go_held = go_seedPuzzleObjects[0])
                {
                    
                }
            }

            bl_trigger = false;
        }
    }
}

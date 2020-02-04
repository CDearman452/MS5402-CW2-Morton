using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_3DItemPickup : MonoBehaviour
{
    //==============================================================================================================
    // Variables
    // Private
    private GameObject go_held;
    
    private RaycastHit rch_PickupCheck;

    private Ray ry_sightRay;
    //---------------------------------------
    // Public
    public GameObject go_cameraObj;
    public GameObject go_pickupPos;
    public GameObject go_cullCamera;

    public float fl_maxPickupDist;
    //==============================================================================================================
    // Update is called once per frame
    void Update()
    {
        //---------------------------------------
        // Check the players Input
        if (Input.GetKeyDown(KeyCode.E))
        {
            //---------------------------------------
            // Dissable the character controller to avoid returning it as a result, then collect raycast data, reactivate the character controller
            GetComponent<CharacterController>().enabled = false;
            ry_sightRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ry_sightRay.origin, ry_sightRay.direction, Color.blue);
            if (Physics.Raycast(ry_sightRay, out rch_PickupCheck, fl_maxPickupDist))
            {
                if (rch_PickupCheck.transform.tag == "Pickup")
                {
                    go_held = rch_PickupCheck.transform.gameObject;
                    go_held.transform.SetParent(go_cameraObj.transform);
                    go_held.transform.position = go_pickupPos.transform.position;
                    go_cullCamera.SetActive(true);
                }
            }
            GetComponent<CharacterController>().enabled = true;
            //---------------------------------------
        }
        //---------------------------------------
    }
    //==============================================================================================================
}

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

    private Vector3 v3_pickupStartPos;
    //---------------------------------------
    // Public
    public GameObject go_cameraObj;
    public GameObject go_pickupPos;
    public GameObject go_cullCamera;

    public LayerMask lm_playerExclude;

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
            ry_sightRay = Camera.main.ScreenPointToRay(Input.mousePosition); // Create Ray
            Debug.DrawRay(ry_sightRay.origin, ry_sightRay.direction, Color.blue); // Debug view of ray in editor
            if (Physics.Raycast(ry_sightRay, out rch_PickupCheck, fl_maxPickupDist, lm_playerExclude))
            {
                if (rch_PickupCheck.transform.tag == "Pickup")
                {
                    if (go_held != null)
                    {
                        go_held.transform.SetParent(null); // Unchild the pickup
                        go_held.transform.position = v3_pickupStartPos; // Return it to its original position
                        go_held.layer = LayerMask.NameToLayer("Default"); // Swap it to the default layermask so that it is no longer on the top render layer
                    }

                    go_held = rch_PickupCheck.transform.gameObject; // Switch the held Item
                    v3_pickupStartPos = new Vector3(go_held.transform.position.x, go_held.transform.position.y, go_held.transform.position.z); // Track the objects original position
                    go_held.transform.SetParent(go_cameraObj.transform); // Set the parent of the pickup
                    go_held.transform.position = go_pickupPos.transform.position; // Move the pickup to the same position as an empy child on the player

                    go_held.layer = LayerMask.NameToLayer("Pickup"); // Switch the pickups layermask so it will be rendered as top layer
                }
            }
            //---------------------------------------
        }
        //---------------------------------------
    }
    //==============================================================================================================
}

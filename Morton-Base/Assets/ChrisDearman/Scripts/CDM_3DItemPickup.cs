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
            ry_sightRay = Camera.main.ScreenPointToRay(Input.mousePosition); // Create Ray
            Debug.DrawRay(ry_sightRay.origin, ry_sightRay.direction, Color.blue); // Debug view of ray in editor
            if (Physics.Raycast(ry_sightRay, out rch_PickupCheck, fl_maxPickupDist, lm_playerExclude)) // If a ray using the above parameters hits something within the specified layer/s and within range return true and fill the raycasthit variable
            {
                if (rch_PickupCheck.transform.tag == "Pickup") // If the object is a pickup
                {
                    if (go_held != null) // If an object is already held
                    {
                        go_held.transform.SetParent(null); // Unchild the pickup
                        go_held.transform.position = v3_pickupStartPos; // Return it to its original position
                        go_held.layer = LayerMask.NameToLayer("Default"); // Swap it to the default layermask so that it is no longer on the top render layer
                    }
                    //---------------------------------------
                    go_held = rch_PickupCheck.transform.gameObject; // Switch the held Item
                    v3_pickupStartPos = new Vector3(go_held.transform.position.x, go_held.transform.position.y, go_held.transform.position.z); // Track the objects original position
                    go_held.transform.SetParent(go_cameraObj.transform); // Set the parent of the pickup
                    go_held.transform.position = go_pickupPos.transform.position; // Move the pickup to the same position as an empy child on the player
                    go_held.layer = LayerMask.NameToLayer("Pickup"); // Switch the pickups layermask so it will be rendered as top layer
                    //---------------------------------------
                }
                else if (rch_PickupCheck.transform.tag == "PickupPlace") // If the object is an object to place a pickup in
                {
                    if (go_held != null) // If an object is held
                    {
                        go_held.transform.SetParent(rch_PickupCheck.transform.gameObject.transform); // Unchild the pickup
                        GameObject _go_temp = rch_PickupCheck.transform.gameObject; // Referance the interacted object as a temp variable
                        go_held.transform.position = _go_temp.transform.GetChild(0).transform.position; // Move the pickup to the set location
                        go_held.layer = LayerMask.NameToLayer("Default"); // Swap it to the default layermask so that it is no longer on the top render layer
                        go_held = null;
                    }
                    //---------------------------------------
                }
            }
            //---------------------------------------
        }
        //---------------------------------------
    }
    //==============================================================================================================
}

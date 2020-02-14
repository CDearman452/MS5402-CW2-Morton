using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDM_3DItemPickup : MonoBehaviour
{
    //==============================================================================================================
    // Variables
    // Private
    private GameObject go_held;
    private GameObject go_pendantSwitch;
    private GameObject[] go_pendants;

    private RaycastHit rch_PickupCheck;

    private Ray ry_sightRay;

    private Vector3 v3_pickupStartPos;
    private Vector3 v3_pickupStartRot;

    private bool bl_pendantPresent;
    //---------------------------------------
    // Public
    public GameObject go_cameraObj;
    public GameObject go_pickupPos;
    public GameObject go_cullCamera;

    public LayerMask lm_playerExclude;

    public float fl_maxPickupDist;
    //==============================================================================================================
    // Start is called before the first frame
    void Start()
    {
        go_pendants = GameObject.FindGameObjectsWithTag("Pickup");
    }
    //==============================================================================================================
    // Update is called once per frame
    void Update()
    {
        //---------------------------------------
        // Check the players Input
        if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.Mouse0))
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
                    go_held.transform.rotation = go_pickupPos.transform.rotation; // Rotate the pickup to the same position as an empty child on the player
                    go_held.layer = LayerMask.NameToLayer("Pickup"); // Switch the pickups layermask so it will be rendered as top layer
                    //---------------------------------------
                }
                else if (rch_PickupCheck.transform.tag == "PickupPlace") // If the object is an object to place a pickup in
                {
                    if (go_held != null) // If an object is held
                    {
                        GameObject _go_temp = rch_PickupCheck.transform.gameObject; // Referance the interacted object as a temp variable
                        //---------------------------------------
                        foreach (GameObject _go in go_pendants)
                        {
                            if (_go.transform.position == _go_temp.transform.GetChild(0).transform.position)
                            {
                                bl_pendantPresent = true;
                                go_pendantSwitch = _go;
                            }
                        }
                        //---------------------------------------
                        if (!bl_pendantPresent)
                        {
                            go_held.transform.SetParent(rch_PickupCheck.transform.gameObject.transform); // Unchild the pickup
                            go_held.transform.position = _go_temp.transform.GetChild(0).transform.position; // Move the pickup to the set location
                            go_held.transform.rotation = _go_temp.transform.GetChild(0).transform.rotation; // Rotate the pickup to the set location
                            go_held.layer = LayerMask.NameToLayer("Default"); // Swap it to the default layermask so that it is no longer on the top render layer
                            go_held = null;
                        }
                        else
                        {
                            go_held.transform.SetParent(rch_PickupCheck.transform.gameObject.transform); // Unchild the pickup
                            go_held.transform.position = _go_temp.transform.GetChild(0).transform.position; // Move the pickup to the set location
                            go_held.transform.rotation = _go_temp.transform.GetChild(0).transform.rotation; // Rotate the pickup to the set location
                            go_held.layer = LayerMask.NameToLayer("Default"); // Swap it to the default layermask so that it is no longer on the top render layer
                            go_held = go_pendantSwitch; // Switch the existing pickup within the portrate to the held item
                            //---------------------------------------
                            go_held.transform.SetParent(go_cameraObj.transform); // Set the parent of the new pickup
                            go_held.transform.position = go_pickupPos.transform.position; // Move the pickup to the same position as an empty child on the player
                            go_held.transform.rotation = go_pickupPos.transform.rotation;
                            go_held.layer = LayerMask.NameToLayer("Pickup"); // Switch the pickups layermask so it will be rendered as top layer
                        }
                        //---------------------------------------
                        bl_pendantPresent = false;
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

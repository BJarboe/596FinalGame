using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;

    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    PlayerMovement playerRotation;

    public Inventory inventory;
    public int inventorySlot = -1;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""

        playerRotation = player.GetComponent<PlayerMovement>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        GameObject pickUpObject = hit.transform.gameObject; // Get the GameObject that was hit

                        // Call the PickUpObject function, which handles the visual aspect of picking up
                        PickUpObject(pickUpObject);
                        inventorySlot = -1;

                        // If the GameObject has an ItemPickUp component, call AddToInventory
                        // This assumes that your inventory logic is handled in the ItemPickUp component
                        ItemPickUp itemPickUp = pickUpObject.GetComponent<ItemPickUp>();
                        if (itemPickUp != null)
                        {
                            itemPickUp.AddToInventory();
                        }
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            RotateObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            {
                StopClipping();
                ThrowObject();
            }

        }

        // Check for number key presses to select items from inventory slots
        for (int i = 0; i < 5; i++) // Assuming there are 5 slots indexed 0 to 4
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                if (inventory.slots[i].transform.childCount > 0)
                {
                    if(heldObj == null)
                    {
                        GameObject prefab = inventory.slots[i].transform.GetChild(0).GetComponent<SpawnItem>().SpawnObject();
                        GameObject item = Instantiate(prefab);
                        inventorySlot = i;
                        PickUpObject(item);
                    }
                    else if(inventorySlot == i)
                    {
                        GameObject.Destroy(heldObj);
                    }
                    else if (inventorySlot == -1)
                    {
                        DropObject();
                        GameObject prefab = inventory.slots[i].transform.GetChild(0).GetComponent<SpawnItem>().SpawnObject();
                        GameObject item = Instantiate(prefab);
                        inventorySlot = i;
                        PickUpObject(item);
                    }
                    else
                    {
                        GameObject.Destroy(heldObj);
                        GameObject prefab = inventory.slots[i].transform.GetChild(0).GetComponent<SpawnItem>().SpawnObject();
                        GameObject item = Instantiate(prefab);
                        inventorySlot = i;
                        PickUpObject(item);
                    }
                }
            }
        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        // Ensure the object is at the hold position when throwing
        heldObj.transform.position = holdPos.position;

        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = LayerMask.NameToLayer("Ground"); //object assigned back to ground layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
        if (inventorySlot != -1)
        {
            inventory.slots[inventorySlot].GetComponent<Slots>().DropItem();
        }
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            playerRotation.canLook = false;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            playerRotation.canLook = true;
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        // Ensure the object is at the hold position when throwing
        heldObj.transform.position = holdPos.position;

        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = LayerMask.NameToLayer("Ground");
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
        if (inventorySlot != -1)
        {
            inventory.slots[inventorySlot].GetComponent<Slots>().DropItem();
        }
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}

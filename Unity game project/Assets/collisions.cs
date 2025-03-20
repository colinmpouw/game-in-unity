using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisions : MonoBehaviour
{
    public GameObject floor; // Reference to the floor object

    // Start is called before the first frame update
    void Start()
    {
        // Add a BoxCollider to the floor object if it doesn't already have one
        if (floor != null && floor.GetComponent<Collider>() == null)
        {
            BoxCollider collider = floor.AddComponent<BoxCollider>();
            collider.isTrigger = false; // Ensure the collider is not a trigger
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

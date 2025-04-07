using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour
{
    public float interactionDistance = 3.5f; // Maximum distance for the player to interact with the door.
    public Transform player;
    public string doorOpenAnimName = "DoorOpen";
    public string doorCloseAnimName = "DoorClose";

    private Animator doorAnim;
    private bool isPlayerInRange = false;
    private bool isOpen = false;

    // Called once when the script is initialized.
    void Start()
    {
        doorAnim = GetComponent<Animator>();
        if (doorAnim == null)
        {
            Debug.LogError("Animator component is missing on the door object.");
        }
    }

    void Update()
    {
        CheckPlayerDistance(); // Continuously checks the player's distance
    }

    // Public method called by the UDPReceiver script to open/close the door based on input.
    public void CheckHigh2(int highprox)
    {
        // If input indicates interaction and the player is in range, toggle the door state.
        if (highprox == 1 && isPlayerInRange)
        {
            ToggleDoor();
        }
    }

    // Check if the player is within interaction range
    void CheckPlayerDistance()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            isPlayerInRange = distance <= interactionDistance;
        }
    }

    // Toggle door open/close
    private void ToggleDoor()
    {
        if (isOpen)
        {
            doorAnim.ResetTrigger("open");
            doorAnim.SetTrigger("close");
        }
        else
        {
            doorAnim.ResetTrigger("close");
            doorAnim.SetTrigger("open");
        }

        isOpen = !isOpen;  // Toggle the door state
    }
}
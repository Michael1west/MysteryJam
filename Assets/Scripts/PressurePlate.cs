using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public DoorController linkedDoor;

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger entered by: " + other.name);
    if (other.CompareTag("Player"))
    {
        Debug.Log("Player detected, attempting to open door");
        if (linkedDoor != null)
        {
            linkedDoor.OpenDoor();
            Debug.Log("OpenDoor called");
        }
        else
        {
            Debug.LogError("No door linked to this pressure plate!");
        }
    }
}

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            linkedDoor.CloseDoor();
        }
    }
}

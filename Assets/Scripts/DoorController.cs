using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject doorVisual;

    public void OpenDoor()
    {
        doorVisual.SetActive(false);
        GetComponent<Collider>().enabled = false;
    }

    public void CloseDoor()
    {
        doorVisual.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }
}

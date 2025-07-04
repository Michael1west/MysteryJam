using UnityEngine;
public class TestCollision : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        Debug.Log("Collided with: " + other.name);
    }
}

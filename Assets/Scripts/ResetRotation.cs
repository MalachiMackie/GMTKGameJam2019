using UnityEngine;

public class ResetRotation : MonoBehaviour
{
    public Vector3 Rotation;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var rotation = collider.gameObject.transform.rotation.eulerAngles;
            collider.gameObject.transform.Rotate(Rotation - rotation);
        }
    }
}

using UnityEngine;

public class Detection : MonoBehaviour
{
    public string objectTag = "";
    public bool isColliding;
    public GameObject currentObject;
    public Vector3 collisionNormal;

    private Collider currentCollider;

    private void OnTriggerStay(Collider other)
    {
       Check(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        Check(other);
    }

    private void Check(Collider other)
    {
         if (objectTag != "" && !isColliding)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
                collisionNormal = hit.point;

            DetectionTag dt = other.GetComponent<DetectionTag>();
            if (dt != null && dt.isEnabled && other != null &&
                !other.isTrigger && dt.HasTag(objectTag))
            {
                isColliding = true;
                currentObject = other.gameObject;
                currentCollider = other;
            }
        }

        if (objectTag == "" && !isColliding)
        {
            if (other != null && !other.isTrigger)
            {
                Debug.Log("i");
                isColliding = true;
                currentCollider = other;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentCollider)
            isColliding = false;
    }

    private void Update()
    {
        // if (currentObject == null || !currentCollider.enabled)
        //     isColliding = false;
        // if (currentObject != null && !currentObject.activeInHierarchy)
        //     isColliding = false;
    }

    public Collider GetCurrentCollider()
    {
        return currentCollider;
    }
}

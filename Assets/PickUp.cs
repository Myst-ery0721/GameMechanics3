using TMPro;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject itemHolder;
    public bool Pick;

    public TextMeshProUGUI text;

    public float pickupRange = 3f; // Maximum distance to pick up
    public float pullSpeed = 10f; // Speed to pull object towards player

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            targetObject = other.gameObject;

            if (!Pick)
                text.text = "Pick up";
            else
                text.text = "Drop";
        }
        else
        {
            targetObject = null;
            text.text = "";
        }
    }

    private void getItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && targetObject != null)
        {
            if (!Pick)
            {
                // Check if item is too far, then pull it closer
                float distance = Vector3.Distance(targetObject.transform.position, itemHolder.transform.position);

                if (distance > pickupRange)
                {
                    StartCoroutine(PullObjectCloser(targetObject));
                }
                else
                {
                    PickUpObject();
                }
            }
            else
            {
                ThrowObject();
            }
        }
    }

    private void PickUpObject()
    {
        Pick = true;
        targetObject.transform.parent = itemHolder.transform;
        Rigidbody rb = targetObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void ThrowObject()
    {
        Pick = false;
        targetObject.transform.parent = null;
        Rigidbody rb = targetObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(itemHolder.transform.forward * 20f, ForceMode.Impulse);
    }

    private System.Collections.IEnumerator PullObjectCloser(GameObject obj)
    {
        while (Vector3.Distance(obj.transform.position, itemHolder.transform.position) > pickupRange)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, itemHolder.transform.position, pullSpeed * Time.deltaTime);
            yield return null;
        }
        PickUpObject(); // Once close, pick it up
    }

    void Update()
    {
        getItem();
    }
}
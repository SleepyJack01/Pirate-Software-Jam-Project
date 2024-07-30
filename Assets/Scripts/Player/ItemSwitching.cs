using UnityEngine;

public class ItemSwitching : MonoBehaviour
{
    public Item[] items;  // Array to hold all available items
    private int currentItemIndex = 0;

    public Transform cam;
    public Transform attackPoint;

    public float throwCooldown;

    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    void Start()
    {
        readyToThrow = true;
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            SwitchItem(1);  // Switch to the next item
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            SwitchItem(-1);  // Switch to the previous item
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToThrow)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        // instantiate object to throw
        Item currentItem = items[currentItemIndex];
        GameObject projectile = Instantiate(currentItem.itemPrefab, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    void SwitchItem(int direction)
    {
        currentItemIndex += direction;

        if (currentItemIndex >= items.Length)
            currentItemIndex = 0;
        else if (currentItemIndex < 0)
            currentItemIndex = items.Length - 1;

    }
}

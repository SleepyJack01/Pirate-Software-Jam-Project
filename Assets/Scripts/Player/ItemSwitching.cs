using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public PlayerInput playerInput;
    private InputAction ThrowAction;
    private InputAction PotionCycleAction;

    void Start()
    {
        readyToThrow = true;
    }

    void Update()
    {
        if (ThrowAction == null)
        {
            ThrowAction = playerInput.actions["Throw"];
        }

        if (PotionCycleAction == null)
        {
            PotionCycleAction = playerInput.actions["PotionCycle"];
        }

        // Proceed if actions are correctly assigned
        if (ThrowAction != null && ThrowAction.triggered && readyToThrow)
        {
            Throw();
        }

        if (PotionCycleAction != null)
        {
            float cycleValue = PotionCycleAction.ReadValue<float>();

            if (cycleValue > 0)
            {
                SwitchItem(1);  // Switch to the next item
            }
            else if (cycleValue < 0)
            {
                SwitchItem(-1);  // Switch to the previous item
            }
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

        UpdateUI(); // Update UI after switching item

    }

    void Awake()
    {
        playerInput.actions.FindActionMap("PlayerControls").Enable();
    }

    // Method to get the currently selected item
    public Item GetCurrentItem()
    {
        if (items.Length == 0)
            return null;

        return items[currentItemIndex];
    }

    // Method to update the UI
    private void UpdateUI()
    {
        Item currentItem = GetCurrentItem();
        if (currentItem != null)
        {
            ItemUI.Instance.SetItem(currentItem.itemIcon);
        }
    }

}

using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public static ItemUI Instance;

    public Image itemIcon; // Reference to the UI Image for item icon

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetItem(Sprite icon)
    {
        itemIcon.sprite = icon;
    }
}

using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private GameObject Slot;
    private int slotCount;

    private void Awake()
    {
        slotCount = 20;
    }

    public int SlotCount
    {
        get { return slotCount; }
    }

    public void ExpandSlot(int amount)
    {
        slotCount += amount;
    }

}

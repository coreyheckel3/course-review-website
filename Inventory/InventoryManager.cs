using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool opened;
    public KeyCode inventoryKey = KeyCode.Tab;
    public Slot slot;

    [Header("Settings")]
    public int inventorySize = 24;
    public int hotbarSize = 6;

    [Header("Refs")]
    public GameObject dropModel;
    public Transform dropPos;
    public GameObject slotTemplate;
    public Transform contentHolder;
    public Transform hotbarContentHolder;

    private List<Slot> inventorySlots;
    private List<Slot> hotbarSlots;

    [SerializeField] private List<Slot> allSlots;


    private void Start()
    {
        GenerateHotbarSlots();
        GenerateSlots();
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            hotbarSlots[0].Try_Use();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            hotbarSlots[1].Try_Use();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            hotbarSlots[2].Try_Use();
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            hotbarSlots[3].Try_Use();
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            hotbarSlots[4].Try_Use();
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            hotbarSlots[5].Try_Use();
        }



        if(Input.GetKeyDown(inventoryKey))
            opened = !opened;
        if(opened)
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(-10000, 0, 0);
        }
    }

    private void GenerateSlots()
    {
        List<Slot> inventorySlots_ = new List<Slot>();
        List<Slot> allSlots_ = new List<Slot>();

        for(int i = 0; i < allSlots.Count; i++)
        {
            allSlots_.Add(allSlots[i]);
        }

        for(int i = 0; i < inventorySize; i++)
        {
            Slot slot = Instantiate(slotTemplate, contentHolder).GetComponent<Slot>();

            inventorySlots_.Add(slot);
            allSlots_.Add(slot);
        }

        inventorySlots = inventorySlots_;
        allSlots = allSlots_;

    }

    private void GenerateHotbarSlots()
    {
        List<Slot> inventorySlots_ = new List<Slot>();
        List<Slot> allSlots_ = new List<Slot>();
        List<Slot> hotbarSlots_ = new List<Slot>();

        for(int i = 0; i < allSlots.Count; i++)
        {
            allSlots_.Add(allSlots[i]);
        }

        for(int i = 0; i < hotbarSize; i++)
        {
            Slot slot = Instantiate(slotTemplate, hotbarContentHolder).GetComponent<Slot>();

            inventorySlots_.Add(slot);
            allSlots_.Add(slot);
            hotbarSlots_.Add(slot);
        }

        inventorySlots = inventorySlots_;
        allSlots = allSlots_;
        hotbarSlots = hotbarSlots_;

    }

    public void DragDrop(Slot from, Slot to)
    {
        if(from.data != to.data)
        {
            ItemSO data = to.data;
            int stackSize = to.stackSize;

            to.data = from.data;
            to.stackSize = from.stackSize;

            from.data = data;
            from.stackSize = stackSize;
        }
        else
        {
            if(from.data.isStackable)
            {
                if(from.stackSize + to.stackSize > from.data.maxStack)
                {
                    int amountLeft = (from.stackSize + to.stackSize) - from.data.maxStack;
                    from.stackSize = amountLeft;
                    to.stackSize = to.data.maxStack;
                }
            }
            else
            {
                ItemSO data = to.data;
                int stackSize = to.stackSize;

                to.data = from.data;
                to.stackSize = from.stackSize;

                from.data = to.data;
                from.stackSize = to.stackSize;
            }
        }
        from.UpdateSlot();
        to.UpdateSlot();

       
    }

    public void AddItem(Pickup pickup)
    {
        if(pickup.data.isStackable)
        {
            Slot stackableSlot = null;

            for(int i = 0; i < inventorySlots.Count; i++)
            {
                if(!inventorySlots[i].IsEmpty)
                {
                    if(inventorySlots[i].data == pickup.data && inventorySlots[i].stackSize < pickup.data.maxStack)
                    {
                        stackableSlot = inventorySlots[i];
                        break;
                    }
                }
            }

            if(stackableSlot != null)
            {
                if(stackableSlot.stackSize + pickup.stackSize > pickup.data.maxStack)
                {
                    int amountLeft = (stackableSlot.stackSize + pickup.stackSize) - pickup.data.maxStack;

                    stackableSlot.AddItemToSlot(pickup.data, pickup.data.maxStack);

                    for(int i = 0; i < inventorySlots.Count; i++) 
                    {
                        if(inventorySlots[i].IsEmpty)
                        {
                            inventorySlots[i].AddItemToSlot(pickup.data, amountLeft);
                            inventorySlots[i].UpdateSlot();
                            break;
                        }
                    }

                    Destroy(pickup.gameObject);
                }
                else
                {
                    stackableSlot.AddStackAmount(pickup.stackSize);

                    Destroy(pickup.gameObject);
                }

                stackableSlot.UpdateSlot();
            }
            else
            {
                Slot emptySlot = null;

                for(int i = 0; i < inventorySlots.Count; i++)
                {
                    if(inventorySlots[i].IsEmpty)
                    {
                        emptySlot = inventorySlots[i];
                        break;
                    }
                }

                if(emptySlot != null)
                {
                    emptySlot.AddItemToSlot(pickup.data, pickup.stackSize);
                    emptySlot.UpdateSlot();

                    Destroy(pickup.gameObject);
                }
                else
                {
                    pickup.transform.position = dropPos.position;
                }

            }
        }
        else
        {
            Slot emptySlot = null;

            for(int i = 0; i < inventorySlots.Count; i++)
            {
                if(inventorySlots[i].IsEmpty)
                {
                    emptySlot = inventorySlots[i];
                    break;
                }
            }

            if(emptySlot != null)
            {
                emptySlot.AddItemToSlot(pickup.data, pickup.stackSize);
                emptySlot.UpdateSlot();

                Destroy(pickup.gameObject);
            }
            else
            {
                pickup.transform.position = dropPos.position;
            }
        }
    }

    public void DropItem(Slot slot)
    {
        Pickup pickup = Instantiate(dropModel, dropPos).AddComponent<Pickup>();
        pickup.transform.position = dropPos.position;
        pickup.transform.SetParent(null);


        
        pickup.data = slot.data;
        pickup.stackSize = slot.stackSize;

        slot.Clean();
    }
}

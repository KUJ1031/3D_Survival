using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPostion;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPostion = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemdata;

        // ������ �ߺ� �������� canStack
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                // ���Կ� ������ �߰�
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemdata = null;
                return;
            }
        }

        // ����ִ� ���� ��������
        ItemSlot emptySlot = GetEmptySlot();

        // �ִٸ�
        if (emptySlot != null)
        {
            // ���Կ� ������ �߰�
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemdata = null;
            return;
        }
        ThrowItem(data);
        CharacterManager.Instance.Player.itemdata = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPostion.position, Quaternion.Euler(Vector3.one * Random.value * 360)); ;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.cousumables.Length; i++)
        {
            selectedStatName.text += selectedItem.cousumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.cousumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.cousumables.Length; i++)
            {
                switch (selectedItem.cousumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.cousumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.cousumables[i].value);
                        break;
                    case ConsumableType.SpeedUp:
                        Debug.Log("��� ���");
                        condition.TempororySpeedUp(selectedItem.cousumables[i].value);
                        break;
                    case ConsumableType.DoubleJump:
                        Debug.Log("�ٳ��� ���");
                        condition.CheckDoubleJumpEnable(selectedItem.cousumables[i].value);
                        break;
                    case ConsumableType.Invincibility:
                        Debug.Log("��Ű ���");
                        condition.TempororyInvincibility(selectedItem.cousumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        if (selectedItem.type == ItemType.Equipable)
        {
            for (int i = 0; i < selectedItem.equipables.Length; i++)
            {
                switch (selectedItem.equipables[i].type)
                {
                    case EquipableType.SpeedUp:
                        condition.PermanentSpeedUp(selectedItem.equipables[i].value);
                        break;
                    case EquipableType.JumpPowerUp:
                        condition.PermanentJumpPowerUp(selectedItem.equipables[i].value);
                        break;
                }
            }
        }
        UpdateUI();
        SelectItem(selectedItemIndex);

    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();

        if (selectedItem.type == ItemType.Equipable)
        {
            for (int i = 0; i < selectedItem.equipables.Length; i++)
            {
                switch (selectedItem.equipables[i].type)
                {
                    case EquipableType.SpeedUp:
                        condition.ResetSpeed(); // ���� �ӵ��� �ǵ����� �޼��尡 �ʿ�
                        break;
                    case EquipableType.JumpPowerUp:
                        condition.ResetJumpPower(); // ������ �ʱ�ȭ�� ��������
                        break;
                }
            }
        }
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

}

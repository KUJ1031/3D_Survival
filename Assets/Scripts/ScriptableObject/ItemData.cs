using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Hunger,
    SpeedUp,
    DoubleJump,
    Invincibility
}

public enum EquipableType
{
    SpeedUp,
    JumpPowerUp
}

[System.Serializable] public class ItemDataCousumable
{
    public ConsumableType type;
    public float value;
}

[System.Serializable] public class ItemDataEquipableType
{
    public EquipableType type;
    public float value;
}

[CreateAssetMenu(fileName = "NewItem", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount = 1;

    [Header("Consumable")]
    public ItemDataCousumable[] cousumables;

    [Header("Equipable")]
    public ItemDataEquipableType[] equipables;

    [Header("Equip")]
    public GameObject equipPrefab;
}

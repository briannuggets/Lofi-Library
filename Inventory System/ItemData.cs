using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class of variables attributed to every item
[CreateAssetMenu(menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public string type;
    public int musicVolume;
    public bool draggable;
    public int cost;
    public int sellAmount;
    public Texture2D icon;
    public string itemDescription;
    public string tip;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object / Item", order = int.MaxValue)]
public class Scriptable_Item : ScriptableObject
{
    public enum tag_item
    {
        equipment,
        Expendables
    }

    [SerializeField]
    private int id = 0;
    public int Id { get { return id; } }

    [SerializeField]
    private string item_name = null;
    public string Name { get { return item_name; } }

    [SerializeField]
    private int gold = 0;
    public int Gold { get { return gold; } }


    [SerializeField]
    private Sprite image = null;
    public Sprite Image { get { return image; } }

    [SerializeField]
    private int idCraft1 = 0, idCraft2 = 0, idCraft3 = 0;

    public int IdCraft1 { get { return idCraft1; } }
    public int IdCraft2 { get { return idCraft2; } }
    public int IdCraft3 { get { return idCraft3; } }

    
    [SerializeField]
    private int itemforcraft = 0;
    public int ItemForCraft { get { return itemforcraft; } }

    [TextArea]
    [SerializeField]
    private string infomation = null;
    public string Info { get { return infomation; } }

    public tag_item itemTag;

    [SerializeField]
    private int OverlapCount;
    public int Overlap {  get { return OverlapCount; } }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragContainer : MonoBehaviour
{
    public Scriptable_Item DragItem;

    public Image image;
    public int count;
    // Start is called before the first frame update
    void Start()
    {
        DragItem = null;
        gameObject.SetActive(false);
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

}

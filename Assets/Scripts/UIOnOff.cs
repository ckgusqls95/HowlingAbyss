using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnOff : MonoBehaviour
{
    private Transform[] Interface;
    // Start is called before the first frame update
    void Start()
    {
        int childCount = this.transform.childCount;

        Interface = new Transform[childCount];


        for(int i  = 0; i < childCount; i++)
        {
            Interface[i] = this.transform.GetChild(0);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            int index = SearchChild("Shop");
            if (Interface[index].gameObject.activeInHierarchy == true)
            {
                Interface[index].gameObject.SetActive(false);
            }
            else
            {
                Interface[index].gameObject.SetActive(true);
            }
        }

    }

    int SearchChild(string name)
    {
        int index = int.MinValue;

        for(int i = 0; i < Interface.Length; i++)
        {
            if(Interface[i].transform.name.Equals(name))
            {
                index = i;
                break;
            }
        }


        return index;
    }
}

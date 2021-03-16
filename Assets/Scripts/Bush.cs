using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bush : MonoBehaviour
{
    List<Transform> insideBush;

    private void OnTriggerEnter(Collider other)
    {
        {
            PhotonView pv = other.transform.gameObject.GetComponent<PhotonView>();
            if (pv == null) return;

            if(pv.IsMine)
            {
                Material mtl = other.transform.GetComponent<Renderer>().material;
                Color color = mtl.color;
                color.a = 0.0f;
                mtl.color = color;
                other.transform.GetComponent<Renderer>().material = new Material(mtl);
            }
            else
            {
                other.transform.GetComponent<MeshRenderer>().enabled = false;
            }

        }

        insideBush.Add(other.transform);
    }


    private void OnTriggerExit(Collider other)
    {     
        if(insideBush.Contains(other.transform))
        {
            other.transform.GetComponent<MeshRenderer>().enabled = true;
            
            Material mtl = other.transform.GetComponent<Renderer>().material;
            Color color = mtl.color;
            color.a = 1.0f;
            mtl.color = color;
            other.transform.GetComponent<Renderer>().material = new Material(mtl);
        }

        
    }
}

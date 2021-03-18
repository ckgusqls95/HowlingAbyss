using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bush : MonoBehaviour
{
    List<Champion> insideBush = new List<Champion>();
    Dictionary<Champion, ParticleSystem[]> Partices = new Dictionary<Champion, ParticleSystem[]>();
    const int RED = 1;
    const int BLUE = 2;

    private void FixedUpdate()
    {
        int count = 0;
        for (int i = 0; i < insideBush.Count; i++)
        {
            if(insideBush[i].transform.CompareTag("Red"))
            {
                if(count == BLUE)
                {
                    count += RED;
                    break;
                }
                else if(count != RED)
                {
                    count = RED;
                }
                
            }
            else if(insideBush[i].transform.CompareTag("Blue") && count != BLUE)
            {
                if(count == RED)
                {
                    count += BLUE;
                    break;
                }
                else if(count != BLUE)
                {
                    count = BLUE;
                }
            }
        }

        if(count >= RED + BLUE)
        {
            foreach (Champion obj in insideBush)
            {
                SkinnedMeshRenderer renderer = obj.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                Material mtl = renderer.material;
                Color color = mtl.color;
                color.a = 0.3f;
                mtl.color = color;
                renderer.material = new Material(mtl);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Champion>(out Champion script))
        {
            PhotonView pv = other.transform.gameObject.GetComponent<PhotonView>();
            if (pv == null) return;

            if(pv.IsMine)
            {
                SkinnedMeshRenderer renderer = other.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                Material mtl = renderer.material;
                Color color = mtl.color;
                color.a = 0.3f;
                mtl.color = color;
                renderer.material = new Material(mtl);
            }
            else
            {
                other.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }

            ParticleSystem[] ps = script.GetComponentsInChildren<ParticleSystem>();
            if(ps.Length > 0)
            {
                Partices.Add(script, ps);
                foreach(var particle in ps)
                {
                    particle.gameObject.SetActive(false);
                }
            }

            insideBush.Add(script);
        }

    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Champion>(out Champion script))
        {
            if (insideBush.Contains(script))
            {
                SkinnedMeshRenderer renderer = other.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                renderer.enabled = true;
                Material mtl = renderer.material;
                Color color = mtl.color;
                color.a = 1.0f;
                mtl.color = color;
                renderer.material = new Material(mtl);
                insideBush.Remove(script);

                if (Partices.ContainsKey(script))
                {
                    ParticleSystem[] ps = Partices[script];
                    for (int i = 0; i < ps.Length; i++)
                    {
                        ps[i].gameObject.SetActive(true);
                    }
                }
            }
        }
        
    }

    
}

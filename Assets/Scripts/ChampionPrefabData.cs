using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Champion Prefab Data", menuName = "Scriptable Object/Champion Prefab Data", order = int.MaxValue)]
[System.Serializable]

public class ChampionPrefabData : ScriptableObject
{
    [SerializeField]
    private GameObject ahriPrefab;
    public GameObject AhriPrefab { get { return ahriPrefab; } }

    [SerializeField]
    private GameObject leeSinPrefab;
    public GameObject LeeSinPrefab { get { return leeSinPrefab; } }

    [SerializeField]
    private GameObject lucianPrefab;
    public GameObject LucianPrefab { get { return lucianPrefab; } }

}

using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ListofBlocks", menuName = "Scriptable Objects/ListofBlocks")]
public class ListofBlocks : ScriptableObject
{



    [SerializeField] public List<GameObject> blockPrefabs;


    public GameObject GetRandomBlock()
    {
        if (blockPrefabs == null || blockPrefabs.Count == 0)
        {
            Debug.LogWarning("Block prefabs list is empty!");
            return null;
        }

        int randomIndex = Random.Range(0, blockPrefabs.Count);
        return blockPrefabs[randomIndex];
    }

}

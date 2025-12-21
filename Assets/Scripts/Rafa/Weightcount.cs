using System.Collections.Generic;
using UnityEngine;

public class Weightcount : MonoBehaviour
{
   
   public List<GameObject> belowBlocks ;    
   public int weightCount;

    void Awake()
    {
        weightCount=0;
        belowBlocks = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void dropWeight(int aboveWeight)
    {
        weightCount = aboveWeight+1;

        for(int i = 0; i < belowBlocks.Count; i++)
        {
            if(belowBlocks[i].gameObject!=null)
            {
                Weightcount belowWeightcount = belowBlocks[i].GetComponent<Weightcount>();
                if(belowWeightcount != null)
                {
                    belowWeightcount.dropWeight(weightCount);
                }
            }
        }
        
    }
}

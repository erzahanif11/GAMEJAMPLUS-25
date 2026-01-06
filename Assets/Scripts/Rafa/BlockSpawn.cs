using System;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{


    [SerializeField] ListofBlocks normal;
    [SerializeField] float wallRight = 12.55857f; // right boundary for spawning (float)
    [SerializeField] float wallLeft = -10.44143f; // left boundary for spawning (float)

    public static BlockSpawn instance;
    public float spawnInterval = 1.5f;
    [SerializeField] GameObject player;

    public string DebugBlockName;
     private
    void Awake()
    {
        instance = this;
    }


float curtime=0;

void Start(){
    SpawnBlock();
}
    void Update()
    {
        curtime+=Time.deltaTime;
        if (curtime >= spawnInterval)
        {
            SpawnBlock();
            curtime=0;
        }
    }
GameObject blockToSpawn ;
    void SpawnBlock()
    {
        if (DebugBlockName != null)
         blockToSpawn=  normal.blockPrefabs.Find(block => block.name == DebugBlockName);
        
        if (blockToSpawn == null)
         blockToSpawn = normal.GetRandomBlock();

    
        if (blockToSpawn != null)
        {
            // Parse width from block name (e.g., "BlockW3" -> width = 3)
            int width = 1; // default
            string blockName = blockToSpawn.name;
            int wIndex = blockName.LastIndexOf('W');
            if (wIndex >= 0 && wIndex < blockName.Length - 1)
            {
                if (int.TryParse(blockName.Substring(wIndex + 1), out int parsedWidth))
                    width = parsedWidth;
            }

            // Option B adapted for float walls: spawn points at wallLeft + k for integer k
            // Compute span in integer steps (number of 1.0 increments between walls)
            int span = Mathf.FloorToInt(wallRight - wallLeft);
            if (span < 0) span = 0;

            // maximum k so that (wallLeft + k) + width <= wallRight
            int maxK = Mathf.Max(0, span - width);

            int k;
            //spawn random near player 10% of the time
            if (player != null && UnityEngine.Random.value < 0.1f)
            {
                // spawn near player: pick nearest grid column to player
                float rel = player.transform.position.x - wallLeft;
                int playerK = Mathf.RoundToInt(rel);
                k = Mathf.Clamp(playerK, 0, maxK);
            }
            // otherwise random
            else
            {
                // random column
                k = UnityEngine.Random.Range(0, maxK + 1);
            }

            float fx = wallLeft + k; // final spawn X (already includes fractional part from wallLeft)

            // clamp final X to ensure block stays within float walls
            float minFx = wallLeft;
            float maxFx = wallRight - width;
            if (maxFx < minFx) maxFx = minFx;
            fx = Mathf.Clamp(fx, minFx, maxFx);

            Vector2 position = new Vector2(fx, transform.position.y);
            Instantiate(blockToSpawn, position, blockToSpawn.transform.rotation);
        }
    }


}


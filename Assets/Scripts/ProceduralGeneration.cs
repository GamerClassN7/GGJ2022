using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public List<GameObject> levelBlocks = new List<GameObject>();
    public List<GameObject> spawnedLevelBlocks = new List<GameObject>();
    public GameObject player = null;
    public GameObject lastBlock;
    private GameObject lastBlockPrefab;
    private int spavnetobjectIndex = 0;
    private int maximumNumberOfPlatformsAtScene = 100;
    private float maximumDistanceOfPlatformFromPlayer = 20.0f;



    GameObject drawPlatform(GameObject lastObject, GameObject objToSpawn)
    {
        MeshFilter meshfilter = lastObject.GetComponent<MeshFilter>();
        Bounds bounds = meshfilter.mesh.bounds;

        float scale = meshfilter.transform.localScale.x;
        Bounds b = new Bounds(bounds.center * scale, bounds.size * scale);

        Vector3 nextBlockLocation = new Vector3(lastObject.transform.position.x, lastObject.transform.position.y, lastObject.transform.position.z + b.size.z + 1.0f);

        return Instantiate(objToSpawn, nextBlockLocation, (Quaternion.identity));
    }

    List<GameObject> spawnSpiralOfPlatforms(GameObject lastObject, GameObject objToSpawn)
    {
        // configuration:
        float horizontalDistancePerPlatform = 0.5f;

        List<GameObject> levelBlocksSpawnTemp = new List<GameObject>();
        Debug.Log("Building LOOP");

        int pieceCount = 10;
        float radius = (pieceCount / 2) * 2;
        float angle = 360f / (float)pieceCount;

        MeshFilter meshfilter = lastObject.GetComponent<MeshFilter>();
        Bounds bounds = meshfilter.mesh.bounds;

        float scale = meshfilter.transform.localScale.x;
        Bounds b = new Bounds(bounds.center * scale, bounds.size * scale);

        Vector3 centerPoint = new Vector3(lastObject.transform.position.x, (lastObject.transform.position.y + radius), this.lastBlock.transform.position.z + b.size.z + 1.0f);

        float heightOffset = radius;

        for (int i = 1; i < pieceCount + 2; i++)
        {
            Quaternion rotation = (Quaternion.AngleAxis((i - 1) * angle, Vector3.back));
            Vector3 direction = rotation * Vector3.down;
            Vector3 position = (lastObject.transform.position + (direction * radius));

            levelBlocksSpawnTemp.Add(Instantiate(objToSpawn, new Vector3(position.x, position.y + heightOffset, position.z + (float)(i * horizontalDistancePerPlatform)), rotation));
        }

        return levelBlocksSpawnTemp;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastBlockPrefab = this.gameObject.transform.GetChild(0).gameObject;
        lastBlock = this.gameObject.transform.GetChild(0).gameObject;
        this.spawnedLevelBlocks.Add(lastBlock);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = this.player.transform.position;

        for (var i = 0; i < this.spawnedLevelBlocks.Count; i++)
        {
            float distance = Vector3.Distance(this.spawnedLevelBlocks[i].transform.position, playerPosition);
            if (distance > this.maximumDistanceOfPlatformFromPlayer && this.spawnedLevelBlocks.Count >= this.maximumNumberOfPlatformsAtScene)
            {
                Destroy(this.spawnedLevelBlocks[i]);
                this.spawnedLevelBlocks.Remove(this.spawnedLevelBlocks[i]);
                spavnetobjectIndex++;
            }
            else
            {
                break;
            }
        }


        if (this.spawnedLevelBlocks.Count <= this.maximumNumberOfPlatformsAtScene)
        {
            int blockToSpawn = Random.Range(0, levelBlocks.Count);

            GameObject instantiatedGameObject;
            GameObject blockObjToSpawn;

            blockObjToSpawn = levelBlocks[blockToSpawn];
            if (blockObjToSpawn.name == lastBlockPrefab.name)
            {
                Debug.Log("Same Block");
                if (blockToSpawn < levelBlocks.Count || blockToSpawn > -1)
                {
                    blockToSpawn = Random.Range(0, (levelBlocks.Count - 1));
                }
            }

            if ((blockToSpawn > -1 && (blockToSpawn < (levelBlocks.Count - 1))))
            {
                instantiatedGameObject = this.drawPlatform(this.lastBlock, this.levelBlocks[blockToSpawn]);
                this.spawnedLevelBlocks.Add(instantiatedGameObject);

            }
            else
            {
                List<GameObject> instantiatedGameObjectLists = this.spawnSpiralOfPlatforms(lastBlock, levelBlocks[0]);
                foreach (var spavnedBlock in instantiatedGameObjectLists)
                {
                    this.spawnedLevelBlocks.Add(spavnedBlock);
                }
                instantiatedGameObject = this.spawnedLevelBlocks[this.spawnedLevelBlocks.Count - 1];
                blockObjToSpawn = levelBlocks[0];
            }

            this.lastBlock = instantiatedGameObject;
            this.lastBlockPrefab = blockObjToSpawn;

        }
    }
}


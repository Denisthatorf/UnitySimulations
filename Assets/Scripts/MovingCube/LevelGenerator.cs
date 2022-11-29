using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private class Chunk
    {
        public GameObject road;
        public List<GameObject> barriers;
        public List<GameObject> checkPoints;
    }

    private const int CHUNK_NUM = 3;
    private const float ROAD_FROM_PLAYER_OFFSET = 2.0f;
    private const int CHECKPOINT_FOR_NEXT_CHUNK = 10;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject barrierPrefab;
    [SerializeField] private GameObject checkPointPrefab;
    [SerializeField] private int offset;

    Queue<Chunk> chunks;
    Queue<GameObject> freeToUseBarriers;
    Queue<GameObject> freeToUseCheckPoints;

    private Vector3 startPosition;
    private Vector3 lastSpawnedPosition;
    private Vector3 roadSize;
    private int numOfPassedCheckPoints;

    public void PassCheckPoint()
    {
        if(numOfPassedCheckPoints == CHECKPOINT_FOR_NEXT_CHUNK)
        {
            GenerateNextChunk();
            numOfPassedCheckPoints = 5;
        }
        
        numOfPassedCheckPoints += 1;
    }

    public void Reset()
    {
        numOfPassedCheckPoints = 0;
        lastSpawnedPosition = startPosition;
        for(int i = 0; i < chunks.Count; i++)
        {
            GenerateNextChunk();    
            numOfPassedCheckPoints = 0;
        }
    }

    private void Start()
    {
        if(offset <= 0)
            throw new Exception("Offset between Cubes must be more than 0");

        chunks = new Queue<Chunk>();
        freeToUseBarriers = new Queue<GameObject>();
        freeToUseCheckPoints = new Queue<GameObject>();

        startPosition = player.transform.position 
            - new Vector3(0, ROAD_FROM_PLAYER_OFFSET, 0);
        lastSpawnedPosition = startPosition;

        roadSize = roadPrefab.GetComponent<BoxCollider>().size;

        for(int i = 0; i < CHUNK_NUM; i++)
        {
            Chunk chunk = new Chunk();
            chunk.road = Instantiate(roadPrefab, lastSpawnedPosition, Quaternion.identity);

            chunk.barriers = new List<GameObject>();
            chunk.checkPoints = new List<GameObject>();

            chunks.Enqueue(chunk);

            lastSpawnedPosition.z += roadSize.z;

            Vector3 pos = new Vector3(-100, -100, -100);
            for (int j = 0; j < 2 * ((int)(roadSize.z) / offset); j++)
            {
                var barrier = Instantiate(barrierPrefab, pos, Quaternion.identity);
                var checkPoint = Instantiate(checkPointPrefab, pos, Quaternion.identity);

                freeToUseBarriers.Enqueue(barrier);
                freeToUseCheckPoints.Enqueue(checkPoint);
            }
        }
    }

    private void GenerateNextChunk()
    {
        Chunk chunk = chunks.Dequeue();
        chunk.road.transform.position = lastSpawnedPosition;
        chunks.Enqueue(chunk);

        GenerateEnvironment(chunk);

        lastSpawnedPosition.z += roadSize.z;
    }

    private void GenerateEnvironment(Chunk chunk)
    {
        chunk.barriers.ForEach(obj => freeToUseBarriers.Enqueue(obj));
        chunk.checkPoints.ForEach(obj => freeToUseCheckPoints.Enqueue(obj));

        chunk.barriers.Clear();
        chunk.checkPoints.Clear();

        Vector3 barrierPos = new Vector3
            (
                lastSpawnedPosition.x,
                lastSpawnedPosition.y + roadSize.y,
                lastSpawnedPosition.z + offset
            );

        ////TODO: comment
        for(int i = 0; i < roadSize.z; i += offset)
        {
            Vector3 checkPointPos = barrierPos;
            checkPointPos.z += 1; //TODO: Size Of Block 

            var checkPoint = freeToUseCheckPoints.Dequeue();
            checkPoint.GetComponent<Transform>().position = checkPointPos;

            var barrierWall = GenerateBarrierWall(barrierPos);

            chunk.checkPoints.Add(checkPoint);
            chunk.barriers.AddRange(barrierWall);

            barrierPos.z += offset;
        }
    }

    //TODO: think
    public List<GameObject> GenerateBarrierWall(Vector3 position)
    {
        const float GAPE_SIZE = 3.0f;

        float freePos = UnityEngine.Random.Range(0.0f, roadSize.x - GAPE_SIZE);
        float roadOffset = roadSize.x / 2.0f;
        Vector3 positionBar2 = position;

        List<GameObject> result = new List<GameObject>();

        //TODO: make deffined by road width
        position.x -= roadOffset;
        position.x += freePos / 2.0f;

        var barrier1 = freeToUseBarriers.Dequeue();
        barrier1.GetComponent<Transform>().position = position;
        barrier1.GetComponent<Transform>().localScale = new Vector3(freePos, 1.0f, 1.0f);
        result.Add(barrier1);

        positionBar2.x -= roadOffset;
        positionBar2.x += freePos;
        positionBar2.x += GAPE_SIZE;
        positionBar2.x += (roadSize.x - GAPE_SIZE - freePos) / 2.0f;

        GameObject barrier2 = freeToUseBarriers.Dequeue();
        barrier2.GetComponent<Transform>().position = positionBar2;
        barrier2.GetComponent<Transform>().localScale = new Vector3(roadSize.x - GAPE_SIZE - freePos, 1.0f, 1.0f);
        result.Add(barrier2);

        return result;
    }
}

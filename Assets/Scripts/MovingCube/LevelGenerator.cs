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

    [SerializeField] private Transform player;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject barrierPrefab;
    [SerializeField] private GameObject checkPointPrefab;
    [SerializeField] private int offset;

    Queue<Chunk> chunks;

    private Vector3 startPosition;
    private Vector3 lastSpawnedPosition;
    private Vector3 roadSize;
    private int checkPointsForNextChunk;
    private int numOfPassedCheckPoints;
    private System.Random rand;

    public void PassCheckPoint()
    {
        numOfPassedCheckPoints += 1;
        if(numOfPassedCheckPoints == checkPointsForNextChunk)
        {
            GenerateNextChunk();
            numOfPassedCheckPoints = 0;
        }
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

        //TODO: move to constructor if possible
        chunks = new Queue<Chunk>();
        rand = new System.Random();

        startPosition = player.transform.position 
            - new Vector3(0, ROAD_FROM_PLAYER_OFFSET, 0);
        lastSpawnedPosition = startPosition;

        roadSize = roadPrefab.GetComponent<BoxCollider>().size;
        checkPointsForNextChunk =  (int)(roadSize.z / offset);

        for(int i = 0; i < CHUNK_NUM; i++)
        {
            Chunk chunk = new Chunk();
            chunk.road = Instantiate(roadPrefab, lastSpawnedPosition, Quaternion.identity);

            chunk.barriers = new List<GameObject>();
            chunk.checkPoints = new List<GameObject>();

            chunks.Enqueue(chunk);

            lastSpawnedPosition.z += roadSize.z;
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
        foreach(var barriers in chunk.barriers)
           Destroy(barriers); 
        foreach(var checkPoint in chunk.checkPoints)
            Destroy(checkPoint);

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

            GameObject barrierWall = GenerateBarrierWall(barrierPos);
            GameObject checkPoint = Instantiate(checkPointPrefab, checkPointPos, Quaternion.identity);

            chunk.barriers.Add(barrierWall);
            chunk.checkPoints.Add(checkPoint);

            barrierPos.z += offset;
        }
    }

    //TODO: think
    public GameObject GenerateBarrierWall(Vector3 position)
    {
        //TODO: make deffined by road width
        int freePos = rand.Next(5, 11);

        GameObject barrier1 = Instantiate(barrierPrefab, position, Quaternion.identity);
        barrier1.GetComponent<Transform>().localScale = new Vector3(freePos, 1.0f, 1.0f);

        //Vector3 secondPos =  position;
        //secondPos.x += freePos;
        //GameObject barrier2 = Instantiate(barrierPrefab, secondPos, Quaternion.identity);
        //barrier1.GetComponent<Transform>().localScale = new Vector3(15 - freePos, 1.0f, 1.0f);
        //result.Add(barrier1);

        return barrier1;
    }
}

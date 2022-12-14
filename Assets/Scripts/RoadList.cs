using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadList : MonoBehaviour
{

    [SerializeField] private Transform[] RoadPrefab;
    [SerializeField] private int StartPos = 6;
    [SerializeField] private int RoadWidth = 6;

    [SerializeField] private List<Transform> Roads;
    private Transform Player;

    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerControll>().transform;
        for (int i = 0; i < 20; i++)
        {
            CreateRoad();
        }
    }

    private int id = 0;

    public Transform GetNextRoad()
    {
        id++;
        if (Roads[0].position.z - Player.position.z < -10f)
        {
            ReUseRoad();
            id--;
        }
        return Roads[id - 1];
    }

    public void CreateRoad()
    {
        int rand = Random.Range(0, RoadPrefab.Length);
        Transform road = Instantiate<Transform>(RoadPrefab[rand], new Vector3(0, 0, StartPos), Quaternion.identity, null);
        Roads.Add(road);
        StartPos += RoadWidth;
    }

    public void ReUseRoad()
    {
        Roads.Add(Roads[0]);
        Roads[0].position = new Vector3(0, 0, StartPos);
        StartPos += RoadWidth;
        Roads.RemoveAt(0);
    }

}

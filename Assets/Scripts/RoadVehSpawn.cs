using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadVehSpawn : MonoBehaviour
{
    [Header("On/Off")]
    [SerializeField] private bool On = true;
    [Header("General Options")]
    [SerializeField] private Transform[] listPrefabs;
    [SerializeField] private int SpawnCountsLeftLine = 5;
    [SerializeField] private int SpawnCountsRightLine = 5;
    [SerializeField] private List<Transform> VehiclesLeft;
    [SerializeField] private List<Transform> VehiclesRight;
    [SerializeField] private Transform LeftLine;
    [SerializeField] private Transform RightLine;
    public Transform PeoplePoint_1;
    public Transform PeoplePoint_2;
    [Range(0, 1)]
    [SerializeField] float PeopleChance = 0.5f;
    private Transform tempObj;
    private Vehicle SelectVeh;

    [Header("Detail Options")]
    [SerializeField] private float CooldownStart = 3f;
    private float CooldownNow;
    [SerializeField] private float CooldownDeviation = 1f;
    [SerializeField] private float Speed = 10f;
    [SerializeField] private float SpeedDeviation = 2f;
    Vector3 lstart, lend, rstart, rend;

    private Transform Player;

    private void OnEnable()
    {
        if (Random.Range(0f, 1f) <= PeopleChance)
        { PeoplePoint_1.gameObject.SetActive(true); }
        else
        { PeoplePoint_1.gameObject.SetActive(false); }
        
        if (Random.Range(0f, 1f) <= PeopleChance)
        { PeoplePoint_2.gameObject.SetActive(true); }
        else
        { PeoplePoint_2.gameObject.SetActive(false); }
    }

    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerControll>().transform;
        if (listPrefabs.Length > 0 && On)
        {
            //LEFT
            lstart = LeftLine.GetChild(0).position;
            lend = LeftLine.GetChild(1).position;
            for (int i = 0; i < SpawnCountsLeftLine; i++)
            {
                CreateLeftVeh();
            }

            //Right
            rstart = RightLine.GetChild(0).position;
            rend = RightLine.GetChild(1).position;
            for (int i = 0; i < SpawnCountsRightLine; i++)
            {
                CreateRightVeh();
            }
        }
        CooldownNow = CooldownStart;



    }

    public void ResetLeftVar(Vehicle To)
    {
        lstart = LeftLine.GetChild(0).position;
        lend = LeftLine.GetChild(1).position;
        To.SetVar(Vehicle.Line.Left, lstart, lend);
    }

    public void ResetRightVar(Vehicle To)
    {
        rstart = RightLine.GetChild(0).position;
        rend = RightLine.GetChild(1).position;
        To.SetVar(Vehicle.Line.Right, rstart, rend);
    }

    private void CreateLeftVeh()
    {
        tempObj = Instantiate<Transform>(listPrefabs[Random.Range(0, listPrefabs.Length)], lstart, Quaternion.identity, transform);
        tempObj.GetComponent<Vehicle>().SetVar(Vehicle.Line.Left, lstart, lend);
        tempObj.gameObject.SetActive(false);
        VehiclesLeft.Add(tempObj);
    }

    private void CreateRightVeh()
    {
        tempObj = Instantiate<Transform>(listPrefabs[Random.Range(0, listPrefabs.Length)], rstart, Quaternion.identity, transform);
        tempObj.GetComponent<Vehicle>().SetVar(Vehicle.Line.Right, rstart, rend);
        tempObj.gameObject.SetActive(false);
        VehiclesRight.Add(tempObj);
    }

    private Vehicle GetLeftVehicle()
    {
        for (int i = 0; i < VehiclesLeft.Count; i++)
        {
            if (!(VehiclesLeft[i].gameObject.activeSelf))
            {
                return VehiclesLeft[i].GetComponent<Vehicle>();
            }
        }
        CreateLeftVeh();
        return VehiclesLeft[VehiclesLeft.Count - 1].GetComponent<Vehicle>();
    }

    private Vehicle GetRightVehicle()
    {
        for (int i = 0; i < VehiclesRight.Count; i++)
        {
            if (!(VehiclesRight[i].gameObject.activeSelf))
            {
                return VehiclesRight[i].GetComponent<Vehicle>();
            }
        }
        CreateRightVeh();
        return VehiclesRight[VehiclesRight.Count - 1].GetComponent<Vehicle>();
    }

    void Update()
    {
        if (CooldownNow > 0f)
        {
            CooldownNow -= Time.deltaTime;
        }
        else if (Vector3.Distance(Player.position, transform.position) < 23f && Player.position.z - transform.position.z < 7f && On)
        {
            CooldownNow = Random.Range(CooldownStart - CooldownDeviation, CooldownStart + CooldownDeviation);
            float speed = Random.Range(Speed - SpeedDeviation, Speed + SpeedDeviation);
            if (Random.Range(0, 2) == 0)
            {
                SelectVeh = GetLeftVehicle();
                SelectVeh.gameObject.SetActive(true);
                ResetLeftVar(SelectVeh);
                SelectVeh.StartMove(speed);
            }
            else
            {
                SelectVeh = GetRightVehicle();
                SelectVeh.gameObject.SetActive(true);
                ResetRightVar(SelectVeh);
                SelectVeh.StartMove(speed);
            }


        }



        
    }
}

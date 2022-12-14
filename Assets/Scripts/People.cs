using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour
{

    private PlayerControll Player;
    private bool Ready = false;

    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerControll>();
    }

    private void OnEnable()
    {
        Ready = true;
    }

    void Update()
    {
        if (Ready && transform.position.z - Player.transform.position.z < 1f)
        {
            Ready = false;
            Player.ThrowPicca(transform);
        }
    }
}

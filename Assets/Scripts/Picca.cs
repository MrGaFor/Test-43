using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picca : MonoBehaviour
{

    private Vector3 From;
    private Vector3 To;
    private PlayerControll Player;
    private float TimerMove = 0f;

    public void SetVar(Vector3 from, Vector3 to, PlayerControll player)
    {
        From = from;
        To = to;
        Player = player;
        TimerMove = Vector3.Distance(From, To) / 5f;
    }

    void Update()
    {
        if (TimerMove > 0f)
        {
            transform.position = Vector3.Lerp(Vector3.Lerp(From, Vector3.Lerp(From, To, 0.5f) + Vector3.up, 1f - TimerMove / 1f), Vector3.Lerp(Vector3.Lerp(From, To, 0.5f) + Vector3.up, To, 1f - TimerMove / 1f), 1f - TimerMove / 1f);
            TimerMove -= Time.deltaTime;
        }
        else
        {
            Player.GetMoney();
            Destroy(gameObject);
        }
    }
}

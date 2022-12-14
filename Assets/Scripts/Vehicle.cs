using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{

    public enum Line { Left, Right };
    [SerializeField] private Line OnLine;
    [SerializeField] private Vector3 StartPos;
    [SerializeField] private Vector3 FinishPos;
    public bool Moves = false;
    public float Timer = 0f;
    public float TimerAll = 0f;

    /*private void OnEnable()
    {
        if (Parent)
        {
            if (OnLine == Line.Left)
            {
                Parent.ResetLeftVar(this);
            }
            else if (OnLine == Line.Right)
            {
                Parent.ResetRightVar(this);
            }
        }
    }*/

    public void SetVar(Line line, Vector3 start, Vector3 end)
    {
        //Line Pos Set
        StartPos = start;
        FinishPos = end;
        OnLine = line;
    }

    public void StartMove(float speed)
    {
        transform.position = StartPos;
        transform.LookAt(FinishPos, Vector3.up);
        TimerAll = speed;
        Timer = speed;
        Moves = true;
    }


    private void FixedUpdate()
    {
        if (Moves && Timer > 0f)
        {
            transform.position = Vector3.Lerp(StartPos, FinishPos, 1 - (Timer / TimerAll));
            Timer -= Time.deltaTime;
        }
        else if (Moves)
        {
            transform.position = StartPos;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerControll>())
        {
            collision.gameObject.GetComponent<PlayerControll>().Dead();
            Vector3 force = (transform.position - collision.transform.position) * -250;
            force.y = 0;
            collision.rigidbody.AddForce( force, ForceMode.Impulse);
        }
    }

}

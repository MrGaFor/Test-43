using UnityEngine;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour
{
    public enum StatePlayer { Live, Dead };
    [Header("General")]
    public StatePlayer State = StatePlayer.Live;
    [SerializeField] private float Speed = 1f;
    private Slider SpeedSlider;
    [SerializeField] private Transform Picca;

    [Header("Timer & Speed")]
    [SerializeField] private bool TimerStepSet = true;
    [SerializeField] private float MaxSpeed = 5f;
    private float TimerStep = 0f;
    [SerializeField] private Image Fill;

    private bool _start = false;
    [Header("Out")]
    [SerializeField] private Text SpeedCount;
    [SerializeField] private Text Coins;
    private RoadList Roads;
    private float offset;

    [Header("Debug Value")]
    [SerializeField] private float Money = 0f;

    private void Start()
    {
        SpeedSlider = GameObject.FindObjectOfType<SpeedSlider>().GetComponent<Slider>();
        offset = transform.position.y;
        Roads = GameObject.FindObjectOfType<RoadList>();
    }

    public void StartGame()
    {
        TimerStep = MaxSpeed - Speed;
        _start = true;
    }

    private bool StartMove = false;
    private float TimerMove = 0f;
    private Vector3 MoveFrom;
    private Vector3 MoveTo;

    public void Step()
    {
        if (!StartMove && State == StatePlayer.Live && _start)
        {
            TimerStep = MaxSpeed - Speed;
            MoveFrom = transform.position;
            MoveTo = Roads.GetNextRoad().position - new Vector3(0, -offset, 3);
            TimerMove = 1f;
            StartMove = true;
        }
    }

    private void FixedUpdate()
    {

        // Speed Change & Timer Step
        if (_start && State == StatePlayer.Live)
        {
            if (SpeedSlider.value != 0.5f)
            {
                Speed = Mathf.Clamp((SpeedSlider.value - 0.5f) * 0.05f + Speed, 0.1f, MaxSpeed - 0.5f);
                SpeedCount.text = "x" + Speed.ToString("0.0");
            }
            if (TimerStepSet)
            {
                if (TimerStep > 0f && !StartMove) // Timer for Step
                {
                    TimerStep -= Time.deltaTime;
                    Fill.fillAmount = TimerStep / (MaxSpeed - Speed);
                }
                else if (TimerStep <= 0f) // Time Out!
                {
                    Dead();
                }
            }
        }
    }

    private void LateUpdate()
    {
        // Move
        if (StartMove)
        {
            if (TimerMove > 0f)
            {
                transform.position = Vector3.Lerp(Vector3.Lerp(MoveFrom, Vector3.Lerp(MoveFrom, MoveTo, 0.5f) + Vector3.up, 1f - TimerMove / 1f), Vector3.Lerp(Vector3.Lerp(MoveFrom, MoveTo, 0.5f) + Vector3.up, MoveTo, 1f - TimerMove / 1f), 1f - TimerMove / 1f);
                float rotateIndex;
                if (TimerMove > 0.5f)
                {
                    rotateIndex = 1f - ((TimerMove * 2f) - 1f) / 1f;
                }
                else
                {
                    rotateIndex = (TimerMove * 2f) / 1f;
                }
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(-30, 0, 0), rotateIndex);
                TimerMove -= Time.deltaTime;
            }
            else
            {
                TimerMove = 0f;
                StartMove = false;
            }
        }
    }

    public void ThrowPicca(Transform To)
    {
        Transform picca = GameObject.Instantiate<Transform>(Picca, transform.position, Quaternion.identity, null);
        picca.GetComponent<Picca>().SetVar(transform.position + new Vector3(0, 0, 0.3f), To.position, this);
    }

    public void GetMoney()
    {
        Money += Mathf.Round(Speed * 10) / 10;
        Coins.text = Money.ToString("0.0") + " Coins";
    }

    private bool once = true;
    public void Dead()
    {
        if (once)
        {
            once = false;
            StartMove = false;
            State = StatePlayer.Dead;
            GameObject.FindObjectOfType<CameraFollow>().enabled = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameObject.FindObjectOfType<StartTimer>().SetData(Money);
        }
    }

}

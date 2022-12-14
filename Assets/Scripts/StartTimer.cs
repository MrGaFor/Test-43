using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartTimer : MonoBehaviour
{

    [SerializeField] private Text TextCount;

    [SerializeField] private float timer = 3f;
    [SerializeField] private Text StartText;
    [SerializeField] private SaveData Data;
    private Color tempColor;
    private float anim = 0f;
    private float value;
    private PlayerControll Player;
    private Image Me;
    private float DarkOffTimer = 0.2f;

    [SerializeField] private GameObject[] listActive;

    private bool start = false;

    void Start()
    {
        start = false;
        Me = GetComponent<Image>();
        tempColor = Me.color;
        Player = GameObject.FindObjectOfType<PlayerControll>();
        timer += 1f;
        LoadData();
    }

    public void LoadData()
    {
        listActive[2].GetComponent<Text>().text = "All Coins: " + Data.AllMoney.ToString() + " coins";
        listActive[1].GetComponent<Text>().text = "Record: " +Data.Record.ToString() + " coins";
    }

    private bool once = true;
    public void SetData(float coins)
    {
        Data.AllMoney += coins;
        if (Data.Record < coins)
            Data.Record = coins;

        LoadData();
        TextCount.gameObject.SetActive(false);
        for (int i = 0; i < listActive.Length; i++)
        {
            listActive[i].SetActive(true);
        }
        StartText.text = "RESTART";
        Me.enabled = true;
        Me.color = tempColor;
    }

    public void SetStart()
    {
        if (once)
        {
            for (int i = 0; i < listActive.Length; i++)
            {
                listActive[i].SetActive(false);
            }
            start = true;
            once = false;
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    void Update()
    {
        if (start)
        {
            TextCount.gameObject.SetActive(true);
            if (anim > 0f)
            {
                anim -= Time.deltaTime;
                value = 1f - anim;
                TextCount.transform.localScale = new Vector3(value, value, value);
            }
            else
            {
                if (timer > 0f)
                {
                    TextCount.transform.localScale = Vector3.zero;
                    timer--;
                    if (timer < 1f)
                    {
                        TextCount.text = "GO";
                    }
                    else
                    {
                        TextCount.text = timer.ToString("0");
                        anim = 1f;
                    }
                }
                else
                {
                    if (DarkOffTimer > 0f)
                    {
                        DarkOffTimer -= Time.deltaTime;
                        Me.color = Color.Lerp(Me.color, Color.clear, DarkOffTimer / 0.2f);
                    }
                    else
                    {
                        Player.StartGame();
                        Me.enabled = false;
                        start = false;
                    }
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    private Slider Me;
    private float Speed = 10f;

    void Start()
    {
        Me = GetComponent<Slider>();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0) && Me.value != 0.5f)
        {
            Me.value = Mathf.Lerp(Me.value, 0.5f, Time.deltaTime * Speed);
        }
    }
}

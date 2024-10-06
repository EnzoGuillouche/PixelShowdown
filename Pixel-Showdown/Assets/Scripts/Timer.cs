using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timer = 60.0f;
    public int time;
    void Update(){
        if (timer > 0){
            timer -= Time.deltaTime;
        }
        else {
            timerText.color = Color.red;
        }
        time = (int)(timer % 60);
        timerText.text = time.ToString();
    }
}

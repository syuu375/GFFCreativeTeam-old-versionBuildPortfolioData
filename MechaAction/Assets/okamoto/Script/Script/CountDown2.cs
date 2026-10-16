using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class CountDown2 : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TimerUI_1;

    public GameObject CircularUI_1;
    public Image CooldownImage;

    private float cooldownTime = 3f;
    private float timer = 0f;
    private bool isCooling = false;

    private void Start()
    {
        CircularUI_1.SetActive(false);
        CooldownImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && !isCooling)
        {
            CircularUI_1.SetActive(true);
            timer = cooldownTime;
            isCooling = true;
            CooldownImage.fillAmount = 1f;
        }

        if (isCooling)
        {
            timer -= Time.deltaTime;
            CooldownImage.fillAmount = timer / cooldownTime;

            if (timer <= 0f)
            {
                isCooling = false;
                CircularUI_1.SetActive(false);
                CooldownImage.fillAmount = 0f;
            }
        }
        TimerUI_1.GetComponent<TextMeshProUGUI>().text = timer.ToString("F1");
    }
}

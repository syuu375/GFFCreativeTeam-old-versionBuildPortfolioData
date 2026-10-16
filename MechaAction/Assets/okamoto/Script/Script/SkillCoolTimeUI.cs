using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCoolTimeUI : MonoBehaviour
{
    [SerializeField] private GameObject GroundCircularUI;
    [SerializeField] private TextMeshProUGUI GroundTimerUI;
    [SerializeField] public Image GroundCoolImage;

    private float GroundcoolTime;
    private float Groundtimer;
    private bool isGroundCooling = false;

    [SerializeField] private GameObject SlashCircularUI;
    [SerializeField] private TextMeshProUGUI SlashTimerUI;
    [SerializeField] public Image SlashCoolImage;

    private float SlashcoolTime;
    private float Slashtimer;
    private bool isSlashCooling = false;

    [SerializeField] private GameObject ShotgunCircularUI;
    [SerializeField] private TextMeshProUGUI ShotgunTimerUI;
    [SerializeField] public Image ShotgunCoolImage;

    private float ShotguncoolTime;
    private float Shotguntimer;
    private bool isShotgunCooling = false;

    [SerializeField] private GameObject RifleCircularUI;
    [SerializeField] private TextMeshProUGUI RifleTimerUI;
    [SerializeField] public Image RifleCoolImage;

    private float RiflecoolTime;
    private float Rifletimer;
    private bool isRifleCooling = false;

    private void Start()
    {
        GroundCircularUI.SetActive(false);
        GroundCoolImage.fillAmount = 0f;

        SlashCircularUI.SetActive(false);
        SlashCoolImage.fillAmount = 0f;

        ShotgunCircularUI.SetActive(false);
        ShotgunCoolImage.fillAmount = 0f;

        RifleCircularUI.SetActive(false);
        RifleCoolImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (isGroundCooling)
        {
            Groundtimer -= Time.deltaTime;
            GroundCoolImage.fillAmount = Groundtimer / GroundcoolTime;

            if (Groundtimer <= 0f)
            {
                isGroundCooling = false;
                GroundCircularUI.SetActive(false);
                GroundCoolImage.fillAmount = 0f;
            }
        }

        if (isSlashCooling)
        {
            Slashtimer -= Time.deltaTime;
            SlashCoolImage.fillAmount = Slashtimer / SlashcoolTime;

            if (Slashtimer <= 0f)
            {
                isSlashCooling = false;
                SlashCircularUI.SetActive(false);
                SlashCoolImage.fillAmount = 0f;
            }
        }

        if (isShotgunCooling)
        {
            Shotguntimer -= Time.deltaTime;
            ShotgunCoolImage.fillAmount = Shotguntimer / ShotguncoolTime;

            if (Shotguntimer <= 0f)
            {
                isShotgunCooling = false;
                ShotgunCircularUI.SetActive(false);
                ShotgunCoolImage.fillAmount = 0f;
            }
        }

        if (isRifleCooling)
        {
            Rifletimer -= Time.deltaTime;
            RifleCoolImage.fillAmount = Rifletimer / RiflecoolTime;

            if (Rifletimer <= 0f)
            {
                isRifleCooling = false;
                RifleCircularUI.SetActive(false);
                RifleCoolImage.fillAmount = 0f;
            }
        }
        GroundTimerUI.GetComponent<TextMeshProUGUI>().text = Groundtimer.ToString("F1");
        SlashTimerUI.GetComponent<TextMeshProUGUI>().text = Slashtimer.ToString("F1");
        ShotgunTimerUI.GetComponent<TextMeshProUGUI>().text = Shotguntimer.ToString("F1");
        RifleTimerUI.GetComponent<TextMeshProUGUI>().text = Rifletimer.ToString("F1");

    }

    public void GroundSkillCoolTime(float time)
    {
        GroundCircularUI.SetActive(true);
        GroundcoolTime = time;
        Groundtimer = GroundcoolTime;
        isGroundCooling = true;
        GroundCoolImage.fillAmount = 1f;
    }

    public void SlashSkillCoolTime(float time)
    {
        SlashCircularUI.SetActive(true);
        SlashcoolTime = time;
        Slashtimer = SlashcoolTime;
        isSlashCooling = true;
        SlashCoolImage.fillAmount = 1f;
    }

    public void ShotgunSkillCoolTime(float time)
    {
        ShotgunCircularUI.SetActive(true);
        ShotguncoolTime = time;
        Shotguntimer = ShotguncoolTime;
        isShotgunCooling = true;
        ShotgunCoolImage.fillAmount = 1f;
    }

    public void RifleSkillCoolTime(float time)
    {
        RifleCircularUI.SetActive(true);
        RiflecoolTime = time;
        Rifletimer = RiflecoolTime;
        isRifleCooling = true;
        RifleCoolImage.fillAmount = 1f;
    }
}

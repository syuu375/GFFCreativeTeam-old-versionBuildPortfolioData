using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _UIGameOver;
    [SerializeField] private GameObject _UIShop;
    //private Transform canvasSkill;
    [SerializeField] private GameObject _UIFade;
    [SerializeField] private GameObject _UIMenu;
    [SerializeField] private GameObject _UITutorial;
    [SerializeField] private GameObject _Tutorial;
    private Fade _fade;

    private void Awake()
    {
        _fade = _UIFade.GetComponent<Fade>();
        _UITutorial.SetActive(false);//‘¬‚­false‚É‚µ‚½‚¢
        _Tutorial.SetActive(false);
    }

    private void Start()
    {
        _UIGameOver.SetActive(false);//Awake‚¾‚Æ‘¼‚©‚çŽæ“¾‚Å‚«‚È‚¢
        _UIShop.SetActive(false);
        _UIMenu.SetActive(false);
       
    }

    public void GameOver()
    {
        _UIGameOver.SetActive(true);//GameOver‰æ–Ê•\Ž¦
    }

    public void Shop()
    {
        _UIShop.SetActive(true);//‹­‰»‰æ–Ê•\Ž¦
    }

    public void Menu(bool _open)
    {
        _UIMenu.SetActive(_open);
    }

    public void Tutorial()
    {
        _UITutorial.SetActive(true);
        _Tutorial.SetActive(true);
    }

    public void FadeIn()
    {
        _UIFade.SetActive(true);//OnEnable‚ÅActive(true)‚©‚ÂFadeIn
    }

    public void FadeOut()
    {
        _fade.Fade_Out();//FadeOut‚ÅŽ©“®‚ÅActive(false)
    }
}

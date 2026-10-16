using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image Fadeimage; 
    private float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void OnEnable()
    {
        StartCoroutine (FadeIn());
    }

    public　void Fade_Out()
    {
        StartCoroutine(FadeOut());
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F) && !isFading)
    //    {
    //        isFading = true;
    //        StartCoroutine(FadeOut());
    //        Debug.Log("フェードアウト");
    //        // new WaitForSeconds(0.5f);
    //    }
    //    if (Input.GetKeyDown(KeyCode.G) && isFading)
    //    {
    //        StartCoroutine(FadeIn());
    //        Debug.Log("フェードイン");
    //        isFading =false;
    //    }
    //}

    //IEnumerator FadeSequence()
    //{
    //isFading = true;
    //yield return StartCoroutine(FadeOut());
    //yield return new WaitForSeconds(0.5f); 
    //yield return StartCoroutine(FadeIn());
    //isFading = false;
    //}

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.1f);

        float timer = 0f;
        Color color = Fadeimage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Fadeimage.color = color;
            yield return null;
        }

        gameObject.SetActive(false);
        yield break;
    }

    public IEnumerator FadeIn()
    {
        float timer = 0f;
        Color color = Fadeimage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            Fadeimage.color = color;
            yield return null;
        }
    }

}

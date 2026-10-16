using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    public void OnButton()
    {
        AudioManager.Instance.PlaySound("click");

        GManager.Instance.NextStage();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

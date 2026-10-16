using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void OnButton()
    {
        AudioManager.Instance.PlaySound("click");

        SceneManager.LoadScene("StageScene");
    }
}

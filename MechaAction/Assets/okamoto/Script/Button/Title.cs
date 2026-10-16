using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void OnButton()
    {
        AudioManager.Instance.PlaySound("click");

        SceneManager.LoadScene("Title");
        GManager.Instance.Title();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        AudioManager.Instance.PlaySound("click");
        Debug.Log("ゲーム終了");

        // ビルド後ならゲーム終了
        Application.Quit();

        // エディタで止めるため（便利）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

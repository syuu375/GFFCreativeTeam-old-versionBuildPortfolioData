using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;       // プレイヤー
    public Vector2 minPos;         // Clamp最小値
    public Vector2 maxPos;         // Clamp最大値
    public float smoothSpeed = 5f; // カメラ追従の滑らかさ

    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float shakeMagnitude = 0.2f;
    //private Coroutine shakeCoroutine;

    void FixedUpdate()
    {
        if (target == null) return;

        // Clampで制限しつつ追従
        float targetX = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
        float targetY = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);
        Vector3 desiredPos = new Vector3(targetX, targetY, transform.position.z);

        // 滑らかに追従
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }

    // カメラ揺れ開始メソッド
    public void ShakeCamera()//float duration = -1f, float magnitude = -1f
    {
        //if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        //shakeCoroutine = StartCoroutine(Shake(
        //  duration > 0 ? duration : shakeDuration,
        //  magnitude > 0 ? magnitude : shakeMagnitude));

        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        //shakeCoroutine = null;
    }
}

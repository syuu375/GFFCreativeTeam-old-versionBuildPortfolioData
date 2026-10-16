using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameralimit : MonoBehaviour
{
    private AreaEnemySpawn _spawn;

    [SerializeField]
    public Vector2 cameraMin;
    public Vector2 cameraMax;
    public Vector2 cameraMinRE;
    public Vector2 cameraMaxRE;

    [SerializeField]
    public GameObject invisibleWall;

    private bool activated = false;

    private void Start()
    {
        _spawn = GetComponent<AreaEnemySpawn>();

        if (invisibleWall != null)
            invisibleWall.SetActive(false);

        GManager.Instance.AreaTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (activated) return;

        // ÉJÉÅÉâêßå¿Çê›íË
        GManager.Instance.SetCameraBounds(cameraMin, cameraMax);
        _spawn.StartSpawn();

        invisibleWall.SetActive(true);

        activated = true;
    }

    public void Clear()
    {
        invisibleWall.SetActive(false);
        GManager.Instance.SetCameraBounds(cameraMinRE,cameraMaxRE);
    }

    public void DeadClear()
    {
        activated = false;
    }
}

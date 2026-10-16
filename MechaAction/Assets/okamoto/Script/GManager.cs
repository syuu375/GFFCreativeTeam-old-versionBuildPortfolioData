using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    private TextMeshProUGUI scorePointText;
    private TextMeshProUGUI lifeText;
    private TextMeshProUGUI TimeText;
    private TextMeshProUGUI BestTimeText;

    public static GManager Instance;
　　private CameraManager _mainCamera;
    private UIController _ui;
    private GameObject _playerobj;
    private Transform _playerposition;
    private Player _player;
    private PlayerHP _playerhp;
    private List<Cameralimit> _areaTriggers = new List<Cameralimit>(); 

    private float currentTime = 0f; // 現在のタイム
    private float targetTime = 180f; //基準タイム
    private float baseScore = 1000f;
    private float rate = 1.5f;       //スコア差
    private bool _isTiming = false;
    public int life = 2;
    public int clear = 0;
    public float score = 0;
    private bool _isOpen = false;
    private bool _isPlaying = false;

    public Vector3 currentpoint;
    private Vector3 _startPosition = new Vector3(0f,3.5f,0f);
    private Vector2 _respawnmin;
    private Vector2 _respawnmax;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;   //一応
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        currentpoint = _startPosition;
        SceneManager.sceneLoaded += SceneLoaded;
        //参照をここにもってくる

    }

    private void Start()
    {
        
    }

    void UpdateScorePointText()
    {
        scorePointText.text = string.Format("Score : {0}", score);
    }

    void UpdateTimeText()
    {
        TimeText.text = "Time: " + currentTime.ToString("F2") + "s";
    }

    public void UpdateLifeText()
    {
        lifeText.text = string.Format("Life : {0}", life);
    }

    public void DisplayBestTime()
    {
        float bestTime = SaveManager.Instance.Load().ClearTime;
        if (bestTime < float.MaxValue)
            BestTimeText.text = "Best Time: " + bestTime.ToString("F2") + "s";
        else
            BestTimeText.text = "Best Time: --";
    }

    public void ScoreUP(float _score)
    {
        score += _score;
    }

    private void Update()
    {
        if (_isTiming)
        {
            currentTime += Time.deltaTime;
        }
        if(scorePointText != null) UpdateScorePointText();
        if (lifeText != null) UpdateLifeText();
        if (TimeText != null) UpdateTimeText();
        if (BestTimeText != null) DisplayBestTime();

        if (Input.GetKeyDown(KeyCode.Escape) && _isPlaying)
        {
            Menu();
            AudioManager.Instance.PlaySound("menu");
        }
    }

    private void Menu()
    {
        _isOpen = !_isOpen;
        _ui.Menu(_isOpen);
        if(_isOpen )
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    // Clamp値を変更
    public void SetCameraBounds(Vector2 min, Vector2 max) //カメラ制限
    {
        _mainCamera.minPos = min;
        _mainCamera.maxPos = max;
    }

    public void OnPlayerHit() //プレイヤーがダメージを受けたとき
    {
        if (_mainCamera != null)
            _mainCamera.ShakeCamera();   // カメラ揺らす
    }

    public void AreaTrigger(Cameralimit area)
    {
        if(!_areaTriggers.Contains(area)) _areaTriggers.Add(area);
    }

    private void DeadAreaTrigger()
    {
        foreach(var area in _areaTriggers)
        {
            area.DeadClear();
        }
    }

    public IEnumerator DiePlayer()
    {
        _player.Dead();//Vector3.zeroにするため　後消す
        AudioManager.Instance.PlaySound("dead");
        if (life <= 0)
        {
            _isTiming = false;
            _ui.GameOver();
            Debug.Log("gameover");
            yield break;
        }

        DeadAreaTrigger();//AreaEnemyのリセット

        _ui.FadeIn();                                                  //フェードインさせる
        yield return new WaitForSeconds(1.5f);

        _player._ChangeState(PlayerState.Respawn);                     //一旦respawnに
        life--;                                                        //life-1
        SetCameraBounds(_respawnmin, _respawnmax);
        _playerposition.position = currentpoint;
        StartCoroutine(_playerhp.ResetHP());
        _ui.FadeOut();
        yield return new WaitForSeconds(1f);

        _player._ReturnNormal();

        yield return new WaitForSeconds(2f);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
           LayerMask.NameToLayer("Enemy"), false);

        yield break;
    }

    public void CheckPoint(Vector3 newPos,Vector2 newmin,Vector2 newmax)
    {
        currentpoint = newPos;
        _respawnmin = newmin;
        _respawnmax = newmax;
    }

    public void Reset()//gameoverのRetry
    {
        life = 2;
        score = 0;
        currentpoint = _startPosition;
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
        // LayerMask.NameToLayer("Enemy"), false);//一応

        //Debug.Log("Reset");//呼ばれた
        //ロードしなおす
    }

    public void Clear()
    {
        AudioManager.Instance.PlaySound("clear");
        _player._ChangeState(PlayerState.Other);
        _isTiming = false;
        score +=baseScore * Mathf.Pow(targetTime / currentTime, rate);//(a,b) aのb乗
        score = Mathf.Round(score * 100f) / 100f;
        score += life * 1000;
        SkillManager.Instance.Point((int)score);
        //スコアを保存

        GameData data = new GameData { ClearStage = clear, ClearTime = currentTime };
        SaveManager.Instance.Save(data);

        if(clear >= 4)
        {
            Title();
            SceneManager.LoadScene("Title");

            return;
        }
        //score = 0;
        //強化UIを出す
        _ui.Shop();
    }

    public void Title()
    {
        score = 0;
        life = 2;
        clear = 0;
        currentTime = 0;
        currentpoint = _startPosition;
    }

    public void NextStage()
    {
        clear++;
        currentTime = 0;
        Reset();
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)//シーンがロードされると呼ばれる
    {
        scorePointText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        lifeText = GameObject.Find("LifeText")?.GetComponent<TextMeshProUGUI>();
        TimeText = GameObject.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
        BestTimeText = GameObject.Find("BestTime")?.GetComponent<TextMeshProUGUI>();
        _mainCamera = FindFirstObjectByType<CameraManager>();
        _playerobj = GameObject.FindWithTag("Player");
        if (_playerobj != null)
        {
            _player = _playerobj.GetComponent<Player>();
            _playerposition = _playerobj.transform;
            _playerhp = _playerobj.GetComponent<PlayerHP>();
            currentpoint = _playerobj.transform.position;
        }
        _ui = FindFirstObjectByType<UIController>();

        if (nextScene.name == "StageScene")
        {
            Time.timeScale = 1f;//menuの時止め解除保険
            AudioManager.Instance.StartGameMusic();
            if(clear == 0)
            {
                _ui.Tutorial();
                //Debug.Log("チュートリアル");

            }
            
            score = 0;
            _player._ReturnNormal();
            _playerposition.position = currentpoint;
            _respawnmax = new Vector2(0, 5f);//カメラ初期化
            _respawnmax = new Vector2(1000, 7f);
            _isTiming = true;
            currentTime = 0;
            life = 2;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
          LayerMask.NameToLayer("Enemy"), false);
            _isPlaying = true;
        }

        if(nextScene.name == "Title")
        {
            AudioManager.Instance.StartTitleMusic();
            _isPlaying = false;
            Time.timeScale = 1f;//menuの時止め解除保険
        }

        if (scorePointText == null) Debug.Log("scorePointText null");
        if (lifeText == null) Debug.Log("lifeText null");
        if (TimeText == null) Debug.Log("TimeText null");
        if (BestTimeText == null) Debug.Log("BestTimeText null");



    }
}

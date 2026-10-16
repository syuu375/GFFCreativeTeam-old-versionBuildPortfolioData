using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandManager : MonoBehaviour
{
    [System.Serializable]
    private class Command
    {
        [SerializeField]
        private string name; //技名
        [SerializeField]
        private List<string> sequence; // 入力手順
        [SerializeField]
        private int maxFrameGap; //入力猶予
        
        public string Name => name; //Nameを呼び出したときnameの値を返す

        /*
         public string Name
        {
            get{ return name }
        } と同義
         */
        public List<string> Sequence => sequence;
        public int MaxFrameGap => maxFrameGap;


        public Command(string name, List<string> sequence, int maxFrameGap)
        {
            this.name = name;
            this.sequence = sequence;
            this.maxFrameGap = maxFrameGap;
        }
    }
    [System.Serializable]
    private struct InputData
    {
        public string Input; //　入力内容
        public int Frame; //入力フレーム

        public InputData(string inpug, int frame)
        {
            Input= inpug;
            Frame = frame;
        }
    }

    [SerializeField] private InputActionAsset inputActions; //inputsystem
    [SerializeField] private int bufferLimit = 30; // 入力履歴の格納量
    [SerializeField] private float frameDuration = 60f; //時間管理のフレーム（高フレーム環境でも安定した入力難易度を設定できる
    [SerializeField] private float Frametest;
    private float _frameTime;
    [SerializeField] private List<Command> commandList = new(); //技定義を編集する可能性、複数箇所の使用があるためclass 
    [SerializeField]private List<InputData> _inputBuffer = new(); //履歴を保持するだけだからstruct
    private int _currentFrame = 0; //入力が行われたフレーム番号を記録するための基準
    private float _frameTimer = 0f; //Time.deltatimeを加算しframeDurationを超えたら_currentframeに加算

    private InputAction _moveAction; 
    private InputAction _punchAction;
    private InputAction _kickAction;
    private InputAction _strongPunchAction;
    private InputAction _strongKickAction;

    private void Awake()
    {
        var map = inputActions.FindActionMap("Player"); //inputActionsAssetに含まれるActionMap Playerを代入

        _moveAction = map.FindAction("Move"); //それぞれのactionの代入
        _punchAction = map.FindAction("Punch");
        _kickAction = map.FindAction("Kick");
        _strongPunchAction = map.FindAction("StrongPunch");
        _strongKickAction = map.FindAction("StrongKick");

        _punchAction.performed += ctx => AddInput("Punch"); //それぞれの入力が入ったら履歴に追加する「処理」を登録
        _kickAction.performed += ctx => AddInput("Kick");
        _strongPunchAction.performed += ctx => AddInput("StrongPunch");
        _strongKickAction.performed += ctx => AddInput("StrongKick");

     //   QualitySettings.vSyncCount = 0;
     //   Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _punchAction.Enable();
        _kickAction.Enable();
        _strongPunchAction.Enable();
        _strongKickAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _punchAction.Disable();
        _kickAction.Disable();
        _strongPunchAction.Disable();
        _strongKickAction.Disable();
    }

    private void Start()
    {
        RegisterCommands();
    }

    private void Update()
    {
        AdvanceFrame();
        DetectDirectionalInput();
        CheckCommands();
    }

    private void AdvanceFrame() //現在frameの管理
    {
        _frameTime = 1 / frameDuration;
        _frameTimer += Time.deltaTime;
        while (_frameTimer >= _frameTime) 
        {
            _currentFrame++;
            _frameTimer -= _frameTime;
        }
        Frametest = _currentFrame;
    }

    private void DetectDirectionalInput()
    {
        Vector2 dir = _moveAction.ReadValue<Vector2>();
        int x = dir.x > 0.5f ? 1 : dir.x < -0.5f ? -1 : 0;
        int y = dir.y > 0.5f ? 3 : dir.y < -0.5f ? -3 : 0;
        int num = 5+x+y;
       // Debug.Log(_currentFrame);
        if(num >= 1 && num <= 9 && num !=5)
        AddInput(num.ToString());
    }//入力方向の管理

    private void AddInput(string input) //入力履歴の管理
    {
        _inputBuffer.Add(new InputData(input, _currentFrame));
        Debug.Log($"入力:{input} Frame: {_currentFrame}");

        if(_inputBuffer.Count > bufferLimit)
            _inputBuffer.RemoveAt(0);
    }

    private void RegisterCommands() //コマンド技の登録
    {
        commandList.Add(new Command("Hadouken", new List<string> { "2", "3", "6", "Punch"}, 100));
        commandList.Add(new Command("Reload", new List<string> { "2","5", "2", "Kick" }, 10));
        commandList.Add(new Command("Shouryuken", new List<string> { "6", "2", "3", "Punch" }, 10));
        commandList.Add(new Command("Tatsumakisenpukyaku", new List<string> { "2", "1", "4", "Kick" }, 10));
        commandList.Add(new Command("TyrantRave", new List<string> { "6", "3", "2","1","4","6", "StrongPunch" }, 10));
        commandList.Add(new Command("Shinkuuhadouken", new List<string> { "2", "3", "6","2","3","6", "StrongPunch" }, 10));
        commandList.Add(new Command("Shinkuutatumakisenpukyaku", new List<string> { "2", "1", "4","2","1","4", "StrongKick" }, 10));
        commandList.Add(new Command("GyakuyogaFlame", new List<string> { "6", "3", "2","1","4", "StrongPunch" }, 10));
        commandList.Add(new Command("irukasan", new List<string> { "4", "4", "4","4","6", "StrongPunch" }, 10));
       
    }

    private void CheckCommands() // 技出力内容を管理
    {
        foreach(var cmd  in commandList) //配列やリストを順に調べるループ文
        {
            if (MatchCommand(cmd))
            {
                Debug.Log($"技発動:{cmd.Name} Frame: {_currentFrame}");
                _inputBuffer.Clear(); //履歴の初期化
                break;
            }
        }
    }

    private bool MatchCommand(Command cmd)
    {
        int step = 0;
        int lastFrame = -1;

        for (int i = 0; i < _inputBuffer.Count; i++)
        {
            var data = _inputBuffer[i];
            string expected = cmd.Sequence[step];

            if (data.Input == expected)
            {
                if (step > 0)
                {
                    int frameGap = data.Frame - lastFrame;
                    if (frameGap > cmd.MaxFrameGap)
                    {
                        step = 0;
                        lastFrame = -1;
                        continue;
                    }
                }

                lastFrame = data.Frame;
                step++;
                      //  Debug.Log($"入力: Input: {data.Input},Fame: {data.Frame} step: {step}");

                if (step >= cmd.Sequence.Count)
                {
                      //  Debug.Log($"入力: Input: {data.Input},Fame: {data.Frame} step: {step}");
                    return true;
                }
            }
            else
            {
                continue;
            }
        }

        return false;
    }

}

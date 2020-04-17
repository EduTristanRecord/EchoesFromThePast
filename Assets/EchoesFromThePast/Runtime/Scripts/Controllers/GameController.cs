using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using DG.Tweening;

public enum Player {
    Blue,
    Red,
    Main
}

public class GameController : MonoBehaviour {

    public static GameController Instance;
    
    public Player activePlayer = Player.Blue;
    
    [Header("Players")]
    public PlayerController bluePlayer;
    public PlayerController redPlayer;
    public PlayerController mainPlayer;

    [Header("Settings")]
    public GameObject lowerLimit;
    public SpawnJump pointOfDead;
    private bool _isFinish = true;

    [Header("Sounds")]
    public AudioClip soundDie;
    public AudioClip soundEnd;

    private Dictionary<Player, PlayerController> _mappingPlayers;

    private Platform[] _platforms;
    private CheckPoint[] _checkPoints;


    private void Awake() {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        _platforms = FindObjectsOfType<Platform>();
        _checkPoints = FindObjectsOfType<CheckPoint>();

        _mappingPlayers = new Dictionary<Player, PlayerController> {
            {Player.Blue, bluePlayer},
            {Player.Red, redPlayer},
            {Player.Main, mainPlayer}
        };
    }
    
    private void Start() {

        bluePlayer.GhostMode(false);
        redPlayer.GhostMode(true);
        mainPlayer.GhostMode(true);

        foreach (Platform platform in _platforms) {
            platform.Switch(activePlayer);
        }

        UIController.Instance.StartRunning(()=> _isFinish = false);
    }

    private void Update() {
        Switch();
    }

    //End Game - Taylor Swift, Ed Sheeran
    public bool EndGame()
    {
        return _isFinish;
    }

    //Unpause - Salted
    public void Unpause()
    {
        _isFinish = true;
        UIController.Instance.StartRunning(() => _isFinish = false);
    }

    /** Switch - 6LACK */
    private void Switch() {
        if (_isFinish) return;
        if (Input.GetButtonDown("RightSwitch")) {
            activePlayer = activePlayer == Player.Blue
                ? Player.Red
                : activePlayer == Player.Red 
                    ? Player.Main
                    : Player.Blue;
            Reset();
        } else if (Input.GetButtonDown("LeftSwitch")) {
            activePlayer = activePlayer == Player.Blue
                ? Player.Main
                : activePlayer == Player.Red 
                    ? Player.Blue
                    : Player.Red;
            Reset();
        }else if (Input.GetButtonDown("Reset"))
        {
            Reset();
        }
    }
    
    
    /** Reset - Tiger JK */
    public void Reset() {
        CameraController2D.Instance.followTarget = _mappingPlayers[activePlayer].transform;
        UIController.Instance.ColorTheWorld();

        SoundController.Instance.SoundEffect(soundDie);

        int index = _mappingPlayers[activePlayer].indexGhost;

        foreach (PlayerController pc in _mappingPlayers.Values) {
            pc.GhostMode(activePlayer != pc.player);
            pc.Reset(index);
        }

        foreach (Platform platform in _platforms) {
            platform.Switch(activePlayer);
        }

        foreach (CheckPoint checkPoint in _checkPoints)
        {
            checkPoint.CheckThisOut(activePlayer);
        }

        //CheckPoint
    }

    

    //WIN - Jay Rock
    public void Win()
    {
        SoundController.Instance.SoundEffect(soundEnd);
        _isFinish = true;
        UIController.Instance.EndGame();

        for (int i = 0; i < _platforms.Length; i++)
        {
            _platforms[i].Switch(PlatformState.Enabled);
        }

        CameraController2D.Instance.SwitchView(()=> {
            foreach (PlayerController pc in _mappingPlayers.Values)
            {
                StartCoroutine(DrawTheLine(pc.GetPositioned(), pc.line, pc.lineContainer));
            }
        });
    }

    //Draw the line - Dagames
    public IEnumerator DrawTheLine(List<List<Vector2>> positions, LineRenderer line, GameObject container)
    {

        for (int j = 0; j < positions.Count; j++)
        {
            List<Vector3> currentPlaced = new List<Vector3>();
            LineRenderer newLine = Instantiate(line, container.transform);

            Color newLineColor = newLine.startColor / (positions.Count - (j));
            newLineColor.a = 1;

            newLine.startColor = newLineColor;
            newLine.endColor = newLineColor;

            for (int i =0; i < positions[j].Count; i+=4)
            {
                if (positions[j].Count > i)
                {
                    currentPlaced.Add(positions[j][i]);
                    newLine.positionCount = currentPlaced.Count;
                    newLine.SetPositions(currentPlaced.ToArray());
                    yield return new WaitForSeconds(1f / positions[j].Count);
                }
            }
            if (j < positions.Count-1 && positions[j].Count!=0)
            {
                SpawnJump dead = Instantiate(pointOfDead, container.transform);
                dead.Initialize(newLineColor, positions[j][positions[j].Count - 1]);
            }

        }

        yield break;
    }

    //The dictionnary Guys - Equipe de Foot
    public PlayerController DictionnaryGuys(Player player)
    {
        return _mappingPlayers[player];
    }
}
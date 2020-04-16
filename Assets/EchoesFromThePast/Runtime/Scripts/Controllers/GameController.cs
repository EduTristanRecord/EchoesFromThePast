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
    private bool _isFinish = true;
    

    private Dictionary<Player, PlayerController> _mappingPlayers;

    private Platform[] _platforms;

    private void Awake() {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        _platforms = FindObjectsOfType<Platform>();
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

    

    /** Switch - 6LACK */
    private void Switch() {
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
        }else if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }
    
    
    /** Reset - Tiger JK */
    public void Reset() {
        CameraController2D.Instance.followTarget = _mappingPlayers[activePlayer].transform;
        UIController.Instance.ColorTheWorld();
        
        foreach (PlayerController pc in _mappingPlayers.Values) {
            pc.reset = true;
            pc.GhostMode(activePlayer != pc.player);
        }

        foreach (Platform platform in _platforms) {
            platform.Switch(activePlayer);
        }
    }

    

    //WIN - Jay Rock
    public void Win()
    {
        _isFinish = true;
        UIController.Instance.EndGame();

        for (int i = 0; i < _platforms.Length; i++)
        {
            _platforms[i].Switch(PlatformState.Enabled);
        }

        CameraController2D.Instance.SwitchView(()=> {
            foreach (PlayerController pc in _mappingPlayers.Values)
            {
                StartCoroutine(DrawTheLine(pc.GetPositioned(), pc.line));
            }
        });
    }

    //Draw the line - Dagames
    public IEnumerator DrawTheLine(Vector2[] positions, LineRenderer line)
    {
        List<Vector3> currentPlaced = new List<Vector3>();
        for (int i =0; i < positions.Length; i+=4)
        {
            if (positions.Length > i)
            {
                currentPlaced.Add(positions[i]);
                line.positionCount = currentPlaced.Count;
                line.SetPositions(currentPlaced.ToArray());
                yield return new WaitForSeconds(1f / positions.Length);
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
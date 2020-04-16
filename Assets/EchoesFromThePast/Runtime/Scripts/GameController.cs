using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Player {
    Blue,
    Red,
    Main
}

public class GameController : MonoBehaviour {

    public static GameController Instance;
    
    public Player activePlayer = Player.Blue;

    [Header("Text Players")]
    public Text textRightPlayerActive;
    public Text textLeftPlayerActive;
    public Text textPlayerActive;

    [Header("Players")]
    public PlayerController bluePlayer;
    public PlayerController redPlayer;
    public PlayerController mainPlayer;

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
        ColorTheWorld();

        bluePlayer.GhostMode(false);
        redPlayer.GhostMode(true);
        mainPlayer.GhostMode(true);

        foreach (Platform platform in _platforms) {
            platform.Switch(activePlayer);
        }
    }

    private void Update() {
        Switch();
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
        ColorTheWorld();
        
        foreach (PlayerController pc in _mappingPlayers.Values) {
            pc.reset = true;
            pc.GhostMode(activePlayer != pc.player);
        }

        foreach (Platform platform in _platforms) {
            platform.Switch(activePlayer);
        }
    }

    //Color the World - Waves Like Walls
    public void ColorTheWorld()
    {
        textPlayerActive.text = activePlayer.ToString();
        textPlayerActive.color = _mappingPlayers[activePlayer].colorPlayer;
        CameraController2D.Instance.ColorImage(_mappingPlayers[activePlayer].colorCamPlayer);

        //Change Color UI
        if (activePlayer == Player.Blue)
        {
            textLeftPlayerActive.text = Player.Main.ToString();
            textLeftPlayerActive.color = _mappingPlayers[Player.Main].colorPlayer;

            textRightPlayerActive.text = Player.Red.ToString();
            textRightPlayerActive.color = _mappingPlayers[Player.Red].colorPlayer;
        }
        else if (activePlayer == Player.Red)
        {
            textLeftPlayerActive.text = Player.Blue.ToString();
            textLeftPlayerActive.color = _mappingPlayers[Player.Blue].colorPlayer;

            textRightPlayerActive.text = Player.Main.ToString();
            textRightPlayerActive.color = _mappingPlayers[Player.Main].colorPlayer;
        }
        else
        {
            textLeftPlayerActive.text = Player.Red.ToString();
            textLeftPlayerActive.color = _mappingPlayers[Player.Red].colorPlayer;

            textRightPlayerActive.text = Player.Blue.ToString();
            textRightPlayerActive.color = _mappingPlayers[Player.Blue].colorPlayer;
        }
    }

    //WIN - Jay Rock
    public void Win()
    {
        Debug.Log("The End");
    }
}
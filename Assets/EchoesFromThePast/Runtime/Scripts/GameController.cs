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
    public Text textPlayerActive;

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
        textPlayerActive.text = activePlayer.ToString();
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
            textPlayerActive.text = activePlayer.ToString();
            Reset();
        } else if (Input.GetButtonDown("LeftSwitch")) {
            activePlayer = activePlayer == Player.Blue
                ? Player.Main
                : activePlayer == Player.Red 
                    ? Player.Blue
                    : Player.Red;
            textPlayerActive.text = activePlayer.ToString();
            Reset();
        }
    }
    
    
    /** Reset - Tiger JK */
    public void Reset() {
        foreach (PlayerController pc in _mappingPlayers.Values) {
            pc.reset = true;
            pc.GhostMode(activePlayer != pc.player);
        }

        foreach (Platform platform in _platforms) {
            platform.Switch(activePlayer);
        }
    }
}
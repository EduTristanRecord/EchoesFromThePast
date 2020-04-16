using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Text Players")]
    public Text textRightPlayerActive;
    public Text textLeftPlayerActive;
    public Text textPlayerActive;

    [Header("HUD")]
    public Text timeGame;
    public Text startCounter;

    public GameObject inGameHUD;
    public GameObject pauseHUD;
    public GameObject endHUD;

    private float _timer;


    void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        endHUD.SetActive(false);

        ColorTheWorld();
        Resume();
    }

    void Update()
    {
        TimeIsRunningOut();

        if (Input.GetKeyDown(KeyCode.Escape) && !GameController.Instance.EndGame())
        {
            if (inGameHUD.activeSelf)
            {
                inGameHUD.SetActive(false);
                pauseHUD.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                inGameHUD.SetActive(true);
                pauseHUD.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    //Start Running - The Comet is coming
    public void StartRunning(Action callback)
    {
        Ease ease = Ease.OutCubic;

        startCounter.text = "3";
        startCounter.transform.DOScale(new Vector3(3, 3, 3), 0.5f).SetEase(ease).OnComplete(() => {

            startCounter.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(ease).OnComplete(() => {

                startCounter.text = "2";
                startCounter.transform.DOScale(new Vector3(3, 3, 3), 0.5f).SetEase(ease).OnComplete(() => {

                    startCounter.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(ease).OnComplete(() => {

                        startCounter.text = "1";
                        startCounter.transform.DOScale(new Vector3(3, 3, 3), 0.5f).SetEase(ease).OnComplete(() => {

                            startCounter.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(ease).OnComplete(() => {
                                startCounter.gameObject.SetActive(false);
                                callback();
                            });

                        });

                    });

                });

            });

        });
    }

    //Time is Running Out - Muse
    private void TimeIsRunningOut()
    {
        if (GameController.Instance.EndGame() || Time.timeScale==0) return;
        _timer += Time.deltaTime;
        var ts = TimeSpan.FromSeconds(_timer);
        timeGame.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);

        timeGame.transform.position += new Vector3(0, Mathf.Cos(_timer * 100) / 10, 0);
    }

    //Color the World - Waves Like Walls
    public void ColorTheWorld()
    {
        Player activePlayer = GameController.Instance.activePlayer;

        textPlayerActive.text = activePlayer.ToString();
        textPlayerActive.color = GameController.Instance.DictionnaryGuys(activePlayer).colorPlayer;
        CameraController2D.Instance.ColorImage(GameController.Instance.DictionnaryGuys(activePlayer).colorCamPlayer);

        //Change Color UI
        if (activePlayer == Player.Blue)
        {
            textLeftPlayerActive.text = Player.Main.ToString();
            textLeftPlayerActive.color = GameController.Instance.DictionnaryGuys(Player.Main).colorPlayer;

            textRightPlayerActive.text = Player.Red.ToString();
            textRightPlayerActive.color = GameController.Instance.DictionnaryGuys(Player.Red).colorPlayer;
        }
        else if (activePlayer == Player.Red)
        {
            textLeftPlayerActive.text = Player.Blue.ToString();
            textLeftPlayerActive.color = GameController.Instance.DictionnaryGuys(Player.Blue).colorPlayer;

            textRightPlayerActive.text = Player.Main.ToString();
            textRightPlayerActive.color = GameController.Instance.DictionnaryGuys(Player.Main).colorPlayer;
        }
        else
        {
            textLeftPlayerActive.text = Player.Red.ToString();
            textLeftPlayerActive.color = GameController.Instance.DictionnaryGuys(Player.Red).colorPlayer;

            textRightPlayerActive.text = Player.Blue.ToString();
            textRightPlayerActive.color = GameController.Instance.DictionnaryGuys(Player.Blue).colorPlayer;
        }
    }

    //End Game - Zomboy
    public void EndGame()
    {
        textLeftPlayerActive.gameObject.SetActive(false);
        textPlayerActive.gameObject.SetActive(false);
        textRightPlayerActive.gameObject.SetActive(false);

        inGameHUD.SetActive(false);
        pauseHUD.SetActive(false);
        endHUD.SetActive(true);
    }

    //Resume - Lil Tjay
    public void Resume()
    {
        inGameHUD.SetActive(true);
        pauseHUD.SetActive(false);
        Time.timeScale = 1;
    }

    //Restart - Sam Smith
    public void Restart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Leave - Gnash
    public void Leave()
    {
        Application.Quit();
    }
}

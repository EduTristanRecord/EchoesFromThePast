﻿using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public Player player;
    [Range(2f, 100f)] public float runSpeed = 40f;
    public Collider2D box;
    public Collider2D circle;

    public Color colorPlayer;
    public Color colorCamPlayer;

    public TrailRenderer trail;
    public LineRenderer line;
    public GameObject lineContainer;

    public bool reset;

    public ParticleSystem dispawn;
    
    private CharacterController2D _characterController;

    private float _horizontalMovement;
    private bool _jump;
    private bool _crouch;
    private int _index;
    private bool _ghostMode;
    private Vector3 _initialPosition;
    private Rigidbody2D _rigidBody;

    private float _nextVelocityY = 0;
    
    private readonly List<List<Vector2>> _ghosts = new List<List<Vector2>>();
    private int _currentTry = 0;

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _characterController = GetComponent<CharacterController2D>();
        _initialPosition = transform.position;
        _ghosts.Add(new List<Vector2>());
    }

    private void Start() {
        _index = 0;
        _ghosts[_currentTry].Add(transform.position);
    }

    private void Update() {
        if (GameController.Instance.EndGame()) return;

        if (reset) {

            Reset();
            trail.Clear();
            reset = false;
        }
        if (_ghostMode) return;
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        if (Input.GetButtonDown("Jump"))
            _jump = true;
        if (Input.GetButtonDown("Crouch"))
            _crouch = true;
        else if (Input.GetButtonUp("Crouch"))
            _crouch = false;
    }

    private void FixedUpdate() {
        if (GameController.Instance.EndGame()) return;
        
        if (!_ghostMode) {
            _characterController.MovesLikeJagger(_horizontalMovement * Time.fixedDeltaTime, _crouch, _jump);
            _jump = false;
            GodSaveTheGhosts();
        } else {
            PlayingTheGhost();
        }
        if (transform.position.y < GameController.Instance.lowerLimit.transform.position.y)
        {
            GameController.Instance.Reset();
        }
    }

    /** Ghost Mode - Jakumo */
    public void GhostMode(bool activated) {
        _ghostMode = activated;
        box.enabled = !activated;
        circle.enabled = !activated;
        _rigidBody.bodyType = activated ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        if (!activated)
        {
            _ghosts.Add(new List<Vector2>());
            _currentTry++;
        }
    }

    /** God Save The Ghosts - Ragequit */
    private void GodSaveTheGhosts() {
        _ghosts[_currentTry].Add(transform.position);
    }

    /** Playing The Ghost - Lex Zaleta */
    private void PlayingTheGhost() {
        _index = Mathf.Clamp(_index + 1, 0, _ghosts[_currentTry].Count - 1);
        transform.DOMove(_ghosts[_currentTry][_index], 0.2f);
    }

    /** Reset - Tiger JK */
    public void Reset() {
        DOTween.KillAll(this);
        dispawn.Play();
        transform.position = _initialPosition;
        _index = 0;
    }

    //Get Positioned - Elias Ndeda
    public List<List<Vector2>> GetPositioned()
    {
        return _ghosts;
    }
}
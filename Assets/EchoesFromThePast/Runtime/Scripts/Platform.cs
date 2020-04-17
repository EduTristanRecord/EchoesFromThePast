using UnityEngine;
using DG.Tweening;

public enum PlatformState {
    Enabled,
    Disabled,
    Invisible
}

public class Platform : MonoBehaviour {
    public Player player;

    private Collider2D _collider;
    private SpriteRenderer _renderer;
    private Sequence _anim;
    
    public void Awake() {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    /** Switch - Will Smith */
    public void Switch(Player activePlayer) {
        if (activePlayer == Player.Main)
            Switch(PlatformState.Invisible);
        else if (player == activePlayer)
            Switch(PlatformState.Enabled);
        else
            Switch(PlatformState.Disabled);
    }

    /** Switch - Iggy Azalea */
    public void Switch(PlatformState state) {
        switch (state) {
            case PlatformState.Enabled:
                MakeMeVisible(true);
                Collide(true);
                break;
            case PlatformState.Disabled:
                MakeMeVisible(false);
                Collide(false);
                break;
            case PlatformState.Invisible:
                MakeMeVisible(false);
                Collide(true);
                break;
            default:
                MakeMeVisible(false);
                Collide(false);
                break;
        }
    }

    /** Make Me Visible - Elisabeth Ehrnrooth */
    private void MakeMeVisible(bool value) {
        //_renderer.enabled = value;
        if (_anim != null)
        {
            _anim.Kill();
        }
        if (value)
        {
            _anim = DOTween.Sequence().Append(_renderer.DOFade(1, 0.3f));
        }
        else
        {
            _anim = DOTween.Sequence().Append(_renderer.DOFade(0, 0.3f));
        }
    }

    /** Collide - Rachel Platten */
    private void Collide(bool value) {
        _collider.enabled = value;
    }
}
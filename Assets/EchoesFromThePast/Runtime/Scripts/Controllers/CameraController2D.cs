using UnityEngine;

using System;
using DG.Tweening;

public class CameraController2D : MonoBehaviour {
    public static CameraController2D Instance;
    
    [Header("Camera Follow Settings")]
    public Transform followTarget;
    public float followSpeed = 5f;
    
    [Range(0f, 1f)] public float followRatioX = 0.5f;
    [Range(0f, 1f)] public float followRatioY = 0.5f;

    [SerializeField] private FollowMode followMode = FollowMode.Ratio;

    [Serializable]
    public class CameraBounds {
        public float top = 1f;
        public float right = 1f;
        public float bottom = -1f;
        public float left = -1f;
    }

    [Header("Camera Bounds settings")]
    public CameraBounds bounds;
    private Camera _camera;
    private bool _isBoundsEnabled;

    [Header("Camera")]
    public Camera endCam;
    public bool camEnd;
    public SpriteRenderer background;
    public Color colorEnd;
    private bool _switchingView;

    private enum FollowMode {
        Ratio,
        Speed
    }

    private void Awake() {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
        
        _camera = GetComponentInChildren<Camera>();
    }

    private void LateUpdate() {
        if (!_switchingView)
        {
            Transform cameraTransform = transform;
            Vector3 position = cameraTransform.position;

            Vector3 followTargetPosition = followTarget.position;
            Vector3 targetPosition = _isBoundsEnabled ? ClampPositionToScreen(followTargetPosition) : followTargetPosition;

            switch (followMode) {
                case FollowMode.Ratio:
                    position.x = Mathf.Lerp(position.x, targetPosition.x, Time.timeScale * followRatioX);
                    position.y = Mathf.Lerp(position.y, targetPosition.y, Time.timeScale * followRatioY);
                    break;
                case FollowMode.Speed:
                    position.x = Mathf.Lerp(position.x, targetPosition.x, Time.deltaTime * followSpeed);
                    position.y = Mathf.Lerp(position.y, targetPosition.y, Time.deltaTime * followSpeed);
                    break;
            }
            cameraTransform.position = position;
        }
    }

    public void SetCameraBounds(CameraBounds cameraBounds) {
        bounds = cameraBounds;
        _isBoundsEnabled = true;
    }

    public void ResetCameraBounds() {
        _isBoundsEnabled = false;
    }
    
    private Vector3 ClampPositionToScreen(Vector3 position) {
        Vector3 bottomLeft = _camera.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = _camera.ScreenToWorldPoint(
            new Vector3(_camera.pixelWidth, _camera.pixelHeight, 0f));
        Vector2 screenSize = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        Vector2 halfScreen = screenSize / 2f;

        position.x = Mathf.Clamp(position.x, bounds.left + halfScreen.x, bounds.right - halfScreen.x);
        position.y = Mathf.Clamp(position.y, bounds.bottom + halfScreen.y, bounds.top - halfScreen.y);

        return position;
    }

    private void OnDrawGizmos() {
        float gizmosCubeSize = 0.3f;

        Vector3 position = transform.position;
        
        Gizmos.color = Color.yellow;
        Vector3 topLeft = new Vector3(bounds.left, bounds.top, position.z);
        Gizmos.DrawCube(topLeft, Vector3.one * gizmosCubeSize);

        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(bounds.left, bounds.bottom, position.z);
        Gizmos.DrawCube(bottomLeft, Vector3.one * gizmosCubeSize);
        
        Gizmos.color = Color.green;
        Vector3 topRight = new Vector3(bounds.right, bounds.top, position.z);
        Gizmos.DrawCube(topRight, Vector3.one * gizmosCubeSize);

        Gizmos.color = Color.blue;
        Vector3 bottomRight = new Vector3(bounds.right, bounds.bottom, position.z);
        Gizmos.DrawCube(bottomRight, Vector3.one * gizmosCubeSize);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    //Color Image - Giovanni Tornambene
    public void ColorImage(Color color)
    {
        background.DOColor(color, 0.3f);
    }

    //Switch View - Ju Ha Wu
    public void SwitchView(Action action)
    {
        if (_switchingView) return;
        _switchingView = true;

        _camera.transform.DOMove(endCam.transform.position,1);
        background.DOColor(colorEnd, 1);

        _camera.DOFieldOfView(endCam.fieldOfView, 1).OnComplete(()=> {
            action();
        });
    }
}


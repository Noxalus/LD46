using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float maxPanSpeed = 20f;

    public Camera Camera;

    [Range(0, 1)]
    public float panBorderThickness = 0.5f;
    public Vector2 panLimitBottomLeft = new Vector2(10, 10);
    public Vector2 panLimitTopRight = new Vector2(10, 10);
    public float scrollSpeed = 20f;

    public float _zoomMin = 1f;
    public float _zoomMax = 20f;

    public bool showPanBorder = false;

    public AnimationCurve _panSpeedCurve = null;
    private Rect _panBorders;
    private Vector2 _screenSize;

    private Vector2 _panOriginPosition = Vector2.zero;

    private void Start()
    {
        _screenSize = new Vector2(Screen.width, Screen.height);
        ComputePanBorders();
    }

    private void ComputePanBorders()
    {
        float width = Screen.width * panBorderThickness;
        float height = Screen.height * panBorderThickness;

        _panBorders = new Rect(
            new Vector2(
                (Screen.width / 2f) - (width / 2f),
                (Screen.height / 2f) - (height / 2f)
            ),
            new Vector2(
                width,
                height
            )
        );
    }

    void OnGUI()
    {
        if (showPanBorder)
        {
            Color color = new Color(1f, 0, 0, 0.5f);
            EditorGUITools.DrawRect(_panBorders, color);
        }
    }

    private void Update()
    {
        //CheckResolution();

        //if (Input.mousePosition.x <= 0 || Input.mousePosition.x >= Screen.width ||
        //    Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height)
        //{
        //    return;
        //}

        float screenRatio = Screen.width / Screen.height;
        Vector3 direction = transform.position;
        direction = Vector3.zero;

        float mouseX = Mathf.Clamp(Input.mousePosition.x, 0, Screen.width);
        float mouseY = Mathf.Clamp(Input.mousePosition.y, 0, Screen.height);
        float zoomFactor = (Camera.orthographicSize / 10f);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //maxPanSpeed *= Camera.orthographicSize / 10f;

        direction.x += maxPanSpeed * horizontal * Time.deltaTime * zoomFactor;
        direction.y += maxPanSpeed * vertical * Time.deltaTime * zoomFactor;

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            _panOriginPosition = Input.mousePosition;
        }

        if (_panOriginPosition != Vector2.zero)
        {
            // Move the camera according the offset (move value)
            direction.x += (_panOriginPosition.x - Input.mousePosition.x) * screenRatio * (maxPanSpeed / 10f) * zoomFactor * Time.deltaTime;
            direction.y += (_panOriginPosition.y - Input.mousePosition.y) * screenRatio * (maxPanSpeed / 10f) * zoomFactor * Time.deltaTime;

            _panOriginPosition = Vector2.Lerp(_panOriginPosition, Input.mousePosition, 0.5f);
        }

        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            _panOriginPosition = Vector2.zero;
        }

        //if (mouseY >= _panBorders.yMax)
        //{
        //    ratio = (Input.mousePosition.y - _panBorders.yMax) / (Screen.height - _panBorders.yMax);
        //    ratio = _panSpeedCurve.Evaluate(ratio);
        //    position.z += maxPanSpeed * ratio * Time.deltaTime;
        //}
        //if (mouseY <= _panBorders.yMin)
        //{
        //    ratio = 1f - (Input.mousePosition.y / _panBorders.yMin);
        //    ratio = _panSpeedCurve.Evaluate(ratio);
        //    position.z -= maxPanSpeed * ratio * Time.deltaTime;
        //}
        //if (mouseX >= _panBorders.xMax)
        //{
        //    ratio = (Input.mousePosition.x - _panBorders.xMax) / (Screen.width - _panBorders.xMax);
        //    ratio = _panSpeedCurve.Evaluate(ratio);
        //    position.x += maxPanSpeed * ratio * Time.deltaTime;
        //}
        //if (mouseX <= _panBorders.xMin)
        //{
        //    ratio = 1f - (Input.mousePosition.x / _panBorders.xMin);
        //    ratio = _panSpeedCurve.Evaluate(ratio);
        //    position.x -= maxPanSpeed * ratio * Time.deltaTime;
        //}

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        //position += transform.forward * scroll * scrollSpeed * Time.deltaTime;
        Camera.orthographicSize -= scroll * scrollSpeed * Time.deltaTime;
        Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, _zoomMin, _zoomMax);

        direction = Camera.transform.TransformDirection(direction);

        //position.x = Mathf.Clamp(position.x, panLimitBottomLeft.x, panLimitTopRight.x);
        //position.y = Mathf.Clamp(position.y, zoomMin, zoomMax);
        //position.z = Mathf.Clamp(position.z, panLimitBottomLeft.y, panLimitTopRight.y);

        float previousY = transform.position.y;
        transform.position += direction;
        transform.position = new Vector3(transform.position.x, previousY, transform.position.z);
    }

    private void CheckResolution()
    {
        if (_screenSize.x != Screen.width || _screenSize.y != Screen.height)
        {
            ComputePanBorders();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerTransform;

    private Vector3 _cameraOffset;

    public float SmoothFactor = 0.5f;

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
        button.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }


    public void Restart()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("game");
    }
}

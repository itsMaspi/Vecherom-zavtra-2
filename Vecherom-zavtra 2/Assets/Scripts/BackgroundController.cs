using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public RawImage background;

    public float angle = 0f;
    public float increaseAngle = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        increaseAngle = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        angle += increaseAngle;
        background.uvRect = new Rect(background.uvRect.x + (0.02f * Time.deltaTime), (Mathf.Sin(ConvertToRadians(angle)/40)), 1f, 1f);
    }
    public float ConvertToRadians(float angle)
    {
        return (Mathf.PI / 180) * angle;
    }
}

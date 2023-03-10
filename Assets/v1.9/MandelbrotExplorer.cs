using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandelbrotExplorer : MonoBehaviour
{
    [SerializeField] Material m_Mandelbrot;
    [SerializeField] Vector2 pos;
    [SerializeField] float scale;
    [SerializeField] float angle;

    Vector2 smoothPos;
    float smoothScale;
    float smoothAngle;

    private void Start()
    {
        scale = 4;
    }

    void FixedUpdate()
    {
        UserInput();
        UpdateFractalShader();
    }

    void UserInput()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            scale *= 0.99f;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            scale *= 1.01f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            angle -= 0.01f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            angle += 0.01f;
        }

        Vector2 dir = new Vector2(0.01f * scale, 0);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);
        dir = new Vector2(dir.x * c, dir.x * s);


        if (Input.GetKey(KeyCode.A))
        {
            pos -= dir;
        }

        if (Input.GetKey(KeyCode.D))
        {
            pos += dir;
        }

        dir = new Vector2(-dir.y, dir.x);

        
        if (Input.GetKey(KeyCode.W))
        {
            pos += dir;
        }

        if(Input.GetKey(KeyCode.S))
        {
            pos -= dir;
        }
        
    }

    void UpdateFractalShader()
    {
        smoothPos = Vector2.Lerp(smoothPos,pos,0.1f);
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.1f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.1f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspectRatio > 1)
        {
            scaleY /= aspectRatio;
        }
        else
        {
            scaleX *= aspectRatio;
        }
        m_Mandelbrot.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        m_Mandelbrot.SetFloat("_Angle", smoothAngle);
    }
}

using System;
using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    private GameManager GM_Script;

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;


    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    void Start()
    {
        GM_Script = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = 0;
        float xRot = 0;

        if(GM_Script.CurrentControlState == GameManager.ControlState.MouseAndKeyboard)
        {
            yRot = Input.GetAxis("Mouse X") * XSensitivity;
            xRot = Input.GetAxis("Mouse Y") * YSensitivity;
        }

        if (GM_Script.CurrentControlState == GameManager.ControlState.Xbox360Controller)
        {
            yRot = Input.GetAxis("Right X Axis") * XSensitivity;
            xRot = Input.GetAxis("Right Y Axis") * -YSensitivity;
        }


        m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

        if(clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

        if(smooth)
        {
            character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }
    }


    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}


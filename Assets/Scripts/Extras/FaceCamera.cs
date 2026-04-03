using UnityEngine;

using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (m_Camera == null) return;
        // Rotate to face camera while staying upright
        transform.forward = m_Camera.transform.forward;
    }
}

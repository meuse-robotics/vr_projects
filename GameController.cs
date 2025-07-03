using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject xrOrigin;
    [SerializeField] GameObject ball;

    [Header("Input Actions")]
    public InputActionReference xButtonAction; // Xボタン (primary button)
    public InputActionReference yButtonAction; // Yボタン (secondary button)

    void OnEnable()
    {
        if (xButtonAction != null)
            xButtonAction.action.performed += OnXButtonPressed;

        if (yButtonAction != null)
            yButtonAction.action.performed += OnYButtonPressed;
            yButtonAction.action.canceled += OnYButtonReleased;

        xButtonAction?.action.Enable();
        yButtonAction?.action.Enable();
    }

    void OnDisable()
    {
        if (xButtonAction != null)
            xButtonAction.action.performed -= OnXButtonPressed;

        if (yButtonAction != null)
            yButtonAction.action.performed -= OnYButtonPressed;
            yButtonAction.action.canceled -= OnYButtonReleased;

        xButtonAction?.action.Disable();
        yButtonAction?.action.Disable();
    }

    private void OnXButtonPressed(InputAction.CallbackContext context)
    {
        //XR Origin,Ballをティーグラウンドに移動
        var pos = xrOrigin.transform.position;
        pos.x = 1f;   pos.y = 21.5f;  pos.z = -1535f;
        xrOrigin.transform.position = pos;
        xrOrigin.transform.rotation = Quaternion.Euler(0, 90, 0);
        pos = ball.transform.position;
        pos.x = 2f; pos.y = 22.5f; pos.z = -1535f;
        ball.transform.position = pos;
        ball.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
    }

    private void OnYButtonPressed(InputAction.CallbackContext context)
    {
        Vector3 scale = ball.transform.localScale;
        scale.x = 2f;
        scale.y = 2f;
        scale.z = 2f;
        ball.transform.localScale = scale;
    }
    private void OnYButtonReleased(InputAction.CallbackContext context)
    {
        Vector3 scale = ball.transform.localScale;
        scale.x = 0.2f;
        scale.y = 0.2f;
        scale.z = 0.2f;
        ball.transform.localScale = scale;
    }
}

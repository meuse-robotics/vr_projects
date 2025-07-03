using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ClubController : MonoBehaviour
{
    [SerializeField] GameObject Club1;
    [SerializeField] GameObject Club5;
    [SerializeField] GameObject Club9;
    [SerializeField] GameObject ClubP;
    [SerializeField] TextMeshProUGUI ClubNumText;

    [Header("Input Actions")]
    public InputActionReference triggerButtonAction;

    void OnEnable()
    {
        if (triggerButtonAction != null)
            triggerButtonAction.action.performed += OnTriggerButonPressed;
        
        triggerButtonAction?.action.Enable();
    }

    void OnDisable()
    {
        if (triggerButtonAction != null)
            triggerButtonAction.action.performed -= OnTriggerButonPressed;
        
        triggerButtonAction?.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Club1.SetActive(true);
        Club5.SetActive(false);
        Club9.SetActive(false);
        ClubP.SetActive(false);
        ClubNumText.text = "1";
    }

    private void OnTriggerButonPressed(InputAction.CallbackContext context)
    {
        if (Club1.activeInHierarchy)
        {
            Club1.SetActive(false);
            Club5.SetActive(true);
            ClubNumText.text = "5";
        }
        else if (Club5.activeInHierarchy)
        {
            Club5.SetActive(false);
            Club9.SetActive(true);
            ClubNumText.text = "9";
        }
        else if (Club9.activeInHierarchy)
        {
            Club9.SetActive(false);
            ClubP.SetActive(true);
            ClubNumText.text = "P";
        }
        else if (ClubP.activeInHierarchy)
        {
            ClubP.SetActive(false);
            Club1.SetActive(true);
            ClubNumText.text = "1";
        }
    }
}


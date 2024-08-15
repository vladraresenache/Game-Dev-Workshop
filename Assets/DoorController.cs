using UnityEngine;
using UnityEngine.UI;

public class DoorController1 : MonoBehaviour
{
    public Animator animator;
    public InputField codeInputField;
    public Button submitButton;
    public GameObject inputPanel;  // Un obiect care conține atât InputField cât și Button
    private string secretCode = "0610";
    private bool isPanelActive = false;

    void Start()
    {
        // Asigură-te că panoul de input este dezactivat la început
        inputPanel.SetActive(false);

        // Setează listener-ul pentru butonul de submit
        submitButton.onClick.AddListener(CheckCode);
    }

    void Update()
    {
        // Afișează/că ascunde panoul de input la apăsarea tastei "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPanelActive = !isPanelActive;  // Comută starea panoului
            inputPanel.SetActive(isPanelActive);

            if (isPanelActive)
            {
                // Setează focusul pe InputField când panoul este activat
                codeInputField.Select();
                codeInputField.ActivateInputField();
            }
        }
    }

    void CheckCode()
    {
        // Verifică dacă codul introdus este corect
        if (codeInputField.text == secretCode)
        {
            Debug.Log("Cod corect! Deschide ușa.");
            OpenDoor();
        }
        else
        {
            Debug.Log("Cod incorect.");
            // Poți adăuga un mesaj pentru utilizator aici, dacă vrei
        }

        // Resetează câmpul de introducere a codului
        codeInputField.text = "";

        // Ascunde panoul de input după ce codul a fost verificat
        inputPanel.SetActive(false);
        isPanelActive = false;
    }

    void OpenDoor()
    {
        // Setează parametru `isOpen` pe true pentru a declanșa animația de deschidere
        animator.SetBool("isOpen", true);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    public Button[] buttons; // Tableau de boutons � naviguer
    private int currentIndex = 0; // Index du bouton actuellement s�lectionn�
    private EventSystem eventSystem; // R�f�rence � l'EventSystem
    private float lastInputTime; // Dernier moment o� une entr�e a �t� enregistr�e
    public float inputDelay = 0.2f; // D�lai entre les enregistrements d'entr�es pour �viter les sauts rapides

    void Start()
    {
        // Obtenir la r�f�rence � l'EventSystem
        eventSystem = EventSystem.current;

        // Assurez-vous qu'il y a des boutons dans le tableau
        if (buttons.Length > 0)
        {
            // S�lectionner le premier bouton au d�part
            SelectButton(currentIndex);
        }
    }

    void Update()
    {
        // D�tection des entr�es clavier et manette
        float verticalInput = Input.GetAxisRaw("Vertical");
        float dpadVerticalInput = Input.GetAxisRaw("D-Pad Vertical");

        if (Time.time - lastInputTime > inputDelay)
        {
            if (verticalInput > 0 || dpadVerticalInput > 0)
            {
                NavigateUp();
                lastInputTime = Time.time;
            }
            else if (verticalInput < 0 || dpadVerticalInput < 0)
            {
                NavigateDown();
                lastInputTime = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            PressButton();
        }
    }

    void NavigateUp()
    {
        if (buttons.Length == 0) return;

        // D�cr�menter l'index pour passer au bouton pr�c�dent
        currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;

        // S�lectionner le nouveau bouton
        SelectButton(currentIndex);
    }

    void NavigateDown()
    {
        if (buttons.Length == 0) return;

        // Incr�menter l'index pour passer au bouton suivant
        currentIndex = (currentIndex + 1) % buttons.Length;

        // S�lectionner le nouveau bouton
        SelectButton(currentIndex);
    }

    void SelectButton(int index)
    {
        // S�lectionner le bouton dans l'EventSystem
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    void PressButton()
    {
        // Appuyer sur le bouton actuellement s�lectionn�
        buttons[currentIndex].onClick.Invoke();
    }
}

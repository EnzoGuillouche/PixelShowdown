using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    public Button[] buttons; // Tableau de boutons à naviguer
    private int currentIndex = 0; // Index du bouton actuellement sélectionné
    private EventSystem eventSystem; // Référence à l'EventSystem
    private float lastInputTime; // Dernier moment où une entrée a été enregistrée
    public float inputDelay = 0.2f; // Délai entre les enregistrements d'entrées pour éviter les sauts rapides

    void Start()
    {
        // Obtenir la référence à l'EventSystem
        eventSystem = EventSystem.current;

        // Assurez-vous qu'il y a des boutons dans le tableau
        if (buttons.Length > 0)
        {
            // Sélectionner le premier bouton au départ
            SelectButton(currentIndex);
        }
    }

    void Update()
    {
        // Détection des entrées clavier et manette
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

        // Décrémenter l'index pour passer au bouton précédent
        currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;

        // Sélectionner le nouveau bouton
        SelectButton(currentIndex);
    }

    void NavigateDown()
    {
        if (buttons.Length == 0) return;

        // Incrémenter l'index pour passer au bouton suivant
        currentIndex = (currentIndex + 1) % buttons.Length;

        // Sélectionner le nouveau bouton
        SelectButton(currentIndex);
    }

    void SelectButton(int index)
    {
        // Sélectionner le bouton dans l'EventSystem
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    void PressButton()
    {
        // Appuyer sur le bouton actuellement sélectionné
        buttons[currentIndex].onClick.Invoke();
    }
}

using UnityEngine;

public class EnterToMainMenu : MonoBehaviour
{
    public GameObject mainMenu; // Référence à votre menu principal

    void Update()
    {
        if (Input.anyKeyDown)
        {
            mainMenu.SetActive(true); // Affiche le menu principal
            // Vous pouvez également désactiver d'autres éléments de la scène si nécessaire
            gameObject.SetActive(false); // Optionnel: désactive le GameObject actuel
        }
    }
}

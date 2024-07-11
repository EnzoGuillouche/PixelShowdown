using UnityEngine;

public class EnterToMainMenu : MonoBehaviour
{
    public GameObject mainMenu; // R�f�rence � votre menu principal

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            mainMenu.SetActive(true); // Affiche le menu principal
            // Vous pouvez �galement d�sactiver d'autres �l�ments de la sc�ne si n�cessaire
            gameObject.SetActive(false); // Optionnel: d�sactive le GameObject actuel
        }
    }
}

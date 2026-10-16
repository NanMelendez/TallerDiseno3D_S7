using UnityEngine;
using UnityEngine.SceneManagement;

// mi selector de niveles, perdon si algo no jala a la primera

public class ChangePanel : MonoBehaviour
{
    [SerializeField] private GameObject panelNiveles; // el panel con los botones Nivel 1 y Nivel 2
    [SerializeField] private GameObject panelCreditos; // el panel de los creditos

    [SerializeField] private string escenaNivel1 = "Nivel1"; // nombre EXACTO del nivel 1
    [SerializeField] private string escenaNivel2 = "Nivel2"; // igual aquí

    private void Awake()
    {
        VolverMenu(); // al empezar todo apagado, solo se ve el menu
        Debug.Log("Menu inicializado");
    }

    public void MostrarNiveles()
    {
        panelNiveles.SetActive(true); 
        panelCreditos.SetActive(false);
        Debug.Log("Mostrando niveles");
    }

    public void MostrarCreditos()
    {
        panelCreditos.SetActive(true);
        panelNiveles.SetActive(false);
        Debug.Log("Mostrando creditos");
    }

    public void VolverMenu()
    {
        panelNiveles.SetActive(false);
        panelCreditos.SetActive(false);
        Debug.Log("Volviendo al menu");
    }

    public void CargarNivel1()
    {
        SceneManager.LoadScene(escenaNivel1);
        Debug.Log("Cargando nivel 1");
    }

    public void CargarNivel2()
    {
        SceneManager.LoadScene(escenaNivel2);
        Debug.Log("Cargando nivel 2");
    }
}
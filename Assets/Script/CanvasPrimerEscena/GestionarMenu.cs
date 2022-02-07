using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestionarMenu : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] Button botonEmpezar;

    [Header("Botones")]
    [SerializeField] Slider barraDeCarga;

    [Header("PanelCarga")]
    [SerializeField] GameObject carga;
    [SerializeField] Text textoRellenar;
    [SerializeField] int totalTime;

    [Header("Fichero")]
    [SerializeField] TextAsset ficheroALeer;

    [Header("Funcionalidad")]
    [SerializeField] ConvertidorFichero cf;
    [SerializeField] Text ficheroAdvertencia;

    // Start is called before the first frame update
    void Start()
    {
        carga.SetActive(false);
        cf = this.GetComponent<ConvertidorFichero>();
        totalTime = 1000;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void showPanels()
    {
        carga.SetActive(true);
        textoRellenar.text = ficheroALeer.name;
        /*string solucion =*/
      string solucion =  cf.nameAndFile(ficheroALeer) ? "El fichero se creó perfectamete" : "Se va a empezar a cargar el ficher";
        ficheroAdvertencia.text = solucion;
        SceneManager.LoadScene("DefinitivoSinTextBien");
       

    }
 }

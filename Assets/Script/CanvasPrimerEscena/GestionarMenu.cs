using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using TMPro;

public class GestionarMenu : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] GameObject botonEmpezar;
    [SerializeField] GameObject textoEntrada;

    [Header("Funcionalidad")]
    [SerializeField] ConvertidorFichero cf;
    [SerializeField] Text ficheroAdvertencia;
    [SerializeField] List<string> totalAnimaciomacionesNombres = new List<string>();
    [SerializeField] List<string> totalAnimaciomacionesNombresrTr = new List<string>();
    [SerializeField] Dictionary<string, TextAsset> todoTXT = new Dictionary<string, TextAsset>();
    [SerializeField] TextAsset myTxt;
    [SerializeField] string pathTXT = "Assets/TXT/";
    int index = 0;
    [SerializeField] public bool buscado = false;
    // Start is called before the first frame update
    void Start()
    {
        //carga.SetActive(false);
        cf = this.GetComponent<ConvertidorFichero>();
        
        AssetDatabase.Refresh();
        //introducir aquí el nombre
        
        
       
    }
    public void FixedUpdate()
    {
        
    }
    public void encontrarFicheros()
    {
        foreach (TextAsset txt in Resources.FindObjectsOfTypeAll(typeof(TextAsset)) as TextAsset[])
        {
            string nombre = AssetDatabase.GetAssetPath(txt);
            if (nombre.Contains("TXT") && !nombre.Contains("RtR"))
            {
                Debug.Log(nombre);
                string[] valor = nombre.Split('.');
                TextAsset prueba = (TextAsset)AssetDatabase.LoadMainAssetAtPath(valor[0] + "RtR.txt");
                if (prueba == null)
                {
                    todoTXT.Add(txt.name, txt);
                    totalAnimaciomacionesNombres.Add(txt.name);
                }
            }

        }
       
    }
    public void buscarFicheroPorTexto()
    {
        string nuevoTexto = textoEntrada.GetComponent<TMP_InputField>().text;;
        TextAsset lista = (TextAsset)AssetDatabase.LoadMainAssetAtPath(pathTXT + nuevoTexto+".txt");
        Debug.Log(lista);
        encontrarFicheros();
        if (totalAnimaciomacionesNombres.Count == 0)
        {
            buscado = false;
            botonEmpezar.SetActive(false);
            ficheroAdvertencia.text = "No existe ningún fichero Con ese nombre";
        }
        else
        {
            buscado = true;
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
    [MenuItem("Window/Animation Start")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(CopyAnim));

    }
    public void OnGUI()
    {
        if (buscado && totalAnimaciomacionesNombres.Count>0)
        {
            GUILayout.BeginArea(new Rect(100, 50, 100,100));
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Select");
            GUILayout.BeginVertical("Box");
            //guardo el indice de la animación que he seleccionado
            index = GUILayout.SelectionGrid(index, totalAnimaciomacionesNombres.Select(x => x).ToArray(), 1);
            if (GUILayout.Button("Reescribir"))
            {
                Debug.Log("La aniamcion seleccionada es  : " + totalAnimaciomacionesNombres[index]);
               
                myTxt = todoTXT[totalAnimaciomacionesNombres[index]];
                string solucion = cf.nameAndFile(myTxt) ? "El fichero se creó perfectamete" : "No se ha podido crear el fichero";
                ficheroAdvertencia.text = solucion;

                botonEmpezar.SetActive(false);

            }

           
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }


    }
    public void empezar()
    {
        SceneManager.LoadScene("DefinitivoSinTextBien");

    }
}

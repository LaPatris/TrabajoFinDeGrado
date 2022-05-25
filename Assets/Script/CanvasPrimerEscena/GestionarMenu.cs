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
    public string listaAnimaciones="";
    [SerializeField] List<string> totalAnimaciomacionesNombresrTr = new List<string>();
    [SerializeField] List<string> animacionesExistentes = new List<string>();
   [SerializeField] Dictionary<string, TextAsset> todoTXT = new Dictionary<string, TextAsset>();
    [SerializeField] TextAsset myTxt;
    public string pathTXT = "Assets/TXT/";
    public string nombreTxt;
    int index = 0;
    bool yaExite = false;
    [SerializeField] public bool buscado = false;
    // Start is called before the first frame update
    private void Awake()
    {
        LoadString();
    }
    void Start()
    {
           //carga.SetActive(false);
           cf = this.GetComponent<ConvertidorFichero>();
        if (!listaAnimaciones.Equals(""))
        {
            animacionesExistentes = listaAnimaciones.Split('_').ToList();
            //comprobamos que sigan existiendo los ficheros guardados

            for (int i=0; i<animacionesExistentes.Count;i++)
            {
                Debug.Log(animacionesExistentes[i]);
                TextAsset lista = (TextAsset)AssetDatabase.LoadMainAssetAtPath(pathTXT + animacionesExistentes[i]+".txt");
                if (lista == null)
                {
                    Debug.Log("no existe");

                    animacionesExistentes[i] = "";


                }
                
            }
        }

        //una vez revisados volvemos a introducir en la lista
        botonEmpezar.SetActive(false);
        AssetDatabase.Refresh();
        //introducir aquí el nombre
        
        
       
    }
    public void FixedUpdate()
    {

       // listaAnimaciones = "";
    }
    public void encontrarFicheros()
    {
        foreach (TextAsset txt in Resources.FindObjectsOfTypeAll(typeof(TextAsset)) as TextAsset[])
        {
            string nombre = AssetDatabase.GetAssetPath(txt);
            if (nombre.Contains(nombreTxt) && !nombre.Contains("RtR"))
            {
                string[] valor = nombre.Split('.');
                TextAsset prueba = (TextAsset)AssetDatabase.LoadMainAssetAtPath(valor[0] + "RtR.txt");
                //quiere decir que no existe el documento reescrito
                if (prueba == null)
                {
                    //entonces añadimos en el diccionario
                    todoTXT.Add(txt.name, txt);
                    totalAnimaciomacionesNombres.Add(txt.name);
                    buscado = true;
                }
            }

        }
       
    }
    //se llama desde el boton
    public void buscarFicheroPorTexto()
    {
        nombreTxt = textoEntrada.GetComponent<TMP_InputField>().text;
        TextAsset lista = (TextAsset)AssetDatabase.LoadMainAssetAtPath(pathTXT + nombreTxt+".txt" );
        bool existe = false;
        if (lista != null)
        {
            Debug.Log("Existe el fichero");

            TextAsset lista2 = (TextAsset)AssetDatabase.LoadMainAssetAtPath(pathTXT + nombreTxt + "RtR.txt");
            animacionesExistentes = listaAnimaciones.Split('_').ToList();
            if (lista2 != null)
            {
                Debug.Log(lista2.name);
                //ya existe

                Debug.Log(listaAnimaciones+ "todo"+ lista2.name);

                if (listaAnimaciones.Contains(lista.name))
                {
                    Debug.Log("existe");
                    
                }
                else
                {

                    Debug.Log("supuestamente no está escrito el nombre ya"+ lista.name);
                    listaAnimaciones += "_" + lista.name;
                }
                existe = true;
                botonEmpezar.SetActive(true);
                buscado = false;
            }
            else
            {
                encontrarFicheros();
            }
            /*
            foreach (string nombre in animacionesExistentes)
            {
                if (nombre.Equals(lista.name))
                {
                   
                }
            }*/
        }
        else
        {
            ficheroAdvertencia.text = "No existe fichero con ese nombre";

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
            EditorGUILayout.LabelField("Selecciona la animación buscada y pulsa Reescribir");
            GUILayout.BeginVertical("Box");
            //guardo el indice de la animación que he seleccionado
            index = GUILayout.SelectionGrid(index, totalAnimaciomacionesNombres.Select(x => x).ToArray(), 1);
            if (GUILayout.Button("Reescribir"))
            {
                myTxt = todoTXT[totalAnimaciomacionesNombres[index]];
                Debug.Log(myTxt.name);
                string solucion = cf.nameAndFile(myTxt) ? "El fichero se creó perfectamete" : "No se ha podido crear el fichero";
                ficheroAdvertencia.text = solucion;
                botonEmpezar.SetActive(true);
                //metemos el nombre del fichero
                if (!listaAnimaciones.Contains(totalAnimaciomacionesNombres[index]))
                    listaAnimaciones = listaAnimaciones + "_"+totalAnimaciomacionesNombres[index];

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
    public void SaveString(string value)
    {
        PlayerPrefs.SetString("listaAnimaciones", listaAnimaciones);
    }
    public void LoadString()
    {
        listaAnimaciones = PlayerPrefs.GetString("listaAnimaciones");
        
    }
    public void OnDestroy()
    {
        SaveString(listaAnimaciones);
       
    }
}

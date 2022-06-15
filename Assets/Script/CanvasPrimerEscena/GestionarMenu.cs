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
    public ConvertidorFichero cf;
    [SerializeField] Text ficheroAdvertencia;
   public List<string> totalAnimaciomacionesNombres = new List<string>();
    public string listaAnimaciones="";
    [SerializeField] List<string> totalAnimaciomacionesNombresrTr = new List<string>();
    [SerializeField] List<string> animacionesExistentes = new List<string>();
   [SerializeField] Dictionary<string, TextAsset> todoTXT = new Dictionary<string, TextAsset>();
    [SerializeField] TextAsset myTxt;
    public string pathTXT = "Assets/BVH/";
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

                if (!animacionesExistentes[i].Equals(""))
                {
                    string[] lineas = File.ReadAllLines(animacionesExistentes[i]);

                    if (lineas == null)
                    {
                        Debug.Log("no existe");

                        animacionesExistentes[i] = "";


                    }
                    else { Debug.Log("existe"); }
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

       //listaAnimaciones = "";
    }
    public void encontrarFicheros()
    {
        foreach (TextAsset txt in Resources.FindObjectsOfTypeAll(typeof(TextAsset)) as TextAsset[])
        {
            string nombre = AssetDatabase.GetAssetPath(txt);
            Debug.Log("Nombre" + nombre);
            if (nombre.Contains(nombreTxt) )
            {
                string[] valor = nombre.Split('.');
                Debug.Log(valor[0]+ valor[1]);
                TextAsset prueba = (TextAsset)AssetDatabase.LoadMainAssetAtPath(valor[0]+ ".bvh");

                string[] lista = File.ReadAllLines(valor[0]+valor[1]);
                //quiere decir que no existe el documento reescrito
                if (lista == null)
                {
                    //entonces añadimos en el diccionario
                    todoTXT.Add(txt.name, txt);
                    bool tiene = false;
                    foreach(string s in totalAnimaciomacionesNombres)
                    {
                        if (s.Equals(txt.name))
                            tiene = true;
                    }
                    if(tiene==false)
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

        string[] lista = File.ReadAllLines(pathTXT + nombreTxt + ".bvh");
        Debug.Log("que es"+ pathTXT + nombreTxt + ".bvh");
        bool existe = false;
        if (lista != null)
        {
            Debug.Log("Existe el fichero");

            animacionesExistentes = listaAnimaciones.Split('_').ToList();

            if (listaAnimaciones.Contains(nombreTxt))
            {
                Debug.Log("existe");

            }
            else
            {

                Debug.Log("supuestamente no está escrito el nombre ya" + pathTXT + nombreTxt + ".bvh");
                listaAnimaciones += "_" + pathTXT + nombreTxt + ".bvh";
                //orgDatos.SetListBones(myTxt, personaje);
            }
            existe = true;
            botonEmpezar.SetActive(true);
            buscado = false;
        }
        /*  else
          {
              encontrarFicheros();
          }*/
    
        
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
            GUILayout.BeginArea(new Rect(510, 270, 100,100));
            GUILayout.FlexibleSpace();
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

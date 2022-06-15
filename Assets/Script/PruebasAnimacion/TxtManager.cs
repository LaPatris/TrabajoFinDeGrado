using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TxtManager : MonoBehaviour
{
    // objeto de clase organizar datos

    [SerializeField] OrganizarDatosFile orgDatos;
    [SerializeField] UImanager uiManager;

    //[SerializeField] AngleCurveCreator curva;
    //[SerializeField] AngleCurveCreator curva;
    // texto seleccionado
    [SerializeField] TextAsset myTxt;
    [SerializeField] string nombreTxt;
    [SerializeField] string pathTXT = "Assets/BVH/";
    // si se ha finalizado la lectura
    [SerializeField] public bool finalizado = false;

    [SerializeField] public bool buscado = false;
    // el personaje a que se le va a aplicar la animación 
    public GameObject personaje;
   // [SerializeField] CopyAnim1 copyAnimacion;
   // clase donde se hace el retargeting
    public animacion copyA;
    // personaje elegido
   //GameObject elegido;
    [SerializeField] GestionarMenu menu;

    public animacionPersonaje animP = new animacionPersonaje();
    public FileReader file = new FileReader();

    int index = 0;
    // las animaciones
    [SerializeField] List<AnimationClip> totalAnimaciomaciones = new List<AnimationClip>();
    //el nomrbe de las animaciones
    public List<string> totalAnimaciomacionesNombres = new List<string>();
    public List<string> totalAnimaciomacionesPath = new List<string>();
   // public Dictionary<string, TextAsset> todoTXT = new Dictionary<string, TextAsset>();
    // Start is called before the first frame update
    void Start()
    {
        AssetDatabase.Refresh();
        List<string> prueba = new List<string>();
        if (!menu.listaAnimaciones.Equals(""))
        {
            prueba = menu.listaAnimaciones.Split('_').ToList();
            Debug.Log(menu.listaAnimaciones);
            foreach(string p in prueba)
            {

               if (!p.Equals(""))
                { string[] lineas = File.ReadAllLines(p);
                    if(lineas!=null)
                    {
                       if(!totalAnimaciomacionesPath.Contains(p))
                        {
                            totalAnimaciomacionesPath.Add(p);
                           List< string>nombres= p.Split('/').ToList();
                            totalAnimaciomacionesNombres.Add(nombres[nombres.Count-1]);
                        }
            
                    }
                }
            }
            Debug.Log(prueba);

        }

                    buscado = true;
        
       // orgDatos = new OrganizarDatosFile();
       // personaje = GameObject.Find("P4DEF");
        //copyAnimacion = personaje.GetComponent<CopyAnim1>();
        // curva = this.gameObject.GetComponent<AngleCurveCreator>();
        // orgDatos.SetListBones(myTxt, curva, personaje);
        //llama para crear el diccionario de las animaciones 


    }

    [MenuItem("Window/Animation Copier")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(CopyAnim));

    }

   /* public void entrada(float timeValue)
    {
        selectedTime = timeValue;
    }
    public bool selectTime()
    {
        return selectedTime == 0.0f ? false : true;

    }*/
    public void OnGUI()
    {
        if ( buscado && personaje!=null)
        {
            
            GUILayout.BeginVertical("Box");
            //guardo el indice de la animación que he seleccionado
            index = GUILayout.SelectionGrid(index, totalAnimaciomacionesNombres.Select(x => x).ToArray(), 1);
            if (GUILayout.Button("Copy") )
            {
                //elegido.active = false;
                // si doy a copiar guardo la animacion
                //selectedAnimationClip = animationClips[index];
                Debug.Log("La aniamcion seleccionada es  : " + totalAnimaciomacionesNombres[index]);

                // myTxt = todoTXT[totalAnimaciomacionesNombres[index]];
                //orgDatos.SetListBones(myTxt, personaje);
                Debug.Log(totalAnimaciomacionesPath[index]);
                file.pathFile = totalAnimaciomacionesPath[index];
            
                file.animP = personaje.GetComponent<animacionPersonaje>();
                file.animP.transform.rotation = file.animP.rotacion.rotation;
                file.obtenerDatosFichero();

                uiManager.botonMas.SetActive(true);
                //file.obtenerEscalaHueso();
                //si se ha terminado de organizar los datos en un diccionario 
                if (file.terminado)
                {
                    //personaje.transform.rotation = personaje.GetComponent<animacionPersonaje>().selfInitRotation;
                    //totalAnimaciomaciones.Add(curva.animacionFinal);
                    //totalAnimaciomaciones.Add(curva.animacionBezierHueso);
                    animP.initAll(file.rotaciones);
                }
                    //copyA.initAll(orgDatos.totalBody);

               // }
                //llamo a la accion de cambiar de estado
                //copiar la animación por defecto en otra animación 
                //copyAnimacion.ReadMyAnimAndChange();
                //copyAnimacion.ChangeToMyAnim(totalAnimaciomaciones[index], selectedTime);
                //copyAnimacion.ChangeToMyAnim(orgDatos.totalBody, selectedTime);
           
                /*if (!copyAnimacion.creadoStado)
                { //si no habia estado creado lo creo
                    copyAnimacion.CreateNewStateAndConexion();
                }
                //cambio el valor del estado
                copyAnimacion.ChangeStateValue();*/

            }

            if (GUILayout.Button("Remove"))
            {// si damos a remove solo si el estado esta creado eliminamos el estado y la transición 
                file.animP.borrado();
                file.animP.setEjecutar(false);
                file.borrado();


              //  uiManager.panel.SetActive(false);
                //uiManager.botonMas.SetActive(false);
            }
            GUILayout.EndVertical();

        }
        

    }
}


/* void Start()
    {
        AssetDatabase.Refresh();
        List<string> prueba = new List<string>();
        if (!menu.listaAnimaciones.Equals(""))
        {
            prueba = menu.listaAnimaciones.Split('_').ToList();
            Debug.Log(menu.listaAnimaciones);
            foreach(string p in prueba)
            {

                TextAsset lista = (TextAsset)AssetDatabase.LoadMainAssetAtPath(menu.pathTXT +p+ "RtR.txt");
            }
            Debug.Log(prueba);
            
        }
   
          foreach (TextAsset txt in Resources.FindObjectsOfTypeAll(typeof(TextAsset)) as TextAsset[])
          {
              string nombre =AssetDatabase.GetAssetPath(txt);
              if (nombre.Contains(nombreTxt) && nombre.Contains("RtR") )
              { todoTXT.Add(txt.name, txt);
                totalAnimaciomacionesNombres.Add(txt.name);
              }

          }
          buscado = true;
        
        orgDatos = new OrganizarDatosFile();
       // personaje = GameObject.Find("P4DEF");
        //copyAnimacion = personaje.GetComponent<CopyAnim1>();
        // curva = this.gameObject.GetComponent<AngleCurveCreator>();
        // orgDatos.SetListBones(myTxt, curva, personaje);
        //llama para crear el diccionario de las animaciones 


    }

 
 public void OnGUI()
    {
        if ( buscado && personaje!=null)
        {
            
            GUILayout.BeginVertical("Box");
            //guardo el indice de la animación que he seleccionado
            index = GUILayout.SelectionGrid(index, totalAnimaciomacionesNombres.Select(x => x).ToArray(), 1);
            if (GUILayout.Button("Copy") )
            {
                //elegido.active = false;
                // si doy a copiar guardo la animacion
                //selectedAnimationClip = animationClips[index];
                Debug.Log("La aniamcion seleccionada es  : " + totalAnimaciomacionesNombres[index]);
               
                myTxt = todoTXT[totalAnimaciomacionesNombres[index]];
                orgDatos.SetListBones(myTxt, personaje);
                //si se ha terminado de organizar los datos en un diccionario 
                if (orgDatos.finalizado)
                {
                    //totalAnimaciomaciones.Add(curva.animacionFinal);
                    //totalAnimaciomaciones.Add(curva.animacionBezierHueso);

                    copyA.initAll(orgDatos.totalBody);

                }
                //llamo a la accion de cambiar de estado
                //copiar la animación por defecto en otra animación 
                //copyAnimacion.ReadMyAnimAndChange();
                //copyAnimacion.ChangeToMyAnim(totalAnimaciomaciones[index], selectedTime);
                //copyAnimacion.ChangeToMyAnim(orgDatos.totalBody, selectedTime);
           
                /*if (!copyAnimacion.creadoStado)
                { //si no habia estado creado lo creo
                    copyAnimacion.CreateNewStateAndConexion();
                }
                //cambio el valor del estado
                copyAnimacion.ChangeStateValue();

            }
            
            if (GUILayout.Button("Remove"))
{// si damos a remove solo si el estado esta creado eliminamos el estado y la transición 
    copyA.setEjecutar(false);
    copyA.borrado();
    orgDatos.borrado();

    uiManager.panel.SetActive(false);
    uiManager.botonMas.SetActive(false);
    personaje.transform.position = copyA.posicionInicial;
}
GUILayout.EndVertical();

        }
        

    }
}
 void Start()
    {
       // AssetDatabase.Refresh();
        List<string> prueba = new List<string>();
        if (!menu.listaAnimaciones.Equals(""))
        {
            prueba = menu.listaAnimaciones.Split('_').ToList();
            Debug.Log(menu.listaAnimaciones);
            foreach(string p in prueba)
            {

                TextAsset lista = (TextAsset)AssetDatabase.LoadMainAssetAtPath(menu.pathTXT +p);
            }
            Debug.Log(prueba);
            
        }
   
        buscado = true;
        
        orgDatos = new OrganizarDatosFile();
       // personaje = GameObject.Find("P4DEF");
        //copyAnimacion = personaje.GetComponent<CopyAnim1>();
        // curva = this.gameObject.GetComponent<AngleCurveCreator>();
        // orgDatos.SetListBones(myTxt, curva, personaje);
        //llama para crear el diccionario de las animaciones 


    }

    [MenuItem("Window/Animation Copier")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(CopyAnim));

    }

   /* public void entrada(float timeValue)
    {
        selectedTime = timeValue;
    }
    public bool selectTime()
    {
        return selectedTime == 0.0f ? false : true;

    }
public void OnGUI()
{
    if (buscado && personaje != null)
    {

        GUILayout.BeginVertical("Box");
        //guardo el indice de la animación que he seleccionado
        index = GUILayout.SelectionGrid(index, totalAnimaciomacionesNombres.Select(x => x).ToArray(), 1);
        if (GUILayout.Button("Copy"))
        {
            //elegido.active = false;
            // si doy a copiar guardo la animacion
            //selectedAnimationClip = animationClips[index];
            Debug.Log("La aniamcion seleccionada es  : " + totalAnimaciomacionesNombres[index]);

            myTxt = todoTXT[totalAnimaciomacionesNombres[index]];
            //orgDatos.SetListBones(myTxt, personaje);
            file.obtenerDatosFichero();
            //file.obtenerEscalaHueso();
            //si se ha terminado de organizar los datos en un diccionario 
            if (orgDatos.finalizado)
            {
                //totalAnimaciomaciones.Add(curva.animacionFinal);
                //totalAnimaciomaciones.Add(curva.animacionBezierHueso);

                animP.initAll(file.rotaciones);
                //copyA.initAll(orgDatos.totalBody);

            }
            //llamo a la accion de cambiar de estado
            //copiar la animación por defecto en otra animación 
            //copyAnimacion.ReadMyAnimAndChange();
            //copyAnimacion.ChangeToMyAnim(totalAnimaciomaciones[index], selectedTime);
            //copyAnimacion.ChangeToMyAnim(orgDatos.totalBody, selectedTime);

            /*if (!copyAnimacion.creadoStado)
            { //si no habia estado creado lo creo
                copyAnimacion.CreateNewStateAndConexion();
            }
            //cambio el valor del estado
            copyAnimacion.ChangeStateValue();

        }

        if (GUILayout.Button("Remove"))
        {// si damos a remove solo si el estado esta creado eliminamos el estado y la transición 
            copyA.setEjecutar(false);
            copyA.borrado();
            orgDatos.borrado();

            uiManager.panel.SetActive(false);
            uiManager.botonMas.SetActive(false);
            personaje.transform.position = copyA.posicionInicial;
        }
        GUILayout.EndVertical();

    }


}
}


/* void Start()
    {
        AssetDatabase.Refresh();
        List<string> prueba = new List<string>();
        if (!menu.listaAnimaciones.Equals(""))
        {
            prueba = menu.listaAnimaciones.Split('_').ToList();
            Debug.Log(menu.listaAnimaciones);
            foreach(string p in prueba)
            {

                TextAsset lista = (TextAsset)AssetDatabase.LoadMainAssetAtPath(menu.pathTXT +p+ "RtR.txt");
            }
            Debug.Log(prueba);
            
        }
   
          foreach (TextAsset txt in Resources.FindObjectsOfTypeAll(typeof(TextAsset)) as TextAsset[])
          {
              string nombre =AssetDatabase.GetAssetPath(txt);
              if (nombre.Contains(nombreTxt) && nombre.Contains("RtR") )
              { todoTXT.Add(txt.name, txt);
                totalAnimaciomacionesNombres.Add(txt.name);
              }

          }
          buscado = true;
        
        orgDatos = new OrganizarDatosFile();
       // personaje = GameObject.Find("P4DEF");
        //copyAnimacion = personaje.GetComponent<CopyAnim1>();
        // curva = this.gameObject.GetComponent<AngleCurveCreator>();
        // orgDatos.SetListBones(myTxt, curva, personaje);
        //llama para crear el diccionario de las animaciones 


    }

 
 public void OnGUI()
    {
        if ( buscado && personaje!=null)
        {
            
            GUILayout.BeginVertical("Box");
            //guardo el indice de la animación que he seleccionado
            index = GUILayout.SelectionGrid(index, totalAnimaciomacionesNombres.Select(x => x).ToArray(), 1);
            if (GUILayout.Button("Copy") )
            {
                //elegido.active = false;
                // si doy a copiar guardo la animacion
                //selectedAnimationClip = animationClips[index];
                Debug.Log("La aniamcion seleccionada es  : " + totalAnimaciomacionesNombres[index]);
               
                myTxt = todoTXT[totalAnimaciomacionesNombres[index]];
                orgDatos.SetListBones(myTxt, personaje);
                //si se ha terminado de organizar los datos en un diccionario 
                if (orgDatos.finalizado)
                {
                    //totalAnimaciomaciones.Add(curva.animacionFinal);
                    //totalAnimaciomaciones.Add(curva.animacionBezierHueso);

                    copyA.initAll(orgDatos.totalBody);

                }
                //llamo a la accion de cambiar de estado
                //copiar la animación por defecto en otra animación 
                //copyAnimacion.ReadMyAnimAndChange();
                //copyAnimacion.ChangeToMyAnim(totalAnimaciomaciones[index], selectedTime);
                //copyAnimacion.ChangeToMyAnim(orgDatos.totalBody, selectedTime);
           
                /*if (!copyAnimacion.creadoStado)
                { //si no habia estado creado lo creo
                    copyAnimacion.CreateNewStateAndConexion();
                }
                //cambio el valor del estado
                copyAnimacion.ChangeStateValue();

            }
            
            if (GUILayout.Button("Remove"))
{// si damos a remove solo si el estado esta creado eliminamos el estado y la transición 
    copyA.setEjecutar(false);
    copyA.borrado();
    orgDatos.borrado();

    uiManager.panel.SetActive(false);
    uiManager.botonMas.SetActive(false);
    personaje.transform.position = copyA.posicionInicial;
}
GUILayout.EndVertical();

        }
        

    }
}
 
 */
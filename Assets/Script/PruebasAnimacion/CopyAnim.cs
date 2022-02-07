
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System;
using Object = UnityEngine.Object;

using UnityEditor;

using System.Linq;
using System.IO;

public class CopyAnim : MonoBehaviour
{



    //need for animation

    [Header("Animations")]

    [SerializeField] AnimationClip animationClipEmpty;// animación vacía que rellenaré
    //lista de los datos de la animación que vamos a seleccionar
    private static List<AnimationClipCurveData> animationCurveClipboard = new List<AnimationClipCurveData>();
    private static List<AnimationClipCurveData> animationCurveClipboard2 = new List<AnimationClipCurveData>();
    private static List<AnimationClipCurveData> aux = new List<AnimationClipCurveData>();

    [Header("Others")]
    [SerializeField] AnimationCurve curve; // curva de animacion
    [SerializeField] public bool changeAnim = false;// booleana para comprobar si se han realizado cambios de estado
    [SerializeField] AnimatorState currentState; // estado al que voy a cambuar
    [SerializeField] AnimatorStateTransition newTransition; // transición 
    [SerializeField] AnimatorController animatorController; //el animator controller que es necesario 
    [SerializeField] AnimatorState defaultState; // el prmer estado por defecto del animator
    [SerializeField] public bool creadoStado = false; // para saber si se ha creado el nuevo estado de la animación
    //object prueba 
    PruebaFBX prueba;
    [Header("MyObj ")]

    [SerializeField] Dictionary<string, AnimationCurve> animClipCurvD = new Dictionary<string, AnimationCurve>();
    [SerializeField] List<string> nameCurves = new List<string>();
    [SerializeField] List<AnimationCurve> curvasAnim = new List<AnimationCurve>();
    [SerializeField] Vector3 initPosition; //init myobj positi
    public List<AnimationClip> animationClips;// lista de animaciones para el edior
    [SerializeField] AnimationClip curveBezier;
    [SerializeField] OrganizarDatosFile txtFile;
    [SerializeField] String path;
    [SerializeField] Type myType;

    [SerializeField] RunTimeChangePosition changePosition;

    private AnimationClip selectedAnimationClip;//para guardara la animacion seleccionada
    private TxtManager txtmanager;

    int index = 0;
    void Start()
    {
        animationClips = Resources.FindObjectsOfTypeAll<AnimationClip>().ToList();
        //creación de la curva de bezier
        txtFile = new OrganizarDatosFile();
        //leo todas las animaciones que tengo
        // animationReader = this.GetComponent<Animation>();
        prueba = GetComponent<PruebaFBX>();
        // creo el objeto prueba( en esta clase creo el avatar, guardo el animator controller y más cosas)
        //curveBezier = this.GetComponent<BezierCurve>();
        txtmanager = GameObject.Find("txt").GetComponent<TxtManager>();
        animatorController = prueba.anim;
        aux = AnimationUtility.GetAllCurves(prueba.animacion, true).ToList();
        myType = aux[0].type;
        if (txtmanager.finalizado)
        ReadMyAnimAndChange(curveBezier);

        // path = "Assets/TXT/Lectura.txt";
    }
    private void CreateNewStateAndConexion()
    {

        //selecciono el animator de prueba( el animator ya está guardado ahí)
        defaultState = animatorController.layers[0].stateMachine.states[0].state;// inicializo el estado incial

        currentState = new AnimatorState();//creo un nuev estado

        currentState = animatorController.layers[0].stateMachine.AddState(animationClipEmpty.name);//relleno el nuevo estado

        newTransition = new AnimatorStateTransition();//creo la transicion
                                                      //con sus valores
        newTransition.destinationState = animatorController.layers[0].stateMachine.states[1].state;

        defaultState.AddTransition(newTransition);

        AssetDatabase.SaveAssets();
        creadoStado = true;
        //se ha creado el estado 



    }

    private void ChangeStateValue()
    {
        currentState.motion = animationClipEmpty;//cambiamos la animación del estado 
        AssetDatabase.SaveAssets();
        changeAnim = true;//la animacion se ha cambiado

    }
    private void ReadMyAnimAndChange(AnimationClip animation)
    {
        //si ya existe la animación( es decir se ha elegido otra antes)
        if (changeAnim)
        {
            animationClipEmpty.ClearCurves();
            //vacio la animación
        }
        animationClipEmpty.wrapMode = WrapMode.Loop;
        // guardo toda la información de las curvas en una lista
        animationCurveClipboard = AnimationUtility.GetAllCurves(animation, true).ToList();

        

        //List<AnimationClipCurveData> ab = AnimationUtility.GetAllCurves(otraAnim, true).ToList();
        // Debug.Log("que tengo" + ab.Count);
        // Esto es para modificar solo una parte de la animación ( es decir tener dos animaciones)
        //animationCurveClipboard2= AnimationUtility.GetAllCurves(animationClips[2], true).ToList();
        int i = 0;
        // aquí tiene que ir la lista de todas las curvas
        Debug.Log(animationCurveClipboard.Count + " este es el numero deanimaciones que tiene animation ");
        foreach (AnimationClipCurveData data in animationCurveClipboard /*aux*/)
        {
            /*
            foreach (Keyframe k in data.curve.keys)
            {
                data.curve.RemoveKey(0);
  
            }
            foreach (AnimationClipCurveData datos in animationCurveClipboard)
            {
                foreach (Keyframe key in datos.curve.keys)
                {
                    data.curve.AddKey(key);

                }
            }*/
            //Para cambiar solo una parte también de la animación 
            /*  if (data.propertyName.Contains("LeftArm"))
              {
                  Debug.Log("que tengo" + ab.Count);
                  //AnimationCurve auxiliar= animationCurveClipboard2[i].curve;
                  data.curve = animationCurveClipboard[i].curve;
                  //curva auxiliar para guardar los valores de cada propiedad de LeftArm en este caso
                  AnimationCurve curve = new AnimationCurve();
                  //cambiar esto por la curva de la qeu vamos a sacar la animación

                  foreach (Keyframe key in ab[0].curve.keys)
                  {
                      /*escribir los valores en un txt
                      StreamWriter writer = new StreamWriter(path, true);
                      writer.WriteLine(data.propertyName+" "+key.time+" "+ data.curve.Evaluate(key.time));
                      writer.Close();*/
            //añadimos todos los datos a la curva
            /*float t = 2.3f;
            Debug.Log(data.propertyName + t + " " + data.curve.Evaluate(t));
            // de bezier
            curve.AddKey(key.time, ab[0].curve.Evaluate(key.time));

        }*/

            /*else
                   {
            AnimationCurve curve = new AnimationCurve();
            foreach (Keyframe key in data.curve.keys)
            {

                curve.AddKey(key.time, data.curve.Evaluate(key.time));

            }*/

            //}
            AnimationCurve curve = new AnimationCurve();
                  foreach (Keyframe key in data.curve.keys)
                    {
                        curve.AddKey(key.time, data.curve.Evaluate(key.time));

                    }

                //lo metemos en la animación
                //si en vez de data form ponemos tranforms salen los mismo valores
                // if(J<1)
                animationClipEmpty.SetCurve(data.path.ToString() + ": ", myType, data.propertyName, curve);
            //J++;


            //para setear la curva
            //  animationClipEmpty.SetCurve(this.name + ": ", data.type, data.propertyName, data.curve);
            //ver si contiene ya el nombre
            /* if (!animClipCurvD.ContainsKey(data.propertyName))
             {// meter en el diccionario los datos 
                 animClipCurvD.Add(data.propertyName, data.curve);
                 curvasAnim.Add(data.curve);
                 nameCurves.Add(data.propertyName);
                 Debug.Log("datos");
             }*/


            /* AnimationCurve curvita= animClipCurvD.Values.ElementAt(1);
              Debug.Log("numero de keys de  la curva " + curvita.keys.Length);
              curvita.RemoveKey(10);
              Debug.Log("numero de resta de keys de  la curva " + curvita.keys.Length);

              curvita.AddKey(0, 3.5f);*/
        }
    }

    //GUI
    
    [MenuItem("Window/Animation Copier")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(CopyAnim));

    }
    public void OnGUI()
    {
        EditorGUILayout.LabelField("Select");

        GUILayout.BeginVertical("Box");
        //guardo el indice de la animación que he seleccionado
        index = GUILayout.SelectionGrid(index, animationClips.Select(x => x.name).ToArray(), 1);
        if (GUILayout.Button("Copy"))
        {// si doy a copiar guardo la animacion
            selectedAnimationClip = animationClips[index];
            Debug.Log("indice " + index);
            Debug.Log("La aniamcion seleccionada es  : " + selectedAnimationClip.name);
            //llamo a la accion de cambiar de estado
           // ReadMyAnimAndChange(selectedAnimationClip);
           
            if (!creadoStado)
            { //si no habia estado creado lo creo
                CreateNewStateAndConexion();
            }
            //cambio el valor del estado
            ChangeStateValue();

        }
        if (GUILayout.Button("Remove"))
        {// si damos a remove solo si el estado esta creado eliminamos el estado y la transición 
            if (creadoStado)
                RemoveState();
        }
        GUILayout.EndVertical();

    }

    //metodo de eliminar estado
    public void RemoveState()
    {
        //eliminamos la transición 

        newTransition = new AnimatorStateTransition();
        newTransition.destinationState = animatorController.layers[0].stateMachine.states[0].state;


        defaultState.RemoveTransition(newTransition);
        //eliminamos el estado 
        animatorController.layers[0].stateMachine.RemoveState(currentState);

        creadoStado = false;

    }
}

/*
 Metiendo directamente el baile
 
 private void ReadMyAnimAndChange(AnimationClip animation)
    {
        //si ya existe la animación( es decir se ha elegido otra antes)
        if(changeAnim)
        {
            animationClipEmpty.ClearCurves();
            //vacio la animación
        }
        animationClipEmpty.wrapMode = WrapMode.Loop;
       // guardo toda la información de las curvas en una lista
        animationCurveClipboard = AnimationUtility.GetAllCurves(animation, true).ToList();

        //aquí tiene que ir la lista de todas las curvas
        foreach (AnimationClipCurveData data in animationCurveClipboard)
        {
            AnimationCurve curve = new AnimationCurve();
           
        foreach (Keyframe key in data.curve.keys)
        {
          curve.AddKey(key.time, data.curve.Evaluate(key.time));

        }
            animationClipEmpty.SetCurve(this.name + ": ", data.type, data.propertyName, curve);
             
        }
 
 */

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


public class CopyAnim1 : MonoBehaviour
{

    //need for animation
    /*
    [Header("Animations")]
    [SerializeField] AnimationClip animationClipEmpty;// animación vacía que rellenaré
    [SerializeField] AnimationClip animationSelf;//MI ANIMACION


    //lista de los datos de la animación que vamos a seleccionar
    [SerializeField] private static List<AnimationClipCurveData> animacionLeida = new List<AnimationClipCurveData>();
    [SerializeField] private static List<AnimationClipCurveData> animacionFutura = new List<AnimationClipCurveData>();


    [Header("Others")]

    [SerializeField] public bool changeAnim = false;// booleana para comprobar si se han realizado cambios de estado
    [SerializeField] AnimatorState currentState; // estado al que voy a cambuar
    [SerializeField] AnimatorStateTransition newTransition; // transición 
    [SerializeField] AnimatorController animatorController; //el animator controller que es necesario 
    [SerializeField] AnimatorState defaultState; // el prmer estado por defecto del animator
    [SerializeField] public bool creadoStado = false; // para saber si se ha creado el nuevo estado de la animación
                                                      //object prueba 
    [SerializeField] Transform myHips;
    [SerializeField] List<Transform> misHuesos;

    //copiamos nuestra animacion en la futura
    public void ReadMyAnimAndChange()
    {/*
        //sObject.Instantiate(aniationSelf);
        // SE VA COPIAR LA ANIMACIÓN DEL PERSONAJE EN VACIO
       animacionFutura = AnimationUtility.GetAllCurves(animationSelf, true).ToList();
        AnimationCurve auxAnim = AnimationCurve.EaseInOut(0, 0, 0, 0);
        auxAnim.preWrapMode = WrapMode.Loop;
        //aniationSelf.ClearCurves();
        foreach (AnimationClipCurveData data in animacionFutura)
        {
            animationClipEmpty.SetCurve(data.path.ToString(), data.type, data.propertyName, data.curve);

        }*/
    }
    //private void SetNewCurve(float temp, float def, AnimationCurve auxAnim)
    /*private void SetNewCurve(float temp, Vector3 def, AnimationCurve auxAnim)
    {
        auxAnim.AddKey(temp, def.magnitude);
     }
   /* private void setearCurve(string nombre, AnimationCurve auxAnim)
    {
        //copia la animación de 
        animacionFutura = AnimationUtility.GetAllCurves(animationSelf, true).ToList();
        foreach (AnimationClipCurveData datos in animacionFutura)
        {
            String[] datosPath = datos.path.Split('/');
            if (datosPath[datosPath.Length - 1].Equals(nombre))
            {//copiamos el nombre, el property name y la animación ponemos la que no sllega por parametro
                //parece que está normalizado por qué???

               // animationClipEmpty.SetCurve(datos.path, datos.type, datos.propertyName, auxAnim);
               
                animationClipEmpty.SetCurve(datos.path, datos.type, /*datos.propertyName"a", auxAnim);

                break;

            }
        }
    }*/
    //modificamos los valores en función de lo que tiene la animación de lo que hemos leido
  /*/*  public void ChangeToMyAnim(Dictionary<String,List<Vector3>>totalBody, float selectedTime)
    {
        int i = 0;
        float timeXFrame = 0;
        float timeD = 0;
        Quaternion antiguoRotHips = myHips.rotation;
        animacionFutura = AnimationUtility.GetAllCurves(animationSelf, true).ToList();
        foreach (KeyValuePair<string, List<Vector3>> hueso in totalBody)
        {       if(i==0)
                 timeXFrame = selectedTime / hueso.Value.Count;
            i++;//por cada curva dentro de la animación 
            //creamos una animación de curva auxiliar que va del 0 0 al 1,0 
         
            //Debug.Log("La curva:" + data.path);
            foreach (AnimationClipCurveData datos in animacionFutura)
            {
                String[] datosPath = datos.path.Split('/');
                if (datosPath[datosPath.Length - 1].Equals(hueso.Key))
                {//copiamos el nombre, el property name y la animación ponemos la que no sllega por parametro
                    var binding = EditorCurveBinding.FloatCurve(datos.path, typeof(Transform), datos.propertyName);
                    AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClipEmpty, binding);
                    curve = AnimationCurve.EaseInOut(0, 0, 1, 0);
                    curve.preWrapMode = WrapMode.Loop;
                    for (int pos=0; pos<hueso.Value.Count;pos++)
                    {
                        Keyframe key = curve.keys[pos];
                        Quaternion currentRotation;
                        float x=  Vector3.Angle(hueso.Value[pos], Vector3.left);

                        float y = Vector3.Angle(hueso.Value[pos], Vector3.up);

                        float z = Vector3.Angle(hueso.Value[pos], Vector3.forward);
                        Vector3  currentEulerAngles = new Vector3(x, y, z);


                        Quaternion nuevaRot = new Quaternion();
                        nuevaRot.eulerAngles = currentEulerAngles;
                        Debug.Log("rotx "+nuevaRot.eulerAngles.x+" roty "+ nuevaRot.eulerAngles.y+" rotz "+ nuevaRot.eulerAngles.z);
                        myHips.rotation = nuevaRot;
                        key.value =myHips.eulerAngles.magnitude;
                        key.time = timeD;
                        curve.AddKey(key);
                        timeD += timeXFrame;
                    }

                    AnimationUtility.SetEditorCurve(animationClipEmpty, binding, curve);

                    curve = null;

                    break;

                }
            }
            timeD = 0;
            
        }
       
       
    }*/
 /*   public void ChangeToMyAnim(/*AnimationClip animacionNuevaDictionary<String, List<Vector3>> totalBody, float selectedTime)
    {
        //gardamos todas las animaciones en animacion leida
        //animacionLeida = AnimationUtility.GetAllCurves(animacionNueva, true).ToList();
        //calculamos el tiempo por frame en función del tiempo seleccionado
        float timeXFrame = selectedTime / animacionLeida[0].curve.keys.Length;
        //seteamos la cadera por defecto
        //animHips = new Vector3(-233.588f, 66.7051f, 935.858f);
        //calculamos la distancia entre la cadeta de la animacion que leemos y la de nuestro objeto
        //float distancia = Vector3.Distance(animHips, myHips.position);
        //foreach (AnimationClipCurveData data in animacionLeida)

        animacionFutura = AnimationUtility.GetAllCurves(animationSelf, true).ToList();
        foreach (KeyValuePair<string, List<Vector3>> hueso in totalBody)
        {//por cada curva dentro de la animación 
            //creamos una animación de curva auxiliar que va del 0 0 al 1,0 
            AnimationCurve auxAnim = AnimationCurve.EaseInOut(0, 0, 1, 0);
            auxAnim.preWrapMode = WrapMode.Loop;
            Debug.Log("vamos a evaluar cada uno de los datos que se encuentran en la animación leida");
            //Debug.Log("La curva:" + data.path);
            foreach (AnimationClipCurveData datos in animacionFutura)
            {
                String[] datosPath = datos.path.Split('/');
                if (datosPath[datosPath.Length - 1].Equals(hueso.Key))
                {//copiamos el nombre, el property name y la animación ponemos la que no sllega por parametro
                 //parece que está normalizado por qué???
                    foreach (Vector3 vectores in hueso.Value)
                    {//añadimos el valor en nuestra animacion auxiliar
                        SetNewCurve(timeXFrame, vectores, auxAnim);
                    }
                    // animationClipEmpty.SetCurve(datos.path, datos.type, datos.propertyName, auxAnim);

                    animationClipEmpty.SetCurve(datos.path, datos.type, datos.propertyName, auxAnim);

                    break;

                }
            }
            /* foreach (Keyframe key in data.curve.keys)
             {//añadimos el valor en nuestra animacion auxiliar
                 SetNewCurve(key.time, key.value, auxAnim);
             }
            //esto funciona, el problema es que nos pone las curvas al final de las cosas 
            String[] subString = data.path.Split(':');
            String nombreCurva = subString[0];
            //llamamos a setear la curva
            setearCurve(nombreCurva, data.curve);
            auxAnim = null;
        }

    }*/
   /* public void CreateNewStateAndConexion()
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

    public void ChangeStateValue()
    {
        //currentState.motion = animationClipEmpty;//cambiamos la animación del estado 
        AssetDatabase.SaveAssets();
        changeAnim = true;//la animacion se ha cambiado

    }

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
}*/


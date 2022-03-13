
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
    {
        //sObject.Instantiate(aniationSelf);
        // SE VA COPIAR LA ANIMACIÓN DEL PERSONAJE EN VACIO
       /* animacionFutura = AnimationUtility.GetAllCurves(aniationSelf, true).ToList();
        AnimationCurve auxAnim = AnimationCurve.EaseInOut(0, 0, 0, 0);
        auxAnim.preWrapMode = WrapMode.Loop;
        //aniationSelf.ClearCurves();
        foreach (AnimationClipCurveData data in animacionFutura)
        {
            animationClipEmpty.SetCurve(data.path.ToString(), data.type, data.propertyName, auxAnim);

        }*/
    }
    private void SetNewCurve(float temp, float def, AnimationCurve auxAnim)
    {
        auxAnim.AddKey(temp, def);
     }
    private void setearCurve(string nombre, AnimationCurve auxAnim)
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
               
                animationClipEmpty.SetCurve(datos.path, datos.type, /*datos.propertyName*/"a", auxAnim);

                break;

            }
        }
    }
    //modificamos los valores en función de lo que tiene la animación de lo que hemos leido
    public void ChangeToMyAnim(AnimationClip animacionNueva, float selectedTime)
    {
       //gardamos todas las animaciones en animacion leida
        animacionLeida = AnimationUtility.GetAllCurves(animacionNueva, true).ToList();
        //calculamos el tiempo por frame en función del tiempo seleccionado
        float timeXFrame = selectedTime / animacionLeida[0].curve.keys.Length;
        //seteamos la cadera por defecto
        //animHips = new Vector3(-233.588f, 66.7051f, 935.858f);
        //calculamos la distancia entre la cadeta de la animacion que leemos y la de nuestro objeto
        //float distancia = Vector3.Distance(animHips, myHips.position);
        foreach (AnimationClipCurveData data in animacionLeida)
        {//por cada curva dentro de la animación 
            //creamos una animación de curva auxiliar que va del 0 0 al 1,0 
            AnimationCurve auxAnim = AnimationCurve.EaseInOut(0, 0, 1, 0);
            auxAnim.preWrapMode = WrapMode.Loop;
            Debug.Log("vamos a evaluar cada uno de los datos que se encuentran en la animación leida");
            Debug.Log("La curva:" + data.path);

           /* foreach (Keyframe key in data.curve.keys)
            {//añadimos el valor en nuestra animacion auxiliar
                SetNewCurve(key.time, key.value, auxAnim);
            }*/
            //esto funciona, el problema es que nos pone las curvas al final de las cosas 
            String[] subString = data.path.Split(':');
            String nombreCurva = subString[0];
            //llamamos a setear la curva
            setearCurve(nombreCurva, data.curve);
            auxAnim = null;
        }
       
    }
    public void CreateNewStateAndConexion()
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
}


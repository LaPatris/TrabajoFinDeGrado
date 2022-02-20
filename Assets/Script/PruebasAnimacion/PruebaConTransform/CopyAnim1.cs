
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
    [SerializeField] AnimationClip aniationSelf;//MI ANIMACION

                                                      //lista de los datos de la animación que vamos a seleccionar
    [SerializeField] private static List<AnimationClipCurveData> miAnimacion = new List<AnimationClipCurveData>();
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
                                                      //[SerializeField] PruebaFBX prueba;

    //copiamos nuestra animacion en la futura
    public void ReadMyAnimAndChange()
    {
        animationClipEmpty.wrapMode = WrapMode.Loop;
       
        miAnimacion = AnimationUtility.GetAllCurves(aniationSelf, true).ToList();


        foreach (AnimationClipCurveData data in miAnimacion)
        {
            animationClipEmpty.SetCurve(data.path.ToString(), data.type, data.propertyName, data.curve);
           
        }
    }
    public void CalculateNewC(AnimationClipCurveData datos, AnimationClipCurveData data)
    {
       
    }
    //modificamos los valores en función de lo que tiene la animación de lo que hemos leido
    public void ChangeToMyAnim(AnimationClip animacionNueva)
    {

        animacionLeida = AnimationUtility.GetAllCurves(animacionNueva, true).ToList();

        animacionFutura = AnimationUtility.GetAllCurves(animationClipEmpty, true).ToList();
        foreach (AnimationClipCurveData data in animacionLeida)
        {
            foreach (AnimationClipCurveData datos in animacionFutura)
            {
                //se tiene qeu poner así porque el path que contienen las animaciones se llama root idk 
                /*if (data.path.Contains("Root") && datos.path.Contains("Hips"))
                {
                    CalculateNewC(datos, data);
                }
                else
                {*/
                    if (data.path.Contains(datos.path))
                    {
                    AnimationCurve curve = new AnimationCurve();

                    foreach (Keyframe key in datos.curve.keys)
                    {
                        datos.curve.RemoveKey(0);
                    }
                    foreach (Keyframe key in datos.curve.keys)
                    {
                        datos.curve.AddKey(key.time, data.curve.Evaluate(key.time));
                        //no setea nada

                    }
                    animationClipEmpty.SetCurve(datos.propertyName + ": ", datos.type, datos.propertyName, datos.curve);


                }
                // }

            }
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


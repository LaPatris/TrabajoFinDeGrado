
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

public class CopyAnimTransform : MonoBehaviour
{


    //need for animation

    [Header("Animations")]
    [SerializeField] AnimationClip animationDefToRead;
    [SerializeField] AnimationClip animationClipEmpty;// animación vacía que rellenaré
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
    public void ReadMyAnimAndChange(AnimationClip animacionNueva)
    {
        //si ya existe la animación( es decir se ha elegido otra antes)
        /*if (changeAnim)
        {
            animationClipEmpty.ClearCurves();
            //vacio la animación
        }*/
        animationClipEmpty.wrapMode = WrapMode.Loop;
        // guardo toda la información de las curvas en una lista
        miAnimacion = AnimationUtility.GetAllCurves(animationDefToRead, true).ToList();

        //animacionFutura = AnimationUtility.GetAllCurves(animacionNueva, true).ToList();

        foreach (AnimationClipCurveData data in miAnimacion)
        {
            //foreach (AnimationClipCurveData datos in animacionFutura)
            //{
                //if ( datos.path.Contains("Head") && data.propertyName.Contains("Head"))
                   // {
                 //  if (data.propertyName.Contains("Nod") && datos.path.Contains("Y"))
                   // {
                        /*AnimationCurve curve = new AnimationCurve();
                        foreach (Keyframe key in data.curve.keys)
                        {
                            data.curve.RemoveKey(1);
                        }
                        foreach (Keyframe key in datos.curve.keys)
                        {
                            data.curve.AddKey(key.time, datos.curve.Evaluate(key.time));
                            //no setea nada
                        }*/
                        animationClipEmpty.SetCurve(data.path.ToString() + ": ", data.type, data.propertyName, data.curve);
                   // }
                        // animationClipEmpty.SetCurve
                        //animationClipEmpty.SetCurve(datos.path.ToString() + ": NUEVO", data.type, datos.propertyName, curve);
                    //}
                /*else
                    {
                        foreach (Keyframe key in data.curve.keys)
                        {

                            data.curve.AddKey(key.time, data.curve.Evaluate(key.time));
                        }

                        animationClipEmpty.SetCurve(data.path.ToString() + ": ", data.type, data.propertyName, data.curve);
                    }*/
                //}
            
        }
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
                if (data.propertyName.Contains(datos.path))
                {
                    AnimationCurve curve = new AnimationCurve();
                   
                    foreach (Keyframe key in data.curve.keys)
                    {
                        datos.curve.RemoveKey(0);
                    }
                    foreach (Keyframe key in datos.curve.keys)
                    {
                        datos.curve.AddKey(key.time, datos.curve.Evaluate(key.time));
                        //no setea nada
                        
                    }
                    animationClipEmpty.SetCurve(datos.propertyName + ": ", datos.type, datos.propertyName, datos.curve);
                    // animationClipEmpty.SetCurve
                    //animationClipEmpty.SetCurve(datos.path.ToString() + ": NUEVO", data.type, datos.propertyName, curve);
                }
                /*}
                else
                {

                    foreach (Keyframe key in data.curve.keys)
                    {
                        data.curve.RemoveKey(0);
                        
                    }
                }*/
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


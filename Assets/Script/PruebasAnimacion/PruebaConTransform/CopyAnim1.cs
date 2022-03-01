
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
    [SerializeField] Transform myHips;
    [SerializeField] Vector3 animHips;
    //[SerializeField] PruebaFBX prueba;
    

    public void changePosition()
    {
        // setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino
        //mi posicion= posicion root de elctura- su posicion inicial + mi opsicion inicial
            //selfRoot.localPosition = (srcRoot.localPosition - srcInitPosition) + selfInitPosition;
        
    }
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
        //estoy hay que pasarselo por parametro
        animHips = new Vector3(-233.588f, 66.7051f, 935.858f);
        float distancia = Vector3.Distance(animHips, myHips.position);
        
     animationClipEmpty.ClearCurves();
       foreach (AnimationClipCurveData data in animacionLeida)
        {
           
            foreach (AnimationClipCurveData datos in animacionFutura)
            {
                String[] datosPath= datos.path.Split('/');


                /*foreach (Keyframe key in datos.curve.keys)
                {
                    datos.curve.RemoveKey(0);

                }*/
                String[] subString = data.path.Split(':');
                Debug.Log("SUBSTRING0" + subString[0]);
                Debug.Log("SUBSTRING1" + subString[1]);
                if (datosPath[datosPath.Length-1].Contains(subString[0]) )
                   {
                    Debug.Log("datos ppname"+datos.propertyName);

                    
                    // AnimationCurve curve = new AnimationCurve();
                    if (datos.propertyName.Contains(subString[1]))
                    {
                        foreach (Keyframe key in data.curve.keys)
                        {
                            datos.curve.AddKey(key.time, data.curve.Evaluate(key.time));
                            //no setea nada

                        }
                    }
                    animationClipEmpty.SetCurve(datos.path, datos.type, datos.propertyName, datos.curve);


                }
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


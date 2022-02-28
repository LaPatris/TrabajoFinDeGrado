
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

using UnityEditor;

using System.IO;
using System.Globalization;
using System;
public class AngleCurveCreator : MonoBehaviour
{

    [Header("Para calcular la Animación")]
    [SerializeField] int numPoints;
    //[SerializeField] int curveCount;
    [SerializeField] float tiMax;
    [SerializeField] public AnimationClip animacionBezierHueso;
    [SerializeField] public AnimationCurve newTotalCurve;
    [SerializeField] Type tipoAnim;
    //[SerializeField] int SEGMENT_COUNT;
    [SerializeField] public bool curveDone;
    [SerializeField] public float tiempo;
    [SerializeField] public bool finalizado = false;
    private int curveCount = 0;
    private int SEGMENT_COUNT = 50;

    [Header("Para calcular la posición nueva")]
    GameObject personaje;
    Vector3 srcInitPosition = new Vector3();
    Vector3 selfInitPosition = new Vector3();
    Vector3 srcRoot;
    Transform selfRoot;



    void Start()
    {

        newTotalCurve = new AnimationCurve();

    }
    public bool inicializarBezier(List<Vector3> puntosCuerpo, float tmin, float tmax, GameObject pers)
    {
        tiMax = tmax;
        personaje = pers;

        srcRoot = puntosCuerpo[0];
        selfRoot = personaje.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);
        //SetInitPosition();
        return inicializarAnimaciones(puntosCuerpo[0], puntosCuerpo[puntosCuerpo.Count - 1], tmin);

    }

    //creo las tres curvas
    public bool inicializarAnimaciones(Vector3 momento0, Vector3 momentoF, float tmin)
    { 
        newTotalCurve = AnimationCurve.EaseInOut(momento0.z, momentoF.z, tmin / tiMax, 1);
        newTotalCurve.preWrapMode = WrapMode.Loop;
        return true;
    }

    private void SetInitPosition()
    {
        //seta las posiciones iniciales(de la root
        srcInitPosition = srcRoot;
        selfInitPosition = selfRoot.localPosition;
    }
    private Vector3 SetPosition(Vector3 posicion)
    {// setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino
        return (srcInitPosition - posicion) + selfInitPosition;
    }

    public void SetNull()
    {

        newTotalCurve = null;
        curveDone = false;

    }

    public void Ready(string hueso, List<Vector3> puntosCuerpo, List<float> timesXframe)
    {
        int j = 0;
        while( (j+1)<puntosCuerpo.Count)
        {
            float angle= Vector3.Angle(puntosCuerpo[j], puntosCuerpo[j + 1]);
            SetNewCurve(timesXframe[j], angle);
            j++;
        }

        if (newTotalCurve == null)
        {

            Debug.Log("NEWcurve ES NULL");
        }
        else
        {
            animacionBezierHueso.SetCurve(hueso.ToString() + ": ", transform.GetType(), newTotalCurve.length.ToString(), newTotalCurve);


            curveDone = true;

        }
    }
 private void SetNewCurve(float temp, float value)
    {
        
            //para curva X
            newTotalCurve.AddKey(temp, value);//desnormalizamos
            //para curva X
         

    }
}
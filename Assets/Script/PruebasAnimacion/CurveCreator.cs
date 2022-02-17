using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

using UnityEditor;

using System.IO;
using System.Globalization;
using System;

public class CurveCreator : MonoBehaviour
{
    [Header("Para calcular la Animación")]
    [SerializeField] int numPoints;
    //[SerializeField] int curveCount;
    [SerializeField] float tiMax;
    [SerializeField] public AnimationClip animacionBezierHueso;
    [SerializeField] public AnimationCurve newCurveX;
    [SerializeField] public AnimationCurve newCurveY;
    [SerializeField] public AnimationCurve newCurveZ;
    [SerializeField] public AnimationCurve newTotalCurve;
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
    [SerializeField] Vector3 srcRoot;
    [SerializeField] Transform selfRoot;
    // [SerializeField] public Dictionary<int, AnimationClip> animacionesBezier= new Dictionary<int, AnimationClip>();
    //[SerializeField] public List<AnimationClip> animacionBezier= new List<AnimationClip>();
    // [SerializeField] Dictionary<int, List<Vector3>> cuerpo;



    void Start()
    {

        //newCurveX = new AnimationCurve();

        //newCurveY = new AnimationCurve();

        // newCurveZ = new AnimationCurve();
        newTotalCurve = new AnimationCurve();

    }
    public bool inicializarBezier(List<Vector3> puntosCuerpo, float tmin, float tmax, GameObject pers)
    {
        tiMax = tmax;
        //curveCount = (int)puntosCuerpo.Count / 3;//va a ser una curva de 3 en 3
        personaje = pers;

       // srcRoot = puntosCuerpo[0];
        //selfRoot = personaje.GetComponent<Transform>().Find("Hips");
       // SetInitPosition();
        return inicializarAnimaciones(puntosCuerpo[0], puntosCuerpo[puntosCuerpo.Count - 1], tmin);

    }

    //creo las tres curvas
    public bool inicializarAnimaciones(Vector3 momento0, Vector3 momentoF, float tmin)
    { /*
        //X
        newCurveX = AnimationCurve.EaseInOut(momento0.x, momentoF.x, tmin / tiMax, 1);
        newCurveX.preWrapMode = WrapMode.Loop;
        //Y
        newCurveY = AnimationCurve.EaseInOut(momento0.y, momentoF.y, tmin / tiMax, 1);
        newCurveY.preWrapMode = WrapMode.Loop;
        //z
        newCurveZ = AnimationCurve.EaseInOut(momento0.z, momentoF.z, tmin / tiMax, 1);
        newCurveZ.preWrapMode = WrapMode.Loop;
    */
        newTotalCurve = AnimationCurve.EaseInOut(momento0.magnitude, momentoF.magnitude, tmin / tiMax, 1);
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
        //animacionBezier.Clear();
        newCurveX = null;
        newCurveY = null;
        newCurveZ = null;
        newTotalCurve = null;
        curveDone = false;

    }

    public void Ready(string hueso, List<Vector3> puntosCuerpo, List<float> timesXframe)
    {
        //int totalCurves = puntosCuerpo.Count / 4;
        //int i = 0;
        /*  for (int j = 0; j < totalCurves; j++)
          {

                  float t = (timesXframe[i + 3] - timesXframe[i]);
                  Vector3 p0 = SetPosition(puntosCuerpo[i]);
                  Vector3 p1 = SetPosition(puntosCuerpo[i+1]);
                  Vector3 p2 = SetPosition(puntosCuerpo[i+2]);
                  Vector3 p3 = SetPosition(puntosCuerpo[i+3]);
                  Vector3 pixel = CalculateCubicBezierPoint(t, p0, p1, p2, p3);

              i += 4;

          }*/

        for (int j = 0; j < puntosCuerpo.Count; j++)
        {


            Vector3 punto = SetPosition(puntosCuerpo[j]).normalized;

            SetNewCurve(timesXframe[j], punto);
            // Debug.Log("El tiempo actual es" + timesXframe[j]);

        }


        //  Debug.Log("Tengo estos valores" + newCurveX.keys + newCurveX.length.ToString());
        if (newCurveX == null)
        {

            Debug.Log("NEWcurve ES NULL");
        }
        else
        {
            //Debug.Log("en principio newCurveX no es null");
            //metemos las mini animaciones de los tres puntos del hueso en el animation Clip llamado animaciones bezierHueso
            animacionBezierHueso.SetCurve(hueso.ToString() + "X: ", transform.GetType(), newCurveX.length.ToString(), newCurveX);
            animacionBezierHueso.SetCurve(hueso.ToString() + "Y: ", transform.GetType(), newCurveY.length.ToString(), newCurveY);
            animacionBezierHueso.SetCurve(hueso.ToString() + "Z: ", transform.GetType(), newCurveZ.length.ToString(), newCurveZ);


            //animacionBezierHueso.SetCurve(hueso.ToString(), transform.GetType(),newTotalCurve.length.ToString(), newTotalCurve);

            //una vez tenemos esto, lo metemos en la lista de animaciones
            // animacionBezier.Add(animacionBezierHueso);
            // esta lista de animaciones se la metemos al diccionario total
            // animacionesBezier.Add(hueso, animacionBezierHueso);

            curveDone = true;


        }
    }

    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //(1-t)3P0 + 3(1-t)2tP1 + 3(1-t)t2P2 + t3P3(cuatros)
        //mirar internet bezierCUrves
        float onet = 1 - t;
        float ttt = t * t * t;
        float tt = t * t;
        float twoonet = onet * onet;
        float threeonet = twoonet * onet;
        Vector3 bz = (threeonet * p0) + (3 * twoonet * t * p1) + (3 * onet * tt * p3) + (ttt * p3);
        return bz;

    }
    private void SetNewCurve(float temp, Vector3 value)
    {
        if (temp < 1800)
        {
             newTotalCurve.AddKey(temp, value.magnitude);
            newTotalCurve.SmoothTangents(0, value.magnitude);
            //vamos a probar solo con la curva en X
           /* newCurveX.AddKey(temp, value.x);//desnormalizamos
                                            //vamos a probar solo con la curva en y
            newCurveY.AddKey(temp, value.y);//desnormalizamos
                                            //vamos a probar solo con la curva en y
            newCurveZ.AddKey(temp, value.z);//desnormalizamos*/
        }

    }
}
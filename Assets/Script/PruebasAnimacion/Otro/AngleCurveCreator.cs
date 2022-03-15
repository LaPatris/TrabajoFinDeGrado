
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
    [SerializeField] public AnimationCurve newCurveX;
    [SerializeField] public AnimationCurve newCurveY;
    [SerializeField] public AnimationCurve newCurveZ;
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
        newCurveX = new AnimationCurve();
        newCurveY = new AnimationCurve();
        newCurveZ = new AnimationCurve();

    }
    public bool inicializarBezier(List<Vector3> puntosCuerpo, float tmin, float tmax, GameObject pers)
    {
        tiMax = tmax;
        personaje = pers;

        //srcRoot = puntosCuerpo[0];
        //selfRoot = personaje.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);
        //SetInitPosition();
        return inicializarAnimaciones(tmin, puntosCuerpo[0], tmax, puntosCuerpo[puntosCuerpo.Count - 1]);

    }

    //creo las tres curvas
    public bool inicializarAnimaciones(float tmin, Vector3 momento0,float tmax, Vector3 momentoF )
    {
        newTotalCurve = AnimationCurve.EaseInOut(tmin,momento0.magnitude, tiMax, momentoF.magnitude );
        newTotalCurve.preWrapMode = WrapMode.Loop;
        //x
        newCurveX = AnimationCurve.EaseInOut(tmin, momento0.x, tiMax, momentoF.x);
        newCurveX.preWrapMode = WrapMode.Loop;
        //Y
        newCurveY = AnimationCurve.EaseInOut(tmin, momento0.y, tiMax, momentoF.y);
        newCurveX.preWrapMode = WrapMode.Loop;
        //Z
        newCurveZ = AnimationCurve.EaseInOut(tmin, momento0.z, tiMax, momentoF.z);
        newCurveZ.preWrapMode = WrapMode.Loop;
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
        newCurveX = null;
        newCurveY = null;
        newCurveZ = null;
       newTotalCurve = null;
        curveDone = false;

    }

    public void Ready(string hueso, List<Vector3> puntosCuerpo, List<float> timesXframe)
    {
        int j = 0;
      //while((j + 1) < puntosCuerpo.Count) 
            foreach(Vector3 pos in puntosCuerpo)
        {
            
        //   SetNewCurve(timesXframe[j], puntosCuerpo[j], puntosCuerpo[j+1]);
         SetNewCurve(timesXframe[j],pos);
            j+=1;
        }
        //EditorCurveBinding.FloatCurve(hueso.ToString(), transform.GetType(), "rotation");
                animacionBezierHueso.SetCurve(hueso.ToString() + ": Position ", transform.rotation.GetType(), newTotalCurve.length.ToString(), newTotalCurve);
              /*animacionBezierHueso.SetCurve(hueso.ToString() + ": Rotation.x ", transform.GetType(), newCurveX.length.ToString(), newCurveX);
              animacionBezierHueso.SetCurve(hueso.ToString() + ": Rotation.y ", transform.GetType(), newCurveY.length.ToString(), newCurveY);
              animacionBezierHueso.SetCurve(hueso.ToString() + ": Rotation.z", transform.GetType(), newCurveZ.length.ToString(), newCurveZ);
            */
            newTotalCurve = null;

            curveDone = true;

        
    }
 //private void SetNewCurve(float temp, float valueX, float valueY, float valueZ)
// private void SetNewCurve(float temp, Vector3 def)
 //private void SetNewCurve(float temp, Vector3 valores1, Vector3 valores2)
 private void SetNewCurve(float temp, Vector3 valores1)
    {
         newTotalCurve.AddKey(temp, valores1.magnitude);

        /*Vector3 dir = (valores1 - valores2).normalized;

        float angleX = Vector3.Angle(dir,Vector3.left);
            float angleY = Vector3.Angle(dir, Vector3.up);
            float angleZ = Vector3.Angle(dir, Vector3.forward);
            newCurveX.AddKey(temp, angleX);//desnormalizamos
            newCurveY.AddKey(temp, angleY);//desnormalizamos
            newCurveZ.AddKey(temp, angleZ);//desnormalizamos*/
     
            //para curva X
         

    }
}
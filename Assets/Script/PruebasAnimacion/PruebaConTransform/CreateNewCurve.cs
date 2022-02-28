using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewCurve : MonoBehaviour
{
    [Header("Para calcular la Animación")]
    [SerializeField] int numPoints;//numero de frames
    [SerializeField] float tiMax;//tiempo máximo de la animación

    //[SerializeField] public AnimationClip animacionBezierHueso;
    //nueva curva, se irá reescribiendo por huesos
    [SerializeField] public AnimationCurve newTotalCurve;
    //ANIMACIÓN QEU VAMOS A LEER
    [SerializeField] public AnimationClip animacionFinal;
    //la cuerva está hecha
    [SerializeField] public bool curveDone;
    //el tiempo en cada momento
    [SerializeField] public float tiempo;
    //si se ha terminado de hacer todas las curvas
    [SerializeField] public bool finalizado = false;
    

    [Header("Para calcular la posición nueva")]
    //personaje ´del qeu vamos a interactuar
    GameObject personaje;
    Vector3 srcInitPosition = new Vector3();
    Vector3 selfInitPosition = new Vector3();
    [SerializeField] Vector3 srcRoot;
    [SerializeField] Transform selfRoot;



    void Start()
    {
        //creamos la curva
        newTotalCurve = new AnimationCurve();

    }
    public bool inicializarBezier(List<Vector3> puntosCuerpo, float tmin, float tmax, GameObject pers)
    {
        tiMax = tmax;
        personaje = pers;
                // srcRoot = puntosCuerpo[0];
        //selfRoot = personaje.GetComponent<Transform>().Find("Hips");
        // SetInitPosition();
        return inicializarAnimaciones(puntosCuerpo[0], puntosCuerpo[puntosCuerpo.Count - 1], tmin);

    }

    //creo las tres curvas
    public bool inicializarAnimaciones(Vector3 momento0, Vector3 momentoF, float tmin)
    { //inicializamos la curva
        newTotalCurve = AnimationCurve.EaseInOut(momento0.magnitude, momentoF.magnitude, tmin / tiMax, 1);
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
    {//reseteramos los valoress
        newTotalCurve = null;
        curveDone = false;

    }

    public void Ready(string hueso, List<Vector3> puntosCuerpo, List<float> timesXframe)
    {
        //crea
        for (int j = 0; j < puntosCuerpo.Count; j++)
        {


            Vector3 punto = SetPosition(puntosCuerpo[j]);//estaba antes normalizado

            SetNewCurve(timesXframe[j], punto);

        }

         if (newTotalCurve == null)
         {

             Debug.Log("NEWcurve ES NULL");
         }
         else
         {
            animacionFinal.SetCurve(hueso.ToString(), transform.GetType(), newTotalCurve.length.ToString(), newTotalCurve);
             curveDone = true;
         }


    }

    private void SetNewCurve(float temp, Vector3 value)
    {
        if (temp < 1800)
        {
            newTotalCurve.AddKey(temp, (value-personaje.transform.position).magnitude);
            newTotalCurve.SmoothTangents(0, (value - personaje.transform.position).magnitude);
        }
    }
}

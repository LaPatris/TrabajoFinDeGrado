using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TxtManager : MonoBehaviour
{

    [SerializeField] OrganizarDatosFile orgDatos;

    [SerializeField] AngleCurveCreator curva;
    //[SerializeField] AngleCurveCreator curva;
    [SerializeField] TextAsset myTxt;
    [SerializeField] public bool finalizado = false;
    [SerializeField] GameObject personaje;
    [SerializeField] CopyAnim1 copyAnimacion;
    [SerializeField] animacion copyA;

    [SerializeField] GameObject elegido;

    int index = 0;
    [SerializeField] float selectedTime = 0f;
    [SerializeField] List<AnimationClip> totalAnimaciomaciones = new List<AnimationClip>();

    [SerializeField] List<string> totalAnimaciomacionesNombres = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        /*coger el txt, llamar organizar datos y después a curva de bezier*/

        orgDatos = new OrganizarDatosFile();
        personaje = GameObject.Find("P4DEF");
        copyAnimacion = personaje.GetComponent<CopyAnim1>();
        copyA = personaje.GetComponent<animacion>();
        curva = this.gameObject.GetComponent<AngleCurveCreator>();
        orgDatos.SetListBones(myTxt, curva, personaje);
        //orgDatos totalbody es un dicchionario de nombre de hueso y lista de transforms de ese hueso
        //curva --> tiene la animación total 
        if (orgDatos.finalizado)
        {
            //totalAnimaciomaciones.Add(curva.animacionFinal);
//totalAnimaciomaciones.Add(curva.animacionBezierHueso);
            totalAnimaciomacionesNombres.Add(myTxt.name);
            finalizado = orgDatos.finalizado;
        }
    }

    [MenuItem("Window/Animation Copier")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow(typeof(CopyAnim));

    }

    public void entrada(float timeValue)
    {
        selectedTime = timeValue;
    }
    public bool selectTime()
    {
        return selectedTime == 0.0f ? false : true;

    }
    public void OnGUI()
    {
        if ( finalizado)
        {
            
            EditorGUILayout.LabelField("Select");
            GUILayout.BeginVertical("Box");
            //guardo el indice de la animación que he seleccionado
            index = GUILayout.SelectionGrid(index, totalAnimaciomacionesNombres.Select(x => x).ToArray(), 1);
            if (GUILayout.Button("Copy") && selectTime() )
            {
                elegido.active = false;
                // si doy a copiar guardo la animacion
                //selectedAnimationClip = animationClips[index];
                Debug.Log("La aniamcion seleccionada es  : " + totalAnimaciomacionesNombres[index]);
                //llamo a la accion de cambiar de estado
                //copiar la animación por defecto en otra animación 
                //copyAnimacion.ReadMyAnimAndChange();
                //copyAnimacion.ChangeToMyAnim(totalAnimaciomaciones[index], selectedTime);
                //copyAnimacion.ChangeToMyAnim(orgDatos.totalBody, selectedTime);
                copyA.initAll(orgDatos.totalBody);

                /*if (!copyAnimacion.creadoStado)
                { //si no habia estado creado lo creo
                    copyAnimacion.CreateNewStateAndConexion();
                }
                //cambio el valor del estado
                copyAnimacion.ChangeStateValue();*/

            }
            else
            {
                if (!selectTime())
                {
                    elegido.active = true;
                }

            }
            if (GUILayout.Button("Remove"))
            {// si damos a remove solo si el estado esta creado eliminamos el estado y la transición 
                if (copyAnimacion.creadoStado)
                    copyAnimacion.RemoveState();
            }
            GUILayout.EndVertical();

        }
        

    }
}



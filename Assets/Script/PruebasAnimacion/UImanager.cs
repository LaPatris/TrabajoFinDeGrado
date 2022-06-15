using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Objetos")]
    [SerializeField] GameObject personajeseleccionado;
    [SerializeField] TxtManager txtManger;
    [SerializeField] FileReader fichero;
    public GameObject panel;
    public GameObject botonMas;
    [SerializeField] GameObject texto;
    [SerializeField] GameObject textoAviso;

    [Header("Botones")]
    [SerializeField] Button botonCosa;
    [SerializeField] Button botonNiña;
    [SerializeField] Button botonVieja;
    [SerializeField] Button botonPoli;
    [SerializeField] Button botonX;
    [SerializeField] Button encontrar;
    [Header("texto")]
    bool[] presionado = new bool[4];



    [SerializeField] List<string> animacionesExistentes = new List<string>();
    public void botonMasAccion()
    {
        panel.SetActive(true);

        textoAviso.SetActive(false);
        botonMas.SetActive(false);
        
    }
    public void botonXAccion()
    {
        panel.SetActive(false);

        botonMas.SetActive(true);
    }
    public void botonEncontrar()
    {
       string nombre=texto.GetComponent<TMP_InputField>().text;
        string[] lista = File.ReadAllLines("Assets/BVH/" + nombre + ".bvh");
        if (!txtManger.GetComponent<GestionarMenu>().listaAnimaciones.Contains(nombre) && lista == null)
        {
            textoAviso.SetActive(true);
            textoAviso.GetComponent<TMP_Text>().text = "Este fichero no existe";
        }
        else
        {

            //si no existe en la lista pero si existe el fichero 
            if (lista != null)
            {
                Debug.Log("este caso");
                textoAviso.SetActive(false);
                animacionesExistentes = txtManger.GetComponent<GestionarMenu>().listaAnimaciones.Split('_').ToList();
                Debug.Log(animacionesExistentes);
                // comprobar si esta el fichero reescrito
                if (!animacionesExistentes.Contains(nombre))
                {
                    if (!txtManger.GetComponent<GestionarMenu>().listaAnimaciones.Contains(nombre))
                    {
                        Debug.Log("hola");
                        txtManger.GetComponent<GestionarMenu>().listaAnimaciones += "_" + "Assets/BVH/" + nombre + ".bvh";

                        txtManger.totalAnimaciomacionesNombres.Add(nombre);

                        txtManger.totalAnimaciomacionesPath.Add("Assets/BVH/" + nombre + ".bvh");
                    }
                }
                else
                {

                    textoAviso.GetComponent<TMP_Text>().text = "Este fichero no existe";

                }
                AssetDatabase.Refresh();
            }
        }

        
        //refresh
    }
    public void datos(string nombre)
    {
        /*if (txtManger.personaje == null)
        {
            txtManger.personaje = GameObject.Find(nombre);
            txtManger.copyA = txtManger.personaje.GetComponent<animacion>();
            txtManger.copyA.posicioneEn0();
        }
        else
        {
            txtManger.personaje.transform.position = txtManger.copyA.posicionInicial;
            txtManger.personaje = GameObject.Find(nombre);
            txtManger.copyA = txtManger.personaje.GetComponent<animacion>();
            txtManger.copyA.posicioneEn0();
        }*/
        if (txtManger.personaje == null)
        {
            txtManger.personaje = GameObject.Find(nombre);
            txtManger.animP = txtManger.personaje.GetComponent<animacionPersonaje>();
            txtManger.animP.posicioneEn0();//setea en la posicion 0
        }
        else
        {
            txtManger.personaje.transform.position = txtManger.animP.posicionInicial;
            txtManger.personaje = GameObject.Find(nombre);

            txtManger.animP = txtManger.personaje.GetComponent<animacionPersonaje>();
            Debug.Log("qeu" + txtManger.animP);
            txtManger.animP.posicioneEn0();//setea en la posicion 0
        }
        botonMas.SetActive(true);
        panel.SetActive(false);

    }
    public void setCosa()
    {
        datos("personaje1Modificado");
    }

    public void setNiña()
    {
        datos("niniaModif");
    }
    public void setVieja()
    {
        datos("personajeVieja");
    }
    public void setPoli()
    {
        datos("P4DEF");
    }
  
    

}

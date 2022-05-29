using System;
using System.Collections;
using System.Collections.Generic;
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
        TextAsset lista = (TextAsset)AssetDatabase.LoadMainAssetAtPath("Assets/TXT/" + nombre + ".txt");
        if (!txtManger.GetComponent<GestionarMenu>().listaAnimaciones.Contains(nombre) && lista==null)
        {
            textoAviso.SetActive(true);
            textoAviso.GetComponent<TMP_Text>().text = "Este fichero no existe";
        }
        else
        {
          
            //si no existe en la lista pero si existe el fichero 
                if ( lista != null)
                {
                    Debug.Log("este caso");
                    textoAviso.SetActive(false);
                    //existe

                    TextAsset lista2 = (TextAsset)AssetDatabase.LoadMainAssetAtPath("Assets/TXT/" + nombre + "RtR.txt");
                    animacionesExistentes = txtManger.GetComponent<GestionarMenu>().listaAnimaciones.Split('_').ToList();
                // comprobar si esta el fichero reescrito
                    if (lista2 != null)
                    {
                        Debug.Log("lista2Existe");
                    //ya existe
                    textoAviso.GetComponent<TMP_Text>().text = "Ya existe";
                    textoAviso.SetActive(true);
                        Debug.Log(txtManger.GetComponent<GestionarMenu>().listaAnimaciones + "todo" + lista2.name);
                    //comprobar que no esté dentro de la lista
                        if (!txtManger.GetComponent<GestionarMenu>().listaAnimaciones.Contains(lista.name))
                        {
                        txtManger.GetComponent<GestionarMenu>().listaAnimaciones += "_" + lista.name;
                        }
                        txtManger.GetComponent<GestionarMenu>().buscado = false;
                    }
                    // si no está el fichero reescrito
                    else
                    {
                    string solucion = txtManger.GetComponent<GestionarMenu>().cf.nameAndFile(lista) ? "El fichero se creó perfectamete" : "No se ha podido crear el fichero";
                    
                    if (!txtManger.GetComponent<GestionarMenu>().listaAnimaciones.Contains(lista.name))
                        txtManger.GetComponent<GestionarMenu>().listaAnimaciones = txtManger.GetComponent<GestionarMenu>().listaAnimaciones + "_" + lista.name;


                    AssetDatabase.Refresh();
                    TextAsset listaReescrita = (TextAsset)AssetDatabase.LoadMainAssetAtPath("Assets/TXT/" + nombre + "RtR.txt");
                    txtManger.totalAnimaciomacionesNombres.Add(listaReescrita.name);
                    txtManger.todoTXT.Add(listaReescrita.name, listaReescrita);
                    }
            }


            AssetDatabase.Refresh();

        }
        //refresh
    }
    public void datos(string nombre)
    {
        if (txtManger.personaje == null)
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
        datos("personajeNiña");
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

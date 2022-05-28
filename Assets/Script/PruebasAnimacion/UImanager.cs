using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject personajeseleccionado;
    [SerializeField] TxtManager txtManger;
    [Header("Botones")]
    [SerializeField] Button botonCosa;
    [SerializeField] Button botonNiña;
    [SerializeField] Button botonVieja;
    [SerializeField] Button botonPoli;
    bool[] presionado = new bool[4];

    public void setCosa()
    {
        txtManger.personaje = GameObject.Find("personaje1Modificado");

        txtManger.copyA = txtManger.personaje.GetComponent<animacion>();
    }

    public void setNiña()
    {
        txtManger.personaje = GameObject.Find("personajeNiña");
    }
    public void setVieja()
    {
        txtManger.personaje = GameObject.Find("personajeVieja");
    }
    public void setPoli()
    {
        txtManger.personaje = GameObject.Find("P4DEF");

        txtManger.copyA = txtManger.personaje.GetComponent<animacion>();
    }
  


}

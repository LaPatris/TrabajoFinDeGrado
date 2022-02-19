using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject []personajeSeleccionado= new GameObject[4];
    [Header("Botones")]
    [SerializeField] Button botonCosa;
    [SerializeField] Button botonNiña;
    [SerializeField] Button botonVieja;
    bool[] presionado = new bool[4];

    
    void Start()
    {
        personajeSeleccionado[0] = GameObject.Find("personaje1");
        personajeSeleccionado[1] = GameObject.Find("personaje2");
        personajeSeleccionado[2] = GameObject.Find("personaje3");
        personajeSeleccionado[3] = GameObject.Find("PruebaP43");
        botonVieja.onClick.AddListener(()=> {
            if (!presionado[2])
            {
                personajeSeleccionado[2].AddComponent<Personaje>();
                presionado[2] = true;
            }

        });
    
    }
    
    public void Awake()
    {
        
    } 

    // Update is called once per frame
    void Update()
    {
        
    }


}

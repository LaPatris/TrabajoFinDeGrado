using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject character;
    [SerializeField] Transform cadera;
    [SerializeField] Transform hueso1;
    [SerializeField] Animator animatorCharacter;

    [SerializeField] GameObject lectura;
    void Start()
    {
        //character = GameObject.Find("personaje1");
        //Problema que tiene esto de se tienen que ir utilizando los anteriores( se podría hacer un árbol pero no se si merece la pena) 
        cadera = transform.Find("Hips");
        hueso1 = cadera.Find("LeftUpLeg");
        lectura = GameObject.Find("txt");
        
    }

    // Update is called once per frame
    void Update()
    {
       // animatorCharacter.Play("baile");
       /* if (hueso1 != null && lectura.GetComponent<leerTXT>().readText)
         {
            Debug.Log("hola");
        string[] lineas = lectura.GetComponent<leerTXT>().getLineOfTxt(0);
        Vector3 new3= new Vector3(int.Parse( lineas[0]), int.Parse(lineas[1]), int.Parse(lineas[2]));

        hueso1.transform.position = new3; 
        }*/
    }
}

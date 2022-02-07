using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Animator animator = gameObject.AddComponent<Animator>();
        animator = GetComponent<Animator>();
        HumanDescription description = AvatarUtils.CreateHumanDescription(gameObject);
        Avatar avatar = AvatarBuilder.BuildHumanAvatar(gameObject, description);
        avatar.name = gameObject.name;
        animator.avatar = avatar;
        gameObject.AddComponent<RetargetingHPH>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

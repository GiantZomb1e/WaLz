using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update

   void FixedUpdate()
   {
     var position = GameObject.Find("Player").transform.position;
     gameObject.transform.position = new Vector3(position.x,position.y,-20);
   }
}

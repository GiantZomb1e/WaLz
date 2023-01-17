using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void export()
    {
    ChunkLoader chunkloader = GameObject.Find("Chunk").GetComponent<ChunkLoader>();
    chunkloader.ExportJson();
    Debug.Log("exported");
    }
    
   
}

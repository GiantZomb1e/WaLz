using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class ChunkLoader : MonoBehaviour
{
    [NonSerialized]
    public string path;
    private TextAsset Filecontent;

    [Serializable]
    private class ChunkWrapper
    {
        [SerializeField]
        public Chunk[] Chunks = new Chunk[10000];
        [Serializable]
        public class Chunk
        {
            [SerializeField]
            public int x;
            public int y;
            public int biome;
            public Row[] Data = new Row[50];

        }

        [Serializable]
        public class Row
        {
            [SerializeField]
            public int[] row = new int[50];
        }
    }

    // Start is called before the first frame update
    //delte later


    void Start()
    {

        ChunkWrapper cw = new ChunkWrapper();
        int f = 0;
        for (int k = 0; k < 3; k++)
        {
            for (int i = 0; i < 3; i++)
            {
                ChunkWrapper.Chunk chunk = new ChunkWrapper.Chunk();
                chunk.x = i;
                chunk.y = k;
                chunk.biome = i % 6;
                cw.Chunks[f++] = chunk;
                for (int y = 0; y < 50; y++)
                {
                    chunk.Data[y] = new ChunkWrapper.Row();
                    for (int x = 0; x < 50; x++)
                    {
                        chunk.Data[y].row[x] = 2;
                    }
                }
            }
        }
        this.path = "test3.json";
        string Json = JsonUtility.ToJson(cw);
        ExportJson(Json);
        ChunkWrapper cl = ImportJson<ChunkWrapper>(this.path);
        for (int y = 0; y < 50; y++)
        {
            for (int x = 0; x < 50; x++)
            {
                RenderChunk(cl, y, x);
            }
        }
    }


    public static T ImportJson<T>(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ExportJson(string json)
    {
        var sr = File.CreateText(this.path);
        sr.WriteLine(json);
        sr.Close();

    }

    private ChunkWrapper.Chunk EmptyChunk(int x, int y)
    {
        ChunkWrapper.Chunk chunk = new ChunkWrapper.Chunk();
        chunk.x = x;
        chunk.y = y;
        for (int y_idx = 0; y_idx < 50; y_idx++)
        {
            chunk.Data[y_idx] = new ChunkWrapper.Row();
            for (int x_idx = 0; x_idx < 50; x_idx++)
            {
                chunk.Data[y_idx].row[x_idx] = 0;
            }
        }
        return chunk;
    }

    private ChunkWrapper.Chunk GetChunk(ChunkWrapper cw, int x, int y)
    {
        for (int i = 0; i < cw.Chunks.Length; i++)
        {
            if (cw.Chunks[i].x == x && cw.Chunks[i].y == y)
            {
                return cw.Chunks[i];
            }
        }
        return null;
    }

    private void RenderChunk(ChunkWrapper cw, int x, int y)
    {
        ChunkWrapper.Chunk torender = GetChunk(cw, x, y);
        if (torender == null)
        {
            torender = EmptyChunk(x, y);
        }
        for (int y_idx = 0; y_idx < 50; y_idx++)
        {
            ChunkWrapper.Row row = torender.Data[y_idx];
            for (int x_idx = 0; x_idx < 50; x_idx++)
            {
                int tile_x = 50 * x + x_idx;
                int tile_y = 50 * y + y_idx;

                int material = torender.Data[y_idx].row[x_idx];
                if (material == 2)
                {
                    Instantiate(GameObject.Find("Tile"), new Vector3((float)tile_x, (float)tile_y, 0f), new Quaternion(0f, 0f, 0f, 0f));
                }
            }
        }
    }
}

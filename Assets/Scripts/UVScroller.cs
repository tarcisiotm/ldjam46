using UnityEngine;
using System.Collections;

public class UVScroller : MonoBehaviour
{
    [SerializeField] Vector2 speed = new Vector2(.75f, .75f);

    Renderer targetRenderer;
    Vector2 TextureOffset;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        TextureOffset = Time.time * speed;
        targetRenderer.material.SetTextureOffset("_MainTex", TextureOffset);
    }

}
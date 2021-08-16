using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SpriteBinder", order = 1)]
public class SpriteBinder : ScriptableObject
{
    public Texture main;
    public Texture normal;
    public int frameCount = 1;
    public float speed;
    public bool loop = true;
}

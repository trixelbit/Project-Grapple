using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteState
{ 
    StartUp = 0,
    Active = 1,
    CoolDown = 2
}

public class AnimatedMaterial
{

    public Renderer Renderer;
    public Transform ParentTransform;
    public Texture SpriteSheet;
    public Texture SpriteNormal;
    public Vector3 TransformOrginalScale;
    public float FrameCount = 1;
    public float ImageSpeed = 1;
    public float ImageIndex = 0;
    public bool Active = false;
    public Vector2 ActiveFrames;
    public bool Loop = true;
    public bool Interuptable = true;
    public bool Completed = false;
    public float OrginalSpriteWidth;
    public float OrginalSpriteHeight;
    public float SpriteWidth;
    public float SpriteHeight;

    #region Constructors
    public AnimatedMaterial(Renderer renderer)
    {
        Renderer = renderer;
    }

    public AnimatedMaterial(Renderer renderer, Transform transform, SpriteBinder sprite)
    {
        Renderer = renderer;
        FrameCount = sprite.frameCount;
        ImageSpeed = sprite.speed;
        ImageIndex = 0;
        ParentTransform = transform;
        TransformOrginalScale = transform.localScale;

        UpdateSprite(sprite);

    }
    #endregion

    public void UpdateSprite(SpriteBinder sprite)
    {
        if (sprite.main.name != Renderer.sharedMaterial.mainTexture.name )
        {
            Renderer.material.EnableKeyword("_NORMALMAP");
            Renderer.material.SetTexture("_MainTex", sprite.main);
            Renderer.material.SetTexture("_EmissionMap", sprite.main);
            if (sprite.normal != null)
            {
                Renderer.material.SetTexture("_BumpMap", sprite.normal);
            }
            
            SpriteSheet = sprite.main;
            Loop = sprite.loop;
            ImageIndex = 0;
            Completed = false;     
            FrameCount = sprite.frameCount;
            ImageSpeed = sprite.speed;
        }
    }

    public bool IsActive()
    {
        if (ActiveFrames != null)
        {
            if (ImageIndex >= ActiveFrames.x && ImageIndex <= ActiveFrames.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void Render()
    {
        Active = IsActive();

        float image_xscale = 1 / FrameCount;

        #region resizes material xscale to match width of 1 frame
        Renderer.material.SetTextureScale("_MainTex", new Vector2(image_xscale, 1f));

        Renderer.material.SetTextureScale("_BumpMap", new Vector2(image_xscale, 1f));

        Renderer.material.SetTextureScale("_EmissionMap", new Vector2(image_xscale, 1f));
        #endregion

        float SingleFrameWidth = Renderer.material.mainTexture.width / FrameCount;
        float SingleFrameHeight = Renderer.material.mainTexture.height;


        // percentage diff between origanl scale and new scale
        float WidthScaleDifference = SingleFrameWidth / OrginalSpriteWidth;
        float HeightScaleDifference = SingleFrameHeight / OrginalSpriteHeight;


        if (SpriteWidth != SingleFrameWidth && Math.Abs(TransformOrginalScale.x * WidthScaleDifference) != Mathf.Infinity )
        {
            ParentTransform.localScale = new Vector3(TransformOrginalScale.x * WidthScaleDifference, ParentTransform.localScale.y, TransformOrginalScale.z * HeightScaleDifference);
        }

        SpriteWidth = SingleFrameWidth;
        SpriteHeight = SingleFrameHeight;

        float offset = (float)Math.Floor(ImageIndex) / FrameCount;

        Renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        Renderer.material.SetTextureOffset("_BumpMap", new Vector2(offset, 0));

        if (Loop || ImageIndex < FrameCount - 1)
        {
            ImageIndex = (ImageIndex + (ImageSpeed * Time.deltaTime)) % FrameCount;
        }
        else 
        {
            ImageIndex = FrameCount - 1;
            Completed = true;
        }


    }
}










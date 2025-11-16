using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Raycast_ObjectDetection : MonoBehaviour
{
    [Header("Nastavení raycastování")]
    public int rayCount = 60;              // počet paprsků, např. každých 2°
    public float maxDistance = 5f;          // maximální dosah vlny
    public LayerMask obstacleLayer;         // vrstva pro kolize (překážky)

    Texture2D visibilityMask;
    Color[] pixels;
    int textureSize = 32;
    public float thickness = 1;

    // Pole pro uložení vzdáleností (délky paprsků)
    public float[] distances;
    Vector2[] polygonPoints;
    float angle;
    float rad;
    Vector3 previousPos = Vector2.zero;
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = Instantiate(renderer.material);

        distances = new float[rayCount];
        polygonPoints = new Vector2[rayCount];

        InitializeMask(rayCount);
        renderer.material.SetTexture("_VisibilityMask", visibilityMask);
        //this.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Update()
    {
        if (previousPos != this.transform.position)
        {
            Effect();
            previousPos = this.transform.position;
        }
    }
    
    private void Effect()
    {
        //this.GetComponent<SpriteRenderer>().enabled = true;
        Vector2 origin2D = transform.position;
        Vector3 origin3D = new Vector3(transform.position.x, transform.position.y, 0);
        RaycastHit hit3D;

        for (int i = 0; i < rayCount; i++)
        {
            // Vypočítat úhel v radiánech od 0 do 2pi
            angle = (360f / rayCount) * i;
            rad = angle * Mathf.Deg2Rad;

            // Směr paprsku v rovině XY
            Vector2 dir2D = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            Vector3 dir3D = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            //  2D raycast
            RaycastHit2D hit2D = Physics2D.Raycast(origin2D, dir2D, maxDistance, obstacleLayer);

            // 3D raycast
            if (hit2D.collider != null)
            {
                // Uložit vzdálenost k překážce
                distances[i] = hit2D.distance;
            }
            else if (Physics.Raycast(origin3D, dir3D.normalized, out hit3D, maxDistance, obstacleLayer))
            {
                distances[i] = hit3D.distance;
            }
            else
            {
                // Žádná překážka v tomto směru do maxDistance
                distances[i] = maxDistance;
            }
            Debug.DrawRay(origin2D, dir2D * distances[i], Color.red);
            angle = (2 * Mathf.PI) * i / distances.Length;
            rad = distances[i] / maxDistance;
            polygonPoints[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * rad;
        }
        UpdateRayLinesMask(distances, maxDistance, thickness);
    }
    void InitializeMask(int size)
    {
        textureSize = size;
        visibilityMask = new Texture2D(textureSize, textureSize, TextureFormat.RFloat, false);
        visibilityMask.wrapMode = TextureWrapMode.Clamp;
        pixels = new Color[textureSize * textureSize];
    }
    public void UpdateRayLinesMask(float[] distances, float maxDistance, float lineThicknessPixels = 2f)
    {
        Color[] pixels = new Color[textureSize * textureSize];
        float center = (textureSize - 1) / 2f;

        // Založíme černou texturu
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.black;

        // Pro každý pixel
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float nx = (x / (float)(textureSize - 1)) * 2f - 1f;
                float ny = (y / (float)(textureSize - 1)) * 2f - 1f;
                float distance = Mathf.Sqrt(nx * nx + ny * ny);
                if (distance > 1f) continue;

                float pixelAngle = Mathf.Atan2(ny, nx);
                if (pixelAngle < 0) pixelAngle += 2f * Mathf.PI;

                int nearestRay = (int)(pixelAngle / (2f * Mathf.PI) * rayCount);
                nearestRay = Mathf.Clamp(nearestRay, 0, rayCount - 1);

                float maxDist = distances[nearestRay] / maxDistance;
                float thicknessAngle = (lineThicknessPixels / (float)textureSize) * Mathf.PI;

                // Přepočet přesnosti šířky pásma na úhel
                float angleDiff = Mathf.Abs(pixelAngle - ((2f * Mathf.PI) * nearestRay / rayCount));
                angleDiff = Mathf.Min(angleDiff, 2f * Mathf.PI - angleDiff); // Korekce úhlu přes nulový bod

                if (angleDiff <= thicknessAngle && distance <= maxDist)
                    pixels[y * textureSize + x] = Color.white;
            }
        }


        visibilityMask.SetPixels(pixels);
        visibilityMask.Apply();
    }

}

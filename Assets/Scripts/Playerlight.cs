using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Playerlight : MonoBehaviour
{
    private CircleCollider2D cld;
    public Volume postProcessingVolume;
    void Start()
    {
        cld = GetComponent<CircleCollider2D>();
        cld.radius = 0f;
        cld.enabled = false;
    }
    public void StartShining()
    {
        GetComponent<soundManager>().StartPlaying(true);
        cld.enabled = true;
        cld.radius = 5f;
        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("GreyScaled"))
        {
            gm.GetComponent<TilemapRenderer>().material.SetFloat("_ExclusionRadius", 3f);
        }
        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("GreyScaledObj"))
        {
            gm.GetComponent<SpriteRenderer>().material.SetFloat("_ExclusionRadius", 3f);
        }

        if (postProcessingVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
        {
            colorAdjustments.postExposure.value = 1.7f;
        }
    }

    public void StopShining()
    {
        GetComponent<soundManager>().StopPlaying(true);
        cld.enabled = false;
        cld.radius = 0f;
        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("GreyScaled"))
        {
            gm.GetComponent<TilemapRenderer>().material.SetFloat("_ExclusionRadius", 1f);
        }
        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("GreyScaledObj"))
        {
            gm.GetComponent<SpriteRenderer>().material.SetFloat("_ExclusionRadius", 1f);
        }

        if (postProcessingVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0f;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collision.gameObject.GetComponent<EnemyAI>().Blind();
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position).normalized * 30);
        }
        else if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<Boss>().Blind();
        }
    }
}

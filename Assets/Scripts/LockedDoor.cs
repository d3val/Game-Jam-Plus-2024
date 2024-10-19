using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] List<GameObject> keys;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float disappearTime = 1.5f;
    public static LockedDoor instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void RemoveKey(GameObject item)
    {
        if (keys.Contains(item))
            keys.Remove(item);
        if (keys.Count == 0)
            OpenDoor();
    }

    void OpenDoor()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color originalColor = spriteRenderer.color;
        Color auxColor = spriteRenderer.color;
        float interpolation;
        for (float i = disappearTime; i > 0; i -= Time.deltaTime)
        {
            interpolation = Mathf.Lerp(0, originalColor.a, i / disappearTime);
            auxColor.a = interpolation;
            spriteRenderer.color = auxColor;
            yield return null;
        }
        Destroy(gameObject);
    }

}

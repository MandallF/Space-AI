using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverClip;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip != null)
            SoundManager.Instance.sfxSource.PlayOneShot(hoverClip);
    }

}

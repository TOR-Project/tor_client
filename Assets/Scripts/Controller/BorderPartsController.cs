using UnityEngine;

public class BorderPartsController : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] partileSystemList;

    public void setParticleBaseColor(Color32 color)
    {
        foreach(ParticleSystem p in partileSystemList)
        {
            ParticleSystem.MainModule module = p.main;
            module.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }
}
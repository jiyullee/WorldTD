using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
    int destroyPoolCount = 30;
    int upgradePoolCount = 1;

    private static EffectManager mInstance;
    public static EffectManager instance
    {
        get 
        { 
            if (mInstance == null) mInstance = FindObjectOfType<EffectManager>();
            return mInstance;
        }
    }

    [SerializeField] public ParticleSystem enemyDestoryedPrefab;
    [SerializeField] public ParticleSystem upgradePrefab;

     Queue<ParticleSystem> destroyEffectPool = new Queue<ParticleSystem>();
     Queue<ParticleSystem> upgradeEffectPool = new Queue<ParticleSystem>();

    void Awake()
    {
       mInstance = this;
      
    }
    private void Start()
    {
        CreateOn();
    }

    public void CreateOn()
    {
        for(int i=0;i<destroyPoolCount;i++)
        {
            destroyEffectPool.Enqueue(CreateDestroyParticle());

        }

        for (int i = 0; i < upgradePoolCount; i++)
        {
            upgradeEffectPool.Enqueue(CreateUpgradeParticle());

        }
    }
 

    private ParticleSystem CreateDestroyParticle()
    {   
       var newParticle = Instantiate(enemyDestoryedPrefab).GetComponent<ParticleSystem>();
       newParticle.transform.SetParent(gameObject.transform); newParticle.Stop();
       return newParticle;
    }
    private ParticleSystem CreateUpgradeParticle()
    {
        var newParticle = Instantiate(upgradePrefab).GetComponent<ParticleSystem>();
        newParticle.transform.SetParent(gameObject.transform); newParticle.Stop();
        return newParticle;

    }





    public static ParticleSystem GetParticle(GameObject gameObject)
    {
        if (gameObject.layer==6)
        {
            if (instance.upgradeEffectPool.Count > 0)
            {
                var particle = instance.upgradeEffectPool.Dequeue();
                particle.transform.SetParent(gameObject.transform);
                particle.transform.position = gameObject.transform.position;
                particle.Play();
                return particle;
            }
            else
            {
                var newParticle = instance.CreateUpgradeParticle();
                newParticle.transform.SetParent(gameObject.transform);
                newParticle.transform.position = gameObject.transform.position;
                newParticle.Play();
                return newParticle;
            }
        }


        else if(gameObject.layer==7)
        {
            if (instance.destroyEffectPool.Count>0)
            {
                var particle = instance.destroyEffectPool.Dequeue();
                particle.transform.SetParent(gameObject.transform);
                particle.transform.position = gameObject.transform.position;
                particle.Play();
                return particle;
            }
            else
            {
                var newParticle = instance.CreateDestroyParticle();
                newParticle.transform.SetParent(gameObject.transform);
                newParticle.transform.position = gameObject.transform.position;            
                newParticle.Play();
                return newParticle;
            }
        
        }
        else
            return null;
    }

    public static ParticleSystem GetDestroyParticle(GameObject gameObject)
    {
        if (instance.destroyEffectPool.Count > 0)
        {
            var particle = instance.destroyEffectPool.Dequeue();
            particle.transform.SetParent(gameObject.transform);
            particle.transform.position = gameObject.transform.position;
            particle.Play();
            return particle;
        }
        else
        {
            var newParticle = instance.CreateDestroyParticle();
            newParticle.transform.SetParent(gameObject.transform);
            newParticle.transform.position = gameObject.transform.position;
            newParticle.Play();
            return newParticle;
        }
    }
    public static ParticleSystem GetUpgradeParticle(GameObject gameObject)
    {
        if (instance.upgradeEffectPool.Count > 0)
        {
            var particle = instance.upgradeEffectPool.Dequeue();
            particle.transform.SetParent(gameObject.transform);
            particle.transform.position = gameObject.transform.position;
            particle.Play();
            return particle;
        }
        else
        {
            var newParticle = instance.CreateUpgradeParticle();
            newParticle.transform.SetParent(gameObject.transform);
            newParticle.transform.position = gameObject.transform.position;
            newParticle.Play();
            return newParticle;
        }
    }

    public static void ReturnParticle(ParticleSystem particle)
    {
       

        particle.transform.SetParent(instance.transform);
        if (particle.transform.parent.gameObject.layer == 7)
        {
            instance.upgradeEffectPool.Enqueue(particle);

            
        }
        else if(particle.transform.parent.gameObject.layer == 6)
        {
            instance.destroyEffectPool.Enqueue(particle);
        }
        
    }


    public static void ReturnUpgradeParticle(ParticleSystem particle)
    {


        particle.transform.SetParent(instance.transform);
     
            instance.upgradeEffectPool.Enqueue(particle);


      

    }

    public static void ReturnDestroyParticle(ParticleSystem particle)
    {


        particle.transform.SetParent(instance.transform);
 
            instance.destroyEffectPool.Enqueue(particle);
        

    }


}

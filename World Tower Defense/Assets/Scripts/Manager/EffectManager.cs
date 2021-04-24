using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
    int destroyPoolCount = 10;
    int upgradePoolCount = 5;

    private static EffectManager mInstance;
    public static EffectManager instance
    {
        get 
        { 
            if (mInstance == null) mInstance = FindObjectOfType<EffectManager>();
            return mInstance;
        }
    }
    private Monster monster;

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

       newParticle.transform.SetParent(null); newParticle.Stop();

       return newParticle;

    }
    private ParticleSystem CreateUpgradeParticle()
    {

        var newParticle = Instantiate(upgradePrefab).GetComponent<ParticleSystem>();

        newParticle.transform.SetParent(null); newParticle.Stop();

        return newParticle;

    }





    public static ParticleSystem GetParticle(Transform transform)
    {

  
        if (transform.name == "Tower(Clone)")
        {

            if (instance.upgradeEffectPool.Count > 0)
            {
                var particle = instance.upgradeEffectPool.Dequeue();
                particle.transform.SetParent(transform);
                particle.transform.position = transform.position;
                particle.Play();
                return particle;

            }
            else
            {

                var newParticle = instance.CreateUpgradeParticle();
                newParticle.transform.SetParent(transform);
                newParticle.transform.position = transform.position;
                newParticle.Play();
                return newParticle;

            }
        }


        else
        {

           
            if (instance.destroyEffectPool.Count>0)
            {
                var particle = instance.destroyEffectPool.Dequeue();
        
                particle.transform.SetParent(transform);
                particle.transform.position = transform.position;
                particle.Play();
                return particle;
               

            }
            else
            {

                var newParticle = instance.CreateDestroyParticle();
             
                newParticle.transform.SetParent(transform);
                newParticle.transform.position = transform.position;
               
                newParticle.Play();
                return newParticle;

            }
        }

  


    }

    public static void ReturnParticle(ParticleSystem particle)
    {
       

        particle.transform.SetParent(instance.transform);
        if (particle.transform.name == "Tower(Clone)")
        {
            instance.upgradeEffectPool.Enqueue(particle);
            
        }
        else
        {
            instance.destroyEffectPool.Enqueue(particle);
      

        }




    }





}

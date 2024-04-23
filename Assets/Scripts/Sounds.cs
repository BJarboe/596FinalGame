using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public static class Sounds
    {
        public static void MakeSound(Sound sound)
        {
            Debug.Log("MakeSound Called");
            Collider[] col = Physics.OverlapSphere(sound.pos, sound.range);

            for(int i = 0; i < col.Length; i++)
            {
                Debug.Log("in loop");
                if (col[i].TryGetComponent(out EnemyBehavior hearer))
                {
                    Debug.Log("enemy detected");
                    hearer.SetIsRespondingToSound(true);
                    hearer.RespondToSound(sound);
                }
                //hearer.SetIsRespondingToSound(false);
                //col[i].GetComponent<EnemyBehavior>().RespondToSound(sound);
            }
        }
    }
}
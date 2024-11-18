using System.Linq;
using ForgeCore.Entities.Health;
using ForgeCore.Utility.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ForgeCore.Entities
{
    [RequireComponent(typeof(HealthSystem))]
    public class LootTable : MonoBehaviour
    {
        [SerializeField]
        private SerializedDictionary<GameObject, float> lootTable = new();

        private void Awake()
        {
            GetComponent<HealthSystem>().OnDeath.AddListener(DropLoot);
        }

        private void DropLoot()
        {
            foreach (var drop in lootTable.ToDictionary().Where(drop => drop.Value / 100f >= Random.Range(0f, 1f)))
            {
                Instantiate(drop.Key, transform.position, drop.Key.transform.rotation);
            }
        }
    }
}

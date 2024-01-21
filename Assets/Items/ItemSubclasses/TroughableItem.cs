
using UnityEngine;

namespace Items.ItemSubclasses
{
    public class TroughableItem : UseableItem
    {
        [SerializeField] private float throwAmount = 500;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void use()
        {
            drop(throwAmount);
        }
    }
}
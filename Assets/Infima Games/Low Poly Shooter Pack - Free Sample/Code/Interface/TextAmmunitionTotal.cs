// Copyright 2021, Infima Games. All Rights Reserved.

using System.Globalization;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Total Ammunition Text.
    /// </summary>
    public class TextAmmunitionTotal : ElementText
    {
        #region METHODS
        
        /// <summary>
        /// Tick.
        /// </summary>
        protected override void Tick()
        {
            if (equippedWeapon.gameObject.GetComponent<Weapon>())
            {
                if (equippedWeapon.gameObject.GetComponent<Weapon>().GetInfiniteReload())
                {
                    textMesh.text = "Infinite";
                }
                else
                {
                    textMesh.text = equippedWeapon.gameObject.GetComponent<Weapon>().reloadAmmunition.ToString();
                }
            }
        }
        
        #endregion
    }
}
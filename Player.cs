using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrex
{
    class Player
    {
        private int currentHealth;
        private int maxHealth;
        private int reservedHealth_Flat;
        private int reservedHealth_Percent;

        private int currentMana;
        private int maxMana;
        private int reservedMana_Flat;
        private int reservedMana_Percent;

        private int currentEnergyShield;
        private int maxEnergyShield;

        private int currentEXP;
        private int maxEXP;
        private int hourEXP;

        private int currentLevel;

        #region Health
        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                currentHealth = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value;
            }
        }

        public int ReservedHealth_Flat
        {
            get
            {
                return reservedHealth_Flat;
            }
            set
            {
                reservedHealth_Flat = value;
            }
        }

        public int ReservedHealth_Percent
        {
            get
            {
                return reservedHealth_Percent;
            }
            set
            {
                reservedHealth_Percent = value;
            }
        }
        #endregion

        #region Mana

        public int CurrentMana
        {
            get
            {
                return currentMana;
            }
            set
            {
                currentMana = value;
            }
        }

        public int MaxMana
        {
            get
            {
                return maxMana;
            }
            set
            {
                maxMana = value;
            }
        }

        public int ReservedMana_Flat
        {
            get
            {
                return reservedMana_Flat;
            }
            set
            {
                reservedMana_Flat = value;
            }
        }

        public int ReservedMana_Percent
        {
            get
            {
                return reservedMana_Percent;
            }
            set
            {
                reservedMana_Percent = value;
            }
        }

        #endregion

        #region EnergyShield

        public int CurrentEnergyShield
        {
            get
            {
                return currentEnergyShield;
            }
            set
            {
                currentEnergyShield = value;
            }
        }

        public int MaxEnergyShield
        {
            get
            {
                return maxEnergyShield;
            }
            set
            {
                maxEnergyShield = value;
            }
        }

        #endregion

        #region Experience

        public int CurrentEXP
        {
            get
            {
                return currentEXP;
            }
            set
            {
                currentEXP = value;
            }
        }

        public int MaxEXP
        {
            get
            {
                return maxEXP;
            }
            set
            {
                maxEXP = value;
            }
        }

        public int HourEXP
        {
            get
            {
                return hourEXP;
            }
            set
            {
                hourEXP = value;
            }
        }

        #endregion

        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
            set
            {
                currentLevel = value;
            }
        }
    }
}

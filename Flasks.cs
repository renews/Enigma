using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrex
{
    class Flasks
    {
        private string metaData;
        private string baseItemType;

        private string prefix;
        private string prefixEffect;
        private string suffix;
        private string suffixEffect;

        private int quality;

        private int currentCharges;
        private int totalCharges;
        private int chargesPerUse;

        private int localStat1Type;
        private int localStat1Value;
        private int localStat2Type;
        private int localStat2Value;

        private bool removesBurning;
        private bool removesShock;
        private bool removesFreezing;
        private bool removesBleeding;
        private bool instantHealing;

        public string MetaData
        {
            get
            {
                return metaData;
            }
            set
            {
                metaData = value;
            }
        }

        public string BaseItemType
        {
            get
            {
                return baseItemType;
            }
            set
            {
                baseItemType = value;
            }
        }

        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value;
            }
        }

        public string PrefixEffect
        {
            get
            {
                return prefixEffect;
            }
            set
            {
                prefixEffect = value;
            }
        }

        public string Suffix
        {
            get
            {
                return suffix;
            }
            set
            {
                suffix = value;
            }
        }

        public string SuffixEffect
        {
            get
            {
                return suffixEffect;
            }
            set
            {
                suffixEffect = value;
            }
        }

        public int Quality
        {
            get
            {
                return quality;
            }
            set
            {
                quality = value;
            }
        }

        public int CurrentCharges
        {
            get
            {
                return currentCharges;
            }
            set
            {
                currentCharges = value;
            }
        }

        public int TotalCharges
        {
            get
            {
                return totalCharges;
            }
            set
            {
                totalCharges = value;
            }
        }

        public int ChargesPerUse
        {
            get
            {
                return chargesPerUse;
            }
            set
            {
                chargesPerUse = value;
            }
        }

        public int LocalStat1Type
        {
            get
            {
                return localStat1Type;
            }
            set
            {
                localStat1Type = value;
            }
        }

        public int LocalStat1Value
        {
            get
            {
                return localStat1Value;
            }
            set
            {
                localStat1Value = value;
            }
        }

        public int LocalStat2Type
        {
            get
            {
                return localStat2Type;
            }
            set
            {
                localStat2Type = value;
            }
        }

        public int LocalStat2Value
        {
            get
            {
                return localStat2Value;
            }
            set
            {
                localStat2Value = value;
            }
        }

        public bool RemovesBurning
        {
            get
            {
                return removesBurning;
            }
            set
            {
                removesBurning = value;
            }
        }

        public bool RemovesShock
        {
            get
            {
                return removesShock;
            }
            set
            {
                removesShock = value;
            }
        }

        public bool RemovesFreezing
        {
            get
            {
                return removesFreezing;
            }
            set
            {
                removesFreezing = value;
            }
        }

        public bool RemovesBleeding
        {
            get
            {
                return removesBleeding;
            }
            set
            {
                removesBleeding = value;
            }
        }

        public bool InstantHealing
        {
            get
            {
                return instantHealing;
            }
            set
            {
                instantHealing = value;
            }
        }

    }
}

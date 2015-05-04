using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrex
{
    class Buffs
    {
        private string name;
        private float charges;
        private float duration;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public float Charges
        {
            get
            {
                return charges;
            }
            set
            {
                charges = value;
            }
        }

        public float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }
    }
}

using System;

namespace AutoTeam.Domain.Model
{
    public class Capacity
    {
        public Classification Classification { get; }

        /// <summary>
        /// The maximal number of students allowed of the given classification
        /// </summary>
        public int Max { get; }
        
        public Capacity(Classification classification, int max)
        {
            Classification = classification ?? throw new ArgumentNullException(nameof(classification));
            Max = max;
        }
        
        public bool CanIncreaseCapacity()
        { 
            return Max < int.MaxValue;
        }

        public Capacity WithIncreasedCapacity()
        {
            if (CanIncreaseCapacity())
                return new Capacity(this.Classification, Max + 1);
            else return this;
        }

        public bool CanDecreaseCapacity()
        {
            return Max > 0;
        }

        public Capacity WithDecreasedCapacity()
        {
            if (CanDecreaseCapacity())
                return new Capacity(this.Classification, Max - 1);
            else return this;
        }
    }
}
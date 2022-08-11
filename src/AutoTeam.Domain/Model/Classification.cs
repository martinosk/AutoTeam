using System;
namespace AutoTeam.Domain.Model
{
    public abstract class Classification
    {
        public abstract string Description { get; }

        public static Classification Create(ClassificationEnum classification)
        {
            return classification switch
            {
                ClassificationEnum.Male => new Male(),
                ClassificationEnum.Female => new Female(),
                _ => throw new ArgumentOutOfRangeException(nameof(classification), "Unknown classification: " + classification.ToString()),
            };
        }
    }
    
    public class Female : Classification
    {
        public override string Description { get { return "Female"; } }
    }

    public class Male : Classification
    {
        public override string Description { get { return "Male"; } }
    }

    public enum ClassificationEnum
    {
        Male,
        Female
    }
}
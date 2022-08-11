using System;

namespace AutoTeam.Domain.Model
{
    public class Student
    {
        public Guid Id { get; }

        public Classification Classification { get; }
        
        public string Name { get; }
        public Group CurrentGroup { get; private set; }

        public static Student WithName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return new Student(name);
        }

        public Student WithClassification(Classification classification)
        {
            if (classification == null)
                throw new ArgumentNullException("classification");
            return new Student(Name, classification, Id);
        }
        
        private Student(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Student(string name, Classification classification, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(nameof(id));
            }

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Classification = classification ?? throw new ArgumentNullException(nameof(classification));
            Id = id;
        }

        public void SetCurrentGroup(Group group)
        {
            if (group == null)
                throw new ArgumentNullException("group");
            this.CurrentGroup = group;
        }

        public void RemoveFromCurrentGroup()
        {
            CurrentGroup = null;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (Student)obj;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

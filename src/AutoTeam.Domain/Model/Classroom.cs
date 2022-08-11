using System;
using System.Linq;

using System.Collections.Generic;

namespace AutoTeam.Domain.Model
{
    public class Classroom : IAggregateRoot
    {
        private readonly Random r;
        private readonly HashSet<Student> students;
        private readonly HashSet<Classification> classifications;

        public IEnumerable<Classification> Classifications { get { return classifications; } }
        public IEnumerable<Student> Students { get { return students; } }
  
        public Groups Groups {  get ; }

        public Classroom(Groups groups)
        {
            this.students = new HashSet<Student>();
            this.classifications = new HashSet<Classification>();
            Groups = groups ?? throw new ArgumentNullException(nameof(groups));
            r = new Random();
        }

        public void AddClassification(Classification classification)
        {
            if (classification == null)
                throw new ArgumentNullException(nameof(classification));
            classifications.Add(classification);
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            students.Add(student);
        }

        public void AddStudents(IEnumerable<Student> students)
        {
            if (students == null)
                throw new ArgumentNullException(nameof(students));

            foreach(var student in students)
                this.students.Add(student);
        }

        public void RemoveStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            students.Remove(student);
        }

        public bool GroupsHasCapacity
        {
            get
            {
                var students = Students.Where(f=>f.CurrentGroup == null).GroupBy(s => s.Classification);

                foreach (var studentClassification in students)
                {
                    if (studentClassification.Count() > Groups.Sum(f => f.RemainingCapacity(studentClassification.Key)))
                        return false;
                }
                return true;
            }
        }


        public void AssignStudentsToGroups()
        {

            // The aggregate root must be responsible for validating the business logic that spans several entities
            // In this case, throw an error if there are too many or too few 
            // of a specific student classification compared to the available group capacities

            if (!GroupsHasCapacity)
                throw new ClassroomGroupCapacityException("Not enough capacity in groups");

            foreach (var student in Students.Where(f=>f.CurrentGroup == null).OrderBy(x => r.Next()))
            {
                Groups.AssignStudent(student);
            }
        }

        public void AcceptAllGroups()
        {
            foreach (var group in Groups)
            {
                group.AcceptGroup();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoTeam.Domain.Model;
using AutoTeam.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTeam.Tests.Model
{
    [TestClass]
    public class ClassroomSystemTest
    {
        [TestMethod]
        public void CreateGroups()
        {
            var boy = Classification.Create(ClassificationEnum.Male);
            var girl = Classification.Create(ClassificationEnum.Female);
            var boys2 = new Capacity(boy, 2);
            var girls2 = new Capacity(girl, 2);
            
            var groups = new Groups(new AssignToBestGroup());
            var g1 = new Group();
            g1.AddCapacity(boys2);
            g1.AddCapacity(girls2);
            var g2 = new Group();
            g2.AddCapacity(boys2);
            g2.AddCapacity(girls2);
            var g3 = new Group();
            g3.AddCapacity(boys2);
            g3.AddCapacity(girls2);
            var g4 = new Group();
            g4.AddCapacity(boys2);
            g4.AddCapacity(girls2);
            var g5 = new Group();
            g5.AddCapacity(boys2);
            g5.AddCapacity(girls2);
            var g6 = new Group();
            g6.AddCapacity(boys2);
            g6.AddCapacity(girls2);
            var allBoys = new Group();
            allBoys.AddCapacity(boys2);
            allBoys.AddCapacity(boys2);

            groups.AddGroup(g1);
            groups.AddGroup(g2);
            groups.AddGroup(g3);
            groups.AddGroup(g4);
            groups.AddGroup(g5);
            groups.AddGroup(g6);
            groups.AddGroup(allBoys);


            var classRoom = new Classroom(groups);
            classRoom.AddStudent(Student.WithName("Farhan").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Albert").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Andreas").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Bertil").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Emilio").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Leon").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Linus").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Magnus").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Malte").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Max").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Saad").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Saxe").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Silas").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Simon").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Vilas").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Vito").WithClassification(boy));
            classRoom.AddStudent(Student.WithName("Asia").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Augusta").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Elisabeth").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Elvira").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Freja").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Hazel").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Lilje").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Lina").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Mathilda").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Móna").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Sally").WithClassification(girl));
            classRoom.AddStudent(Student.WithName("Sienna").WithClassification(girl));


            for (int i = 1; i <= 100; i++)
            {

                classRoom.AssignStudentsToGroups();
                List<string> groupOutput = new List<string>();
                var groupNumber = 1;

                foreach (var group in classRoom.Groups)
                {
                    groupOutput.Add("Gruppe " + groupNumber + ":");
                    groupOutput.AddRange(group.CurrentMembers.Select(f => f.Name));
                    groupNumber++;
                    groupOutput.Add(Environment.NewLine);
                }
                
                File.WriteAllLines(Path.GetTempPath() + "Legegrupper" + i + ".txt", groupOutput);

                classRoom.AcceptAllGroups();
            }
        }
    }
}

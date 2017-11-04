using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

/*
Dependency Inversion:

A. High-level modules should not depend on low-level modules. Both should depend on abstractions.
B. Abstractions should not depend on details. Details should depend on abstractions.

Let's take the classical example of a copy module which reads characters from the keyboard and writes them
to the printer device. The high level class containing the logic is the Copy class. The low level classes 
are KeyboardReader and PrinterWriter.

In a bad design the high level class uses directly and depends heavily on the low level classes. In such a
case if we want to change the design to direct the output to a new FileWriter class we have to make changes
in the Copy class. (Let's assume that it is a very complex class, with a lot of logic and really hard to test).

In order to avoid such problems we can introduce an abstraction layer between high level classes and low
level classes. Since the high level modules contain the complex logic they should not depend on the low
level modules so the new abstraction layer should not be created based on low level modules. Low level
modules are to be created based on the abstraction layer.

According to this principle the way of designing a class structure is to start from high level modules to
the low level modules:  High Level Classes --> Abstraction Layer --> Low Level Classes

*/

namespace Dependency_Inversion_Principle
{
    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }


    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    public class Person
    {
        public string Name;
    }

    // low-level
    public class Relationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations
            = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        //        Bad approach
        //        public List<(Person, Relationship, Person)> Relations => relations;
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
                return relations
                    .Where(x => x.Item1.Name == name
                                && x.Item2 == Relationship.Parent).Select(r => r.Item3);
        }
    }

    //high-level
    class Program 
    {
        //        Bad approach
        //        public Program(Relationships relationships)
        //        {
        //            var relations = relationships.Relations;
        //            foreach (var relation in relations.Where(x =>
        //                x.Item1.Name == "John"
        //                && x.Item2 == Relationship.Parent))
        //            {
        //                Console.WriteLine($"Json has a parent called{relation.Item3.Name}");
        //            }
        //        }


        //Better approach 
        public Program(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("John"))
            {
                WriteLine($"John has a child called {p.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Matt" };

            // low-level module
            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Program(relationships);
        }

    }
}
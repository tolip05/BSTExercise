using System;

namespace BstExercize
{
    class Program
    {
        static void Main(string[] args)
        {
            IBinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Insert(10);
            bst.Insert(5);
            bst.Insert(3);
            bst.Insert(1);
            bst.Insert(4);
            bst.Insert(8);
            bst.Insert(9);
            bst.Insert(37);
            bst.Insert(39);
            bst.Insert(45);

            bst.EachInOrder(Console.WriteLine);
            Console.WriteLine(string.Join(" ",bst.Range(3,5)));

        }
    }
}

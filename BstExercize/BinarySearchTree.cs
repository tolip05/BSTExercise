using System;
using System.Collections.Generic;
using System.Text;

namespace BstExercize
{
    public class BinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable
    {

        private Node root;

        public BinarySearchTree()
        {
            
        }

        private BinarySearchTree(Node node)
        {
            this.PreOrderCopy(node);
        }

        private Node FinedElement(T element)
        {
            Node current = this.root;
            while (current != null)
            {
                if (current.Value.CompareTo(element) > 0)
                {
                    current = current.Left;
                }else if (current.Value.CompareTo(element) < 0)
                {
                    current = current.Right;
                }
                else
                {
                    break;
                }
            }
            return current;
        }
        private void PreOrderCopy(Node node)
        {
            if (node == null)
            {
                return;
            }
            this.Insert(node.Value);
            this.PreOrderCopy(node.Left);
            this.PreOrderCopy(node.Right);
        }

        public void Insert(T element)
        {
            this.root = this.Insert(this.root,element);
          
        }

        private Node Insert(Node node, T element)
        {
            if (node == null)
            {
                node = new Node(element);
            }
            else if (element.CompareTo(node.Value) < 0)
            {
                node.Left = this.Insert(node.Left,element);
            }
            else if (element.CompareTo(node.Value) > 0)
            {
                node.Right = this.Insert(node.Right,element);
            }
            node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
            return node;
        }

       

        public bool Contains(T element)
        {
            Node current = this.FinedElement(element);

            return current != null;
        }

        public int Count()
        {
          return this.Count(this.root);
        }
        private int Count(Node node)
        {
            if (node == null)
            {
                return 0;
            }
            return node.Count;
        }

        public void Delete(T element)
        {
            if (this.Count(this.root) == 0 || !this.Contains(element))
            {
                throw new InvalidOperationException();
            }
            this.Delete(this.root,element);
        }

        private Node Delete(Node node, T element)
        {
            if (node == null)
            {
                return null;
            }
            int compare = node.Value.CompareTo(element);
            if (compare > 0)
            {
                node.Left = this.Delete(node.Left,element);
            }
            else if (compare < 0)
            {
                node.Right = this.Delete(node.Right,element);
            }
            else
            {
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                Node leftMost = this.SubtreeLeftmost(node.Right);

                node.Value = leftMost.Value;
                node.Right = this.Delete(node.Right,leftMost.Value);
            }
            node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
            return node;
        }

        private Node SubtreeLeftmost(Node node)
        {
            Node current = node;
            while (current.Left != null)
            {
                current = current.Left;
            }
            return current;
        }

        
        public void DeleteMax()
        {
            if (this.root == null)
            {
                throw new InvalidOperationException();
            }
            this.DeleteMax(this.root);
        }

        private Node DeleteMax(Node node)
        {
            if (node.Right == null)
            {
                return node.Left;
            }
            node.Right = this.DeleteMax(node.Right);
            node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
            return node;
        }

        public void DeleteMin()
        {
            if (this.root == null)
            {
                return;
            }
            Node parent = null;
            Node current = this.root;
            while (current.Left != null)
            {
                parent = current;
                current = current.Left;
            }
            if (parent == null)
            {
                this.root = this.root.Right;
                this.root.Count = 1 + this.Count(this.root.Left)
                    + this.Count(this.root.Right);
            }
            else
            {
                parent.Left = parent.Right;

            }
           
        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(this.root, action);
        }

        private void EachInOrder(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }
            this.EachInOrder(node.Left,action);
            action(node.Value);
            this.EachInOrder(node.Right,action);
        }

        

        

        public IEnumerable<T> Range(T startRange, T endRange)
        {
            Queue<T> queue = new Queue<T>();
            this.Range(this.root, queue, startRange, endRange);
            return queue;
            
        }

        private void Range(Node node, Queue<T> queue, T start, T end)
        {
            if (node == null)
            {
                return;
            }

            int nodeInLowerRange = start.CompareTo(node.Value);
            int nodeInHigherRange = end.CompareTo(node.Value);

            if (nodeInLowerRange < 0)
            {
                this.Range(node.Left, queue, start, end);
            }
            if (nodeInLowerRange <= 0 && nodeInHigherRange >= 0)
            {
                queue.Enqueue(node.Value);
            }
            if (nodeInHigherRange > 0)
            {
                this.Range(node.Right, queue, start, end);
            }
        }

        public int Rank(T element)
        {
            return this.Rank(this.root,element);
        }

        private int Rank(Node node, T element)
        {
            if (node == null)
            {
                return 0;
            }
            int compare = node.Value.CompareTo(element);

            if (compare > 0)
            {
                return this.Rank(node.Left, element);
            }
            else if (compare < 0)
            {
                return 1 + this.Count(node.Left) + this.Rank(node.Right, element);
            }
            return this.Count(node.Left);
        }

        public BinarySearchTree<T> Search(T element)
        {
            Node current = this.FinedElement(element);
            return new BinarySearchTree<T>(current);
        }
        public T Floor(T element)
        {
            return this.Select(this.Rank(element) - 1);
        }

        public T Ceiling(T element)
        {
            return this.Select(this.Rank(element) + 1);
        }
        public T Select(int rank)
        {
            Node node = this.Select(this.root, rank);

            if (node == null)
            {
                throw new InvalidOperationException();
            }
            return node.Value;
        }

        private Node Select(Node node, int rank)
        {
            if (node == null)
            {
                return null;
            }

            int leftCount = this.Count(node.Left);

            if (leftCount.CompareTo(rank)> 0)
            {
                return this.Select(node.Left,rank);
            }
            else if (leftCount.CompareTo(rank) < 0)
            {
                return this.Select(node.Right,rank - (leftCount + 1));
            }
            return node;
        }

        private class Node
        {

            public Node(T value)
            {
                this.Value = value;
            }
            public T Value { get; set; }
            public int Count { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }
        }
    }
}

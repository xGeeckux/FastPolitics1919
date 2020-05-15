using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFastPolitics1919.AVL
{
    [Serializable]
    public class SortList<T>
    {
        private Node<T> Root { get; set; }
        public int TreeLength { get; set; }
        public int Length => TreeLength;
        public int Count => TreeLength;

        //- Add Node
        public void Add(T _obj, float value)
        {
            TreeLength++;
            Node<T> new_node = new Node<T>(_obj, value);
            if (Root == null)
                Root = new_node;
            else
                Root = RecursiveInsert(Root, new_node);
        }

        //- Balance & Insert
        private Node<T> RecursiveInsert(Node<T> current, Node<T> n)
        {
            if (current == null)
            {
                current = n;
                return current;
            }
            if (n.Value < current.Value)
            {
                current.Left = RecursiveInsert(current.Left, n);
                current = BalanceTree(current);
            }
            else if (n.Value >= current.Value)
            {
                current.Right = RecursiveInsert(current.Right, n);
                current = BalanceTree(current);
            }
            return current;
        }
        private Node<T> BalanceTree(Node<T> current)
        {
            int b_factor = BalanceFactor(current);
            if (b_factor > 1)
            {
                if (BalanceFactor(current.Left) > 0)
                    current = RotateLL(current);
                else
                    current = RotateLR(current);
            }
            else if (b_factor < -1)
            {
                if (BalanceFactor(current.Right) > 0)
                    current = RotateRL(current);
                else
                    current = RotateRR(current);
            }
            return current;
        }

        //- Remove Node
        public void Remove(float value)
        {
            TreeLength--;
            Root = Remove(Root, value);
        }
        private Node<T> Remove(Node<T> current, float value)
        {
            Node<T> parent;
            if (current == null)
                return null;
            else
            {
                if (value < current.Value)
                {
                    current.Left = Remove(current.Left, value);
                    if (BalanceFactor(current) == -2)
                    {
                        if (BalanceFactor(current.Right) <= 0)
                            current = RotateRR(current);
                        else
                            current = RotateRL(current);
                    }
                }
                else if (value > current.Value)
                {
                    current.Right = Remove(current.Right, value);
                    if (BalanceFactor(current) == 2)
                    {
                        if (BalanceFactor(current.Left) >= 0)
                            current = RotateLL(current);
                        else
                            current = RotateLR(current);
                    }
                }
                else
                {
                    if (current.Right != null)
                    {
                        parent = current.Right;
                        while (parent.Left != null)
                            parent = parent.Left;
                        current.Value = parent.Value;
                        current.Object = parent.Object;
                        current.Right = Remove(current.Right, parent.Value);
                        if (BalanceFactor(current) == 2)
                        {
                            if (BalanceFactor(current.Left) >= 0)
                                current = RotateLL(current);
                            else
                                current = RotateLR(current);
                        }
                    }
                    else
                        return current.Left;
                }
            }
            return current;
        }
        public SortList<T> RemoveEveryUnefficient(double value)
        {
            SortList<T> local = new SortList<T>();
            List<Node<T>> copy_list = this.GetNodes().ToList();
            foreach (Node<T> node in copy_list)
            {
                if (node.Value != value)
                    local.Add(node.Object, node.Value);
            }
            return local;
        }

        //- Search
        public bool IsIn(float value)
        {
            Node<T> node = FindNode(value);
            if (node != null)
                return true;
            return false;
        }
        public T Find(float value)
        {
            Node<T> node = FindNode(value);
            if(node != null)
                return node.Object;
            return default(T);
        }
        public Node<T> FindNode(float value)
        {
            try { return Find(value, Root); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        private Node<T> Find(float value, Node<T> current)
        {
            if (current == null)
                return null;
            if (value < current.Value)
            {
                if (value == current.Value)
                    return current;
                else
                    return Find(value, current.Left);
            }
            else
            {
                if (value == current.Value)
                    return current;
                else
                    return Find(value, current.Right);
            }
        }
        public T[] FindEveryUnefficient(double value)
        {
            List<T> local = new List<T>();
            List<Node<T>> copy_list = this.GetNodes().ToList();
            foreach (Node<T> node in copy_list)
            {
                if (node.Value == value)
                    local.Add(node.Object);
            }
            return local.ToArray();
        }

        //- Show
        public void Show()
        {
            if (Root == null)
            {
                Console.WriteLine("List is empty");
                return;
            }
            InOrderDisplayTree(Root);
            Console.WriteLine();
        }
        private void InOrderDisplayTree(Node<T> current)
        {
            if (current != null)
            {
                InOrderDisplayTree(current.Left);
                Console.WriteLine(" > (" + current.Value + ")");
                InOrderDisplayTree(current.Right);
            }
        }
        private List<Node<T>> InOrderList { get; set; }
        private void InOrderFillList(Node<T> current)
        {
            if (current != null)
            {
                InOrderFillList(current.Left);
                InOrderList.Add(current);
                InOrderFillList(current.Right);
            }
        }
        public Node<T>[] GetNodes()
        {
            InOrderList = new List<Node<T>>();
            if (Root == null)
                return InOrderList.ToArray();
            InOrderFillList(Root);
            return InOrderList.ToArray();
        }
        public T[] Get()
        {
            InOrderList = new List<Node<T>>();
            List<T> local = new List<T>();
            if (Root == null)
                return local.ToArray();
            InOrderFillList(Root);
            foreach (Node<T> node in InOrderList)
                local.Add(node.Object);
            return local.ToArray();
        }

        //- Operatoren
        private int Max(int left, int right)
        {
            return left > right ? left : right;
        }
        private int GetHeight(Node<T> current)
        {
            int height = 0;
            if (current != null)
            {
                int left = GetHeight(current.Left);
                int right = GetHeight(current.Right);
                int max = Max(left, right);
                height = max + 1;
            }
            return height;
        }
        private int BalanceFactor(Node<T> current)
        {
            int left = GetHeight(current.Left);
            int right = GetHeight(current.Right);
            int b_factor = left - right;
            return b_factor;
        }

        //- Rotation of nodes
        private Node<T> RotateRR(Node<T> parent)
        {
            Node<T> pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;
            return pivot;
        }
        private Node<T> RotateLL(Node<T> parent)
        {
            Node<T> pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;
            return pivot;
        }
        private Node<T> RotateLR(Node<T> parent)
        {
            Node<T> pivot = parent.Left;
            parent.Left = RotateRR(pivot);
            return RotateLL(parent);
        }
        private Node<T> RotateRL(Node<T> parent)
        {
            Node<T> pivot = parent.Right;
            parent.Right = RotateLL(pivot);
            return RotateRR(parent);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFastPolitics1919.AVL
{
    [Serializable]
    public class Node<T>
    {
        public T Object { get; set; }
        public float Value { get; set; }

        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }

        public Node(T _obj, float value)
        {
            Object = _obj;
            Value = value;
        }
    }
}

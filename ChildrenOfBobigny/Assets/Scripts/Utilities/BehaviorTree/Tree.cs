using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree();
        }

        protected virtual void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract Node SetupTree();

        protected void AttachRoot(Node node)
        {
            node.Root = _root;

            if (node.Children.Count == 0)
                return;

            foreach (Node child in node.Children)
            {
                child.Root = _root;
                AttachRoot(child);
            }
        }
    }
}
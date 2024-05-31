using UnityEngine;

namespace BehaviourTree
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        protected Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree();

            AttachRoot(_root);
        }

        protected virtual void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract Node SetupTree();

        private void AttachRoot(Node node)
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
using Eastermaster.Helper;

namespace Eastermaster.TreeBehaviour
{
    public abstract class NestedNode<T> : TreeNode<T> where T : IContext
    {
        protected TreeNode<T> currentNode;
        protected TreeNode<T>[] childNodes;
        protected int index;

        public override TreeNode<T> Init(T _ctx)
        {
            base.Init(_ctx);
            childNodes = this.GetComponentsInNearChildren<TreeNode<T>>();
            return this;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            currentNode = childNodes[0];
            currentNode.OnEnter();
            index = 0;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            currentNode.OnFixedUpdate();
        }

        protected void NextNode()
        {
            index++;
            currentNode = childNodes[index];
            currentNode.OnEnter();
        }
    }
}
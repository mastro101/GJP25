using Eastermaster.Helper;

namespace Eastermaster.TreeBehaviour
{
    public abstract class NestedNode<T> : TreeNode<T> where T : IContext
    {
        protected TreeNode<T> currentNode;
        protected TreeNode<T>[] childNode;
        protected int index;

        public override TreeNode<T> Init(T _ctx)
        {
            base.Init(_ctx);
            childNode = this.GetComponentsInNearChild<TreeNode<T>>();
            return this;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            currentNode = childNode[0];
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
            currentNode = childNode[index];
            currentNode.OnEnter();
        }
    }
}

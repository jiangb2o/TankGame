using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    class QuadTree
    {
        private static int capacity = 4;
        private int level;
        private List<GameObject> objects;
        public Rectangle Bounds { get; private set; }
        private QuadTree[] nodes;

        private Pen drawPen = new Pen(Color.White);

        public QuadTree(int pLevel, Rectangle pBounds)
        {
            level = pLevel;
            Bounds = pBounds;
            objects = new List<GameObject>();
            nodes = new QuadTree[4];
        }

        private void Split()
        {
            int subWidth = Bounds.Width / 2;
            int subHeight = Bounds.Height / 2;
            int x = Bounds.X;
            int y = Bounds.Y;

            // 第一, 二, 三, 四象限
            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        public void Insert(GameObject gameObject)
        {
            if(objects.Count + 1 > capacity)
            {
                if(nodes[0] == null)
                {
                    Split();
                }
                QuadTree insertNode = null;

                Rectangle rect = gameObject.GetRectangle();
                foreach (QuadTree node in nodes)
                {
                    if (rect.IntersectsWith(node.Bounds))
                    {
                        // 同时在多个子区域, 直接添加到父节点
                        if (insertNode != null)
                        {
                            objects.Add(gameObject);
                            break;
                            
                        } else 
                        {
                            insertNode = node;
                        }
                    }
                    if(node == nodes[3] && insertNode != null)
                        insertNode.Insert(gameObject);
                }
            } else
            {
                objects.Add(gameObject);
            }
        }

        // 将可能与rect碰撞的对象加入result中
        public void Retrieve(Rectangle target, List<GameObject> result)
        {
            result.AddRange(objects);
            if(nodes[0] != null)
            {
                foreach (QuadTree node in nodes)
                {
                    if (target.IntersectsWith(node.Bounds))
                    {
                        node.Retrieve(target, result);
                    }
                }
            }
        }

        public void Draw()
        {
            GameFramwork.g.DrawRectangle(drawPen, Bounds);
            if(this.nodes[0] != null)
            {
                foreach(var node in nodes)
                {
                    node.Draw();
                }
            }
        }

    }
}

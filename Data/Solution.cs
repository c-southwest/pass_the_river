using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Data
{
    public class Solution
    {
        int n_good; //商人数
        int n_bad;  //随从数
        int volume; //船的容量
        // direction: up=1, down=0
        int up = 1, down = 0;

        public Solution(int n_good, int n_bad, int volume)
        {
            this.n_good = n_good;
            this.n_bad = n_bad;
            this.volume = volume;
        }

        public List<string> GetSolution()
        {
            //初始条件判断
            if (n_good < n_bad) return null;

            //安全状态图
            var matrix = new int[n_good + 1, n_bad + 1];
            for (int i = 0; i <= n_good; i++)
            {
                for (int j = 0; j <= n_bad; j++)
                {
                    if (i == 0 | i == n_good | ((i >= j) && (n_good - i >= n_bad - j)))
                    {
                        matrix[i, j] = 1;
                    }
                }
            }

            //状态节点额外信息
            var Info_matrix = new Info[n_good + 1, n_bad + 1];
            // 保存路径
            var path = new List<Tuple<int, int>>();

            int x = n_good, y = n_bad; //Start point
            bool result = DFS(matrix, x, y, up, Info_matrix, path);

            //将节点路径加工成用户可读的过关攻略
            List<string> answer = new List<string>();
            for (int i = path.Count - 1; i >= 0; i--)
            {
                if (i == 0) break;
                int dx = path[i].Item1 - path[i - 1].Item1;
                int dy = path[i].Item2 - path[i - 1].Item2;
                string turn = (dx < 0 || dy < 0) ? "运回" : "运出";
                answer.Add($"第{path.Count - i}次：{turn}{Math.Abs(dx)}个商人 + {Math.Abs(dy)}个仆从");
            }

            //Console.WriteLine("\n通关秘籍：");
            //foreach (var item in answer)
            //{
            //    Console.WriteLine(item);
            //}
            return answer;
        }

        bool DFS(int[,] matrix, int x, int y, int direction, Info[,] Info_matrix, List<Tuple<int, int>> path)
        {
            //查询matrix判断当前状态是否安全
            if (matrix[x, y] == 0) return false;
            // 判断当前节点是否以相同方向访问过
            if (Info_matrix[x, y].up == true & direction == up) return false;
            if (Info_matrix[x, y].down == true & direction == down) return false;

            if (x == 0 & y == 0)
            {
                path.Add(new Tuple<int, int>(x, y));
                return true;
            }

            // 修改状态信息
            if (direction == down) Info_matrix[x, y].down = true;
            else Info_matrix[x, y].up = true;

            //当前节点是以up方向访问的，那么它会尝试down方向
            if (direction == up)
            {   // 遍历X,Y移动的可能性组合
                for (int i = 0; i <= volume; i++)
                {
                    for (int j = volume - i; j >= 0; j--)
                    {
                        // j = 向左的步数
                        if (i == 0 & j == 0) continue; //跳过无意义的移动
                        int new_x = x - j;
                        int new_y = y - i;
                        if (new_x < 0 | new_y < 0) continue;    //不合法的移动
                        if (DFS(matrix, new_x, new_y, down, Info_matrix, path) == true)
                        {
                            path.Add(new Tuple<int, int>(x, y));
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i <= volume; i++)
                {
                    for (int j = 0; j <= volume - i; j++)
                    {
                        if (i == 0 & j == 0) continue;
                        int new_x = x + j;
                        int new_y = y + i;
                        if (new_x > n_good | new_y > n_bad) continue;
                        if (DFS(matrix, new_x, new_y, up, Info_matrix, path) == true)
                        {
                            path.Add(new Tuple<int, int>(x, y));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //自定义结构体，用于记录状态点的额外信息
        struct Info
        {
            public bool up;    //访问到状态节点的时候方向
            public bool down;
        };

        //打印二维数组
        public void ShowMatrix(int[,] matrix)
        {
            for (int i = n_bad; i >= 0; i--)
            {
                for (int j = 0; j <= n_good; j++)
                    Console.Write($"{matrix[j, i]} ");
                Console.WriteLine();
            }
        }
    }
}

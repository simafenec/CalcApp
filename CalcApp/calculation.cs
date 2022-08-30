using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALC
{
    class KEISANNKI
    {
        private string calc(double x,double y, char c)
        {
            //計算機プログラム　与えられた値と記号に対して答えを文字列として返す
            if (c == '+')
            {
                return (x + y).ToString();
            }
            else if (c == '-')
            {
                return (x - y).ToString();
            }
            else if (c == '*')
            {
                return (x * y).ToString();
            }
            else
            {
                return (x / y).ToString();
            }
          
        }
        private int Search_pos_min(int[] nums) {
            //-1でない数字で最小のものを探す
            //もし渡された配列に-1しか入っていなかった場合は-1がそのまま返される。
            int return_value =nums.Max();
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] < 0) { continue; }
                else if(nums[i]<return_value){
                    return_value = nums[i];
                }
            }
            return return_value;
        }
        public string Formula_simplified(string formula) {
            //式を簡単にする
            string[] tag_list = new string[] { "+-",  "--" };
            string[] new_list = new string[] { "-", "+" };
            string str = formula;
            for (int i = 0; i < tag_list.Length; i++) {
                str = str.Replace(tag_list[i], new_list[i]);
            }
            return str;
        }
        public string[] SplitFormula(string formula) {
            //与えられた数式をぶつ切りにする
            List<string> return_list = new List<string>();
            char[] op = new char[] { '(', ')', '*', '/', '+'};
            char minus = '-';
            string temp="";
            //マイナス記号が一度きたときtrueにする変数を追加する
            bool exist_minus = false;
            //マイナス記号はすべて後ろの数字と合わせて負の数として扱う
            for (int i = 0; i < formula.Length; i++) {
                if (op.Contains(formula[i]))
                {
                    //opに含まれる記号のいずれかがその位置にあるとき
                    //リストに追加する
                    if (temp != "")
                    {
                        return_list.Add(temp);
                        temp = "";
                    }
                    return_list.Add(formula[i].ToString());
                    continue;
                }
                else if (formula[i] == minus && !exist_minus)
                {
                    //その位置にマイナス記号がありexist_minusがfalseの時
                    //マイナス記号があったことを認識する
                    if (temp != "") {
                        return_list.Add(temp);
                        temp = "";

                    }
                    exist_minus = true;
                    temp += formula[i].ToString();
                    continue;

                }
                else if (formula[i] == minus && exist_minus)
                {
                    exist_minus = false;
                    if (temp != "")
                    {
                        return_list.Add(temp);
                        temp = "";

                    }
                    temp = formula[i].ToString();
                    continue;
                }
                else {
                    temp += formula[i].ToString();
                    continue;
                }
            }
            return_list.Add(temp);
            return return_list.ToArray();
        }
        //配列を用いた計算プログラム
        //
        public string Calc_new(string[] formula) {
            string temp_ans;
            string[] op = new string[] { "(", ")", "*", "/", "+" ,"-"};
            if (formula.Length > 1)
            {
                //まずは括弧を探す
                int ex_leftbr = Array.IndexOf(formula, op[0]);
                if (ex_leftbr != -1)
                {
                    //左括弧が存在した時
                    int ex_rightbr = Array.LastIndexOf(formula, op[1]);
                    if (ex_rightbr == -1)
                    {
                        //左括弧があって右括弧がない時はエラーを返す
                        return "Error";
                    }
                    else
                    {
                        temp_ans= Calc_new(formula[(ex_leftbr + 1)..ex_rightbr]);
                        formula[ex_leftbr] = temp_ans;
                        var list = formula.ToList();
                        list.RemoveRange(ex_leftbr+1, ex_rightbr-ex_leftbr);
                        formula = list.ToArray();
                        return Calc_new(formula);
                    }
                    
                }
                else {
                    //括弧がないので計算に移る
                    int ex_times = Array.IndexOf(formula, op[2]);
                    int ex_div = Array.IndexOf(formula, op[3]);
                    int[] ints = new int[] { ex_times, ex_div };
                    int min_ex = Search_pos_min(ints);
                    //掛け算割り算を先に処理する
                    if (min_ex != -1) {
                        double x = double.Parse(formula[min_ex - 1]);
                        double y = double.Parse(formula[min_ex + 1]);
                        char c = char.Parse(formula[min_ex]);
                        temp_ans = calc(x, y, c);
                        formula[min_ex - 1] = temp_ans;
                        var list = formula.ToList();
                        list.RemoveRange(min_ex, 2);
                        formula = list.ToArray();
                        return Calc_new(formula);
                    }
                    //次に足し算を計算する
                    int ex_plus = Array.IndexOf(formula, op[4]);
                    if (ex_plus != -1) {
                        double x = double.Parse(formula[ex_plus - 1]);
                        double y = double.Parse(formula[ex_plus + 1]);
                        char c = char.Parse(formula[ex_plus]);
                        temp_ans = calc(x, y, c);
                        formula[ex_plus - 1] = temp_ans;
                        var list = formula.ToList();
                        list.RemoveRange(ex_plus, 2);
                        formula = list.ToArray();
                        return Calc_new(formula);
                    }
                    int ex_minus = Array.IndexOf(formula, op[5]);
                    if (ex_minus != -1)
                    {
                        double x = double.Parse(formula[ex_minus - 1]);
                        double y = double.Parse(formula[ex_minus + 1]);
                        char c = char.Parse(formula[ex_minus]);
                        temp_ans = calc(x, y, c);
                        formula[ex_minus - 1] = temp_ans;
                        var list = formula.ToList();
                        list.RemoveRange(ex_minus, 2);
                        formula = list.ToArray();
                        return Calc_new(formula);
                    }
                    //ここまで処理すると数字だけ残るはずなのであとはぜんぶ足し合わせる
                    double sum = 0.0;
                    for (int i = 0; i < formula.Length; i++) {
                        sum += double.Parse(formula[i]);
                    }
                    return sum.ToString();
                } 
            }
            //入力が一つの数字だった場合はそのまま返す
            return formula[0].ToString();
        }
    }
}
     

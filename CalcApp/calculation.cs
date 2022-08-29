using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALC
{
    class KEISANNKI
    {
        private string Super_Easy_calc(double x,double y, char c)
        {
            //計算機プログラム　超簡易型
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
        private string Splitstring(string str, int s, int e) {
            //str[s]からstr[e]までを切り出す。
            //[s..(e + 1)]
            return str.Substring(s,(e-s+1));
        }
        private int Search_op(string str, char c, int s, int e)
        {
            //入力
            //str : 式 c: 探したい演算子
            //s,e: 探す範囲(str[s] からstr[e]までを探す)
            //出力 あればその位置を、なければ-1を返す
            string part_str = Splitstring(str,s,e);
            return part_str.IndexOf(c);
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
                    return_list.Add(temp);
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
        //一行で入力可能なプログラム  ()演算子も扱えるようにする
        //負の数への対応が複雑になりすぎるため一時開発中断
        
        public string Calc(string formula)
        {
            char[] op = new char[] { '(', ')', '*', '/', '+', '-' };//計算記号をまとめておく
            string return_value=" ",ans; //最終的な結果をここに入れる
            int start = 0;
            int end = formula.Length - 1;
            // ()から探す
            int exist_op_left_brak = Search_op(formula, op[0], start, end);//左括弧を探す
            int exist_op_right_brak = Search_op(formula, op[1], start, end);//右括弧を探す
            if (exist_op_left_brak != -1 && exist_op_right_brak != -1)//もしどっちもあれば括弧内のみを抜き出してCalcに渡す
            {
                if (exist_op_left_brak == start && exist_op_right_brak == end)//この場合は括弧の意味がないので中身を出してCalcに渡す
                {
                    //formula[(start+1)..(end-1)]
                    return Calc(Splitstring(formula,start+1,end-1));
                }
                else
                {
                    ans = Calc(Splitstring(formula,exist_op_left_brak + 1, exist_op_right_brak - 1));
                    if (exist_op_left_brak == 0) { return_value = ans +Splitstring(formula,exist_op_right_brak + 1, end); }
                    else if (exist_op_right_brak == end) { return_value = Splitstring(formula,start, exist_op_left_brak - 1) + ans; }
                    else { return_value = Splitstring(formula,start, exist_op_left_brak - 1) + ans + Splitstring(formula,exist_op_right_brak + 1, end); }
                    return Calc(Formula_simplified(return_value));
                }
            }
            else
            {   

                int exist_op_times = Search_op(formula, op[2], start, end); //掛け算記号を探す
                int exist_op_div = Search_op(formula, op[3], start, end); //割り算記号を探す
                int[] div_times = new int[] { exist_op_div, exist_op_times };
                int min_place = Search_pos_min(div_times); //掛け算と割り算で前側にある方を探す。
                if (exist_op_times == min_place&&exist_op_times!=-1) { //掛け算記号が式の中にあったときの処理 
                    //前に記号があればその場所を指す　前側には同記号はSearch_op関数の性質上来ない　
                    //先に掛け算がきたときの処理であるため割り算記号も前には来ない
                    //よってこの処理が行なわれているときに前側にありうるのは足し算記号と引き算記号のみ
                    int exist_op_front = Math.Max(Search_op(formula, op[4], start, exist_op_times - 1),Search_op(formula,op[5],start,exist_op_times-1));
                    //後ろに記号があればその場所を探す　後ろには同じ符号も来るので注意！
                    int exist_op_times_back = Search_op(formula, op[2], exist_op_times + 1, end) ;
                    int exist_op_div_back = Search_op(formula, op[3], exist_op_times + 1, end);
                    int exist_op_plus_back = Search_op(formula, op[4], exist_op_times+1,end) ;
                    int exist_op_minus_back = Search_op(formula, op[5], exist_op_times + 1, end) ;
                    //後々の処理のために配列に入れる
                    int[] exist_op_back = new int[] { exist_op_plus_back, exist_op_minus_back, exist_op_div_back ,exist_op_times_back};

                    if (exist_op_plus_back ==-1&& exist_op_minus_back==-1&&exist_op_div_back==-1&&exist_op_times_back==-1) {
                        //掛け算記号の後ろに計算記号がないとき
                        if (exist_op_front == -1)
                        {
                            //前にも記号がないとき　
                            double x = double.Parse(Splitstring(formula, start, exist_op_times - 1));
                            double y = double.Parse(Splitstring(formula, exist_op_times + 1, end));
                            char c = formula[exist_op_times];
                            return_value = Super_Easy_calc(x, y, c);//前後に記号がなければ計算はそれ以上発生しないので返り値に入れてしまう
                            return return_value;
                        }
                        else if (exist_op_front == 0) {
                            //掛け算記号の前の記号が先頭に来ている、すなわち負数が来ているとき
                            
                            return "466574";
                        }
                        else
                        {
                            //前には記号があるとき
                            double x = double.Parse(Splitstring(formula, exist_op_front + 1, exist_op_times - 1));
                            double y = double.Parse(Splitstring(formula, exist_op_times + 1, end));
                            char c = formula[exist_op_times];
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_front) + ans;
                            return Calc(return_value);
                        }
                    }
                    else{
                        //掛け算記号の後ろに記号があるとき
                        int min_op_pos = Search_pos_min(exist_op_back) + exist_op_times + 1; //後ろにある記号のうち最も掛け算記号に近い場所を探した　
                        if (exist_op_front == -1)
                        {
                            //前には記号がないとき　
                            double x = double.Parse(Splitstring(formula, start, exist_op_times - 1));
                            double y = double.Parse(Splitstring(formula, exist_op_times + 1, min_op_pos - 1));
                            char c = formula[exist_op_times];
                            ans = Super_Easy_calc(x, y, c);
                            return_value = ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                        else if (exist_op_front == 0)
                        {
                            //掛け算記号の前の記号が先頭に来ている、すなわち負数が来ているとき
                            return "466574";
                        }
                        else
                        {
                            //前にも記号があるとき
                            double x = double.Parse(Splitstring(formula, exist_op_front + 1, exist_op_times - 1));
                            double y = double.Parse(Splitstring(formula, exist_op_times + 1, min_op_pos - 1));
                            char c = formula[exist_op_times];
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_front) + ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }

                }
                else if (exist_op_div == min_place&&exist_op_div!=-1) {
                    //割り算記号があった時の処理
                    int exist_op_front = Math.Max(Search_op(formula,op[4],start,exist_op_div-1),Search_op(formula,op[5],start,exist_op_div+1));
                    int exist_times_back = Search_op(formula, op[2], exist_op_div + 1, end);
                    int exist_div_back = Search_op(formula, op[3], exist_op_div + 1, end);
                    int exist_plus_back = Search_op(formula, op[4], exist_op_div + 1, end);
                    int exist_minus_back = Search_op(formula, op[5], exist_op_div + 1, end);
                    
                    int[] exist_back = new int[] { exist_plus_back, exist_minus_back ,exist_div_back,exist_times_back};
                    if (exist_op_front != -1)
                    {
                        //前に記号があった時
                        double x = double.Parse(Splitstring(formula, exist_op_front + 1, exist_op_div - 1));
                        char c = formula[exist_op_div];
                        if (exist_plus_back == -1 && exist_minus_back == -1&&exist_div_back==-1&&exist_times_back==-1)
                        {
                            //後ろに記号がなかった時

                            double y = double.Parse(Splitstring(formula, exist_op_div + 1, end));

                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_div - 1) + ans;
                            return Calc(return_value);
                        }
                        else
                        {
                            //後ろに記号があった時
                            int min_op_pos =Search_pos_min(exist_back) + exist_op_div + 1;
                            double y = double.Parse(Splitstring(formula, exist_op_div + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_div - 1) + ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                    else {
                        //前に記号がなかった時
                        double x = double.Parse(Splitstring(formula,start,exist_op_div-1));
                        char c = formula[exist_op_div];
                        if (exist_plus_back == -1 && exist_minus_back == -1&&exist_div_back==-1&&exist_times_back==-1)
                        {
                            //後ろに記号がなかった時 最後の計算なので値を返す

                            double y = double.Parse(Splitstring(formula, exist_op_div + 1, end));

                            ans = Super_Easy_calc(x, y, c);
                            return_value =ans;
                            return return_value;
                        }
                        else
                        {
                            //後ろに記号があった時
                            int min_op_pos = Search_pos_min(exist_back) + exist_op_div + 1;
                            double y = double.Parse(Splitstring(formula, exist_op_div + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                }
                int exist_op_plus = Search_op(formula, op[4], start, end);//足し算記号を探す
                if (exist_op_plus != -1) {
                    //足し算記号があった時 前に存在しうるのは引き算のみ
                    int exist_op_front = Search_op(formula,op[5],start,exist_op_plus-1);
                    int exist_plus_back = Search_op(formula, op[4], exist_op_plus + 1, end);
                    int exist_minus_back = Search_op(formula, op[5], exist_op_plus + 1, end);
                    int[] exist_back = new int[] { exist_plus_back, exist_minus_back };
                    if (exist_op_front == -1)
                    {
                        //前に記号がなかった時
                        int x = int.Parse(Splitstring(formula, start, exist_op_plus - 1));
                        char c = formula[exist_op_plus];
                        if (exist_plus_back == -1 && exist_minus_back == -1)
                        {
                            //後ろにも記号がなかった時
                            double y = double.Parse(Splitstring(formula, exist_op_plus + 1, end));
                            return_value = Super_Easy_calc(x, y, c);
                            return return_value;
                        }
                        else
                        {
                            //後ろに記号があった時
                            int min_op_pos = Search_pos_min(exist_back) + exist_op_plus + 1;
                            double y = double.Parse(Splitstring(formula, exist_op_plus + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                    else {
                        //前に記号があった時
                        double x = double.Parse(Splitstring(formula, exist_op_front + 1, exist_op_plus - 1));
                        char c = formula[exist_op_plus];
                        if (exist_plus_back == -1 && exist_minus_back == -1)
                        {
                            //後ろに記号がなかった時

                            double y = double.Parse(Splitstring(formula, exist_op_plus + 1, end));

                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_plus - 1) + ans;
                            return Calc(return_value);
                        }
                        else
                        {
                            //後ろに記号があった時
                            int min_op_pos = Search_pos_min(exist_back) + exist_op_plus + 1;
                            double y = double.Parse(Splitstring(formula, exist_op_plus + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_plus - 1) + ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                }
                int exist_op_minus = Search_op(formula, op[5], start, end);
                if (exist_op_minus != -1)
                {
                    //引き算記号があった時
                    //式の先頭に引き算記号が来たときは負の数であるため別処理にする
                    if (exist_op_minus==0) {
                        int[] ops = new int[] { Search_op(formula, op[2], start + 1, end), Search_op(formula, op[3], start + 1, end), Search_op(formula, op[4], start + 1, end), Search_op(formula, op[5], start + 1, end) };
                        int exist_op_sub = Search_pos_min(ops);//頭の記号よりも後ろにある記号のうち一番近いものを改めて探す
                        if (exist_op_sub != -1)
                        {
                            exist_op_sub++;//先頭のマイナス記号の分後ろにずらす
                            //先頭に引き算記号があり後ろに算術記号が他にあった場合
                            double x = double.Parse(Splitstring(formula,start,exist_op_sub-1));//マイナス記号ごと数字として扱う
                            char c = formula[exist_op_sub];
                            int next_op = Search_op(formula, op[5], exist_op_sub + 1, end);//後ろ側に算術記号がありその後ろが更に負の数である可能性があるため次のマイナス記号の場所だけ確認しておく
                            
                            if (next_op == 0) {
                                
                                //負の数同士の計算であった場合
                                //そのマイナスの記号より後ろを探す
                                int[] ops2 = new int[]{ Search_op(formula, op[2], exist_op_sub+2, end), Search_op(formula, op[3], exist_op_sub+2, end), Search_op(formula, op[4], exist_op_sub + 2, end), Search_op(formula, op[5], exist_op_sub + 2, end) };
                                int exist_op_subsub = Search_pos_min(ops2);
                                if (exist_op_subsub != -1)
                                {
                                    //後ろがまだあるとき
                                    exist_op_subsub += exist_op_sub + 1;
                                    double y = double.Parse(Splitstring(formula, exist_op_sub + 1, exist_op_subsub - 1));
                                    ans = Super_Easy_calc(x, y, c);
                                    return_value = ans + Splitstring(formula, exist_op_subsub, end);
                                    return Calc(return_value);
                                }
                                else {
                                    //後ろがもうないとき
                                    double y = double.Parse(Splitstring(formula,exist_op_sub+1,end));
                                    return_value = Super_Easy_calc(x, y, c);
                                    return return_value;
                                }
                            }
                            else if (next_op!=-1) {
                                //後ろが負の数ではないが引き算記号がその先にまだ存在しているとき
                                int[] ops2 = new int[] { Search_op(formula, op[2], exist_op_sub + 1, end), Search_op(formula, op[3], exist_op_sub + 1, end), Search_op(formula, op[4], exist_op_sub + 1, end), Search_op(formula, op[5], exist_op_sub + 1, end) };
                                int exist_op_subsub = Search_pos_min(ops2);
                                if (exist_op_subsub != -1)
                                {
                                    //後ろにある記号を改めて探してあった時　next_opを使おうとすると複雑になりそうなのでここでは二度手間を踏む
                                    exist_op_subsub += exist_op_sub + 1;
                                    double y = double.Parse(Splitstring(formula,exist_op_sub+1,exist_op_subsub-1));
                                    ans = Super_Easy_calc(x, y, c);
                                    return_value = ans + Splitstring(formula, exist_op_subsub, end);
                                    return Calc(return_value);

                                }
                                
                            }
                            else
                            {
                                //負の値と正の値の計算
                                double y = double.Parse(Splitstring(formula, exist_op_sub + 1, end));
                                return_value = Super_Easy_calc(x, y, c);
                                return return_value;
                            }
                        }
                        else {
                            //後ろ側に他に記号がない場合はただの負の数なのでそのまま値を返す
                            return formula;
                        }

                    }
                    else {
                        //先頭の数は正
                        double x = double.Parse(Splitstring(formula, start, exist_op_minus - 1)); //前に記号はもう来ない
                        char c = formula[exist_op_minus];
                        int exist_back = Search_op(formula, op[5], exist_op_minus + 1, end);
                        if (exist_back != -1)
                        {
                            
                            if (exist_back == 0)
                            {
                                exist_back += exist_op_minus + 1;
                                //後ろの数が負であった場合
                                int exist_back_sub = Search_op(formula, op[5], exist_back + 1, end);
                                if (exist_back_sub!=-1) {
                                    exist_back_sub += exist_back + 1;
                                    //負数の後ろにまだ引き算があるとき
                                    double y = double.Parse(Splitstring(formula,exist_op_minus+1,exist_back_sub-1));
                                    ans = Super_Easy_calc(x, y, c);
                                    return_value = ans + Splitstring(formula,exist_back_sub,end);
                                    return Calc(return_value);
                                }
                                else {
                                    double y = double.Parse(Splitstring(formula, exist_op_minus + 1, end));
                                    return_value = Super_Easy_calc(x, y, c);
                                    return return_value;

                                }

                            }
                            else {
                                //後ろの数が正であった場合
                                exist_back += exist_op_minus + 1;
                                //後ろに記号がある
                                double y = double.Parse(Splitstring(formula, exist_op_minus + 1, exist_back - 1));
                                ans = Super_Easy_calc(x, y, c);
                                return_value = ans + Splitstring(formula, exist_back, end);
                                return Calc(return_value);
                            }
                        }
                        else
                        {
                            //後ろに記号がない
                            double y = double.Parse(Splitstring(formula, exist_op_minus + 1, end));
                            return_value = Super_Easy_calc(x, y, c);
                            return return_value;
                        }
                    }
                }

            }
            return_value = formula;
            return return_value;
        }

        public string Calc_new(string[] formula) {
            string[] op = new string[] { "(", ")", "*", "/", "+" };
            if (formula.Length > 1)
            {
                //まずは括弧を探す
                int ex_leftbr = Array.IndexOf(formula, op[0]);
                if (ex_leftbr != -1)
                {
                    //左括弧が存在した時
                    int ex_rightbr = Array.IndexOf(formula, op[1]);
                    if (ex_rightbr == -1)
                    {
                        //左括弧があって右括弧がない時はエラーを返す
                        return "Error";
                    }
                    else
                    {
                        return Calc_new(formula[(ex_leftbr + 1)..ex_rightbr]);

                    }
                    
                }
                else {
                    //括弧がないので計算に移る
                    int ex_times = Array.IndexOf(formula, op[2]);
                    int ex_div = Array.IndexOf(formula, op[3]);
                    //掛け算割り算を先に処理する
                    //足し算まで処理すると数字だけ残るはずなのであとはぜんぶ足し合わせる
                } 
            }
            //一つの数字になったら、或いは入力が一つの数字だった場合はそのまま返す
            return formula[0].ToString();
        }
    }
}
     

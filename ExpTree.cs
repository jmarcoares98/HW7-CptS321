using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Cpts321
{
    public class ExpTree
    {
        private Node root;
        private static Dictionary<string, Double> dict = new Dictionary<String, Double>();
        public string saveExp { get; private set; }

        // where nodes are being bult
        private abstract class Node
        {
            public abstract double Eval();
        }

        // Node representing a binary operator
        // support operators for addition, subtraction, multiplication, and division
        private class OpNode : Node
        {
            public char op;
            public Node left, right;

            public OpNode(char newOp, Node newLeft, Node newRight)
            {
                this.op = newOp;
                this.left = newLeft;
                this.right = newRight;
            }

            public override double Eval()
            {
                if (op == '+')
                {
                    return (this.left.Eval() + this.right.Eval());
                }

                else if (op == '-')
                {
                    return (this.left.Eval() - this.right.Eval());
                }

                else if (op == '*')
                {
                    return (this.left.Eval() * this.right.Eval());
                }

                else if (op == '/')
                {
                    if (this.right.Eval() != 0)
                    {
                        return (this.left.Eval() / this.right.Eval());
                    }

                    //this one returns NaN if is divided by 0
                    else
                    {
                        return double.NaN;
                    }
                }

                return Double.NaN;
            }
        }

        // Node representing a variable
        private class VariableNode : Node
        {
            private string var;


            public VariableNode(string newVar)
            {
                this.var = newVar;
                dict[var] = 0.0;
            }

            public override double Eval()
            {
                return dict[var];
            }
        }

        // Node representing a constant numerical value
        private class ValueNode : Node
        {
            private double val;

            public ValueNode(double newVal)
            {
                val = newVal;
            }

            public override double Eval()
            {
                return val;
            }
        }

        ////for parsing the expression we use Djikstra's Shunting Yard Algorithm
        //public void ShuntingYard(string expr)
        //{
        //    Queue<string> que = new Queue<string>();
        //    Stack<string> stk = new Stack<string>();

        //    for (int i = expr.Length - 1; i >= 0; i--)
        //    {
        //        switch(expr[i])
        //        {
        //            case '*':
        //                if (stk.Count > 0)
        //                {
        //                    string op = stk.Peek();
        //                    if (op == "/")
        //                        que.Enqueue(stk.Pop());
        //                }
        //            case '/':

        //        }
        //    }
        //}

        private Node makeTree(string expression)
        {
            double result;

            //recursively add and subtract from the end of the expression to the start
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                switch (expression[i])
                {
                    case '+':
                    case '-':
                        return new OpNode(expression[i],
                        makeTree(expression.Substring(0, i)),
                        makeTree(expression.Substring(i + 1)));
                }
            }

            //this ones recursively multiplies and divides
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                switch (expression[i])
                {
                    case '*':
                    case '/':
                        return new OpNode(expression[i],
                        makeTree(expression.Substring(0, i)),
                        makeTree(expression.Substring(i + 1)));
                }
            }

            if (double.TryParse(expression, out result))
            {
                return new ValueNode(result);
            }
            return new VariableNode(expression);
        }

        public ExpTree(string expression)
        {
            saveExp = expression; // this will save the expression

            this.root = makeTree(expression);
        }

        public void SetVar(string var, double val)
        {
            try
            {
                dict.Add(var, val);
            }
            catch
            {
                dict[var] = val;
            }
        }

        public string[] GetVar()
        {
            return dict.Keys.ToArray();
        }

        public double Eval()
        {
            if (root != null)
                return root.Eval();
            else
                return double.NaN;
        }
    }
}

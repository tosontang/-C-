using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Xml;
namespace t__
{
    public partial class Form1 : Form
    {
        #region parameter
        string filename="";//当前打开文件路径
        string sourcecode="";//待编译源代码
        string resultpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//编译结果的文件保存路径
        Translator tra = new Translator();
        #endregion
        #region ui
        public Form1()
        {
            InitializeComponent();
        }
        #endregion
        #region event
            #region 文件
            private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                openFileDialog1.FileName = " ";
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                openFileDialog1.Filter = "C文件(*.c)|*.c";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = openFileDialog1.FileName.ToString();
                    String[] lines = File.ReadAllLines(filename);
                    textBox1.Text = "";
                    foreach (string line in lines)
                    {
                        textBox1.AppendText(line + Environment.NewLine);
                    }
                    textBox1.Visible = true;
                    保存ToolStripMenuItem.Enabled = true;
                    另存为ToolStripMenuItem.Enabled = true;
                    关闭ToolStripMenuItem.Enabled = true;
                    分析ToolStripMenuItem.Enabled = true;
                }
            }
            private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                textBox1.Visible = true;
                保存ToolStripMenuItem.Enabled = true;
                另存为ToolStripMenuItem.Enabled = true;
                关闭ToolStripMenuItem.Enabled = true;
                分析ToolStripMenuItem.Enabled = true;
            }
            private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                this.Close();
            }
            private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (!File.Exists(filename))
                {
                    MessageBox.Show("文件路径失效，请重新选择保存文件");
                    saveFileDialog1.FileName = "";
                    saveFileDialog1.Filter = "C文件(*.c)|*.c";
                    saveFileDialog1.InitialDirectory = "C:\\";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        filename = saveFileDialog1.FileName;
                    }
                    FileStream fs = File.OpenWrite(filename);
                    StreamWriter wr = new StreamWriter(fs);
                    foreach (string line in textBox1.Lines)
                    {
                        wr.WriteLine(line);
                    }
                    wr.Flush();
                    wr.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = File.OpenWrite(filename);
                    StreamWriter wr = new StreamWriter(fs);
                    foreach (string line in textBox1.Lines)
                    {
                        wr.WriteLine(line);
                    }
                    wr.Flush();
                    wr.Close();
                    fs.Close();
                }
            }
            private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                filename = " ";
                textBox1.Text = "";
                textBox1.Visible = false;
                保存ToolStripMenuItem.Enabled = false;
                另存为ToolStripMenuItem.Enabled = false;
                关闭ToolStripMenuItem.Enabled = false;
                分析ToolStripMenuItem.Enabled = false;
                词法分析ToolStripMenuItem.Enabled = false;
                语法分析ToolStripMenuItem.Enabled = false;
                中间代码ToolStripMenuItem.Enabled = false;
                目标代码ToolStripMenuItem.Enabled = false;
            }
            private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "C文件(*.c)|*.c";
                saveFileDialog1.InitialDirectory = "C:\\";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                }
                FileStream fs = File.OpenWrite(filename);
                StreamWriter wr = new StreamWriter(fs);
                foreach (string line in textBox1.Lines)
                {
                    wr.WriteLine(line);
                }
                wr.Flush();
                wr.Close();
                fs.Close();
            }
            #endregion
            #region 编译
            private void 分析ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                sourcecode = textBox1.Text.ToString();
                //MessageBox.Show(lexpath);
                词法分析ToolStripMenuItem.Enabled = true;
                语法分析ToolStripMenuItem.Enabled = true;
                中间代码ToolStripMenuItem.Enabled = true;
                目标代码ToolStripMenuItem.Enabled = true;
                dataGridView1.Rows.Clear();
                tra.Initial(sourcecode, resultpath, dataGridView1);
                tra.CheckProgram();
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
                ShowState(tra.errorinfo, tra.errorline);
                textBox3.Visible = true;
            }
            #endregion         
            #region 查看
            private void 词法分析ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                String[] lines = File.ReadAllLines(tra.lexpath);
                textBox2.Text = "";
                foreach (string line in lines)
                {
                    textBox2.AppendText(line + Environment.NewLine);
                }
                treeView1.Visible = false;
                textBox2.Visible = true;
                dataGridView1.Visible = false;
            }
            private void 语法分析ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                ShowSyntaxTree();
                treeView1.Visible = true;
                textBox2.Visible = false;
                dataGridView1.Visible = false;
            }
            private void button1_Click(object sender, EventArgs e)
            {
                treeView1.ExpandAll();
            }
            private void button2_Click(object sender, EventArgs e)
            {
                treeView1.CollapseAll();
            }
            private void 中间代码ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                treeView1.Visible = false;
                textBox2.Visible = false;
                dataGridView1.Visible = true;
                 
            }
            private void 目标代码ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                String[] lines = File.ReadAllLines(tra.goalpath);
                textBox2.Text = "";
                foreach (string line in lines)
                {
                    textBox2.AppendText(line + Environment.NewLine);
                }
                treeView1.Visible = false;
                textBox2.Visible = true;
                dataGridView1.Visible = false;
            }
            #endregion
            #region 路径
            private void 路径ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    resultpath = folderBrowserDialog1.SelectedPath;
                }
            }
            #endregion
        #endregion
        #region change
            private void textBox1_TextChanged(object sender, EventArgs e)
            {
                词法分析ToolStripMenuItem.Enabled = false;
                语法分析ToolStripMenuItem.Enabled = false;
                中间代码ToolStripMenuItem.Enabled = false;
                目标代码ToolStripMenuItem.Enabled = false;
                treeView1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                dataGridView1.Visible = false;
            }
            private void treeView1_VisibleChanged(object sender, EventArgs e)
            {
                button1.Visible = treeView1.Visible;
                button2.Visible = treeView1.Visible;
            }
        #endregion
        #region function
            //显示语法分析树
            private void ShowSyntaxTree()
            {
                int cur=0;
                TreeNode tn = new TreeNode(tra.syntree[cur].info);
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(tn);
                MakeSyntaxTree(cur, tn);
            }
            //将树synttree的cur结点的子结点同步为tn的子结点
            private void MakeSyntaxTree(int cur, TreeNode tn)
            {
                int cn = 0;
                while (cn < tra.syntree[cur].childNum)
                {
                    int cc = tra.syntree[cur].child[cn];
                    TreeNode ctn = new TreeNode(tra.syntree[cc].info);
                    tn.Nodes.Add(ctn);
                    MakeSyntaxTree(cc, ctn);
                    cn++;
                }
            }
            //显示运行状态信息
            private void ShowState(string info, int line)
            {
                if (line == 0)
                {
                    textBox3.Text = "分析成功";
                }
                else
                {
                    textBox3.Text = "line(" + line.ToString() + "):  " + info;
                }
            }
        #endregion
    }
    #region struct
    //符号表结点
    public class Node<Type>
    {
        public string symbol;
        public string scope;
        public Node<Type> next;
        public Node(string s1, string sc)
        {
            symbol=s1;
            scope = sc;
            next=null;
        }
        public Node()
        {
            symbol=default(string);
            scope = default(string);
            next=null;
        }
        public string Symbol
        {
            get  
            {  
                return symbol;  
            }  
            set  
            {  
                symbol = value;  
            }  
        }
        public Node<Type> Next
        {
            get  
            {  
                return next;  
            }  
            set  
            {  
                next = value;  
            }  
        }
    }
    //符号表链式结构
    public class LinkList
    {
        public Node<Type> head; //单链表的头结点  
        //头结点属性  
        public Node<Type> Head
        {
            get
            {
                return head;
            }
            set
            {
                head = value;
            }
        }
        //构造器  
        public LinkList()
        {
            head = null;
        }
        //求单链表的长度  
        public int GetLength()
        {
            Node<Type> p = head;
            int len = 0;
            while (p != null)
            {
                ++len;
                p = p.Next;
            }
            return len;
        }
        //清空单链表  
        public void Clear()
        {
            head = null;
        }
        //判断单链表是否为空  
        public bool IsEmpty()
        {
            if (head == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //在单链表的末尾添加新元素  
        public int Add(string item, string scope)
        {
            int i = 1;
            Node<Type> q = new Node<Type>(item, scope);
            Node<Type> p = new Node<Type>();
            if (head == null)
            {
                head = q;
                return i;
            }
            p = head;
            while (p.Next != null)
            {
                p = p.Next;
                i++;
            }
            p.Next = q;
            return i;
        }
        //删除单链表的第i个结点  
        public void Delete(int i)
        {
            if (IsEmpty() || i < 0 || i > GetLength())
            {
                return;
            }
            Node<Type> q = new Node<Type>();
            if (i == 1)
            {
                q = head;
                head = head.Next;
                return;
            }
            Node<Type> p = head;
            int j = 1;
            while (p.Next != null && j < i)
            {
                ++j;
                q = p;
                p = p.Next;
            }
            if (j == i)
            {
                q.Next = p.Next;
            }
        }
        //在单链表中查找symbol结点,失败返回-1 
        public int Locate(string symbol)
        {
            Node<Type> p = new Node<Type>();
            p = head;
            int i = 1;
            while (p != null)
            {
                if (p.symbol.Equals(symbol))
                {
                    return i;
                }
                else
                {
                    p = p.Next;
                    i++;
                }
            }
            return -1;
        }
        //删除域为scope的结点
        public void update(string scope)
        {
            Node<Type> p = new Node<Type>();
            p = head;
            while (p != null)
            {
                if (p.scope.Equals(scope))
                {
                    p = p.next;
                }
                else
                {
                    while (p.next != null)
                    {
                        if (p.next.scope.Equals(scope))
                        {
                            p.next = p.next.next;
                            continue;
                        }
                        else
                        {
                            p = p.next;
                        }
                    }
                    break;
                }
            }
        }
    }
    //函数表数据结构
    public class FuncName
    {
        public bool isreturn;                       //是否带有返回值
        public int start;                           //中间代码的起始行
        public int sentenceline;                    //中间代码的行数
        public int paranum;                         //参数个数
        public int varnum;                          //变量个数
        public string name;                         //函数名
        public string sentenceblock;                //中间代码
        public string lastcode;                     //目标代码
        public string[] paralist=new string[32];    //参数表（最多容纳32个参数）
        public string[] varlist=new string[512];    //变量表（最多容纳512个变量）
    }
    //词法分析器
    class lexical
    {
        //variable
        private string sourcecode;
        private char ch;
        private string strToken;
        public string scope;
        public int cur;
        public int code;
        public int line;
        public string info;
        public bool end;
        public LinkList sblt=new LinkList();
        //function
        public void Initial(string s)
        {
            end = false;
            sourcecode = s;
            sourcecode = sourcecode + "#";
            ch = ' ';
            cur = 0;
            code = 0;
            info = "";
            line = 1;
        }
        private void GetChar()
        {
            ch = sourcecode[cur];
            cur++;
        }
        private void GetBC()
        {
            while(ch==' '||ch=='\t'||ch=='\n'||ch=='\r'){
                if (ch == '\n')
                {
                    line++;
                }
                GetChar();
            }
        }
        private void Concat()
        {
            strToken=strToken+ch;
        }
        private void Reserve()
        {
            if(strToken=="int"){
                code=1;
            }else if(strToken=="void"){
                code=2;
            }else if(strToken=="if"){
                code=3;
            }else if(strToken=="else"){
                code=4;
            }else if(strToken=="while"){
                code=5;
            }else if(strToken=="return"){
                code=6;
            }else{
                code=0;
            }
        }
        private void Retract()
        {
            ch=' ';
            cur--;
        }
        public int InsertId()
        {
            int res=0;
            res=sblt.Locate(strToken);
            if(res==-1){
                sblt.Add(strToken, scope);
            }
            return res;
        }
        private int CheckId()
        {
            return sblt.Locate(strToken);
        }
        public bool lex()
        {
            strToken = "";
            GetChar();
            GetBC();
            if (char.IsLetter(ch))
            {
                while (char.IsLetterOrDigit(ch))
                {
                    Concat();
                    GetChar();
                }
                Retract();
                Reserve();
                if (code == 0)
                {
                    code = 7;
                }
                info = strToken;
            }
            else if (char.IsDigit(ch))
            {
                while (char.IsDigit(ch))
                {
                    Concat();
                    GetChar();
                }
                Retract();
                code = 8;
                info = strToken;
            }
            else if (char.Equals(ch, '='))
            {
                GetChar();
                if (char.Equals(ch, '='))
                {
                    code = 14;
                    info = "==";
                }
                else
                {
                    Retract();
                    code = 9;
                    info = "=";
                }
            }
            else if (char.Equals(ch, '+'))
            {
                code = 10;
                info = "+";
            }
            else if(char.Equals(ch, '-'))
            {
                code=11;
                info = "-";
            }
            else if(char.Equals(ch, '*'))
            {
                code=12;
                info = "*";
            }
            else if(char.Equals(ch, '/'))
            {
                code=13;
                info = "/";
            }
            else if(char.Equals(ch, '>'))
            {
                GetChar();
                if(char.Equals(ch, '='))
                {
                    code=14;
                    info = ">=";
                }
                else
                {
                    Retract();
                    code=14;
                    info = ">";
                }
            }
            else if(char.Equals(ch, '<'))
            {
                GetChar();
                if(char.Equals(ch, '='))
                {
                    code=14;
                    info = "<=";
                }
                else{
                    Retract();
                    code=14;
                    info = "<";
                }
            }
            else if(char.Equals(ch, '!'))
            {
                GetChar();
                if(char.Equals(ch, '='))
                {
                    code=14;
                    info = "!=";
                }
                else
                {
                    return false;
                }
            }
            else if(char.Equals(ch, ';'))
            {
                code = 15;
                info = ";";
            }
            else if(char.Equals(ch, ','))
            {
                code = 16;
                info = ",";
            }
            else if (char.Equals(ch, '('))
            {
                code = 17;
                info = "(";
            }
            else if (char.Equals(ch, ')'))
            {
                code = 18;
                info = "）";
            }
            else if (char.Equals(ch, '{'))
            {
                code = 19;
                info = "{";
            }
            else if (char.Equals(ch, '}'))
            {
                code = 20;
                info = "}";
            }
            else if (char.Equals(ch, '#'))
            {
                code = -1;
                info = "#";
                //MessageBox.Show(info);
                end = true;
            }
            else
            {
                return false;
            }
            //MessageBox.Show(info);
            return true;
        }
    }
    //语法树数据结构
    class SynTree 
    {
        public int parentCur;   //父结点游标
        public int childNum;    //子结点数量
        public int[] child;     //子结点游标保存数组
        public string info;     //当前结点信息
        public string place;    //当前结点的变量名
        public int line;        //当前结点首行
        public int linenum;     //当前结点包含行数
        public string midsen;   //当前结点包含的中间代码
        public string lastsen;  //当前结点包含的目标代码
    }
    #endregion
    class Translator
    {
        #region variable
        public int cur;             //语法分析树结点游标
        public int errorline;       //错误发生行
        public int supervarnum;     //全局变量数
        public int funcnamecount;   //函数的数量
        public bool isreturn;       //函数是否有返回值
        public bool iscontrol;      //是否是控制语句
        public string[] supervar;   //全局变量表
        public string errorinfo;    //错误返回信息
        public string lexpath;      //词法二元式保存路径
        public string midpath;      //中间代码的保存路径
        public string goalpath;     //目标代码的保存路径
        public lexical lexic;       //词法分析器
        public SynTree[] syntree;   //语法分析树
        public FuncName[] funcname; //函数表
        private int codeline;       //中间代码表的下一打印行数
        private int midline;        //下一打印行所在行数
        private int count;          //中间变量的数量
        private int ifnum;          //函数体内if语句的数量
        private int whilenum;       //函数体内while语句的数量
        private string scope;       //当前位置所在域
        private string wdata;       //待写入文件数据区
        private string cachepath;   //缓存文件存放路径
        private FileStream lexfs;
        private FileStream midfs;
        private FileStream goalfs;
        private FileStream cachefs;
        private StreamWriter lexsw;
        private StreamWriter midsw;
        private StreamWriter goalsw;
        private StreamWriter cachesw;
        private DataGridView dgv1;
        #endregion
        #region function
        public void Initial(string s1, string s2, DataGridView dgv)
        {
            lexpath = s2 + "\\lex.dat";
            midpath = s2 + "\\mid.dat";
            goalpath = s2 + "\\goal.dat";
            cachepath = s2 + "\\cache.dat";
            errorline = 0;
            count = 0;
            cur = 0;
            lexic = new lexical();
            syntree = new SynTree[10240];
            funcname = new FuncName[1024];
            supervar = new string[512];
            supervarnum = 0;
            lexic.scope = "";
            midline = 0;
            funcnamecount = 0;
            dgv1 = dgv;
            codeline = 0;
            if(File.Exists(lexpath))
            {
                lexfs = new FileStream(lexpath, FileMode.Truncate);
            }
            else
            {
                lexfs = new FileStream(lexpath, FileMode.Create);
            }
            lexsw = new StreamWriter(lexfs);
            if (File.Exists(midpath))
            {
                midfs = new FileStream(midpath, FileMode.Truncate);
            }
            else
            {
                midfs = new FileStream(midpath, FileMode.Create);
            }
            midsw = new StreamWriter(midfs);
            if (File.Exists(goalpath))
            {
                goalfs = new FileStream(goalpath, FileMode.Truncate);
            }
            else
            {
                goalfs = new FileStream(goalpath, FileMode.Create);
            }
            goalsw = new StreamWriter(goalfs);
            if (File.Exists(cachepath))
            {
                cachefs = new FileStream(cachepath, FileMode.Truncate);
            }
            else
            {
                cachefs = new FileStream(cachepath, FileMode.Create);
            }
            cachesw = new StreamWriter(cachefs);
            lexic.Initial(s1);
        }
        public bool CheckProgram()
        {
            int thiscur = cur;
            if (syntree[cur] == null)
            {
                syntree[cur] = new SynTree();
            }
            syntree[cur].childNum = 0;
            syntree[cur].info = "Program";
            syntree[cur].child = new int[1];
            if (CheckDeclareList(thiscur))
            {
                EmitFormula();
                GenerateLastCode();
                lexsw.Flush();
                lexsw.Close();
                lexfs.Close();
                midsw.Flush();
                midsw.Close();
                midfs.Close();
                goalsw.Flush();
                goalsw.Close();
                goalfs.Close();
                return true;
            }
            else
            {
                lexsw.Flush();
                lexsw.Close();
                lexfs.Close();
                midsw.Flush();
                midsw.Close();
                midfs.Close();
                cachesw.Flush();
                cachesw.Close();
                cachefs.Close();
                goalsw.Flush();
                goalsw.Close();
                goalfs.Close();
                File.Delete(lexpath);
                File.Delete(midpath);
                File.Delete(goalpath);
                errorline = lexic.line;
                return false;
            }
        }
        private bool CheckDeclareList(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "声明串";
            if (CheckDeclare(thiscur))
            {
                while (true)
                {
                    curcache = lexic.cur;
                    linecache = lexic.line;
                    if (!lexic.end && lexic.lex() && lexic.code == -1)
                    {
                        return true;
                    }
                    else
                    {
                        lexic.cur = curcache;
                        lexic.line = linecache;
                        if (CheckDeclare(thiscur))
                        {
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
        private bool CheckDeclare(int parent)
        {
            cur++;
            string sc;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[3];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "声明";
            if (!lexic.end && lexic.lex() && lexic.code == 1)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = "int";
                wdata = "<1, int>\n";
                lexsw.Write(wdata);
                isreturn=true;
                if (!lexic.end && lexic.lex() && lexic.code == 7)
                {
                    if (lexic.InsertId() != -1)
                    {
                        errorinfo = "函数名错误";
                        return false;
                    }
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].child = new int[1];
                    syntree[cur].childNum = 1;
                    syntree[cur].info = "ID";
                    syntree[cur].child[0] = cur + 1;
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[cur].parentCur = cur - 1;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = lexic.info;
                    wdata = "<7, " + lexic.info + ">\n";
                    lexsw.Write(wdata);
                    sc = lexic.scope;
                    lexic.scope = lexic.scope + lexic.info;
                    AddFuncName(lexic.info);
                    funcname[funcnamecount - 1].isreturn = true;
                    scope = lexic.info;
                    if (CheckDeclareType(thiscur))
                    {
                        lexic.sblt.update(lexic.scope);
                        lexic.scope = sc;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    errorinfo = "函数名错误";
                    return false;
                }
            }
            else if (lexic.code == 2)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = "void";
                wdata = "<2, void>\n";
                lexsw.Write(wdata);
                isreturn=false;
                if (!lexic.end&&lexic.lex() && lexic.code == 7)
                {
                    if (lexic.InsertId() != -1)
                    {
                        errorinfo = "函数名错误";
                        return false;
                    }
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].child = new int[1];
                    syntree[cur].childNum = 1;
                    syntree[cur].info = "ID";
                    syntree[cur].child[0] = cur + 1;
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[cur].parentCur = cur - 1;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = lexic.info;
                    wdata = "<7, " + lexic.info + ">\n";
                    lexsw.Write(wdata);
                    sc = lexic.scope;
                    lexic.scope = lexic.scope + lexic.info;
                    scope = lexic.info;
                    AddFuncName(lexic.info);
                    funcname[funcnamecount - 1].isreturn = false;
                    if (CheckFunctionDeclare(thiscur))
                    {
                        lexic.sblt.update(lexic.scope);
                        lexic.scope = sc;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    errorinfo = "函数名错误";
                    return false;
                }
            }
            else
            {
                errorinfo = "函数返回值类型错误";
                return false;
            }
        }
        private bool CheckDeclareType(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[cur].parentCur = parent;
            syntree[cur].child = new int[1];
            syntree[cur].childNum = 0;
            syntree[cur].info = "声明类型";
            curcache = lexic.cur;
            linecache = lexic.line;
            if (CheckVariableDeclare(thiscur))
            {
                DelFuncName();
                return true;
            }
            else
            {
                cur = thiscur;
                lexic.cur = curcache;
                lexic.line = linecache;
                syntree[cur].childNum = 0;
                if (CheckFunctionDeclare(thiscur))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
                
        }
        private bool CheckVariableDeclare(int parent)
        {
            cur++;
            int thiscur = cur; 
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "变量声明";
            if (!lexic.end && lexic.lex() && lexic.code == 15)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = ";";
                wdata = "<15, ;>\n";
                lexsw.Write(wdata);
                return true;
            }
            else
            {
                errorinfo = "变量声明错误（可能缺少“;”）";
                return false;
            }

        }
        private bool CheckFunctionDeclare(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[4];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "函数声明";
            if (!lexic.end && lexic.lex() && lexic.code == 17)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = "(";
                wdata = "<17, (>\n";
                lexsw.Write(wdata);
                if (CheckFormalParameter(thiscur))
                {
                    if (!lexic.end && lexic.lex() && lexic.code == 18)
                    {
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[thiscur].childNum++;
                        syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                        syntree[cur].parentCur = thiscur;
                        syntree[cur].childNum = 0;
                        syntree[cur].info = ")";
                        wdata = "<18, )>\n";
                        lexsw.Write(wdata);
                        funcname[funcnamecount - 1].sentenceblock = PrintFormula(funcname[funcnamecount - 1].name, "", "", "");
                        funcname[funcnamecount - 1].start = midline;
                        whilenum = 0;
                        ifnum = 0;
                        midline++;
                        if (CheckSentenceBlock(thiscur))
                        {
                            funcname[funcnamecount - 1].lastcode = syntree[syntree[thiscur].child[1]].lastsen+syntree[syntree[thiscur].child[3]].lastsen;
                            funcname[funcnamecount - 1].sentenceblock = funcname[funcnamecount - 1].sentenceblock + syntree[syntree[thiscur].child[3]].midsen;
                            funcname[funcnamecount - 1].sentenceline = syntree[syntree[thiscur].child[3]].linenum + 1;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        errorinfo = "函数声明错误（可能缺少“）”）";
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                errorinfo = "函数声明错误（可能缺少“（”）";
                return false;
            }
        }
        private bool CheckFormalParameter(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "形参";
            curcache = lexic.cur;
            linecache = lexic.line;
            if (CheckParameterList(thiscur))
            {
                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                return true;
            }
            else
            {
                lexic.cur = curcache;
                lexic.line = linecache;
                syntree[thiscur].childNum = 0;
                if (!lexic.end && lexic.lex() && lexic.code == 2)
                {
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = "void";
                    wdata = "<2, void>\n";
                    lexsw.Write(wdata);
                    funcname[funcnamecount-1].paranum = 0;
                    syntree[thiscur].lastsen = "";
                    return true;
                }
                else
                {
                    errorinfo = "形参错误";
                    return false;
                }
            }
        }
        private bool CheckParameterList(int parent)
        {
            int curcache;
            int linecache;
            int i=0;
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "参数列表";
            funcname[funcnamecount - 1].paranum = 0;
            if (CheckParameter(thiscur))
            {
                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[i]].lastsen;
                while (true)
                {
                    curcache = lexic.cur;
                    linecache = lexic.line;
                    if (!lexic.end && lexic.lex() && lexic.code == 16)
                    {
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[thiscur].childNum++;
                        syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                        syntree[cur].childNum = 0;
                        syntree[cur].parentCur = thiscur;
                        syntree[cur].info = ",";
                        wdata = "<16, ,>\n";
                        lexsw.Write(wdata);
                        if (CheckParameter(thiscur))
                        {
                            i++;
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + syntree[syntree[thiscur].child[i]].lastsen;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        lexic.line = linecache;
                        lexic.cur = curcache;
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        private bool CheckParameter(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[2];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "参数";
            if (!lexic.end && lexic.lex() && lexic.code == 1)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = "int";
                wdata = "<1, int>\n";
                lexsw.Write(wdata);
                if (!lexic.end && lexic.lex() && lexic.code == 7)
                {
                    if (lexic.InsertId() != -1)
                    {
                        errorinfo = "参数名错误";
                        return false;
                    }
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].child = new int[1];
                    syntree[cur].childNum = 1;
                    syntree[cur].info = "ID";
                    syntree[cur].child[0] = cur + 1;
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[cur].parentCur = cur-1;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = lexic.info;
                    wdata = "<7, " + lexic.info + ">\n";
                    syntree[thiscur].lastsen = "        POP    " + lexic.info + "\n";
                    lexsw.Write(wdata);
                    funcname[funcnamecount - 1].paralist[funcname[funcnamecount - 1].paranum] = lexic.info;
                    funcname[funcnamecount - 1].paranum++;
                    funcname[funcnamecount - 1].varlist[funcname[funcnamecount - 1].varnum] = lexic.info;
                    funcname[funcnamecount - 1].varnum++;
                    return true;
                }
                else
                {
                    errorinfo = "参数错误";
                    return false;
                }
            }
            else
            {
                errorinfo = "参数错误";
                return false;
            }
        }
        private bool CheckSentenceBlock(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[4];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "语句块";
            syntree[thiscur].line = midline;
            if (!lexic.end && lexic.lex() && lexic.code == 19)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = "{";
                wdata = "<19, {>\n";
                lexsw.Write(wdata);
                if (CheckInternalDeclare(thiscur))
                {
                    if (CheckSentenceList(thiscur))
                    {
                        if (!lexic.end && lexic.lex() && lexic.code == 20)
                        {
                            cur++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].childNum = 0;
                            syntree[cur].info = "}";
                            wdata = "<20, }>\n";
                            lexsw.Write(wdata);
                            syntree[thiscur].lastsen = syntree[syntree[thiscur].child[2]].lastsen;
                            syntree[thiscur].midsen = syntree[syntree[thiscur].child[2]].midsen;
                            syntree[thiscur].linenum = syntree[syntree[thiscur].child[2]].linenum;
                            return true;
                        }
                        else
                        {
                            errorinfo = "语句块错误（可能缺少“}”）";
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                errorinfo = "语句块错误（缺少“{”）";
                return false;
            }
        }
        private bool CheckInternalDeclare(int parent)
        {
            int curcache;
            int linecache;
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "内部声明";
            while (true)
            {
                curcache = lexic.cur;
                linecache = lexic.line;
                if (!lexic.end && lexic.lex() && lexic.code == 1)
                {
                    lexic.cur = curcache;
                    lexic.line = linecache;
                    if (CheckInternalVariableDeclare(thiscur))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    lexic.line = linecache;
                    lexic.cur = curcache;
                    return true;
                }
            }
        }
        private bool CheckInternalVariableDeclare(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[3];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "内部变量声明";
            if (!lexic.end && lexic.lex() && lexic.code == 1)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = "int";
                wdata = "<1, int>\n";
                lexsw.Write(wdata);
                if (!lexic.end && lexic.lex() && lexic.code == 7)
                {
                    if (lexic.InsertId() != -1)
                    {
                        errorinfo = "内部变量名错误";
                        return false;
                    }
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].child = new int[1];
                    syntree[cur].childNum = 1;
                    syntree[cur].info = "ID";
                    syntree[cur].child[0] = cur + 1;
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[cur].parentCur = cur - 1;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = lexic.info;
                    wdata = "<7, " + lexic.info + ">\n";
                    lexsw.Write(wdata);
                    funcname[funcnamecount - 1].varlist[funcname[funcnamecount - 1].varnum] = lexic.info;
                    funcname[funcnamecount - 1].varnum++;
                    if (!lexic.end && lexic.lex() && lexic.code == 15)
                    {
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[thiscur].childNum++;
                        syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                        syntree[cur].parentCur = thiscur;
                        syntree[cur].childNum = 0;
                        syntree[cur].info = ";";
                        wdata = "<15, ;>\n";
                        lexsw.Write(wdata);
                        return true;
                    }
                    else
                    {
                        errorinfo = "内部变量声明错误（可能缺少“;”）";
                        return false;
                    }
                }
                else
                {
                    errorinfo = "内部变量声明错误（可能缺少变量名）";
                    return false;
                }
            }
            else
            {
                errorinfo = "内部变量声明错误";
                return false;
            }
        }
        private bool CheckSentenceList(int parent)
        {
            cur++;
            int i;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            i = 0;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "语句串";
            syntree[thiscur].line = midline;
            if (CheckSentence(thiscur))
            {
                syntree[thiscur].linenum = syntree[syntree[thiscur].child[i]].linenum;
                syntree[thiscur].midsen = syntree[syntree[thiscur].child[i]].midsen;
                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[i]].lastsen;
                while (true)
                {
                    curcache = lexic.cur;
                    linecache = lexic.line;
                    if (!lexic.end && lexic.lex() && (lexic.code == 7 || lexic.code == 3 || lexic.code == 5 || lexic.code == 6))
                    {
                        lexic.cur=curcache;
                        lexic.line = linecache;
                        if (CheckSentence(thiscur))
                        {
                            i++;
                            syntree[thiscur].linenum = syntree[thiscur].linenum+syntree[syntree[thiscur].child[i]].linenum;
                            syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[i]].midsen;
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + syntree[syntree[thiscur].child[i]].lastsen;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        lexic.line = linecache;
                        lexic.cur = curcache;
                        return true;
                    }
                }
                
            }
            else
            {
                return false;
            }
        }
        private bool CheckSentence(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "语句";
            curcache = lexic.cur;
            linecache = lexic.line;
            syntree[thiscur].line = midline;
            if (!lexic.end && lexic.lex())
            {
                switch (lexic.code)
                {
                    //if语句
                    case 3:
                        {
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            if (CheckIfSentence(thiscur))
                            {
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[0]].linenum;
                                syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen;
                                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                                syntree[thiscur].line = syntree[syntree[thiscur].child[0]].line;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    //while语句
                    case 5:
                        {
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            if (CheckWhileSentence(thiscur))
                            {
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[0]].linenum;
                                syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen;
                                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                                syntree[thiscur].line = syntree[syntree[thiscur].child[0]].line;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    //return语句
                    case 6:
                        {
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            if (CheckReturnSentence(thiscur))
                            {
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[0]].linenum;
                                syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen;
                                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                                syntree[thiscur].line = syntree[syntree[thiscur].child[0]].line;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    //赋值语句
                    case 7:
                        {
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            if (CheckAssignmentSentence(thiscur))
                            {
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[0]].linenum;
                                syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen;
                                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                                syntree[thiscur].line = syntree[syntree[thiscur].child[0]].line;
                                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
            else
            {
                errorinfo = "语句错误";
                return false;
            }
        }
        private bool CheckAssignmentSentence(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[4];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "赋值语句";
            syntree[thiscur].line = midline;
            if (!lexic.end && lexic.lex() && lexic.code == 7)
            {
                if (lexic.InsertId() == -1)
                {
                    errorinfo = "变量名错误";
                    return false;
                }
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].child = new int[1];
                syntree[cur].childNum = 1;
                syntree[cur].info = "ID";
                syntree[cur].child[0] = cur + 1;
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[cur].parentCur = cur - 1;
                syntree[cur].childNum = 0;
                syntree[cur].info = lexic.info;
                wdata = "<7, " + lexic.info + ">\n";
                lexsw.Write(wdata);
                if (!lexic.end && lexic.lex() && lexic.code == 9)
                {
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = "=";
                    wdata = "<9, =>\n";
                    lexsw.Write(wdata);
                    iscontrol = false;
                    if (CheckExpression(thiscur))
                    {
                        if (!lexic.end && lexic.lex() && lexic.code == 15)
                        {
                            cur++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].childNum = 0;
                            syntree[cur].info = ";";
                            wdata = "<9, ;>\n";
                            lexsw.Write(wdata);
                            wdata = "( =, " + syntree[syntree[thiscur].child[2]].place + " ";
                            if (!syntree[syntree[thiscur].child[2]].place.Equals(syntree[syntree[thiscur].child[0] + 1].info))
                            {
                                syntree[thiscur].midsen = syntree[syntree[thiscur].child[2]].midsen + PrintFormula("=", syntree[syntree[thiscur].child[2]].place, "", syntree[syntree[thiscur].child[0] + 1].info);
                                midline++;
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[2]].linenum + 1;
                            }
                            else
                            {
                                syntree[thiscur].midsen = syntree[syntree[thiscur].child[2]].midsen;
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[2]].linenum;
                            }
                            syntree[thiscur].lastsen =syntree[syntree[thiscur].child[2]].lastsen + "        MOV    " + funcname[funcnamecount - 1].name + "_" + syntree[syntree[thiscur].child[0] + 1].info + ",  EBX\n";
                            return true; 
                        }
                        else
                        {
                            errorinfo = "赋值语句错误（可能缺少“;”）";
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    errorinfo = "赋值语句错误（可能缺少“=”）";
                    return false;
                }
            }
            else
            {
                errorinfo = "赋值语句错误（可能缺少变量名）";
                return false;
            }
        }//v
        private bool CheckReturnSentence(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[3];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "return语句";
            if (!lexic.end && lexic.lex() && lexic.code == 6)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].childNum = 0;
                syntree[cur].parentCur = thiscur;
                syntree[cur].info = "return";
                wdata = "<6, return>\n";
                lexsw.Write(wdata);
                curcache = lexic.cur;
                linecache = lexic.line;
                //没有返回值
                if (!lexic.end && lexic.lex() && lexic.code == 15)
                {
                    if (isreturn)
                    {
                        errorinfo = "return语句返回值与函数声明不符合";
                        return false;
                    }
                    else
                    {
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[thiscur].childNum++;
                        syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                        syntree[cur].childNum = 0;
                        syntree[cur].parentCur = thiscur;
                        syntree[cur].info = ";";
                        wdata = "<15, ;>\n";
                        lexsw.Write(wdata);
                        syntree[thiscur].line = midline;
                        syntree[thiscur].linenum = 1;
                        syntree[thiscur].midsen = PrintFormula("END", "", "", "");
                        midline++;
                        if (funcname[funcnamecount - 1].name.Equals("main"))
                        {
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        MOV    AX,  4C00h\n";
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        INT    21h\n";
                        }
                        else
                        {
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        RET\n";
                        }
                        return true;
                    }
                }
                //有返回值
                else
                {
                    if (!isreturn)
                    {
                        errorinfo = "return语句返回值与函数声明不符合";
                        return false;
                    }
                    else
                    {
                        lexic.line = linecache;
                        lexic.cur = curcache;
                        iscontrol = false;
                        if (CheckExpression(thiscur))
                        {
                            if (!lexic.end && lexic.lex() && lexic.code == 15)
                            {
                                cur++;
                                if (syntree[cur] == null)
                                {
                                    syntree[cur] = new SynTree();
                                }
                                syntree[thiscur].childNum++;
                                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                                syntree[cur].childNum = 0;
                                syntree[cur].parentCur = thiscur;
                                syntree[cur].info = ";";
                                wdata = "<15, ;>\n";
                                lexsw.Write(wdata);
                                syntree[thiscur].midsen = syntree[syntree[thiscur].child[1]].midsen + PrintFormula("=", syntree[syntree[thiscur].child[1]].place, "", "EAX") + PrintFormula("END", "", "", "");
                                midline++;
                                midline++;
                                syntree[thiscur].line = syntree[syntree[thiscur].child[1]].line;
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[1]].linenum+2;
                                if (funcname[funcnamecount - 1].name.Equals("main"))
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        MOV    AX,  4C00h\n";
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        INT    21h\n";
                                }
                                else
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        PUSH    EBX\n";
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        RET\n";
                                }
                                return true;
                            }
                            else
                            {
                                errorinfo = "return语句错误（可能缺少“;”）";
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                errorinfo = "return语句错误";
                return false;
            }
        }
        private bool CheckWhileSentence(int parent)
        {
            cur++;
            int thiscur = cur;
            int midlinecache;
            string sc;
            string op;
            string lable;
            string lable1;
            string lable2;
            string lable3;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[5];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "while语句";
            syntree[thiscur].line = midline;
            lable = GererateWhileLable();
            lable1 = lable + "_1";
            lable2 = lable + "_2";
            lable3 = lable + "_3";
            if (!lexic.end && lexic.lex() && lexic.code == 5)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].childNum = 0;
                syntree[cur].parentCur = thiscur;
                syntree[cur].info = "while";
                wdata = "<5, while>\n";
                lexsw.Write(wdata);
                if (!lexic.end && lexic.lex() && lexic.code == 17)
                {
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].childNum = 0;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].info = "(";
                    wdata = "<17, (>\n";
                    lexsw.Write(wdata);
                    iscontrol = true;
                    if (CheckExpression(thiscur))
                    {
                        midlinecache = midline;
                        midline = midline + 2;
                        codeline = midline;
                        syntree[thiscur].midsen = syntree[syntree[thiscur].child[2]].midsen;
                        if (!lexic.end && lexic.lex() && lexic.code == 18)
                        {
                            cur++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].childNum = 0;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].info = ")";
                            wdata = "<18, )>\n";
                            lexsw.Write(wdata);
                            sc = lexic.scope;
                            lexic.scope = lexic.scope + "while";
                            if (CheckSentenceBlock(thiscur))
                            {
                                codeline = midlinecache;
                                lexic.sblt.update(lexic.scope);
                                lexic.scope = sc;
                                op="j"+syntree[syntree[syntree[thiscur].child[2]].child[1]+1].info;
                                syntree[thiscur].linenum = syntree[syntree[thiscur].child[2]].linenum + syntree[syntree[thiscur].child[4]].linenum + 3;
                                syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula(op, syntree[syntree[syntree[thiscur].child[2]].child[0]].place, syntree[syntree[syntree[thiscur].child[2]].child[2]].place, (midlinecache+2).ToString());
                                syntree[thiscur].midsen = syntree[thiscur].midsen +PrintFormula("j", (syntree[thiscur].line+syntree[thiscur].linenum).ToString(), "", "");
                                codeline = midline;
                                syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[4]].midsen;
                                syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula("j", (syntree[thiscur].line).ToString(), "", "");
                                midline++;
                                codeline = midline;


                                syntree[thiscur].lastsen = lable1+":\n"+syntree[syntree[thiscur].child[2]].lastsen;
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen +"        CMP    EBX,  ECX\n";
                                if(syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("<"))
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JL    " + lable2 + "\n";
                                }
                                else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("<="))
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JLE    " + lable2 + "\n";
                                }
                                else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals(">"))
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JG    " + lable2 + "\n";
                                }
                                else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals(">="))
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JGE    " + lable2 + "\n";
                                }
                                else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("=="))
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JE    " + lable2 + "\n";
                                }
                                else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("!="))
                                {
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JNE    " + lable2 + "\n";
                                }
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JMP    " + lable3 + "\n";
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + lable2 + ":\n" + syntree[syntree[thiscur].child[4]].lastsen;
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JMP    " + lable1 + "\n";
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + lable3 + ":\n";
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            errorinfo = "while语句错误（可能缺少“)”）";
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    errorinfo = "while语句错误（可能缺少“(”）";
                    return false;
                }
            }
            else
            {
                errorinfo = "while语句错误";
                return false;
            }
        }
        private bool CheckIfSentence(int parent)
        {
            cur++;
            int thiscur = cur;
            string sc;
            string lable;
            string lable1;
            string lable2;
            string lable3;
            string lable4;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            int midlinecache;
            int codelinecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[7];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "if语句";
            syntree[thiscur].line = midline;
            lable = GererateIfLable();
            lable1 = lable + "_1";
            lable2 = lable + "_2";
            lable3 = lable + "_3";
            lable4 = lable + "_4";
            if (!lexic.end && lexic.lex() && lexic.code == 3)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].childNum = 0;
                syntree[cur].parentCur = thiscur;
                syntree[cur].info = "if";
                if (!lexic.end && lexic.lex() && lexic.code == 17)
                {
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].childNum = 0;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].info = "(";
                    iscontrol = true;
                    if (CheckExpression(thiscur))
                    {
                        midlinecache = midline;
                        midline=midline+2;
                        codeline = midline;
                        if (!lexic.end && lexic.lex() && lexic.code == 18)
                        {
                            cur++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].childNum = 0;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].info = ")";
                            sc = lexic.scope;
                            lexic.scope = lexic.scope + "if";
                            if (CheckSentenceBlock(thiscur))
                            {
                                codelinecache = codeline;
                                lexic.sblt.update(lexic.scope);
                                lexic.scope = sc;
                                curcache = lexic.cur;
                                linecache = lexic.line;
                                //有else语句
                                if (!lexic.end && lexic.lex() && lexic.code == 4)
                                {
                                    codeline++;
                                    cur++;
                                    if (syntree[cur] == null)
                                    {
                                        syntree[cur] = new SynTree();
                                    }
                                    syntree[thiscur].childNum++;
                                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                                    syntree[cur].childNum = 0;
                                    syntree[cur].parentCur = thiscur;
                                    syntree[cur].info = "else";
                                    sc = lexic.scope;
                                    lexic.scope = lexic.scope + "else";
                                    midline++;
                                    if (CheckSentenceBlock(thiscur))
                                    {
                                        lexic.sblt.update(lexic.scope);
                                        lexic.scope = sc;
                                        syntree[thiscur].linenum = syntree[syntree[thiscur].child[2]].linenum + syntree[syntree[thiscur].child[4]].linenum + syntree[syntree[thiscur].child[6]].linenum + 3;
                                        syntree[thiscur].midsen = syntree[syntree[thiscur].child[2]].midsen;
                                        codeline = midlinecache;
                                        syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula("j" + syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info, syntree[syntree[syntree[thiscur].child[2]].child[0]].place, syntree[syntree[syntree[thiscur].child[2]].child[2]].place, (syntree[thiscur].line+syntree[syntree[thiscur].child[2]].linenum+2).ToString());
                                        syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula("j", (syntree[thiscur].line + syntree[syntree[thiscur].child[2]].linenum + 3 + syntree[syntree[thiscur].child[4]].linenum).ToString(), "", "");
                                        syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[4]].midsen;
                                        codeline = codelinecache;
                                        syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula("j", (syntree[thiscur].line + syntree[thiscur].linenum).ToString(), "", "");
                                        syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[6]].midsen;
                                        codeline = midline+2;
                                        syntree[thiscur].lastsen = lable1 + ":\n" + syntree[syntree[thiscur].child[2]].lastsen;
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        CMP    EBX,  ECX\n";
                                        if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("<"))
                                        {
                                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JL    " + lable2 + "\n";
                                        }
                                        else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("<="))
                                        {
                                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JLE    " + lable2 + "\n";
                                        }
                                        else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals(">"))
                                        {
                                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JG    " + lable2 + "\n";
                                        }
                                        else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals(">="))
                                        {
                                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JGE    " + lable2 + "\n";
                                        }
                                        else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("=="))
                                        {
                                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JE    " + lable2 + "\n";
                                        }
                                        else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("!="))
                                        {
                                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JNE    " + lable2 + "\n";
                                        }
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JMP    " + lable3 + "\n";
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + lable2 + ":\n" + syntree[syntree[thiscur].child[4]].lastsen;
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JMP    " + lable4 + "\n";
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + lable3 + ":\n" + syntree[syntree[thiscur].child[6]].lastsen + lable4 + ":\n";
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                //没有else语句
                                else
                                {
                                    syntree[thiscur].linenum = syntree[syntree[thiscur].child[2]].linenum + syntree[syntree[thiscur].child[4]].linenum + 2;
                                    syntree[thiscur].midsen = syntree[syntree[thiscur].child[2]].midsen;
                                    codeline = midlinecache;
                                    syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula("j" + syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info, syntree[syntree[syntree[thiscur].child[2]].child[0]].place, syntree[syntree[syntree[thiscur].child[2]].child[2]].place, (syntree[thiscur].line + syntree[syntree[thiscur].child[2]].linenum+2).ToString());
                                    syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula("j", (syntree[thiscur].line+ syntree[thiscur].linenum).ToString(), "", "");
                                    syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[4]].midsen;
                                    lexic.line = linecache;
                                    lexic.cur = curcache;
                                    codeline = midline+2;
                                    syntree[thiscur].lastsen = lable1 + ":\n" + syntree[syntree[thiscur].child[2]].lastsen;
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        CMP    EBX,  ECX\n";
                                    if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("<"))
                                    {
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JL    " + lable2 + "\n";
                                    }
                                    else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("<="))
                                    {
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JLE    " + lable2 + "\n";
                                    }
                                    else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals(">"))
                                    {
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JG    " + lable2 + "\n";
                                    }
                                    else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals(">="))
                                    {
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JGE    " + lable2 + "\n";
                                    }
                                    else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("=="))
                                    {
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JE    " + lable2 + "\n";
                                    }
                                    else if (syntree[syntree[syntree[thiscur].child[2]].child[1] + 1].info.Equals("!="))
                                    {
                                        syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JNE    " + lable2 + "\n";
                                    }
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        JMP    " + lable4 + "\n";
                                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + lable2 + ":\n" + syntree[syntree[thiscur].child[4]].lastsen+lable4+":\n";
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            errorinfo = "if语句错误（可能缺少“)”）";
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    errorinfo = "if语句错误（可能缺少“(”）";
                    return false;
                }
            }
            else
            {
                errorinfo = "if语句错误";
                return false;
            }
        }
        private bool CheckExpression(int parent)
        {
            cur++;
            int thiscur = cur;
            int i=0;//逻辑运算符的数量
            bool isc=iscontrol;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "表达式";
            syntree[thiscur].line = midline;
            if (CheckAdditiveExpression(thiscur, "EBX"))
            {
                syntree[thiscur].linenum = syntree[syntree[thiscur].child[0]].linenum;
                while (true)
                {
                    if (i > 1)
                    {
                        return false;
                    }
                    curcache = lexic.cur;
                    linecache = lexic.line;
                    if (!lexic.end && lexic.lex() && lexic.code == 14)
                    {
                        i++;
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[thiscur].childNum++;
                        syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                        syntree[cur].parentCur = thiscur;
                        syntree[cur].child = new int[1];
                        syntree[cur].childNum = 1;
                        syntree[cur].info = "relop";
                        syntree[cur].child[0] = cur + 1;
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[cur].parentCur = cur - 1;
                        syntree[cur].childNum = 0;
                        syntree[cur].info = lexic.info;
                        wdata = "<14, " + lexic.info + ">\n";
                        lexsw.Write(wdata);
                        if (CheckAdditiveExpression(thiscur, "ECX"))
                        {
                            syntree[thiscur].linenum = syntree[thiscur].linenum + syntree[syntree[thiscur].child[2 * i]].linenum;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (i == 1 && !isc)
                        {
                            return false;
                        }
                        else if (i == 0 && isc)
                        {
                            return false;
                        }
                        else if (!isc)
                        {
                            syntree[thiscur].place = syntree[syntree[thiscur].child[0]].place;
                            syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen;
                            syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            return true;
                        }
                        else
                        {
                            syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen + syntree[syntree[thiscur].child[2]].midsen;
                            syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen + syntree[syntree[thiscur].child[2]].lastsen;
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            return true;
                        }
                        
                    }
                }
                
            }
            else
            {
                return false;
            }

        }
        private bool CheckAdditiveExpression(int parent, string root)
        {
            cur++;
            int i;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "加法表达式";
            i = 0;//运算符的个数
            syntree[thiscur].line = midline;
            if (CheckItem(thiscur))
            {
                syntree[thiscur].place = syntree[syntree[thiscur].child[0]].place;
                syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen;
                syntree[thiscur].linenum = syntree[syntree[thiscur].child[i]].linenum;
                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[i]].lastsen + "        MOV    " + root + ",  item\n";
                while (true)
                {
                    curcache = lexic.cur;
                    linecache = lexic.line;
                    if (!lexic.end && lexic.lex())
                    {
                        if (lexic.code == 10)
                        {
                            cur++;
                            i++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].childNum = 0;
                            syntree[cur].info = "+";
                            wdata = "<10, +>\n";
                            lexsw.Write(wdata);
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        PUSH    " + root + "\n";
                            if (CheckItem(thiscur))
                            {
                                syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[i * 2]].midsen + PrintFormula("+", syntree[thiscur].place, syntree[syntree[thiscur].child[i * 2]].place, syntree[thiscur].place);
                                syntree[thiscur].linenum = syntree[thiscur].linenum+syntree[syntree[thiscur].child[i * 2]].linenum+1;
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + syntree[syntree[thiscur].child[i * 2]].lastsen + "        POP    " + root + "\n" + "        ADD    " + root + ",  item\n";
                                midline++;
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if(lexic.code==11)
                        {
                            cur++;
                            i++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].childNum = 0;
                            syntree[cur].info = "-";
                            wdata = "<11, ->\n";
                            lexsw.Write(wdata);
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        PUSH    " + root + "\n";
                            if (CheckItem(thiscur))
                            {
                                syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[i * 2]].midsen + PrintFormula("-", syntree[thiscur].place, syntree[syntree[thiscur].child[i * 2]].place, syntree[thiscur].place);
                                syntree[thiscur].linenum = syntree[thiscur].linenum + syntree[syntree[thiscur].child[i * 2]].linenum + 1;
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + syntree[syntree[thiscur].child[i * 2]].lastsen + "        POP    " + root + "\n" + "        SUB    " + root + ",  item\n";
                                midline++;
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            return true;
                        }
                    }
                    else
                    {
                        lexic.line = linecache;
                        lexic.cur = curcache;
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        private bool CheckItem(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            int i;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "项";
            syntree[thiscur].line = midline;
            i = 0;
            if (CheckFactor(thiscur))
            {
                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[i]].lastsen + "        MOV    EAX,  factor\n";
                syntree[thiscur].place = syntree[syntree[thiscur].child[i]].place;
                syntree[thiscur].midsen = syntree[syntree[thiscur].child[i]].midsen;
                syntree[thiscur].linenum = syntree[syntree[thiscur].child[i]].linenum;
                while (true)
                {
                    curcache = lexic.cur;
                    linecache = lexic.line;
                    if (!lexic.end && lexic.lex())
                    {
                        if (lexic.code == 12)
                        {
                            cur++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            i++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].childNum = 0;
                            syntree[cur].info = "*";
                            wdata = "<12, *>\n";
                            lexsw.Write(wdata);
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        PUSH    EAX\n";
                            if (CheckFactor(thiscur))
                            {
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + syntree[syntree[thiscur].child[i * 2]].lastsen + "        POP    EAX\n";
                                syntree[thiscur].linenum = syntree[thiscur].linenum + syntree[syntree[thiscur].child[i * 2]].linenum + 1;
                                syntree[thiscur].midsen=syntree[thiscur].midsen+ syntree[syntree[thiscur].child[i * 2]].midsen+PrintFormula("*", syntree[thiscur].place, syntree[syntree[thiscur].child[i*2]].place, syntree[thiscur].place);
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        MUL    factor\n";
                                midline++;
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (lexic.code == 13)
                        {
                            cur++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            i++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].childNum = 0;
                            syntree[cur].info = "/";
                            wdata = "<13, />\n";
                            lexsw.Write(wdata);
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        PUSH    EAX\n";
                            if (CheckFactor(thiscur))
                            {
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + syntree[syntree[thiscur].child[i * 2]].lastsen + "        POP    EAX\n";
                                i++;
                                syntree[thiscur].linenum = syntree[thiscur].linenum + syntree[syntree[thiscur].child[i * 2]].linenum + 1;
                                syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[i * 2]].midsen + PrintFormula("/", syntree[thiscur].place, syntree[syntree[thiscur].child[i * 2]].place, syntree[thiscur].place);
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        MOV    EDX,  0h\n";
                                syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        DIV    factor\n";
                                midline++;
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            lexic.line = linecache;
                            lexic.cur = curcache;
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        MOV    item,  EAX\n";
                            return true;
                        }
                    }
                    else
                    {
                        lexic.line = linecache;
                        lexic.cur = curcache;
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        private bool CheckFactor(int parent)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[3];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "因子";
            syntree[thiscur].line = midline;
            if (!lexic.end && lexic.lex())
            {
                if (lexic.code == 8)
                {
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].child = new int[1];
                    syntree[cur].childNum = 1;
                    syntree[cur].info = "NUM";
                    syntree[cur].child[0] = cur + 1;
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[cur].parentCur = cur - 1;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = lexic.info;
                    wdata = "<8, " + lexic.info + ">\n";
                    lexsw.Write(wdata);
                    syntree[thiscur].place = GenerateIntermediateVariable();
                    syntree[thiscur].midsen = PrintFormula("=", lexic.info, "", syntree[thiscur].place);
                    midline++;
                    syntree[thiscur].linenum = 1;
                    syntree[thiscur].lastsen = "        MOV    factor,  " + lexic.info + "\n";
                    return true;
                }
                else if (lexic.code == 7)
                {
                    if (lexic.InsertId() == -1)
                    {
                        errorinfo = "因子名错误";
                        return false;
                    }
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].child = new int[1];
                    syntree[cur].childNum = 1;
                    syntree[cur].info = "ID";
                    syntree[cur].child[0] = cur + 1;
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[cur].parentCur = cur - 1;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = lexic.info;
                    wdata = "<7, " + lexic.info + ">\n";
                    lexsw.Write(wdata);
                    if (CheckFtype(thiscur, lexic.info))
                    {
                        syntree[thiscur].midsen = syntree[syntree[thiscur].child[1]].midsen;
                        syntree[thiscur].linenum = syntree[syntree[thiscur].child[1]].linenum;
                        syntree[thiscur].lastsen = syntree[syntree[thiscur].child[1]].lastsen;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (lexic.code == 17)
                {
                    cur++;
                    if (syntree[cur] == null)
                    {
                        syntree[cur] = new SynTree();
                    }
                    syntree[thiscur].childNum++;
                    syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                    syntree[cur].parentCur = thiscur;
                    syntree[cur].childNum = 0;
                    syntree[cur].info = "(";
                    wdata = "<17, (>\n";
                    lexsw.Write(wdata);
                    iscontrol = false;
                    if (CheckExpression(thiscur))
                    {
                        syntree[thiscur].line = syntree[syntree[thiscur].child[1]].line;
                        if (!lexic.end && lexic.lex() && lexic.code == 18)
                        {
                            cur++;
                            if (syntree[cur] == null)
                            {
                                syntree[cur] = new SynTree();
                            }
                            syntree[thiscur].childNum++;
                            syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                            syntree[cur].parentCur = thiscur;
                            syntree[cur].childNum = 0;
                            syntree[cur].info = ")";
                            wdata = "<18, )>\n";
                            lexsw.Write(wdata);
                            syntree[thiscur].place = syntree[syntree[thiscur].child[1]].place;
                            syntree[thiscur].midsen = syntree[syntree[thiscur].child[1]].midsen;
                            syntree[thiscur].linenum = syntree[syntree[thiscur].child[1]].linenum;
                            syntree[thiscur].lastsen = syntree[syntree[thiscur].child[1]].lastsen + "        MOV    factor,  EBX\n";
                            return true;
                        }
                        else
                        {
                            errorinfo = "因子错误（可能缺少“）”）";
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                errorinfo = "因子错误";
                return false;
            }
        }
        private bool CheckFtype(int parent, string idname)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            int funcnamecur;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "FTYPE";
            curcache = lexic.cur;
            linecache = lexic.line;
            funcnamecur = FindFuncName(idname);
            syntree[thiscur].line = midline;
            //FTYPE不为空
            if (!lexic.end && lexic.lex() && lexic.code == 17)
            {
                if (funcnamecur == -1)
                {
                    return false;
                }
                lexic.line = linecache;
                lexic.cur = curcache;
                if (CheckCall(thiscur, funcnamecur))
                {
                    syntree[parent].place = "EAX";
                    syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen + PrintFormula("call", funcname[funcnamecur].name, "", "");
                    syntree[thiscur].linenum = syntree[syntree[thiscur].child[0]].linenum + 1;
                    syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen + "        CALL    _" + funcname[funcnamecur].name + "\n";
                    syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        POP    factor\n";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //FTYPE为空
            else
            {
                lexic.line = linecache;
                lexic.cur = curcache;
                syntree[thiscur].place = GenerateIntermediateVariable();
                syntree[parent].place = syntree[thiscur].place;
                syntree[thiscur].midsen = PrintFormula("=", idname, "", syntree[thiscur].place);
                midline++;
                syntree[thiscur].linenum = 1;
                syntree[thiscur].lastsen = "        MOV    EAX," + funcname[funcnamecount - 1].name + "_" + idname + "\n";
                syntree[thiscur].lastsen = syntree[thiscur].lastsen + "        MOV    factor,  EAX\n";
                return true;
            }
        }
        private bool CheckCall(int parent, int funcnamecur)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[3];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "call";
            syntree[thiscur].line = midline;
            if (!lexic.end && lexic.lex() && lexic.code == 17)
            {
                cur++;
                if (syntree[cur] == null)
                {
                    syntree[cur] = new SynTree();
                }
                syntree[thiscur].childNum++;
                syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                syntree[cur].parentCur = thiscur;
                syntree[cur].childNum = 0;
                syntree[cur].info = "(";
                wdata = "<17, (>\n";
                lexsw.Write(wdata);
                if (CheckActualParameter(thiscur, funcnamecur))
                {
                    if (!lexic.end && lexic.lex() && lexic.code == 18)
                    {
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[thiscur].childNum++;
                        syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                        syntree[cur].parentCur = thiscur;
                        syntree[cur].childNum = 0;
                        syntree[cur].info = ")";
                        wdata = "<18, )>\n";
                        lexsw.Write(wdata);
                        syntree[thiscur].linenum = syntree[syntree[thiscur].child[1]].linenum;
                        syntree[thiscur].midsen = syntree[syntree[thiscur].child[1]].midsen;
                        syntree[thiscur].lastsen = syntree[syntree[thiscur].child[1]].lastsen;
                        return true;
                    }
                    else
                    {
                        errorinfo = "call语句错误（可能缺少“)”）";
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                errorinfo = "call错误（可能缺少“(”）";
                return false;
            }
        }
        private bool CheckActualParameter(int parent, int funcnamecur)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "实参";
            curcache=lexic.cur;
            linecache = lexic.line;
            syntree[thiscur].line = midline;
            //实参为空
            if (!lexic.end && lexic.lex() && lexic.code == 18)
            {
                lexic.line = linecache;
                lexic.cur = curcache;
                if (funcname[funcnamecur].paranum == 0)
                {
                    syntree[thiscur].midsen = "";
                    syntree[thiscur].linenum = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //实参不为空
            else
            {
                lexic.line = linecache;
                lexic.cur = curcache;
                if (CheckActualParameterList(thiscur, funcnamecur))
                {
                    syntree[thiscur].linenum = syntree[syntree[thiscur].child[0]].linenum;
                    syntree[thiscur].midsen = syntree[syntree[thiscur].child[0]].midsen;
                    syntree[thiscur].lastsen = syntree[syntree[thiscur].child[0]].lastsen;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool CheckActualParameterList(int parent, int funcnamecur)
        {
            cur++;
            int thiscur = cur;
            if (syntree[thiscur] == null)
            {
                syntree[thiscur] = new SynTree();
            }
            int i=0;
            int j=0;
            int curcache;
            int linecache;
            syntree[parent].childNum++;
            syntree[parent].child[syntree[parent].childNum - 1] = thiscur;
            syntree[thiscur].parentCur = parent;
            syntree[thiscur].child = new int[1024];
            syntree[thiscur].childNum = 0;
            syntree[thiscur].info = "实参列表";
            iscontrol = false;
            syntree[thiscur].line = midline;
            if (CheckExpression(thiscur))
            {
                syntree[thiscur].linenum = syntree[syntree[thiscur].child[i]].linenum;
                syntree[thiscur].midsen = syntree[syntree[thiscur].child[i]].midsen;
                syntree[thiscur].lastsen = syntree[syntree[thiscur].child[i]].lastsen+"        PUSH    EBX\n";
                i++;
                while (true)
                {
                    curcache = lexic.cur;
                    linecache = lexic.line;
                    if (!lexic.end && lexic.lex() && lexic.code == 16)
                    {
                        cur++;
                        if (syntree[cur] == null)
                        {
                            syntree[cur] = new SynTree();
                        }
                        syntree[thiscur].childNum++;
                        syntree[thiscur].child[syntree[thiscur].childNum - 1] = cur;
                        syntree[cur].parentCur = thiscur;
                        syntree[cur].childNum = 0;
                        syntree[cur].info = ",";
                        wdata = "<16, ,>\n";
                        lexsw.Write(wdata);
                        iscontrol = false;
                        if (CheckExpression(thiscur))
                        {
                            syntree[thiscur].lastsen = syntree[thiscur].lastsen + syntree[syntree[thiscur].child[i * 2]].lastsen + "        PUSH    EBX\n";
                            syntree[thiscur].linenum = syntree[thiscur].linenum + syntree[syntree[thiscur].child[i * 2]].linenum;
                            syntree[thiscur].midsen = syntree[thiscur].midsen + syntree[syntree[thiscur].child[i * 2]].midsen;
                            i++;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        lexic.line = linecache;
                        lexic.cur = curcache;
                        if (i != funcname[funcnamecur].paranum)
                        {
                            return false;
                        }
                        else
                        {
                            for (j = 0; j < i; j++)
                            {
                                syntree[thiscur].midsen = syntree[thiscur].midsen + PrintFormula("param", syntree[syntree[thiscur].child[2 * j]].place, "", "");
                                syntree[thiscur].linenum++;
                            }
                            return true;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
        private string GenerateIntermediateVariable()
        {
            count++;
            return "T" + count.ToString();
        }
        private string GererateWhileLable()
        {
            whilenum++;
            return funcname[funcnamecount - 1].name + "_while" + whilenum.ToString();
        }
        private string GererateIfLable()
        {
            ifnum++;
            return funcname[funcnamecount - 1].name + "_if" + ifnum.ToString();
        }
        private string PrintFormula(string s1, string s2, string s3, string s4)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dr.CreateCells(dgv1);
            dr.Cells[0].Value = codeline;
            dr.Cells[1].Value = s1;
            dr.Cells[2].Value = s2;
            dr.Cells[3].Value = s3;
            dr.Cells[4].Value = s4;
            dgv1.Rows.Add(dr);
            codeline++;
            return s1+"     "+s2+"     "+s3+"     "+s4+"\n";
        }
        private void EmitFormula() 
        {
            int i = 0, j = -1;
            int maincur;
            string midcode="";
            DataGridViewRow dr = new DataGridViewRow();
            maincur = FindFuncName("main");
            cachesw.Write("j    "+funcname[maincur].start.ToString()+'\n');
            dr.CreateCells(dgv1);
            dr.Cells[0].Value = -1;
            dr.Cells[1].Value = "j";
            dr.Cells[2].Value = funcname[maincur].start;
            dr.Cells[3].Value = "";
            dr.Cells[4].Value = "";
            dgv1.Rows.Add(dr);
            for (i = 0; i < funcnamecount; i++)
            {
                cachesw.Write(funcname[i].sentenceblock);
            }
            cachesw.Flush();
            cachesw.Close();
            cachefs.Close();
            String[] lines = File.ReadAllLines(cachepath);
            File.Delete(cachepath);
            foreach (string line in lines)
            {
                midcode = midcode + j.ToString() + "    " + line+"\n";
                j++;
            }
            midsw.Write(midcode);
        }
        private int FindFuncName(string s) 
        {
            int i=0;
            while (funcname[i] != null)
            {
                if (funcname[i].name.Equals(s))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
        private void AddFuncName(string s) 
        {
            funcname[funcnamecount] = new FuncName();
            funcname[funcnamecount].name = s;
            funcname[funcnamecount].varnum = 0;
            funcname[funcnamecount].paranum = 0;
            funcname[funcnamecount].sentenceline = 0;
            funcnamecount++;
        }
        private void DelFuncName()
        {
            funcnamecount--;
            supervar[supervarnum] = funcname[funcnamecount].name;
            supervarnum++;
            funcname[funcnamecount] = null;
        }
        #endregion
        private void GenerateLastCode()
        {
            int i, j;
            string headcode;
            headcode = "_STACK    SEGMENT    STACK    'STACK'\n";
            headcode = headcode + "          DB    32766    DUP(0)\n";
            headcode = headcode + "          TOS    DW    0\n";
            headcode = headcode + "_STACK    ENDS\n";
            headcode = headcode + "_DATA    SEGMENT 'DATA'\n";
            for (i = 0; i < supervarnum; i++)
            {
                headcode = headcode + supervar[i] + "    DW    ?\n";
            }
            for (i = 0; i < funcnamecount; i++)
            {
                for (j = 0; j < funcname[i].paranum; j++)
                {
                    headcode = headcode + funcname[i].paralist[j] + "    DW    ?\n";
                }
            }
            headcode = headcode + "item" + "    DW    ?\n";
            headcode = headcode + "factor" + "    DW    ?\n";
            headcode = headcode + "_DATA    ENDS\n";
            headcode = headcode + "_TEXT    SEGMENT    'CODE'\n";
            headcode = headcode + "         ASSUME    CS:_TEXT,    DS:_DATA,    SS:_STACK\n";
            for (i = 0; i < funcnamecount; i++)
            {
                if (funcname[i].name.Equals("main"))
                {
                    headcode = headcode + "start:  MOV    AX,  _DATA\n";
                    headcode = headcode + "        MOV    DS,  AX\n";
                    headcode = headcode + "        CLI\n";
                    headcode = headcode + "        MOV    AX,  _STACK\n";
                    headcode = headcode + "        MOV    SS,  AX\n";
                    headcode = headcode + "        MOV    SP,  Offset  TOS\n";
                    headcode = headcode + "        STI\n";
                    headcode = headcode + funcname[i].lastcode;
                }
                else
                {
                    headcode = headcode + "_" + funcname[i].name + "    PROC    Far\n";
                    headcode = headcode + funcname[i].lastcode;
                    headcode = headcode + "_" + funcname[i].name + "    ENDP\n";
                }
            }
            headcode = headcode + "_TEXT    ENDS\n";
            headcode = headcode + "        END    start\n";
            goalsw.Write(headcode);

        }
    }
}
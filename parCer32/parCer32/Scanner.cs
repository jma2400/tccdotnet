using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace parCer32
{
    #region Scanner

    public partial class Scanner
    {
        public string Input;
        public int LastPos = 0;
        public int StartPos = 0;
        public int EndPos = 0;
        public int CurrentLine;
        public int CurrentColumn;
        public int CurrentPosition;
        public List<Token> Skipped;
        public Dictionary<TokenType, Regex> Patterns;

        private Token LookAheadToken;
        private List<TokenType> Tokens;
        private List<TokenType> SkipList;

        public Scanner()
        {
            Regex regex;
            Patterns = new Dictionary<TokenType, Regex>();
            Tokens = new List<TokenType>();
            LookAheadToken = null;

            SkipList = new List<TokenType>();
            SkipList.Add(TokenType.WHITESPACE);
            SkipList.Add(TokenType.EOL);
            SkipList.Add(TokenType.COMMENTLINE);
            SkipList.Add(TokenType.COMMENTBLOCK);

            regex = new Regex(@"<.+?>", RegexOptions.Compiled);
            Patterns.Add(TokenType.DIRECTIVE, regex);
            Tokens.Add(TokenType.DIRECTIVE);

            regex = new Regex(@"float|double|u?int|char|u?longlong|u?long|u?short", RegexOptions.Compiled);
            Patterns.Add(TokenType.DATATYPE, regex);
            Tokens.Add(TokenType.DATATYPE);

            regex = new Regex(@"include", RegexOptions.Compiled);
            Patterns.Add(TokenType.INCLUDE, regex);
            Tokens.Add(TokenType.INCLUDE);

            regex = new Regex(@"switch", RegexOptions.Compiled);
            Patterns.Add(TokenType.SWITCH, regex);
            Tokens.Add(TokenType.SWITCH);

            regex = new Regex(@"case", RegexOptions.Compiled);
            Patterns.Add(TokenType.CASE, regex);
            Tokens.Add(TokenType.CASE);

            regex = new Regex(@"if", RegexOptions.Compiled);
            Patterns.Add(TokenType.IF, regex);
            Tokens.Add(TokenType.IF);

            regex = new Regex(@"else", RegexOptions.Compiled);
            Patterns.Add(TokenType.ELSE, regex);
            Tokens.Add(TokenType.ELSE);

            regex = new Regex(@"for", RegexOptions.Compiled);
            Patterns.Add(TokenType.FOR, regex);
            Tokens.Add(TokenType.FOR);

            regex = new Regex(@"while", RegexOptions.Compiled);
            Patterns.Add(TokenType.WHILE, regex);
            Tokens.Add(TokenType.WHILE);

            regex = new Regex(@"default", RegexOptions.Compiled);
            Patterns.Add(TokenType.DEFAULT, regex);
            Tokens.Add(TokenType.DEFAULT);

            regex = new Regex(@"return", RegexOptions.Compiled);
            Patterns.Add(TokenType.RETURN, regex);
            Tokens.Add(TokenType.RETURN);

            regex = new Regex(@"scanf", RegexOptions.Compiled);
            Patterns.Add(TokenType.SCANF, regex);
            Tokens.Add(TokenType.SCANF);

            regex = new Regex(@"printf", RegexOptions.Compiled);
            Patterns.Add(TokenType.PRINTF, regex);
            Tokens.Add(TokenType.PRINTF);

            regex = new Regex(@"do", RegexOptions.Compiled);
            Patterns.Add(TokenType.DO, regex);
            Tokens.Add(TokenType.DO);

            regex = new Regex(@"break", RegexOptions.Compiled);
            Patterns.Add(TokenType.BREAK, regex);
            Tokens.Add(TokenType.BREAK);

            regex = new Regex(@"@?\""%[dfsc]\""", RegexOptions.Compiled);
            Patterns.Add(TokenType.TYPESPEC, regex);
            Tokens.Add(TokenType.TYPESPEC);

            regex = new Regex(@"&", RegexOptions.Compiled);
            Patterns.Add(TokenType.REFOPER, regex);
            Tokens.Add(TokenType.REFOPER);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled);
            Patterns.Add(TokenType.IDENTIFIER, regex);
            Tokens.Add(TokenType.IDENTIFIER);

            regex = new Regex(@"\+|-|%|/|\*", RegexOptions.Compiled);
            Patterns.Add(TokenType.ARTOPERATOR, regex);
            Tokens.Add(TokenType.ARTOPERATOR);

            regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Patterns.Add(TokenType.NUMBER, regex);
            Tokens.Add(TokenType.NUMBER);

            regex = new Regex(@",", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMA, regex);
            Tokens.Add(TokenType.COMMA);

            regex = new Regex(@"=", RegexOptions.Compiled);
            Patterns.Add(TokenType.EQUALS, regex);
            Tokens.Add(TokenType.EQUALS);

            regex = new Regex(@";", RegexOptions.Compiled);
            Patterns.Add(TokenType.SEMICOL, regex);
            Tokens.Add(TokenType.SEMICOL);

            regex = new Regex(@":", RegexOptions.Compiled);
            Patterns.Add(TokenType.COLON, regex);
            Tokens.Add(TokenType.COLON);

            regex = new Regex(@"#", RegexOptions.Compiled);
            Patterns.Add(TokenType.SHARP, regex);
            Tokens.Add(TokenType.SHARP);

            regex = new Regex(@"^$", RegexOptions.Compiled);
            Patterns.Add(TokenType.EOF, regex);
            Tokens.Add(TokenType.EOF);

            regex = new Regex(@"\(", RegexOptions.Compiled);
            Patterns.Add(TokenType.LPAREN, regex);
            Tokens.Add(TokenType.LPAREN);

            regex = new Regex(@"\)", RegexOptions.Compiled);
            Patterns.Add(TokenType.RPAREN, regex);
            Tokens.Add(TokenType.RPAREN);

            regex = new Regex(@"\{", RegexOptions.Compiled);
            Patterns.Add(TokenType.LBRACE, regex);
            Tokens.Add(TokenType.LBRACE);

            regex = new Regex(@"\}", RegexOptions.Compiled);
            Patterns.Add(TokenType.RBRACE, regex);
            Tokens.Add(TokenType.RBRACE);

            regex = new Regex(@">=|<=|==|!=|>|<", RegexOptions.Compiled);
            Patterns.Add(TokenType.RELOP, regex);
            Tokens.Add(TokenType.RELOP);

            regex = new Regex(@"&&|\|\|", RegexOptions.Compiled);
            Patterns.Add(TokenType.LOGOP, regex);
            Tokens.Add(TokenType.LOGOP);

            regex = new Regex(@"\+\+|--", RegexOptions.Compiled);
            Patterns.Add(TokenType.INCRE, regex);
            Tokens.Add(TokenType.INCRE);

            regex = new Regex(@"true|false", RegexOptions.Compiled);
            Patterns.Add(TokenType.BOOL, regex);
            Tokens.Add(TokenType.BOOL);

            regex = new Regex(@"!", RegexOptions.Compiled);
            Patterns.Add(TokenType.NOT, regex);
            Tokens.Add(TokenType.NOT);

            regex = new Regex(@"", RegexOptions.Compiled);
            Patterns.Add(TokenType.NULL, regex);
            Tokens.Add(TokenType.NULL);

            regex = new Regex(@"@?\""(\""\""|[^\""])*\""", RegexOptions.Compiled);
            Patterns.Add(TokenType.STRING, regex);
            Tokens.Add(TokenType.STRING);

            regex = new Regex(@"\s+", RegexOptions.Compiled);
            Patterns.Add(TokenType.WHITESPACE, regex);
            Tokens.Add(TokenType.WHITESPACE);

            regex = new Regex(@"[\n\r]", RegexOptions.Compiled);
            Patterns.Add(TokenType.EOL, regex);
            Tokens.Add(TokenType.EOL);

            regex = new Regex(@"//[^\n]*\n?", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMENTLINE, regex);
            Tokens.Add(TokenType.COMMENTLINE);

            regex = new Regex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMENTBLOCK, regex);
            Tokens.Add(TokenType.COMMENTBLOCK);


        }

        public void Init(string input)
        {
            this.Input = input;
            StartPos = 0;
            EndPos = 0;
            CurrentLine = 0;
            CurrentColumn = 0;
            CurrentPosition = 0;
            Skipped = new List<Token>();
            LookAheadToken = null;
        }

        public Token GetToken(TokenType type)
        {
            Token t = new Token(this.StartPos, this.EndPos, this.CurrentLine, this.CurrentColumn);
            t.Type = type;
            return t;
        }






        public Token Scan(params TokenType[] expectedtokens)
        {
            Token tok = LookAhead(expectedtokens);

            LookAheadToken = null;
            StartPos = tok.EndPos;
            EndPos = tok.EndPos;
            return tok;
        }





        public Token LookAhead(params TokenType[] expectedtokens)
        {
            int i;
            int startpos = StartPos;
            Token tok = null;
            List<TokenType> scantokens;




            if (LookAheadToken != null
                && LookAheadToken.Type != TokenType._UNDETERMINED_
                && LookAheadToken.Type != TokenType._NONE_) return LookAheadToken;


            if (expectedtokens.Length == 0)
                scantokens = Tokens;
            else
            {
                scantokens = new List<TokenType>(expectedtokens);
                scantokens.AddRange(SkipList);
            }

            do
            {

                int len = -1;
                TokenType index = (TokenType)int.MaxValue;
                string input = Input.Substring(startpos);
                string count = Input.Substring(0, EndPos);
                CurrentLine = count.Split('\n').Length;
                int a = count.LastIndexOf(Environment.NewLine);
                CurrentColumn = EndPos - a;

                tok = new Token(startpos, EndPos, CurrentLine, CurrentColumn);


                for (i = 0; i < scantokens.Count; i++)
                {
                    Regex r = Patterns[scantokens[i]];
                    Match m = r.Match(input);
                    if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len)))
                    {
                        len = m.Length;
                        index = scantokens[i];
                    }
                }

                if (index >= 0 && len >= 0)
                {
                    tok.EndPos = startpos + len;
                    tok.Text = Input.Substring(tok.StartPos, len);
                    tok.Type = index;
                    if (tok.Type != TokenType.WHITESPACE)
                    {
                        //Console.WriteLine("Token: {0} \n Type: {1,-12} Line: {2,3} \n", tok.Text, tok.Type, tok.LinePos);
                    }
                }
                else
                {
                    if (tok.StartPos < tok.EndPos - 1)
                    {
                        tok.Text = Input.Substring(tok.StartPos, 1);
                    }
                }

                if (SkipList.Contains(tok.Type))
                {
                    startpos = tok.EndPos;
                    Skipped.Add(tok);
                }
            }
            while (SkipList.Contains(tok.Type));

            LookAheadToken = tok;
            return tok;
        }
    }

    #endregion

    #region Token

    public enum TokenType
    {


        _NONE_ = 0,
        _UNDETERMINED_ = 1,


        Start = 2,
        Header = 3,
        Declaration = 4,
        InsideDeclaration = 5,
        Assignment = 6,
        DecAssignment = 7,
        Expr = 8,
        Atom = 9,
        Function = 10,
        Parameters = 11,
        CodeBlock = 12,
        Break = 13,
        Switch = 14,
        SwitchCase = 15,
        Statement = 16,
        If = 17,
        Condition = 18,
        CondLogExpr = 19,
        CondExpr = 20,
        Else = 21,
        IfForLoopBlock = 22,
        For = 23,
        ForDeclaration = 24,
        ForAssignment = 25,
        Increment = 26,
        While = 27,
        DoWhile = 28,
        WhileLoopBlock = 29,
        Printf = 30,
        Scanf = 31,
        Return = 32,


        DIRECTIVE = 33,
        DATATYPE = 34,
        INCLUDE = 35,
        SWITCH = 36,
        CASE = 37,
        IF = 38,
        ELSE = 39,
        FOR = 40,
        WHILE = 41,
        DEFAULT = 42,
        RETURN = 43,
        SCANF = 44,
        PRINTF = 45,
        DO = 46,
        BREAK = 47,
        TYPESPEC = 48,
        REFOPER = 49,
        IDENTIFIER = 50,
        ARTOPERATOR = 51,
        NUMBER = 52,
        COMMA = 53,
        EQUALS = 54,
        SEMICOL = 55,
        COLON = 56,
        SHARP = 57,
        EOF = 58,
        LPAREN = 59,
        RPAREN = 60,
        LBRACE = 61,
        RBRACE = 62,
        RELOP = 63,
        LOGOP = 64,
        INCRE = 65,
        BOOL = 66,
        NOT = 67,
        NULL = 68,
        STRING = 69,
        WHITESPACE = 70,
        EOL = 71,
        COMMENTLINE = 72,
        COMMENTBLOCK = 73
    }

    public class Token
    {
        private int startpos;
        private int endpos;
        private int linepos;
        private int columnpos;
        private string text;
        private object value;

        public int LinePos
        {
            get { return linepos; }
            set { linepos = value; }
        }

        public int ColumnPos
        {
            get { return columnpos; }
            set { columnpos = value; }
        }

        public int StartPos
        {
            get { return startpos; }
            set { startpos = value; }
        }

        public int Length
        {
            get { return endpos - startpos; }
        }

        public int EndPos
        {
            get { return endpos; }
            set { endpos = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public TokenType Type;

        public Token()
            : this(0, 0, 0, 0)
        {
        }

        public Token(int start, int end, int line, int column)
        {
            Type = TokenType._UNDETERMINED_;
            startpos = start;
            linepos = line;
            columnpos = column;
            endpos = end;
            Text = "";
            Value = null;
        }

        public void UpdateRange(Token token)
        {
            if (token.StartPos < startpos) startpos = token.StartPos;
            if (token.EndPos > endpos) endpos = token.EndPos;
        }

        public override string ToString()
        {
            if (Text != null)
                return Type.ToString() + " '" + Text + "'";
            else
                return Type.ToString();
        }
    }

    #endregion
}

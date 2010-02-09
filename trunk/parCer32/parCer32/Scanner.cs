using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace parCer32
{
    #region Scanner

    public partial class Scanner
    {
        public string Input;
        public int StartPos = 0;
        public int LastPos = 0;
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

            regex = new Regex(@"void", RegexOptions.Compiled);
            Patterns.Add(TokenType.VOID, regex);
            Tokens.Add(TokenType.VOID);

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

            regex = new Regex(@"([a-zA-z_]|[0-9_])", RegexOptions.Compiled);
            Patterns.Add(TokenType.CHARIDENT, regex);
            Tokens.Add(TokenType.CHARIDENT);

            regex = new Regex(@"([a-zA-Z_][a-zA-Z_]|[0-9_][0-9_])|([a-zA-Z_][0-9_])|([0-9_][a-zA-Z_])", RegexOptions.Compiled);
            Patterns.Add(TokenType.CHARIDENT2, regex);
            Tokens.Add(TokenType.CHARIDENT2);

            regex = new Regex(@"\+|-|%|/|\*", RegexOptions.Compiled);
            Patterns.Add(TokenType.ARTOPERATOR, regex);
            Tokens.Add(TokenType.ARTOPERATOR);

            regex = new Regex(@"[a-zA-Z_]*", RegexOptions.Compiled);
            Patterns.Add(TokenType.CASECHAR, regex);
            Tokens.Add(TokenType.CASECHAR);

            regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Patterns.Add(TokenType.NUMBER, regex);
            Tokens.Add(TokenType.NUMBER);

            regex = new Regex(@"\'", RegexOptions.Compiled);
            Patterns.Add(TokenType.QUOT, regex);
            Tokens.Add(TokenType.QUOT);

            regex = new Regex(@"\""", RegexOptions.Compiled);
            Patterns.Add(TokenType.DQUOT, regex);
            Tokens.Add(TokenType.DQUOT);

            regex = new Regex(@"\*", RegexOptions.Compiled);
            Patterns.Add(TokenType.POINTER, regex);
            Tokens.Add(TokenType.POINTER);

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

            regex = new Regex(@"\[", RegexOptions.Compiled);
            Patterns.Add(TokenType.LARRAY, regex);
            Tokens.Add(TokenType.LARRAY);

            regex = new Regex(@"\]", RegexOptions.Compiled);
            Patterns.Add(TokenType.RARRAY, regex);
            Tokens.Add(TokenType.RARRAY);

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
                    if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len )))
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
                       
                    }
                }
                else
                {
                    if (tok.StartPos < tok.EndPos - 1)
                        tok.Text = Input.Substring(tok.StartPos, 1);
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

            //Non terminal tokens:
            _NONE_  = 0,
            _UNDETERMINED_= 1,

            //Non terminal tokens:
            Start   = 2,
            Header  = 3,
            Declaration= 4,
            Vreturn = 5,
            Array   = 6,
            ArAssignment= 7,
            ArContent= 8,
            VarArray= 9,
            InsideDeclaration= 10,
            Assignment= 11,
            DecAssignment= 12,
            Expr    = 13,
            Char    = 14,
            Atom    = 15,
            Function= 16,
            Parameters= 17,
            ParArray= 18,
            CodeBlock= 19,
            Break   = 20,
            Switch  = 21,
            SwitchCase= 22,
            CaseComp= 23,
            Statement= 24,
            If      = 25,
            Condition= 26,
            CondLogExpr= 27,
            CondExpr= 28,
            Else    = 29,
            IfForLoopBlock= 30,
            For     = 31,
            ForDeclaration= 32,
            ForAssignment= 33,
            Increment= 34,
            While   = 35,
            DoWhile = 36,
            WhileLoopBlock= 37,
            Printf  = 38,
            Scanf   = 39,
            Return  = 40,

            //Terminal tokens:
            DIRECTIVE= 41,
            DATATYPE= 42,
            INCLUDE = 43,
            VOID    = 44,
            SWITCH  = 45,
            CASE    = 46,
            IF      = 47,
            ELSE    = 48,
            FOR     = 49,
            WHILE   = 50,
            DEFAULT = 51,
            RETURN  = 52,
            SCANF   = 53,
            PRINTF  = 54,
            DO      = 55,
            BREAK   = 56,
            TYPESPEC= 57,
            REFOPER = 58,
            IDENTIFIER= 59,
            CHARIDENT= 60,
            CHARIDENT2= 61,
            ARTOPERATOR= 62,
            CASECHAR= 63,
            NUMBER  = 64,
            QUOT    = 65,
            DQUOT   = 66,
            POINTER = 67,
            COMMA   = 68,
            EQUALS  = 69,
            SEMICOL = 70,
            COLON   = 71,
            SHARP   = 72,
            EOF     = 73,
            LPAREN  = 74,
            RPAREN  = 75,
            LBRACE  = 76,
            RBRACE  = 77,
            LARRAY  = 78,
            RARRAY  = 79,
            RELOP   = 80,
            LOGOP   = 81,
            INCRE   = 82,
            BOOL    = 83,
            NOT     = 84,
            NULL    = 85,
            STRING  = 86,
            WHITESPACE= 87,
            EOL     = 88,
            COMMENTLINE= 89,
            COMMENTBLOCK= 90
    }

    public class Token
    {
        private int startpos;
        private int endpos;
        private string text;
        private int linepos;
        private int columnpos;
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

        public int StartPos { 
            get { return startpos;} 
            set { startpos = value; }
        }

        public int Length { 
            get { return endpos - startpos;} 
        }

        public int EndPos { 
            get { return endpos;} 
            set { endpos = value; }
        }

        public string Text { 
            get { return text;} 
            set { text = value; }
        }

        public object Value { 
            get { return value;} 
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

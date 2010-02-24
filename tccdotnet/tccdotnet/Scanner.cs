using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace tccdotnet
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
        public List<Token> Skipped; // tokens that were skipped
        public Dictionary<TokenType, Regex> Patterns;

        private Token LookAheadToken;
        private List<TokenType> Tokens;
        private List<TokenType> SkipList; // tokens to be skipped

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

            regex = new Regex(@"([a-zA-Z_]|[0-9_])", RegexOptions.Compiled);
            Patterns.Add(TokenType.CHARVALUE, regex);
            Tokens.Add(TokenType.CHARVALUE);

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

            regex = new Regex(@"\.\.\.", RegexOptions.Compiled);
            Patterns.Add(TokenType.VARIABLEPARAMS, regex);
            Tokens.Add(TokenType.VARIABLEPARAMS);

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
            Token t = new Token(this.StartPos, this.EndPos,this.CurrentLine, this.CurrentColumn);
            t.Type = type;
            return t;
        }

         /// <summary>
        /// executes a lookahead of the next token
        /// and will advance the scan on the input string
        /// </summary>
        /// <returns></returns>
        public Token Scan(params TokenType[] expectedtokens)
        {
            Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
            LookAheadToken = null; // reset lookahead token, so scanning will continue
            StartPos = tok.EndPos;
            EndPos = tok.EndPos; // set the tokenizer to the new scan position
            return tok;
        }

        /// <summary>
        /// returns token with longest best match
        /// </summary>
        /// <returns></returns>
        public Token LookAhead(params TokenType[] expectedtokens)
        {
            int i;
            int startpos = StartPos;
            Token tok = null;
            List<TokenType> scantokens;


            // this prevents double scanning and matching
            // increased performance
            if (LookAheadToken != null 
                && LookAheadToken.Type != TokenType._UNDETERMINED_ 
                && LookAheadToken.Type != TokenType._NONE_) return LookAheadToken;

            // if no scantokens specified, then scan for all of them (= backward compatible)
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
            GlobalDecl= 4,
            Array   = 5,
            ArAssignment= 6,
            ArContent= 7,
            VarArray= 8,
            LocalDecl= 9,
            Assignment= 10,
            DecAssignment= 11,
            Expr    = 12,
            Char    = 13,
            Atom    = 14,
            Function= 15,
            Parameters= 16,
            ParArray= 17,
            CodeBlock= 18,
            Break   = 19,
            Switch  = 20,
            SwitchCase= 21,
            CaseComp= 22,
            Statement= 23,
            If      = 24,
            Condition= 25,
            CondLogExpr= 26,
            CondExpr= 27,
            Else    = 28,
            IfForLoopBlock= 29,
            For     = 30,
            ForDeclaration= 31,
            ForAssignment= 32,
            Increment= 33,
            While   = 34,
            DoWhile = 35,
            WhileLoopBlock= 36,
            Printf  = 37,
            Scanf   = 38,
            Return  = 39,

            //Terminal tokens:
            DIRECTIVE= 40,
            DATATYPE= 41,
            INCLUDE = 42,
            VOID    = 43,
            SWITCH  = 44,
            CASE    = 45,
            IF      = 46,
            ELSE    = 47,
            FOR     = 48,
            WHILE   = 49,
            DEFAULT = 50,
            RETURN  = 51,
            SCANF   = 52,
            PRINTF  = 53,
            DO      = 54,
            BREAK   = 55,
            TYPESPEC= 56,
            REFOPER = 57,
            IDENTIFIER= 58,
            CHARVALUE= 59,
            ARTOPERATOR= 60,
            CASECHAR= 61,
            NUMBER  = 62,
            QUOT    = 63,
            DQUOT   = 64,
            POINTER = 65,
            COMMA   = 66,
            EQUALS  = 67,
            SEMICOL = 68,
            COLON   = 69,
            SHARP   = 70,
            EOF     = 71,
            LPAREN  = 72,
            RPAREN  = 73,
            LBRACE  = 74,
            RBRACE  = 75,
            LARRAY  = 76,
            RARRAY  = 77,
            RELOP   = 78,
            LOGOP   = 79,
            INCRE   = 80,
            BOOL    = 81,
            NOT     = 82,
            NULL    = 83,
            STRING  = 84,
            VARIABLEPARAMS= 85,
            WHITESPACE= 86,
            EOL     = 87,
            COMMENTLINE= 88,
            COMMENTBLOCK= 89
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
            endpos = end;
            linepos = line;
            columnpos = column;
            Text = ""; // must initialize with empty string, may cause null reference exceptions otherwise
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

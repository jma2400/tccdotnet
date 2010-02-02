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

            regex = new Regex(@"float|double|u?int|u?longlong|u?long|u?short|char", RegexOptions.Compiled);
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

            regex = new Regex(@"[a-zA-Z_]|[0-9_]", RegexOptions.Compiled);
            Patterns.Add(TokenType.CHARIDENT, regex);
            Tokens.Add(TokenType.CHARIDENT);

            regex = new Regex(@"\+|-|%|/|\*", RegexOptions.Compiled);
            Patterns.Add(TokenType.ARTOPERATOR, regex);
            Tokens.Add(TokenType.ARTOPERATOR);

            regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Patterns.Add(TokenType.NUMBER, regex);
            Tokens.Add(TokenType.NUMBER);

            regex = new Regex(@"\'", RegexOptions.Compiled);
            Patterns.Add(TokenType.QUOT, regex);
            Tokens.Add(TokenType.QUOT);

            regex = new Regex(@"\""", RegexOptions.Compiled);
            Patterns.Add(TokenType.DQUOT, regex);
            Tokens.Add(TokenType.DQUOT);

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
            Token t = new Token(this.StartPos, this.EndPos);
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

                tok = new Token(startpos, EndPos);

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


            _NONE_  = 0,
            _UNDETERMINED_= 1,

            Start   = 2,
            Header  = 3,
            Declaration= 4,
            InsideDeclaration= 5,
            Assignment= 6,
            DecAssignment= 7,
            Expr    = 8,
            Char    = 9,
            Atom    = 10,
            Function= 11,
            Parameters= 12,
            CodeBlock= 13,
            Break   = 14,
            Switch  = 15,
            SwitchCase= 16,
            Statement= 17,
            If      = 18,
            Condition= 19,
            CondLogExpr= 20,
            CondExpr= 21,
            Else    = 22,
            IfForLoopBlock= 23,
            For     = 24,
            ForDeclaration= 25,
            ForAssignment= 26,
            Increment= 27,
            While   = 28,
            DoWhile = 29,
            WhileLoopBlock= 30,
            Printf  = 31,
            Scanf   = 32,
            Return  = 33,


            DIRECTIVE= 34,
            DATATYPE= 35,
            INCLUDE = 36,
            VOID    = 37,
            SWITCH  = 38,
            CASE    = 39,
            IF      = 40,
            ELSE    = 41,
            FOR     = 42,
            WHILE   = 43,
            DEFAULT = 44,
            RETURN  = 45,
            SCANF   = 46,
            PRINTF  = 47,
            DO      = 48,
            BREAK   = 49,
            TYPESPEC= 50,
            REFOPER = 51,
            IDENTIFIER= 52,
            CHARIDENT= 53,
            ARTOPERATOR= 54,
            NUMBER  = 55,
            QUOT    = 56,
            DQUOT   = 57,
            COMMA   = 58,
            EQUALS  = 59,
            SEMICOL = 60,
            COLON   = 61,
            SHARP   = 62,
            EOF     = 63,
            LPAREN  = 64,
            RPAREN  = 65,
            LBRACE  = 66,
            RBRACE  = 67,
            RELOP   = 68,
            LOGOP   = 69,
            INCRE   = 70,
            BOOL    = 71,
            NOT     = 72,
            NULL    = 73,
            STRING  = 74,
            WHITESPACE= 75,
            EOL     = 76,
            COMMENTLINE= 77,
            COMMENTBLOCK= 78
    }

    public class Token
    {
        private int startpos;
        private int endpos;
        private string text;
        private object value;

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
            : this(0, 0)
        {
        }

        public Token(int start, int end)
        {
            Type = TokenType._UNDETERMINED_;
            startpos = start;
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

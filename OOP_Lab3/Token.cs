using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Lab3
{
    class Token
    {
        public string Content { get; }
        public TokenType Type { get; }

        public Token(string content, TokenType type)
        {
            Content = content;
            Type = type;
        }
    }
}

using FormsParse.Models;
using System.Xml.Linq;

namespace FormsParse
{
    public class FormParser
    {
        private FormRow _currentGroup = new();
        private FormItem _currentItem;

        private readonly List<FormRow> _groups = new();
        private int _pos = -1;
        private string _input = "";

        bool CanMoveNext => _input.Length > 0 && _pos < _input.Length;
        bool IsNewLine => _input[_pos] is '\n' || _input[_pos] is '\r';
        bool IsTagSeparator => _input[_pos] is ',';
        bool IsColumnSeparator => _input[_pos] is '|';
        bool IsTagClose => _input[_pos] is ')';
        bool IsRowBreak => _input[_pos] is '-' && CanMoveNext && Peek() is '-';
        bool IsCompoundTag => _input[_pos] is '#' && CanMoveNext && Peek() is '(';
        bool IsWhitespace => _input[_pos] is ' ';

        private char Peek()
        {
            return _input[_pos + 1];
        }

        public FormDefinition Parse(string configuration)
        {
            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentException($"'{nameof(configuration)}' cannot be null or empty.", nameof(configuration));
            }

            _input = configuration;
            _pos = 0;
            _groups.Add(_currentGroup);

            while (CanMoveNext)
            {
                Next();
            }

            return new FormDefinition(_groups);
        }

        void Next()
        {
            if (IsNewLine)
            {
                _pos++;
            }
            else if(IsRowBreak)
            {
                ParseSectionBreak();
            }
            else if(IsColumnSeparator)
            {
                ParseColumnSeparator();
            }
            else 
            {
                ParseTag();
            }
        }

        private void ParseSectionBreak()
        {
            _pos++;
            _pos++;
            _currentGroup = new();
            _groups.Add(_currentGroup);
        }

        private void ParseColumnSeparator()
        {
            if(IsColumnSeparator)
            {
                _currentGroup.AddColumn();
                _pos++;
            }
        }

        void ParseTag()
        {
            if(IsCompoundTag)
            {
                ParseCompoundTag();
            }
            else
            {
                _currentItem = new Button
                {
                    Name = ParseTagName()
                };
                _currentGroup.AddItem(_currentItem);
            }
        }


        // (name[, color='Blue', type='Button']))
        void ParseCompoundTag()
        {
            ParseTagOpen();
            string name = ParseTagName(true);
            ParseTagSeparator();
            string color = ParseTagColor();
            ParseTagSeparator();
            string type = ParseTagType();
            ParseTagClose();

            if(type == "Button")
            {
                _currentItem = new Button();
            }
            else
            {
                _currentItem = new Label();
            }

            _currentItem.Name = name;
            _currentItem.Color = color;     

            _currentGroup.AddItem(_currentItem);
        }

        void ParseTagOpen()
        {
            // #(
            if(IsCompoundTag)
            {
                _pos++;
                _pos++;
            }
        }

        void ParseTagClose()
        {
            // )
            if(IsTagClose) _pos++;
        }

        string ParseTagName(bool isCompound = false)
        {
            ParseWhitespace();

            var token = "";
            
            while (CanMoveNext && !IsNewLine)
            {
                if (isCompound && IsTagSeparator || IsTagClose)
                    break;

                token += _input[_pos++];
            }

            return token;
        }

        void ParseTagSeparator()
        {
            // ,
            ParseWhitespace();
            if(IsTagSeparator) _pos++;
            ParseWhitespace();
        }

        private void ParseWhitespace()
        {
            while (CanMoveNext && IsWhitespace)
            {
                _pos++;
            }
        }

        string ParseTagColor()
        {
            var token = "";
            while (CanMoveNext && !IsNewLine)
            {
                if (IsTagSeparator || IsTagClose)
                    break;

                token += _input[_pos++];
            }

            return KnownColors.IsKnownColor(token) ? token : KnownColors.Default;
        }

        string ParseTagType()
        {
            var token = "";
            while (CanMoveNext && !IsNewLine)
            {
                if (IsTagSeparator || IsTagClose)
                    break;

                token += _input[_pos++];
            }

            return token == "Label" ? "Label" : "Button";
        }
    }
    
}
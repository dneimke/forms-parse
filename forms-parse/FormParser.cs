using FormsParse.Models;

namespace FormsParse
{

    public class FormParser
    {
        private FormGroup _currentGroup = new();
        private FormItem _currentItem = new();

        private List<FormGroup> _groups = new();
        private int _pos = -1;
        private string _input = "";

        bool CanMoveNext => _input.Length > 0 && _pos < _input.Length;
        bool IsNewLine => _input[_pos] is '\n' || _input[_pos] is '\r';
        bool IsTagSeparator => _input[_pos] is ',';
        bool IsColumnSeparator => _input[_pos] is '|';
        bool IsTagClose => _input[_pos] is ')';
        bool IsSectionBreak => _input[_pos] is '-' && CanMoveNext && Peek() is '-';
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
                // Console.WriteLine($"Looping: {_pos}");

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
            else if(IsSectionBreak)
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
            _currentGroup.AddColumn();
            _pos++;
        }

        void ParseTag()
        {
            _currentItem = new();

            if(IsCompoundTag)
            {
                _currentItem = new();
                _currentGroup.AddItem(_currentItem);

                ParseCompoundTag();
            }
            else
            {
                _currentItem = new();
                _currentGroup.AddItem(_currentItem);

                ParseTagName();
            }
        }


        void ParseCompoundTag()
        {
            ParseTagOpen();
            ParseTagName(true);
            ParseTagSeparator();
            ParseTagColor();
            ParseTagClose();
        }

        void ParseTagOpen()
        {
            // #(
            _pos++;
            _pos++;
        }

        void ParseTagClose()
        {
            // )
            _pos++;
        }

        void ParseTagName(bool isCompound = false)
        {
            var token = "";
            while (CanMoveNext && !IsNewLine)
            {
                if (isCompound && IsTagSeparator || IsTagClose)
                    break;

                token += _input[_pos++];
            }

            if (!string.IsNullOrEmpty(token))
            {
                _currentItem.Name = token;
            }
        }

        void ParseTagSeparator()
        {
            // ,
            ParseWhitespace();
            _pos++;
            ParseWhitespace();
        }

        private void ParseWhitespace()
        {
            while (CanMoveNext && IsWhitespace)
            {
                _pos++;
            }
        }

        void ParseTagColor()
        {
            var token = "";
            while (CanMoveNext && !IsNewLine)
            {
                if (IsTagSeparator || IsTagClose)
                    break;

                token += _input[_pos++];
            }

            if (!string.IsNullOrEmpty(token))
            {
                _currentItem.Color = KnownColors.IsKnownColor(token) ? token : KnownColors.Default;
            }
        }
    }
    
}
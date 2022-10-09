using FormsParse.Models;
using System.Drawing;
using System.Xml.Linq;

namespace FormsParse
{
    public class FormParser
    {
        private FormRow _currentGroup = new();
        private List<Connection> _connections = new();
        private FormItem _currentItem;

        private readonly List<FormRow> _groups = new();
        private int _pos = -1;
        private string _input = "";

        bool CanMoveNext => _input.Length > 0 && _pos < _input.Length;
        bool IsNewLine => _input[_pos] is '\n' || _input[_pos] is '\r';
        bool IsColumnSeparator => _input[_pos] is '|';
        bool IsTagClose => _input[_pos] is ')';
        bool IsRowBreak => _input[_pos] is '-' && CanMoveNext && Peek() is '-';
        bool IsCompoundTag => _input[_pos] is '#' && CanMoveNext && Peek() is '(';
        bool IsProcessingInstruction => _input[_pos] is '-';
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

            return new FormDefinition(_groups, _connections);
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
            else if (IsProcessingInstruction)
            {
                var token = ParseProcessingInstruction();
                switch (token.ToLower()) 
                {
                    case "connections":
                        ParseConnections();
                        break;
                    default:
                        throw new ApplicationException($"Unknown token: {token}");
                }
            }
            else 
            {
                ParseTag();
            }
        }

        void ParseConnections()
        {
            ParseWhitespace();

            while (CanMoveNext)
            {
                ParseConnection();
                ParseWhitespace();
            }
        }

        private void ParseSectionBreak()
        {
            _pos++;
            _pos++;
            _currentGroup = new();
            _groups.Add(_currentGroup);
        }

        private string ParseProcessingInstruction()
        {
            _pos++;
            ParseWhitespace();

            var token = "";

            while (CanMoveNext && !IsNewLine)
            {
                token += _input[_pos++];
            }

            _pos++;
            return token;
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
                _currentItem = new Button(new())
                {
                    Name = ParseTagName()
                };
                _currentGroup.AddItem(_currentItem);
            }
        }


        // #(name[, color='Blue', type='Button']))
        void ParseCompoundTag()
        {
            ParseTagOpen();
            var attributeString = ParseTagAttributes();
            ParseTagClose();

            var attributes = AttributeParser.Parse(attributeString);
            attributes.TryGetValue("type", out var type);
            attributes.TryGetValue("name", out var name);
            attributes.TryGetValue("color", out var color);

            _currentItem = type?.ToLower() switch
            {
                "button" => new Button(attributes),
                "label" => new Label(attributes),
                "switch" => new Switch(attributes),
                _ => new Button(attributes)
            };
                
            _currentItem.Name = name ?? throw new ApplicationException("Name attribute not present in collection");
            _currentItem.Color = color ?? KnownColors.Default;     

            _currentGroup.AddItem(_currentItem);
        }

        // #(type: deactivate, source: 1, target: 2)
        void ParseConnection()
        {
            ParseTagOpen();
            var attributeString = ParseTagAttributes();
            ParseTagClose();

            var attributes = AttributeParser.Parse(attributeString);
            attributes.TryGetValue("type", out var type);
            attributes.TryGetValue("source", out var source);
            attributes.TryGetValue("target", out var target);

            Connection connection = type?.ToLower() switch
            {
                "deactivate" => new DeactivateConnection(attributes),
                "activate" => new ActivateConnection(attributes),
                _ => throw new ApplicationException($"Unknown connection type: {type}")
            };

            connection.Source = source ?? throw new ApplicationException("A source for the connection was not specified");
            connection.Target = target ?? throw new ApplicationException("A target for the connection was not specified");

            _connections.Add(connection);
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
               token += _input[_pos++];
            }

            return token;
        }

        string ParseTagAttributes()
        {
            ParseWhitespace();

            var token = "";

            while (CanMoveNext && !IsTagClose && !IsNewLine)
            {
                token += _input[_pos++];
            }

            return token;
        }

        private void ParseWhitespace()
        {
            while (CanMoveNext && (IsWhitespace || IsNewLine))
            {
                _pos++;
            }
        }
    }
    
}
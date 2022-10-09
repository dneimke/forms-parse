using System.Formats.Asn1;

namespace FormsParse.Models
{
    public static class KnownColors
    {
        public const string Red = nameof(Red);
        public const string Blue = nameof(Blue);
        public const string Gray = nameof(Gray);
        public const string Black = nameof(Black);
        public const string Green = nameof(Green);

        public const string Default = Blue;

        public static bool IsKnownColor(string color) => color switch
        {
            nameof(Red) => true,
            nameof(Green) => true,
            nameof(Blue) => true,
            nameof(Gray) => true,
            nameof(Black) => true,
            _ => false
        };
    }

    public class FormDefinition
    {
        private FormRow _currentRow;

        public FormRow CurrentRow => _currentRow;

        public List<Connection> Connections { get; set; } = new();


        public FormDefinition()
        {
            _currentRow = new();
            Rows.Add(_currentRow);
        }

        public FormDefinition(List<FormRow> groups) : this(groups, new())
        {
            
        }

        public FormDefinition(List<FormRow> groups, List<Connection> connections)
        {
            Rows = groups ?? throw new ArgumentNullException(nameof(groups));
            Connections = connections; 

            if (groups.Any())
            {
                _currentRow = groups.Last();
            }
            else
            {
                _currentRow = new();
                Rows.Add(_currentRow);
            }
        }

        public int RowCount => Rows.Count;
        public List<FormItem> AllItems => Rows.SelectMany(x => x.Columns)
                    .SelectMany(x => x.Items)
                    .ToList();
        public int TotalItemCount => AllItems.Count;
        public List<FormRow> Rows { get; private set; } = new();
    }

    public abstract class FormItem
    {
        private Dictionary<string, string> _attributes;

        protected FormItem(Dictionary<string, string> attributes)
        {
            _attributes = attributes;
        }

        public Dictionary<string, string> Attributes => _attributes;
        public string Type { get; protected set; } = "";
        public string Name { get; set; } = "";
        public string Color { get; set; } = KnownColors.Default;
        public string? Tag => _attributes.Where(x => x.Key.ToLower() == "tag").Select(x => x.Value).FirstOrDefault();
    }

    public abstract class Connection
    {
        private Dictionary<string, string> _attributes;

        protected Connection(Dictionary<string, string> attributes)
        {
            _attributes = attributes;
        }

        public Dictionary<string, string> Attributes => _attributes;
        public string Type { get; protected set; } = "";
        public string Source { get; set; } = "";
        public string Target { get; set; } = "";
    }

    public class Button : FormItem 
    {
        public Button(Dictionary<string, string> attributes) : base(attributes)
        {
            Type = nameof(Button);
        }
    }

    public class Label : FormItem 
    {
        public Label(Dictionary<string, string> attributes) : base(attributes)
        {
            Type = nameof(Label);
        }
    }

    public class Switch : FormItem
    {
        public Switch(Dictionary<string, string> attributes) : base(attributes)
        {
            Type = nameof(Switch);
        }
    }

    public class ActivateConnection : Connection
    {
        public ActivateConnection(Dictionary<string, string> attributes) : base(attributes)
        {
            Type = nameof(ActivateConnection);
        }
    }

    public class DeactivateConnection : Connection
    {
        public DeactivateConnection(Dictionary<string, string> attributes) : base(attributes)
        {
            Type = nameof(DeactivateConnection);
        }
    }

    public class FormColumn
    {
        public List<FormItem> Items { get; } = new();
    }

    public class FormRow
    {
        public List<FormColumn> Columns { get; } = new();

        public FormRow()
        {
            AddColumn();
        }

        internal void AddColumn()
        {
            Columns.Add(new());
        }

        internal void AddItem(FormItem item, int? columnIndex = null)
        {
            if(columnIndex.HasValue && columnIndex >= Columns.Count)
            {
                throw new ArgumentOutOfRangeException("Position exceeds length of columns.");
            }

            if(!columnIndex.HasValue)
                columnIndex = Columns.Count - 1;

            Columns[columnIndex.Value].Items.Add(item);
        }
    }
}


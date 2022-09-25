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


        public FormDefinition()
        {
            _currentRow = new();
            Rows.Add(_currentRow);
        }

        public FormDefinition(List<FormRow> groups)
        {
            Rows = groups ?? throw new ArgumentNullException(nameof(groups));

            if(groups.Any()) { 
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
        public string Type { get; protected set; } = "";
        public string Name { get; set; } = "";
        public string Color { get; set; } = KnownColors.Default;
    }

    public class Button : FormItem 
    {
        public Button()
        {
            Type = nameof(Button);
        }
    }

    public class Label : FormItem 
    {
        public Label()
        {
            Type = nameof(Label);
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


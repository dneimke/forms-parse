namespace FormsParse.Models
{
    internal static class KnownColors
    {
        public const string Red = nameof(Red);
        public const string Blue = nameof(Blue);
        public const string Gray = nameof(Gray);
        public const string Black = nameof(Black);
        public const string Green = nameof(Green);

        public const string Default = Blue;

        internal static bool IsKnownColor(string color) => color switch
        {
            nameof(Red) => true,
            nameof(Green) => true,
            nameof(Blue) => true,
            nameof(Gray) => true,
            nameof(Black) => true,
            _ => false
        };

        //        internal static string Parse(string color) => color switch
        //{
        //            IsKnownColor(color) => color,
        //            _ => Default
        //        };
    }

    public class FormDefinition
    {
        private FormGroup _currentGroup;

        public FormGroup CurrentGroup => _currentGroup;


        public FormDefinition()
        {
            _currentGroup = new();
            Groups.Add(_currentGroup);
        }

        public FormDefinition(List<FormGroup> groups)
        {
            Groups = groups ?? throw new ArgumentNullException(nameof(groups));

            if(groups.Any()) { 
                _currentGroup = groups.Last();
            }
            else
            {
                _currentGroup = new();
                Groups.Add(_currentGroup);
            }
        }

        public int GroupCount => Groups.Count;
        public List<FormItem> AllItems => Groups.SelectMany(x => x.Columns)
                    .SelectMany(x => x.Items)
                    .ToList();
        public int TotalItemCount => AllItems.Count;
        public List<FormGroup> Groups { get; private set; } = new();

        void NewGroup()
        {
            _currentGroup = new();
            Groups.Add(_currentGroup);
        }
    }

    public class FormItem
    {
        public string Name { get; set; } = "";
        public string Color { get; set; } = KnownColors.Default;
    }

    public class FormColumn
    {
        public List<FormItem> Items { get; } = new();
    }

    public class FormGroup
    {
        public List<FormColumn> Columns { get; } = new();

        public FormGroup()
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


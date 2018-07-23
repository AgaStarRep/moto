using System;

namespace Motohusaria.DTO
{
    public class OptionsResponse
{
    public OptionsItem[] Results { get; set; }

    public OptionsPagination Pagination { get; set; }

    public class OptionsItem
    {
        public string Id { get; set; }

        public string Text { get; set; }
    }

    public class OptionsPagination
    {
        public bool More { get; set; }
    }
}
}

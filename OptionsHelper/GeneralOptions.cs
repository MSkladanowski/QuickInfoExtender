using System.ComponentModel;

namespace OptionsHelper
{
    public class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        [DefaultValue(@"https://www.google.com/search?q=")]
        private string _searchUrl;

        [Category("Base options")]
        [DisplayName("Search engine url")]
        [Description("Live empty if you don't want to see button")]
        [DefaultValue(@"https://www.google.com/search?q=")]
        public string SearchUrl
        {
            get { return _searchUrl; }
            set { _searchUrl = value; }
        }
    }
}

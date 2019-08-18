using System.ComponentModel;

namespace OptionsHelper
{
    public class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        private string searchUrl;

        [Category("Base options")]
        [DisplayName("Search engine url")]
        [Description("Live empty if you don't want to see button")]
        [DefaultValue(@"https://www.google.com/search?q=")]
        public string SearchUrl
        {
            get
            {
                return (string.IsNullOrEmpty(searchUrl) ? "https://www.google.com/search?q=" : searchUrl).Replace(" ","");
            }
            set
            {
                searchUrl = value;
            }
        }

        [Category("Base options")]
        [DisplayName("Show button \"Search in browser")]
        [Description("")]
        public bool ShowButtonToBrowser { get; set; }

    }
}

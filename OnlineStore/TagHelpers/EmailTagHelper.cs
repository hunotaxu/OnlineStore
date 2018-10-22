using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OnlineStore.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        private const string RecipientAttributeName = "mail-to";
        private const string DomainAttributeName = "email-domain";

        [HtmlAttributeName(RecipientAttributeName)]
        public string MailTo { get; set; }

        [HtmlAttributeName(DomainAttributeName)]
        public string EmailDomain { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var emailTarget = $"{MailTo}@{EmailDomain}";
            output.Attributes.SetAttribute("href", $"mailto:{emailTarget}");
            output.Content.SetContent(emailTarget);
        }
    }
}

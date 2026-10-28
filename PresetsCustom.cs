using MimeKit;
using uwap.WebFramework;
using uwap.WebFramework.Accounts;
using uwap.WebFramework.Localization;
using uwap.WebFramework.Mail;
using uwap.WebFramework.Plugins;
using uwap.WebFramework.Responses.DefaultUI;

namespace uwap.Website;

public class PresetsCustom : PresetsHandler
{
    public static readonly UsersPlugin UsersPlugin = new()
    {
        DefaultAccent = "violet",
        DefaultBackground = "dark",
        DefaultDesign = "layers",
        DefaultFont = "ubuntu"
    };

    protected override string SupportEmail => "support@uwap.org";

    public override async Task<MailSendResult> WarningMailAsync(Request req, User user, string subject, string text, string? useThisAddress = null)
    {
        return (await MailManager.Out.SendAsync(
            new MailboxAddress("uwap.org Support", "support@uwap.org"),
            new MailboxAddress(user.Username, useThisAddress ?? user.MailAddress),
            $"[uwap.org] {subject}",
            $"{WebsiteTexts.Hello}, {user.Username}!\n{text}\n\n{WebsiteTexts.WarningFooter}".Replace("\n", "<br />"),
            true)).Result;
    }

    public override string Favicon(Request req)
        => "/icons/u.ico";

    public override void Navigation(Request request, WebFramework.Elements.Page page)
    {
        if (request.Domain == "uwap.org" || request.Domains.Contains("uwap.org"))
            if (request.LoggedIn)
                page.Navigation =
                [
                    new WebFramework.Elements.Button(WebsiteTexts.UwapDotOrg, "/"),
                    Presets.AccountButton(request),
                    new WebFramework.Elements.Button(WebsiteTexts.About, "/about", "right"),
                    new WebFramework.Elements.Button(WebsiteTexts.Projects, "/projects"),
                    new WebFramework.Elements.Button(WebsiteTexts.Guides, "/guides"),
                    new WebFramework.Elements.Button(WebsiteTexts.Notes, "https://notes.uwap.org"),
                    new WebFramework.Elements.Button(WebsiteTexts.Files, "https://files.uwap.org"),
                    new WebFramework.Elements.Button(WebsiteTexts.Mail, "https://mail.uwap.org")
                ];
            else page.Navigation =
                [
                    new WebFramework.Elements.Button(WebsiteTexts.UwapDotOrg, "/"),
                    Presets.AccountButton(request),
                    new WebFramework.Elements.Button(WebsiteTexts.About, "/about", "right"),
                    new WebFramework.Elements.Button(WebsiteTexts.Projects, "/projects"),
                    new WebFramework.Elements.Button(WebsiteTexts.Guides, "/guides")
                ];
        else page.Navigation =
            [
                new WebFramework.Elements.Button(WebsiteTexts.UwapDotOrg, "https://uwap.org"),
                new WebFramework.Elements.Button(CommonTexts.Menu, "/")
            ];
    }

    public override List<IStyle> Styles(Request req, out string fontUrl)
        => [UsersPlugin.GetStyle(req, out fontUrl, UsersPluginPathValue)];

    public override string UsersPluginPath(Request req)
        => UsersPluginPathValue;

    public static string UsersPluginPathValue { get; set; } = "/account";

    public override void ModifyPage(Request req, Page page)
    {
        page.Favicon = new(req, "/icons/u.ico");
        
        if (req.Domain == "uwap.org" || req.Domains.Contains("uwap.org"))
        {
            page.Menus.Add(new Menu("wf-menu", CommonTexts.Menu, [
                ..AuthButtons(req),
                new LinkButton(WebsiteTexts.About, "/about"),
                new LinkButton(WebsiteTexts.Projects, "/projects"),
                new LinkButton(WebsiteTexts.Guides, "/guides"),
                ..AppButtons(req)
            ]));
            page.NavBar.Islands.Add(new([new LinkButton(WebsiteTexts.UwapDotOrg, "/")]));
        }
        else
        {
            page.Menus.Add(new Menu("wf-menu", CommonTexts.Menu, [
                ..AuthButtons(req),
                new LinkButton(CommonTexts.Home, "/")
            ]));
            page.NavBar.Islands.Add(new([new LinkButton(WebsiteTexts.UwapDotOrg, "https://uwap.org")]));
        }
        
        page.NavBar.Islands.Add(new([new PopupButton(CommonTexts.Menu, "wf-menu")]));
    }
    
    private static AbstractButton[] AppButtons(Request req)
        => req.LoggedIn ? [
                new LinkButton(WebsiteTexts.Notes, "https://notes.uwap.org"),
                new LinkButton(WebsiteTexts.Files, "https://files.uwap.org"),
                new LinkButton(new("bi bi-envelope", WebsiteTexts.Mail), "https://mail.uwap.org")
        ] : [];
}
using System.Web;
using MimeKit;
using uwap.WebFramework;
using uwap.WebFramework.Accounts;
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

    protected override string? SupportEmail => "support@uwap.org";

    public override async Task<MailSendResult> WarningMailAsync(Request req, User user, string subject, string text, string? useThisAddress = null)
    {
        return (await MailManager.Out.SendAsync(
            new MailboxAddress("uwap.org Support", "support@uwap.org"),
            new MailboxAddress(user.Username, useThisAddress ?? user.MailAddress),
            $"[uwap.org] {subject}",
            $"Hello, {user.Username}!\n{text}\n\nIf this wasn't you, please secure your account. If you can't access your account, reply to this email to reach our support.\n\nRegards,\nuwap.org Support".Replace("\n", "<br />"),
            true, true)).Result;
    }

    public override string? Favicon(Request req)
        => "/icons/u.ico";

    public override void Navigation(Request request, WebFramework.Elements.Page page)
    {
        if (request.Domain == "uwap.org" || request.Domains.Contains("uwap.org"))
            if (request.LoggedIn)
                page.Navigation =
                [
                    new WebFramework.Elements.Button("uwap.org", "/"),
                    Presets.AccountButton(request),
                    new WebFramework.Elements.Button("About", "/about", "right"),
                    new WebFramework.Elements.Button("Projects", "/projects"),
                    new WebFramework.Elements.Button("Guides", "/guides"),
                    new WebFramework.Elements.Button("Notes", "https://notes.uwap.org"),
                    new WebFramework.Elements.Button("Files", "https://files.uwap.org"),
                    new WebFramework.Elements.Button("Mail", "https://mail.uwap.org")
                ];
            else page.Navigation =
                [
                    new WebFramework.Elements.Button("uwap.org", "/"),
                    Presets.AccountButton(request),
                    new WebFramework.Elements.Button("About", "/about", "right"),
                    new WebFramework.Elements.Button("Projects", "/projects"),
                    new WebFramework.Elements.Button("Guides", "/guides")
                ];
        else page.Navigation =
            [
                new WebFramework.Elements.Button("uwap.org", "https://uwap.org"),
                new WebFramework.Elements.Button("Menu", "/")
            ];
    }

    public override List<IStyle> Styles(Request req, out string fontUrl)
        => [UsersPlugin.GetStyle(req, out fontUrl, UsersPluginPathValue)];

    public override string UsersPluginPath(Request req)
        => UsersPluginPathValue;

    public static string UsersPluginPathValue = "/account";

    public override void ModifyPage(Request req, Page page)
    {
        page.Favicon = new(req, "/icons/u.ico");
        
        if (req.Domain == "uwap.org" || req.Domains.Contains("uwap.org"))
        {
            page.Menus.Add(new Menu("wf-menu", "Menu", [
                ..AuthButtons(req),
                new LinkButton(new("bi bi-info-circle", "About"), "/about"),
                new LinkButton(new("bi bi-hdd-rack", "Projects"), "/projects"),
                new LinkButton(new("bi bi-book", "Guides"), "/guides"),
                ..AppButtons(req)
            ]));
            page.NavBar.Islands.Add(new([new LinkButton("uwap.org", "/")]));
            page.NavBar.Islands.Add(new([new PopupButton(new("bi bi-list", "Menu"), "wf-menu")]));
        }
        else
        {
            page.NavBar.Islands.Add(new([new LinkButton("uwap.org", "https://uwap.org")]));
            page.NavBar.Islands.Add(new([new LinkButton("Home", "/")]));
        }
    }
    
    private static AbstractButton[] AppButtons(Request req)
        => req.LoggedIn ? [
                new LinkButton(new("bi bi-card-text", "Notes"), "https://notes.uwap.org"),
                new LinkButton(new("bi bi-folder", "Files"), "https://files.uwap.org"),
                new LinkButton(new("bi bi-envelope", "Mail"), "https://mail.uwap.org")
            ] : [];
}
using MimeKit;
using uwap.WebFramework;
using uwap.WebFramework.Accounts;
using uwap.WebFramework.Elements;
using uwap.WebFramework.Mail;
using uwap.WebFramework.Plugins;

namespace uwap.Website;

public class PresetsCustom : PresetsHandler
{
    public readonly static UsersPlugin UsersPlugin = new()
    {
        DefaultAccent = "violet",
        DefaultBackground = "dark",
        DefaultDesign = "layers",
        DefaultFont = "ubuntu"
    };

    protected override string? SupportEmail => "support@uwap.org";

    public override MailSendResult WarningMail(Request req, User user, string subject, string text, string? useThisAddress = null)
    {
        return MailManager.Out.Send(
            new MailboxAddress("uwap.org Support", "support@uwap.org"),
            new MailboxAddress(user.Username, useThisAddress ?? user.MailAddress),
            $"[uwap.org] {subject}",
            $"Hello, {user.Username}!\n{text}\n\nIf this wasn't you, please secure your account. If you can't access your account, reply to this email to reach our support.\n\nRegards,\nuwap.org Support".Replace("\n", "<br />"),
            true, true);
    }

    public override string? Favicon(Request req)
        => "/icons/u.ico";

    public override void Navigation(Request request, Page page)
    {
        if (request.Domain == "uwap.org" || request.Domains.Contains("uwap.org"))
            if (request.LoggedIn)
                page.Navigation =
                [
                    new Button("uwap.org", "/"),
                    Presets.AccountButton(request),
                    new Button("About", "/about", "right"),
                    new Button("Projects", "/projects"),
                    new Button("Guides", "/guides"),
                    new Button("Notes", "https://notes.uwap.org"),
                    new Button("Files", "https://files.uwap.org"),
                    new Button("Mail", "https://mail.uwap.org")
                ];
            else page.Navigation =
                [
                    new Button("uwap.org", "/"),
                    Presets.AccountButton(request),
                    new Button("About", "/about", "right"),
                    new Button("Projects", "/projects"),
                    new Button("Guides", "/guides")
                ];
        else page.Navigation =
            [
                new Button("uwap.org", "https://uwap.org"),
                new Button("Menu", "/")
            ];
    }

    public override List<IStyle> Styles(Request req, out string fontUrl)
        => [UsersPlugin.GetStyle(req, out fontUrl, UsersPluginPathValue)];

    public override string UsersPluginPath(Request req)
        => UsersPluginPathValue;

    public static string UsersPluginPathValue = "/account";
}
using uwap.WebFramework;
using uwap.WebFramework.Accounts;
using uwap.WebFramework.Elements;
using uwap.WebFramework.Mail;
using uwap.WebFramework.Plugins;
using uwap.Website;

if (Server.DebugMode = Environment.UserName != "root")
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("RUNNING IN DEBUG MODE");
    Console.ResetColor();
}

Console.WriteLine($"Useful Web App Project {Parsers.VersionString(System.Reflection.Assembly.GetExecutingAssembly())}");

//prod+debug settings
if (Server.DebugMode)
{
    Server.Config.HttpsPort = 4430;
    Server.LoadCertificate("any", "../Certificates/localhost.pfx");
    Server.Config.CacheExtensions = [];
    Server.Config.Domains.CanonicalDomains.Add("localhost", "uwap.org");
}
else
{
    Server.Config.HttpPort = 80;
    Server.Config.HttpsPort = 443;
    Server.LoadCertificate("any", "../Certificates/uwap.pfx", "#0}G]Wtdt(6*K");
    Server.Config.AutoCertificate.Email = "flo@uwap.org";
}
PresetsCustom.UsersPluginPathValue = Server.DebugMode ? "https://localhost:4430/account" : "https://account.uwap.org";

//general
Server.Config.WorkerInterval = 60;
Server.Config.ConfigureServices = s => s.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o => o.MultipartBodyLengthLimit = 4294967296);

//backups
Server.Config.Backup.Enabled = true;

//domains
Server.Config.Domains.TitleExtensions.Add("uwap.org", "uwap.org");
Server.Config.Domains.TitleExtensions.Add("pmpwsk.com", "pmpwsk.com");
Server.Config.Domains.CopyrightNames.Add("any", "uwap.org");
Server.Config.Domains.CopyrightNames.Add("pmpwsk.com", "Florian Pompowski");
Server.Config.Domains.Redirect.Add("www.uwap.org", "uwap.org");
Server.Config.Domains.Redirect.Add("www.pmpwsk.com", "pmpwsk.com");
Server.Config.Domains.Redirect.Add("pages.uwap.org", "files.uwap.org");
Server.Config.FileCorsDomain = "*";

//plugins
PluginManager.Map("uwap.org", new uwapOrgPlugin());
PluginManager.Map(Server.DebugMode ? "uwap.org/account" : "account.uwap.org", PresetsCustom.UsersPlugin);
PluginManager.Map(Server.DebugMode ? "uwap.org/r" : "r.uwap.org", new RedirectPlugin());
PluginManager.Map(Server.DebugMode ? "uwap.org/server" : "server.uwap.org", new ServerPlugin());
PluginManager.Map(Server.DebugMode ? "uwap.org/files" : "files.uwap.org", new FilePlugin() { DefaultProfileSizeLimit = 16777216});
PluginManager.Map(Server.DebugMode ? "uwap.org/mail" : "mail.uwap.org", new MailPlugin());
PluginManager.Map(Server.DebugMode ? "uwap.org/notes" : "notes.uwap.org", new NotesPlugin());
PluginManager.Map("any", new WrongDomainPlugin());

//accounts
Server.Config.Accounts.Enabled = true;
Server.Config.Accounts.WildcardDomains.Add("uwap.org");
Server.Config.Accounts.UserTables.Add("any", UserTable.Import("Users"));
Server.Config.Accounts.FailedAttempts.LogBans = true;

//mail
MailManager.ServerDomain = "uwap.org";
MailManager.DnsServers = MailManager.DnsServersCloudflare;
MailManager.In.TryStart();

//presets
Presets.Handler = new PresetsCustom();

//start server and block the thread
Server.Start();

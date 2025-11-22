using uwap.WebFramework;
using uwap.WebFramework.Plugins;

namespace uwap.Website;

public partial class uwapOrgPlugin : Plugin
{
    public override Task Handle(Request req)
    {
        Presets.CreatePage(req, "Useful Web App Project");
        switch (req.Path)
        {
            case "/":
                if (req.Method == "HEAD")
                    break;
                else throw new NotFoundSignal(); //HEAD just returns 200, GET is handled by the .wfpg parser
            case "/kill":
                if (Server.DebugMode && req.IsAdmin)
                    Environment.Exit(0);
                throw new NotFoundSignal();
            default:
                throw new NotFoundSignal();
        }

        return Task.CompletedTask;
    }
}
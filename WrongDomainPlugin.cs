using uwap.WebFramework;
using uwap.WebFramework.Elements;
using uwap.WebFramework.Plugins;

namespace uwap.Website;

public class WrongDomainPlugin : Plugin
{
    public override Task Handle(Request req)
    {
        Presets.CreatePage(req, "Error", out _, out var e);
        if (req.Path == "/")
            e.Add(new ButtonElement("Hey...", "You're visiting the uwap.org server from a domain that hasn't been assigned to any of our websites. Click here to go to the main domain.", "https://uwap.org", "red"));
        else req.Status = 404;
        return Task.CompletedTask;
    }
}

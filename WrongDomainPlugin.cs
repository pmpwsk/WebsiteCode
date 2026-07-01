using uwap.WebFramework;
using uwap.WebFramework.Elements;
using uwap.WebFramework.Plugins;
using uwap.WebFramework.Responses;

namespace uwap.Website;

public class WrongDomainPlugin : Plugin
{
    [Endpoint("/")]
    protected static IResponse Handle(Request req)
    {
        Presets.CreatePage(req, "Error", out var page, out var e);
        e.Add(new ButtonElement("Hey...", "You're visiting the uwap.org server from a domain that hasn't been assigned to any of our websites. Click here to go to the main domain.", "https://uwap.org", "red"));
        return new LegacyPageResponse(page, req);
    }
}

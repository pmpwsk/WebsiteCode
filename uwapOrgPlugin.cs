using uwap.WebFramework;
using uwap.WebFramework.Plugins;
using uwap.WebFramework.Responses;

namespace uwap.Website;

public class uwapOrgPlugin : Plugin
{
    protected static IResponse HandleRoot(Request req)
        => req.Method == "HEAD" ? new DummyResponse() : StatusResponse.NotFound;
    
    [Endpoint("/kill")]
    protected static StatusResponse HandleKill(Request req)
    {
        if (Server.DebugMode && req.IsAdmin)
            Environment.Exit(0);
        return StatusResponse.NotFound;
    }
}
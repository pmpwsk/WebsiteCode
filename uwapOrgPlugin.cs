using uwap.WebFramework;
using uwap.WebFramework.Plugins;
using uwap.WebFramework.Responses;

namespace uwap.Website;

public class uwapOrgPlugin : Plugin
{
    public override Task<IResponse> HandleAsync(Request req)
        => Task.FromResult(Handle(req));
        
    public static IResponse Handle(Request req)
    {
        Presets.CreatePage(req, "Useful Web App Project");
        switch (req.Path)
        {
            case "/":
                if (req.Method == "HEAD")
                    return new DummyResponse();
                else
                    return StatusResponse.NotFound; //HEAD just returns 200, GET is handled by the .wfpg parser
            case "/kill":
                if (Server.DebugMode && req.IsAdmin)
                    Environment.Exit(0);
                return StatusResponse.NotFound;
            default:
                return StatusResponse.NotFound;
        }
    }
}
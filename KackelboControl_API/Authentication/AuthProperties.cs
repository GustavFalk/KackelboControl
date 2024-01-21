using System.Collections.ObjectModel;

namespace KackelboControl_API.Authentication;

public class AuthProperties
{
    public static readonly string ApiKeyHeaderName = "ApiKey";

    public static readonly string Salt = "aa8f1f98-da82-489d-9af6-0494422bbf86";

    public static readonly ReadOnlyCollection<string> Keys = new ReadOnlyCollection<string>(
        new List<string>()
    {
        "8EAE3341B36B9C3C7862F301DE9AA718FB04D214E691FD6FC57B386C98797243"
    });
}

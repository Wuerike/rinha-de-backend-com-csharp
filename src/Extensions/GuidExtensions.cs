using UUIDNext;

namespace RinhaBackend.Extensions;

public static class GuidExtensions
{
    private static readonly Guid OID_NAMESPACE = Guid.Parse("6ba7b812-9dad-11d1-80b4-00c04fd430c8");

    public static Guid ToGuidV5(this string baseString)
    {
        return Uuid.NewNameBased(OID_NAMESPACE, baseString);
    }
}

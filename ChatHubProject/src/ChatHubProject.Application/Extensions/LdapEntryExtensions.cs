using Novell.Directory.Ldap;

namespace ChatHubProject.Application.Extensions
{
    public static class LdapEntryExtensions
    {
        public static LdapAttribute? TryGetAttribute(this LdapEntry conn, string attribute)
        {
            try { return conn.GetAttribute(attribute); }
            catch { return null; }
        }
    }
}
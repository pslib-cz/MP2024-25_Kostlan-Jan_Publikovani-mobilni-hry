// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("2EzLHOVVwsNCEXoyG2uIYVfQCKihUnZg2PUv6mprk3cuCt4WDqeDqKHjXvQFFnfgDtB0g6lWRZmlwg8hTEKeT9XCgp2OwZx0GoLFYnAsTznrGCqM+gS/Nb+yv0vktkQZymP855PGr846ggXl7eclL7UWQ1Cymf5rG6kqCRsmLSIBrWOt3CYqKiouKyglQAH/UH/iWMCe2rP6GBb9Jq+a9qkqJCsbqSohKakqKiuR6HwUcF2YltQ+bEqucMr1nM5aQR40pEklTzqRdeJ/4u2xSm4nUcRbJzAiLkDo0mi7AfYs8xpCNwc3w++m6Rz2aZBGGXa8jJwK4UIWbRgXQHHtIYtsMFV4ojyzqb974M0TcCU9bSKCnke2kuWNpmABzw4QjikoKisq");
        private static int[] order = new int[] { 3,2,5,6,10,9,6,10,9,12,10,12,12,13,14 };
        private static int key = 43;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

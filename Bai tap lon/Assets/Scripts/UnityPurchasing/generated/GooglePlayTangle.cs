// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("8hVL0XPUDikaKnOVIGHsOoYHAxwCsDMQAj80Oxi0erTFPzMzMzcyMR5UjmhS2v/8KzTG9DOfqZpNXZIQpEM6M1o8//qLuJ9qPrCHoCrPd5Tw+VzLFHBj730Em0d/Ypf9Tx3cueH7kLqV55NCaL1cOYOpnEEhAcX/sDM9MgKwMzgwsDMzMrGmY3a4euV68p3k9c+A0AKs9aSBWsmwhf7mMC/r8Llfxh47UrDsHLdaAW6NnfTEvZ5SO7mxHal+d8sr8YnZjh7ToMIyU0+zCPefuNzqom87FfzgCYj4ZdP04xmkg53mp1PPo1XN1jAERJ2C5KfBg7bG2o5cGcd6g9tvRxsJtxnuGOkf4YDDNr7jaAum9pkm0RmuRXi59YQuBqPVSTAxMzIz");
        private static int[] order = new int[] { 13,13,3,4,10,9,13,9,10,13,13,11,13,13,14 };
        private static int key = 50;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

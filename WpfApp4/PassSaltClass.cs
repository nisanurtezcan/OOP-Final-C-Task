using static WpfApp4.AESClass;
using static WpfApp4.BruteForceClass;

namespace WpfApp4
{
    public class PassSaltClass
    {
        public static string MyPassword()
        {
            string myPass = "myPassword";
            return myPass;
        }

        public static string MySalt()
        {
            string mySalt = "saltye00";
            return mySalt;
        }
    }
}
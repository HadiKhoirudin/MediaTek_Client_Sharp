namespace mtkclient.MTK.Client
{
    internal class seccfg
    {
        public static string CreateSeccfg(string proses, bool crtical)
        {
            string ulock;
            if (proses == "unlock")
            {
                ulock = "03 00 00 00";
            }
            else
            {
                ulock = "01 00 00 00";
            }

            string crit;
            if (crtical)
            {
                crit = "01 00 00 00";
            }
            else
            {
                crit = "00 00 00 00";
            }
            var kk =
                "4D 4D 4D 4D 04 00 00 00 3C 00 00 00 " + ulock + crit + " 00 00 00 00 45 45 45 45";
            return kk;
        }
    }
}

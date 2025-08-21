namespace mtkclient.library.xflash
{
    internal class MtkWatchdogValueCalculatorService
    {
        public static uint CalculateDisable(uint wdgAddress, uint hardwareCode)
        {
            switch (wdgAddress)
            {
                case 268464128U:
                    return 570425444U;
                case 270606336U:
                    return 570425344U;
                case 270602240U:
                    return 570425444U;
                case 268465152U:
                    return 570425344U;
                case 3221225472U:
                    return 0U;
                case 8704U:
                    switch (hardwareCode)
                    {
                        case 25173U:
                            return 1881014272U;
                        case 25169U:
                        case 25878U:
                            return 2147680256U;
                        case 25206U:
                        case 33123U:
                            return 1628176384U;
                        default:
                            return 1879199744U;
                    }
                    break;
                default:
                    return 570425444U;
            }
        }
    }
}

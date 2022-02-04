using UnityEngine;
using UnityEditor;

public class Const
{
    public const int NO_ERROR = 0;
    public const int ERR_SERVER_BLOCKED = 1;
    public const int ERR_VERSION_MISMATCHED = 2;
    public const int ERR_NETWORK_MISMATCHED = 3;
    public const int ERR_WALLET_CONNECTION_FAILED = 4;
    public const int ERR_USER_BANNED = 5;
    public const int ERR_NO_CHARACTER = 6;

    public const bool PRODUCTION = true;
    public const bool MEMORY_SAVE = true;

    public const int TERMS_VERSION = 1;

    public static long START_BLOCK = 78773461; // depolyed block of TorWorld - 101년 : 81365461
    public const long ONE_YEAR = 2592000; // 30 * 24 * 86400
    public const long QUARTER = ONE_YEAR / 4;
    public static long ELECTION_START_BLOCK = START_BLOCK + ONE_YEAR; // 0대 임기 시작 년도 봄

    public const string TOR_COIN = "TRT";
    public static int SUBSCRIBE_FEE = 2;
    public static int MONARCH_REGIST_FEE = 300;
    public static int MINING_TAX_SETTLING_DELAY = 259200;

    public static bool CONSTANT_LOADED = false;
}
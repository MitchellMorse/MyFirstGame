public static class IntExtensions
{

    public static int AddBitToInt(this int intToAddTo, int bitToAdd)
    {
        return intToAddTo |= bitToAdd;
    }

    public static int RemoveBitFromInt(this int intToRemoveFrom, int bitToRemove)
    {
        return intToRemoveFrom &= ~bitToRemove;
    }

    public static bool CheckForExistenceOfBit(this int intToCheck, int bitToCheckFor)
    {
        return (intToCheck & (int) bitToCheckFor) != 0;
    }
}

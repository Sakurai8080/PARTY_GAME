public static class DiceCalc
{
    public static int DiceSum(params int[] nums)
    {
        int res = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            res += nums[i];
        }
        return res;
    }
}

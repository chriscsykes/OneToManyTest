namespace OneToManyTest.Hobbies
{
    public static class HobbyConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Hobby." : string.Empty);
        }

    }
}
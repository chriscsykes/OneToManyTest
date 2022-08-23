namespace OneToManyTest.Customers
{
    public static class CustomerConsts
    {
        private const string DefaultSorting = "{0}FirstName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Customer." : string.Empty);
        }

    }
}
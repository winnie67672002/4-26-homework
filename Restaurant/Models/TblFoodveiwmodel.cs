namespace Restaurant.Models
{
    public class TblFoodSearchParams
    {
        public string? Name { get; set; }
        public int MinStar { get; set; }
        public int MaxStar { get; set; }
    }
    public class TblFoodSearchViewModel
    {
      
        public TblFoodSearchParams SearchParams { get; set; }
     
        public List<TblFood> Foods { get; set; }

        public TblFoodSearchViewModel()
        {
            
            SearchParams = new TblFoodSearchParams();
            Foods = new List<TblFood>();
        }
    }
}

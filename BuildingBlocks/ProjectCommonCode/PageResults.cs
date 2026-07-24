namespace ProjectCommonCode
{


    //pagenumber=1
    //pagesize=10
    //TotalnumberofRecords=100,
    //Results
    public class PageResults<T>
    {


        // the current page


        public int PageNumber { get; set; }


        //pagesize


        public int PageSize { get; set; }


        public int TotalNumberOfRecords { get; set; }


        public IEnumerable<T> Results { get; set; } 
    }
}
